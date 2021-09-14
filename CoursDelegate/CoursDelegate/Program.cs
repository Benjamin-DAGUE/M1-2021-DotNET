using System;

namespace CoursDelegate
{
    class Program
    {
        static void Main(string[] args)
        {
            //Action et Func sont des délégués génériques qui sont utilisés pour créer des références vers une méthode.
            //On peut affecter une référence à une méthode existante de la classe
            Action<string> methodReturnVoid = WriteWarningMessage;
            //Puis l'appeler 
            methodReturnVoid("Message à afficher");
            //On peut également créer une méthode anonyme inline
            methodReturnVoid = (msg) => Console.WriteLine(msg);
            //Puis l'appeler 
            methodReturnVoid("Message à afficher");

            //Le système de délégués permet de créer des événéments ou des callbacks

            DoWorkWithCallback();
            System.Threading.Thread.Sleep(2000);

            Console.Clear();
            DoWorkWithEvent();
        }

        static void WriteWarningMessage(string message)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ForegroundColor = color;
        }

        static void DoWorkWithCallback()
        {
            object isEndedLocker = new object();//objet utilisé pour vérouiller l'accès à la variable isEnded et éviter un accès concurentiel (modification/lecture par deux threads simultanément).
            bool isEnded = false;

            Worker worker = new Worker();

            Console.WriteLine("Démarrage du traitement async avec un callback");

            worker.DoWorkWithCallbackAsync(() =>
            {
                lock (isEndedLocker) //On attend la libération du vérou puis on vérouille l'accès à la variable isEnded.
                {
                    isEnded = true; //On marque le traitement comme terminé pour terminer le programme.
                }
            });

            Console.WriteLine("Traitement en cours");

            bool exitLoop = false;

            do
            {
                lock (isEndedLocker) //On attend la libération du vérou puis on vérouille l'accès à la variable isEnded.
                {
                    exitLoop = isEnded; //On copie dans une variable temporaire la valeur de la variable isEnded.
                }
                System.Threading.Thread.Sleep(100); //On attend quelques instants pour éviter de surcharger le thread principal.
            } while (exitLoop == false);

            Console.WriteLine("Traitement terminé");
        }

        static void DoWorkWithEvent()
        {
            object isEndedLocker = new object(); //objet utilisé pour vérouiller l'accès à la variable isEnded et éviter un accès concurentiel (modification/lecture par deux threads simultanément).
            bool isEnded = false;

            Worker worker = new Worker();
            worker.WorkEnded += (sender, e) =>
            {
                lock (isEndedLocker) //On attend la libération du vérou puis on vérouille l'accès à la variable isEnded.
                {
                    isEnded = true; //On marque le traitement comme terminé pour terminer le programme.
                }
            };

            Console.WriteLine("Démarrage du traitement async avec un événement");
            
            worker.DoWorkWithEventAsync();

            Console.WriteLine("Traitement en cours");

            bool exitLoop = false;

            do
            {
                lock (isEndedLocker) //On attend la libération du vérou puis on vérouille l'accès à la variable isEnded.
                {
                    exitLoop = isEnded; //On copie dans une variable temporaire la valeur de la variable isEnded.
                }
                System.Threading.Thread.Sleep(100); //On attend quelques instants pour éviter de surcharger le thread principal.
            } while (exitLoop == false);

            Console.WriteLine("Traitement terminé");
        }
    }
}
