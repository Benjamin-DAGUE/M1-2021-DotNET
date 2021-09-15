using CoursLinq.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoursLinq
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Person> people = new List<Person>()
            {
                new Person()
                {
                    FirstName = "Klervia",
                    LastName = "BAUTRAIS",
                    EMailAddress = "k.bautrais@iia-laval.fr",
                    Birthdate = new DateTime(1995, 7, 25)
                },
                new Person()
                {
                    FirstName = "Lucien",
                    LastName = "BIGOT",
                    EMailAddress = "k.bigot@iia-laval.fr",
                    Birthdate = new DateTime(1992, 4, 12)
                },
                new Person()
                {
                    FirstName = "Matthieu",
                    LastName = "DURET",
                    EMailAddress = "m.duret@iia-laval.fr",
                    Birthdate = new DateTime(2001, 11, 12)
                },
                new Person()
                {
                    FirstName = "Jennifer",
                    LastName = "JUBERT",
                    EMailAddress = "j.jubert@iia-laval.fr",
                    Birthdate = new DateTime(2001, 11, 16)
                },
                new Person()
                {
                    FirstName = "Corentin",
                    LastName = "LUCIEN",
                    EMailAddress = "c.lucien@iia-laval.fr",
                    Birthdate = new DateTime(2002, 1, 5)
                },
                new Person()
                {
                    FirstName = "Thibaut",
                    LastName = "MINARD",
                    EMailAddress = "t.minard@iia-laval.fr",
                    Birthdate = new DateTime(2004, 2, 20)
                },
                new Person()
                {
                    FirstName = "Thomas",
                    LastName = "NEVO",
                    EMailAddress = "t.nevo@iia-laval.fr",
                    Birthdate = new DateTime(2004, 2, 20)
                },
            };

            Console.WriteLine("1 - Afficher les personnes nées avant l'an 2000");
            people                                              //On part de la liste des personnes.
                .Where(p => p.Birthdate.Year < 2000)            //On filtre pour conserver les personnes nées avant l'an 2000.
                .ToList()                                       //On exécute la requête en créant une liste du réslutat.
                .ForEach(p => Console.WriteLine(p.FullName));   //Pour chaque personne dans la liste du résultat, on affiche son nom et son prénom.

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("2 - Afficher les personnes nées en janvier et en février");
            people
                .Where(p => new[] { 1, 2, 6 }.Contains(p.Birthdate.Month))
                //.Where(p => p.Birthdate.Month <= 2)
                .Where(p => p.Birthdate.Month <= 3)
                .ToList()
                .ForEach(p => Console.WriteLine(p.FullName));

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("3 - Afficher les personnes nées un samedi ou un dimanche");
            people
                .Where(p => new[] { DayOfWeek.Saturday, DayOfWeek.Sunday }.Contains(p.Birthdate.DayOfWeek))
                //.Where(p => p.Birthdate.DayOfWeek == DayOfWeek.Saturday || p.Birthdate.DayOfWeek == DayOfWeek.Sunday)
                .ToList()
                .ForEach(p => Console.WriteLine(p.FullName));

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("4 - Afficher les personnes pour lesquelles le prénom a plus de 7 caractères");
            people
                .Where(p => p.FirstName.Length > 7)
                .ToList()
                .ForEach(p => Console.WriteLine(p.FullName));

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("5 - Afficher les personnes nées en janvier et en février et pour lesquelles le prénom a plus de 7 caractères");
            people
                .Where(p => p.Birthdate.Month <= 2 && p.FirstName.Length > 7)
                //.Where(p => p.Birthdate.Month <= 2)
                //.Where(p => p.FirstName.Length > 7)
                .ToList()
                .ForEach(p => Console.WriteLine(p.FullName));

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("6 - Calculer la moyenne d'age des personnes");
            double averageAge = people.Average(p => p.Birthdate.CalculateAge());
            Console.WriteLine("L'âge moyen est : " + averageAge);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("7 - Afficher les personnes de la plus ancienne à la plus jeune");
            people
                .OrderBy(p => p.Birthdate)
                .ToList()
                .ForEach(p => Console.WriteLine(p.FullName));

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("8 - Afficher les personnes dont l'age est supérieur à la moyenne d'âge");
            people
                //Cette méthode calcul la moyenne d'âge plusieurs fois (une foi par personne présente dans la liste)
                //.Where(p => p.Birthdate.CalculateAge() > people.Average(p => p.Birthdate.CalculateAge()))
                .Where(p => p.Birthdate.CalculateAge() > averageAge)
                .ToList()
                .ForEach(p => Console.WriteLine(p.FullName));

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("9 - Saisisez une chaîne et afficher les personnes dont le nom contient la recherche");
            Console.Write("Votre recherche : ");
            string search = Console.ReadLine();
            people
                .Where(p => p.FullName.ToLower().Contains(search?.ToLower()))
                .ToList()
                .ForEach(p => Console.WriteLine(p.FullName));

            Console.ReadLine();
        }
    }
}
