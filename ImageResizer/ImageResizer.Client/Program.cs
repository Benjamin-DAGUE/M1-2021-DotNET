using ImageResizer.Core;
using System;
using System.Drawing;
using System.IO;

namespace ImageResizer.Client
{
    class Program
    {
        #region Fields

        /// <summary>
        ///     Chemine du dossier à écouter.
        /// </summary>
        private static string _InputDirectory;

        /// <summary>
        ///     Chemin du dossier dans lequel créer l'image traitée.
        /// </summary>
        private static string _OutputDirectory;

        #endregion

        #region Methods

        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 2)
                {
#if DEBUG
                    args = new string[]
                    {
                        @"C:\Users\bdague\Desktop\Dir\Source",
                        @"C:\Users\bdague\Desktop\Dir\Trt"
                    };
#else
                    throw new Exception("Vous devez spécifier en premier argument le chemin de l'image d'origine et en deuxième le chemin de la copie.");
#endif
                }

                _InputDirectory = args[0];
                _OutputDirectory = args[1];

                using FileSystemWatcher watcher = new FileSystemWatcher(_InputDirectory);
                watcher.Created += Watcher_Created;
                watcher.EnableRaisingEvents = true;

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
#if DEBUG
                Console.WriteLine(ex.ToString());
#else
                Console.WriteLine(ex.Message);
#endif
            }
        }

        private static void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Nouveau fichier, ouverture de : {e.FullPath}");
            
            if (ImageProcessing.TryOpenFile(e.FullPath))
            {
                string outputPath = e.FullPath.Replace(_InputDirectory, _OutputDirectory);
                Console.WriteLine($"Traitement de : {e.FullPath}");

                try
                {
                    if (File.Exists(outputPath))
                    {
                        Console.WriteLine($"Le fichier existe déjà et va être supprimé : {outputPath}");
                        File.Delete(outputPath);
                    }

                    ImageProcessing.MakeImageSquare(e.FullPath, outputPath);
                    Console.WriteLine($"Traitement terminé : {e.FullPath}");

                    File.Delete(e.FullPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur de traitement : {e.FullPath}");
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                Console.WriteLine($"Impossible d'ouvrir le fichier : {e.FullPath}");
            }
        }

        #endregion
    }
}
