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
            
            if (TryOpenFile(e.FullPath))
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

                    MakeImageSquare(e.FullPath, outputPath);
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

        private static bool TryOpenFile(string filePath)
        {
            bool isFileOpen = false;

            do
            {
                try
                {
                    using FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
                    isFileOpen = true;
                }
                catch (IOException ex)
                {
                    if (File.Exists(filePath) == false)
                    {
                        return false;   //Si le fichier n'existe plus, on arrête l'attente.
                    }
                    System.Threading.Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    return false; //Si on a une autre erreur qu'une erreur de type IOException.
                }
            } while (isFileOpen == false);

            return isFileOpen;
        }

        private static void MakeImageSquare(string inputPath, string outputPath)
        {
            if (string.IsNullOrWhiteSpace(inputPath) || File.Exists(inputPath) == false)
            {
                throw new Exception("Le fichier à traiter n'existe pas.");
            }

            if (string.IsNullOrWhiteSpace(outputPath) || File.Exists(outputPath) == true)
            {
                throw new Exception("Le chemin de sortie n'est pas valide ou existe déjà.");
            }

            try
            {
                using Image image = Image.FromFile(inputPath); //Ouverture de l'image
                
                if (image.Width != image.Height) //Si la hauteur est différente de la largeur
                {
                    bool isWidthLarger = image.Width > image.Height;
                    int largerSize = isWidthLarger ? image.Width : image.Height;
                    int smallerSize = isWidthLarger ? image.Height : image.Width;
                    int position = (largerSize - smallerSize) / 2;
                    Point imagePoint = isWidthLarger ? new Point(0, position) : new Point(position, 0);

                    using Bitmap resizedImage = new Bitmap(image, largerSize, largerSize); //Création d'une nouvelle image à la bonne dimension
                    resizedImage.SetResolution(image.HorizontalResolution, image.VerticalResolution); //On reprend les DPI d'origines

                    using Graphics graphics = Graphics.FromImage(resizedImage); //Graphics permet de modifier l'image
                    graphics.Clear(Color.Transparent);  //On met un fond transparent
                    graphics.DrawImage(image, imagePoint);  //On copie l'image d'origine à la bonne position

                    resizedImage.Save(outputPath); //Sauvegarde du résultat
                }
                else
                    {
                        image.Save(outputPath); //Si la largeur est égal à la hauteur, on sauvegarde l'image d'origine.
                        }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors du traitement de l'image.", ex);
            }
        }

        #endregion
    }
}
