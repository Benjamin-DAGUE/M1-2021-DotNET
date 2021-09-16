using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursPoo
{
    class BWeakReference
    {
        #region Fields

        private WeakReference<A> _WeakInstanceOfA;

        #endregion

        #region Constructors

        public BWeakReference(A instanceOfA)
        {
            //Dans ce cas de figure, on a une référence faible vers l'instance de A fournie.
            this._WeakInstanceOfA = new WeakReference<A>(instanceOfA);

        }

        #endregion

        #region Methods

        public void SayHello()
        {
            //Pour manipuler une référence faible : 
            if (this._WeakInstanceOfA.TryGetTarget(out A instanceOfA))
            {
                //La méthode TryGetTarget permet d'accéder à l'instance de A.
                //Si l'instance de A n'existe plus en mémoire, la méthode retourne faux.
                //Sinon, la méthode TryGetTarget retourne true.
                instanceOfA.HelloFromA();
            }
            else
            {
                Console.WriteLine("Impossible de dire bonjour.");
            }
        }

        #endregion
    }
}
