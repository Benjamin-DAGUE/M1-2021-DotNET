using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursDelegate
{
    class Worker
    {
        #region Implémentation avec un événement

        /// <summary>
        ///     Déclenché lorsque le traitement asynchrone est terminé.
        /// </summary>
        public event EventHandler WorkEnded; //Création d'un événement. Un événement est une liste d'abonnée à rappeler lorsque l'objet change d'état (par exemple dans notre cas lorsque le traitement est terminé).

        /// <summary>
        ///     Effectue un travail asynchrone et déclenche l'événement <see cref="WorkEnded"/> à la fin du traitement.
        /// </summary>
        public void DoWorkWithEventAsync()
        {
            Task.Run(() =>
            {
                System.Threading.Thread.Sleep(5000);        //Attente de 5 secondes.
                WorkEnded?.Invoke(this, new EventArgs());   //Si on a des abonnonés, on appel les abonnés.
            });
        }

        #endregion

        #region Implémentation avec un callback

        /// <summary>
        ///     Effectue un travail asynchrone et déclenche la méthode <see cref="endedCallback"/> passée en paramètre.
        /// </summary>
        /// <param name="endedCallback">Référence d'une méthode à appeler à la fin du traitement.</param>
        public void DoWorkWithCallbackAsync(Action endedCallback)
        {
            Task.Run(() =>
            {
                System.Threading.Thread.Sleep(5000);    //Attente de 5 secondes.
                endedCallback?.Invoke();                //Si on a une référence de méthode en paramètre, on appel la méthode.
            });
        }

        #endregion
    }
}
