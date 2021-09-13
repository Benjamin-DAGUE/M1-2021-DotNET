using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook.Models
{
    /// <summary>
    ///     Représente une personne.
    /// </summary>
    public class Person
    {
        #region Fields (Attributs)

        /// <summary>
        ///     Nom de famille de la personne.
        /// </summary>
        private string _FirstName;

        /// <summary>
        ///     Date de naissance de la personne.
        /// </summary>
        private DateTime _Birthdate;

        #endregion

        #region Properties (Accesseurs / propriétés)

        /// <summary>
        ///     Obtient ou définit le nom de famille de la personne.
        /// </summary>
        /// <remarks>
        ///     Cette version expose un attribut définit dans la classe. Elle a l'avantage de permettre de personaliser le comportement des méthodes get et set.
        /// </remarks>
        public string FirstName { get => this._FirstName; set => this._FirstName = value; }

        /// <summary>
        ///     Obtient ou définit le prénom de la personne.
        /// </summary>
        /// <remarks>
        ///     Avec cette syntaxe, il n'est pas nécessaire de déclarer un attribut et d'écrir les méthodes get et set;
        /// </remarks>
        public string LastName { get; set; }

        /// <summary>
        ///     Obtient ou définit la date de naissance de la personne.
        /// </summary>
        /// <remarks>
        ///     Cette syntaxe est la plus verbeuse, son comportement est similaire à la propriété <see cref="FirstName"/>.
        /// </remarks>
        public DateTime Birthdate
        {
            get { return _Birthdate; }
            set { _Birthdate = value; }
        }

        /// <summary>
        ///     Obtient le nom complet de la personne.
        /// </summary>
        /// <remarks>
        ///     Cette syntaxe permet de créer une propriété en lecture seule.
        /// </remarks>
        public string FullName => this.FirstName + " " + this.LastName;

        /// <summary>
        ///     Obtient ou définit l'adresse email de la personne.
        /// </summary>
        public string EMailAddress { get; set; }

        #endregion

        #region Constructors

        #endregion

        //Méthodes

        //constructeur / destructeur

        //Événements
    }
}
