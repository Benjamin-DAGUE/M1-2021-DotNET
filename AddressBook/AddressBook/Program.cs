using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using AddressBook.Models;

namespace AddressBook
{
    class Program
    {
        #region Fields

        /// <summary>
        ///     Liste des personnes présentes dans le carnet.
        /// </summary>
        private static List<Person> _People;

        #endregion

        #region Methods

        /// <summary>
        ///     Méthode principale du programme.
        /// </summary>
        /// <param name="args">Arguments passés lors de l'appel du programme.</param>
        static void Main(string[] args)
        {
            //_People = LoadFakePeople();
            _People = LoadFromFile();

            bool exit = false;

            #region Main Menu

            do
            {
                Console.WriteLine("Bienvenue dans le carnet d'adresse .NET Console !");
                Console.WriteLine("1 : Parcourir le carnet");
                Console.WriteLine("2 : Ajouter une personne");
                Console.WriteLine("3 : Rechercher une personne");
                Console.WriteLine("0 : Quitter");

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "0":
                        exit = true;
                        break;
                    case "1":
                        ReadPeople(_People);
                        break;
                    case "2":
                        AddPerson();
                        break;
                    case "3":
                        Search();
                        break;
                    default:
                        break;
                }

                Console.Clear();

            } while (exit == false);

            #endregion

            #region Sauvegarde
            
            try
            {
                string dirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AddressBook");
                string filePath = Path.Combine(dirPath, "data.json");
                string json = JsonSerializer.Serialize(_People);

                Directory.CreateDirectory(dirPath);

                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la sauvegarde du fichier de données.");
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                Console.ReadKey();
            }

            #endregion
        }

        /// <summary>
        ///     Charge la liste des personnes (données de test).
        /// </summary>
        /// <returns>Liste des personnes du carnet d'adresse.</returns>
        private static List<Person> LoadFakePeople() => new List<Person>()
        {
            new Person()
            {
                FirstName = "Benjamin",
                LastName = "DAGUÉ",
                Birthdate = new DateTime(1987, 12, 24),
                EMailAddress = "benjamin.dague@etskirsch.fr"
            },
            new Person()
            {
                FirstName = "Emmanuel",
                LastName = "STEPHANT",
                Birthdate = new DateTime(1990, 1, 1),
                EMailAddress = "emmanuel.stephant@etskirsch.fr"
            },
            new Person()
            {
                FirstName = "Jean",
                LastName = "DAGUET",
                Birthdate = new DateTime(1991, 2, 2),
                EMailAddress = "jdup@gmail.com"
            }
        };

