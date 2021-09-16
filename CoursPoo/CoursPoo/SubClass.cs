using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursPoo
{
    class SubClass : BaseClass
    {
        //Même si BaseClass.MyProperty n'est pas une propriété virtuelle ou abstraite
        //elle peut être redéfinie par masquage.
        //Attention, le comportement du masquage est différent d'une surcharge puisqu'il sera toujours possible pour l'appelant d'avoir accès à la définition de la propriété parente.
        public new string MyProperty => "SubClass";


        //Lorsqu'une méthode virtuelle est surchargé, c'est son comportement qui va être appelée lorsque l'instance va être manipulée.
        //Si la méthode masque le comportement du parent, il n'est pas possible de l'appeler depuis l'extérieur.
        public override void MyMethod()
        {
            //base.MyMethod(); //Permet d'appeler l'implémentation de la classe de base.
            Console.WriteLine("SubClass.MyMethod");
        }

        //Il est possible d'exposer l'implémentation d'une méthode virtuelle depuis une autre méthode.
        public void MyMethodFromBaseClass()
        {
            base.MyMethod();
        }

        //Le mot clef override permet également de définir les membres abstraits du parent.
        public override void MyMethodAbstract()
        {
            Console.WriteLine("SubClass.MyMethodAbstract");
        }
    }
}
