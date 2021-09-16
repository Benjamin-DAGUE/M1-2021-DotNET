using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursPoo
{
    abstract class BaseClass : Object //Même si ce n'est pas précisé, une classe pour laquelle on ne définie pas de parent hérite de la classe Object.
    {
        //Cette propriété n'est pas abstraite ou virtuelle. Pour autant, on ne peut pas empêcher un enfant de la masquer.
        public string MyProperty => "BaseClass"; 

        //Une méthode virtuelle est une méthode qui permet à un enfant de la surcharger avec modification ou remplacement du comportement.
        //Contrairement à une méthode abstraite, les enfants ne sont pas obligés de l'implémenter, ce qui oblige le parent à définir un comportement de base (et donc un corps pour la méthode).
        public virtual void MyMethod()
        {
            Console.WriteLine("BaseClass.MyMethod");
        }

        //Une méthode abstraite (c'est à dire sans comportement) peut être définit uniquement dans une classe abstraite.
        //Une méthode abstraite doit obligatoirement être implémentée par les classes enfants, à moins que l'enfant ne soit lui même abstrait.
        public abstract void MyMethodAbstract();
    }
}