        /// <summary>
        ///     Charge la liste des personnes depuis un fichier.
        /// </summary>
        /// <returns></returns>
        private static List<Person> LoadFromFile()
        {
            List<Person> result = new List<Person>();

            try
            {
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AddressBook", "data.json");

                if (File.Exists(filePath))
                {
                    result = JsonSerializer.Deserialize<List<Person>>(File.ReadAllText(filePath));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors du chargement du fichier de données.");
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                Console.ReadKey();
            }

            return result;
        }

        /// <summary>
        ///     Permet de parcourir une liste de personne spécifiée.
        /// </summary>
        /// <param name="people">Liste de personne à parcourir.</param>
        /// <param name="searchTerm">Termes de la recherche.</param>
        private static void ReadPeople(List<Person> people, string searchTerm = null)
        {
            bool exit = false;
            int currentIndex = 0;

            do
            {
                Console.Clear();

                if (string.IsNullOrWhiteSpace(searchTerm) == false)
                {
                    Console.WriteLine("Voici les résultats pour la recherche suivante :");
                    Console.WriteLine(searchTerm);
                }

                if (people.Count == 0)
                {
                    Console.WriteLine(people == _People ? "Le carnet est vide" : "Aucun résultat");
                    Console.WriteLine("Appuyez sur une touche pour retourner au menu principal...");
                    Console.ReadKey();
                    exit = true;
                    break;
                }

                Person person = people[currentIndex];

                //Console.WriteLine(string.Format("Prénom : {0}", person.FirstName));
                Console.WriteLine($"Prénom : {person.FirstName}");
                Console.WriteLine($"Nom : {person.LastName}");
                Console.WriteLine($"Date de naissance : {person.Birthdate.ToShortDateString()}");
                Console.WriteLine($"EMail : {person.EMailAddress}");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("1 : Suivant");
                Console.WriteLine("2 : Précédent");
                Console.WriteLine("3 : Supprimer");
                Console.WriteLine("0 : Retour");

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "0":
                        //Retour au menu principal.
                        exit = true;
                        break;
                    case "1":
                        //On va à la personne suivante ou au début du carnet si on est à la fin.
                        currentIndex = currentIndex + 1 == people.Count ? 0 : currentIndex + 1;
                        break;
                    case "2":
                        //On va à la personne précédente ou à la fin du carnet si on est au début.
                        currentIndex = currentIndex - 1 < 0 ? people.Count - 1 : currentIndex - 1;
                        break;
                    case "3":
                        //On supprime la personne de la liste en cours de lecture.
                        people.Remove(person);
                        //On supprime du carnet également sur la liste en cours de lecture n'est pas le carnet (cas de la recherche).
                        if (_People != people)
                        {
                            _People.Remove(person);
                        }
                        //On change l'index si on supprime la personne à la fin de la liste pour prendre la personne précédente.
                        currentIndex = currentIndex >= people.Count ? people.Count - 1 : currentIndex;
                        break;
                    default:
                        break;
                }
            } while (exit == false);
        }

        /// <summary>
        ///     Ajoute une nouvelle personne dans le carnet d'adresse.
        /// </summary>
        private static void AddPerson()
        {
            Console.Clear();

            Person person = new Person();

            Console.Write("Prénom : ");
            person.FirstName = Console.ReadLine();

            Console.Write("Nom : ");
            person.LastName = Console.ReadLine();

            Console.Write("Date de naissance : ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime birthDate))
            {
                person.Birthdate = birthDate;
            }

            Console.Write("Email : ");
            person.EMailAddress = Console.ReadLine();

            _People.Add(person);
        }

        private static bool Filter(Person p)
        {
            return p.FirstName?.StartsWith("B") == true;
        }

        /// <summary>
        ///     Recherche une personne dans le carnet d'adresse.
        /// </summary>
        private static void Search()
        {
            Console.Clear();

            string searchTerm = null;

            //On contruit une requête avec des filtres .Where() en fonction des choix de l'utilisateur.
            IEnumerable<Person> query = _People;

            Console.Write("Rechercher par le prénom : ");
            string firstNameSearch = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(firstNameSearch) == false)
            {
                query = query.Where(p => p.FirstName?.ToLower()?.Contains(firstNameSearch?.ToLower()) == true);
                searchTerm = $"Le prénom contient \"{firstNameSearch}\"{Environment.NewLine}";
            }

            Console.Write("Rechercher par le nom : ");
            string lastNameSearch = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(lastNameSearch) == false)
            {
                query = query.Where(p => p.LastName?.ToLower()?.Contains(lastNameSearch?.ToLower()) == true);
                searchTerm += $"Le nom contient \"{lastNameSearch}\"{Environment.NewLine}";
            }

            Console.Write("Rechercher par l'adresse email : ");
            string eMailSearch = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(eMailSearch) == false)
            {
                query = query.Where(p => p.EMailAddress?.ToLower().Contains(eMailSearch?.ToLower()) == true);
                searchTerm += $"L'adresse email contient \"{eMailSearch}\"{Environment.NewLine}";
            }

            //On utilise la fonction de parcourir d'une liste de personne pour afficher les résultats.
            ReadPeople(query.ToList(), searchTerm);
        }

        #endregion
    }
}
