using System;
using System.Drawing;
using System.IO;

namespace ImageResizer.Core
{
    public static class ImageProcessing
    {
        /// <summary>
        ///     Transforme une image au format carré.
        /// </summary>
        /// <param name="inputPath">Image à traiter.</param>
        /// <param name="outputPath">Chemin de l'image traitée.</param>
        public static void MakeImageSquare(string inputPath, string outputPath)
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

        /// <summary>
        ///     Attend qu'un fichier soit disponible.
        /// </summary>
        /// <param name="filePath">Chemin du fichier à attendre.</param>
        /// <returns>Vrai si le fichier est ouvert, faux en cas d'erreur.</returns>
        public static bool TryOpenFile(string filePath)
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
    }
}
