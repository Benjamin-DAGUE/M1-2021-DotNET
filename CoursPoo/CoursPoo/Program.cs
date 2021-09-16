using System;
using System.Threading.Tasks;

namespace CoursPoo
{
    class Program
    {
        static BStrongReference _BStrongRef;
        static BWeakReference _BWeakRef;

        static async Task Main(string[] args)
        {

            SubClass sc = new SubClass();
            BaseClass bc = sc;

            //Dans ce cas de figure, puisque la méthode est surchargée par SubClass, on appel bien le même comportement, peut importe le type de référence manipulé.
            sc.MyMethod();
            bc.MyMethod();

            //Dans ce cas de figure, puisque la propriété est masquée par SubClass, on appel le comportement du parent lorsque le type de la référence manipulée est du type du parent.
            Console.WriteLine(sc.MyProperty);
            Console.WriteLine(bc.MyProperty);

            //Si l'instance de _B est static, cette instance va vivre indéfiniement.

            
            //B prend en paramètre une instance de A.
            //En fonction du comportement de B, deux cas de figure sont possibles :
            // - B garde une référence forte vers une instance de A : il va donc garder une référence vers l'instance et empêcher sa destruction.
            // - B garde une référence faible vers une instance de A : dans ce cas, il ne vas pas empêcher la suppression de l'instance de A.
            _BStrongRef = new BStrongReference(new A());
            _BWeakRef = new BWeakReference(new A());

            GC.Collect();

            await Task.Delay(TimeSpan.FromSeconds(30));

            GC.Collect();

            _BStrongRef.SayHello();
            _BWeakRef.SayHello();


            Console.ReadLine();

            //A partir du moment où l'on ne conserve plus un référence vers l'instance de A, elle pourra être supprimée de la mémoire par le GarbageCollector.

        }
    }
}
