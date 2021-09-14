using System;
using System.Drawing;
using System.IO;

namespace ImageResizer.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 2)
                {
#if DEBUG
                    args = new string[]
                    {
                        @"C:\Users\bdague\Desktop\source.png",
                        @"C:\Users\bdague\Desktop\copy.png"
                    };
#else
                    throw new Exception("Vous devez spécifier en premier argument le chemin de l'image d'origine et en deuxième le chemin de la copie.");
#endif
                }

                MakeImageSquare(args[0], args[1]);
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

        private static void MakeImageSquare(string inputPath, string outputPath)
        {
            Console.WriteLine($"Traitement de [{inputPath}] vers [{outputPath}]");

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
    }
}
