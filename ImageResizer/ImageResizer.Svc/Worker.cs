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
        ///     Instance du journal � utiliser.
        /// </summary>
        private readonly ILogger<Worker> _Logger;

        /// <summary>
        ///     Chemin du dossier � �couter.
        /// </summary>
        private readonly string _InputPath;

        /// <summary>
        ///     Chemin du dossier dans lequel copier les images trait�es.
        /// </summary>
        private readonly string _OutputPath;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initialise une nouvelle instance de la classe <see cref="Worker"/>.
        /// </summary>
        /// <param name="logger">Instance du journal � utiliser.</param>
        /// <param name="configuration">Instance de la configuration � utiliser.</param>
        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            this._Logger = logger;
            this._InputPath = configuration.GetValue<string>("InputPath");
            this._OutputPath = configuration.GetValue<string>("OutputPath");
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Point d'entr�e de l'ex�cution du service.
        /// </summary>
        /// <param name="stoppingToken">Jeton d'arr�t du service.</param>
        /// <returns>T�che pouvant �tre attendu.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using FileSystemWatcher watcher = new FileSystemWatcher(this._InputPath);
            watcher.Created += Watcher_Created;
            watcher.EnableRaisingEvents = true;

            this._Logger.LogInformation($"D�marrage de l'�coute du dossier {this._InputPath}");

            while (!stoppingToken.IsCancellationRequested)
            {
                //On attend tant que l'arr�t du service n'est pas demand�.
                await Task.Delay(100);
            }

            watcher.EnableRaisingEvents = false;
            watcher.Created -= Watcher_Created;
        }

        /// <summary>
        ///     M�thode d�clench�e lorsqu'un fichier ou un dossier est ajout� dans le dossier �cout�.
        /// </summary>
        /// <param name="sender">Instance qui a d�clench�e l'�v�nement.</param>
        /// <param name="e">Argument de l'�v�nement.</param>
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
                        this._Logger.LogWarning($"Le fichier existe d�j� et va �tre supprim� : {outputPath}");
                        File.Delete(outputPath);
                    }

                    ImageProcessing.MakeImageSquare(e.FullPath, outputPath);
                    this._Logger.LogInformation($"Traitement termin� : {e.FullPath}");

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
