using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursPoo
{
    class BStrongReference
    {
        #region Fields

        private A _InstanceOfA;

        #endregion

        #region Constructors

        public BStrongReference(A instanceOfA)
        {
            //Dans ce cas de figure, on a une référence forte vers l'instance de A fournie.
            this._InstanceOfA = instanceOfA;
        }

        #endregion

        #region Methods

        public void SayHello()
        {
            this._InstanceOfA.HelloFromA();
        }

        #endregion
    }
}
