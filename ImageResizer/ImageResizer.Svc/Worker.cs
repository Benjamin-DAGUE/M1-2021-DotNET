using ImageResizer.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ImageResizer.Svc
{
    public class Worker : BackgroundService
    {
        #region Fields

        /// <summary>
        ///     Instance du journal à utiliser.
        /// </summary>
        private readonly ILogger<Worker> _Logger;

        /// <summary>
        ///     Chemin du dossier à écouter.
        /// </summary>
        private readonly string _InputPath;

        /// <summary>
        ///     Chemin du dossier dans lequel copier les images traitées.
        /// </summary>
        private readonly string _OutputPath;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initialise une nouvelle instance de la classe <see cref="Worker"/>.
        /// </summary>
        /// <param name="logger">Instance du journal à utiliser.</param>
        /// <param name="configuration">Instance de la configuration à utiliser.</param>
        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            this._Logger = logger;
            this._InputPath = configuration.GetValue<string>("InputPath");
            this._OutputPath = configuration.GetValue<string>("OutputPath");
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Point d'entrée de l'exécution du service.
        /// </summary>
        /// <param name="stoppingToken">Jeton d'arrêt du service.</param>
        /// <returns>Tâche pouvant être attendu.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using FileSystemWatcher watcher = new FileSystemWatcher(this._InputPath);
            watcher.Created += Watcher_Created;
            watcher.EnableRaisingEvents = true;

            this._Logger.LogInformation($"Démarrage de l'écoute du dossier {this._InputPath}");

            while (!stoppingToken.IsCancellationRequested)
            {
                //On attend tant que l'arrêt du service n'est pas demandé.
                await Task.Delay(100);
            }

            watcher.EnableRaisingEvents = false;
            watcher.Created -= Watcher_Created;
        }

        /// <summary>
        ///     Méthode déclenchée lorsqu'un fichier ou un dossier est ajouté dans le dossier écouté.
        /// </summary>
        /// <param name="sender">Instance qui a déclenchée l'événement.</param>
        /// <param name="e">Argument de l'événement.</param>
        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            this._Logger.LogInformation($"Nouveau fichier, ouverture de : {e.FullPath}");

            if (ImageProcessing.TryOpenFile(e.FullPath))
            {
                string outputPath = e.FullPath.Replace(this._InputPath, this._OutputPath);
                this._Logger.LogInformation($"Traitement de : {e.FullPath}");

                try
                {
                    if (File.Exists(outputPath))
                    {
                        this._Logger.LogWarning($"Le fichier existe déjà et va être supprimé : {outputPath}");
                        File.Delete(outputPath);
                    }

                    ImageProcessing.MakeImageSquare(e.FullPath, outputPath);
                    this._Logger.LogInformation($"Traitement terminé : {e.FullPath}");

                    File.Delete(e.FullPath);
                }
                catch (Exception ex)
                {
                    this._Logger.LogError($"Erreur de traitement : {e.FullPath}{Environment.NewLine}{ex}");
                }
            }
            else
            {
                this._Logger.LogError($"Impossible d'ouvrir le fichier : {e.FullPath}");
            }
        }

        #endregion
    }
}
