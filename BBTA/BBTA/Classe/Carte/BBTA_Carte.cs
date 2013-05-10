using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace EditeurCarteXNA
{
    /// <summary>
    /// La classe contient les informations de base pour les cartes de jeux et elle est utilisé dans l'éditeur de niveau.
    /// </summary>
    [Serializable]
    public class BBTA_Carte
    {

        private List<TuileEditeur> listeTuile = new List<TuileEditeur>();
        //Redéfini le nom de la balise XML. Par défault elle aurait porté le nom de l'attribut situé en dessous.
        [XmlElement("EnTete")]
        //Pour sérializer correctement, chaques attributs doivent être publique.
        public InfoCarte InformationCarte { get; set; }

        public List<TuileEditeur> ListTuile { get { return listeTuile; } }

        /// <summary>
        /// Constructeur de base de Classe BBTA_Carte.
        /// La classe contient les informations de base pour les cartes de jeux et elle est utilisé dans l'éditeur de niveau.
        /// </summary>
        public BBTA_Carte()
        {

        }

        /// <summary>
        /// Constructeur de Classe BBTA_Carte.
        /// La classe contient les informations de base pour les cartes de jeux et elle est utilisé dans l'éditeur de niveau.
        /// </summary>
        /// <param name="infoCarte">
        /// Objet InfoCarte qui contient le nom de la carte, les dimensions et le nombre max et min de joueur.
        /// </param>
        /// <param name="listeTuile">
        /// Objet List TuileEditeur. Chaque tuile contient un identifiant et un Rectangle.
        /// </param>
        public BBTA_Carte(InfoCarte infoCarte, List<TuileEditeur> listeTuile)
        {
            this.InformationCarte = infoCarte;
            this.listeTuile = listeTuile;
        }


        /// <summary>
        /// Constructeur sans objet InfoCarte de la Classe BBTA_Carte.
        /// La classe contient les informations de base pour les cartes de jeux et elle est utilisé dans l'éditeur de niveau.
        /// </summary>
        /// <param name="nomCarte">
        /// string du nom de la carte.
        /// </param>
        /// <param name="nbColonne">
        /// int du nombre de colonne de la carte.
        /// </param>
        /// <param name="nbRange">
        /// int du nombre de range de la carte.
        /// </param>
        /// <param name="nbJoueurMin">
        /// Nombre minimum de joueur sur la carte.
        /// </param>
        /// <param name="nbJoueurMax">
        /// Nombre maximum de joueur sur la carte
        /// </param>
        /// <param name="listeTuile">
        /// Liste de TuileEditeur
        /// </param>
        public BBTA_Carte(string nomCarte, int nbColonne, int nbRange, int nbJoueurMin, int nbJoueurMax, List<TuileEditeur> listeTuile)
        {
            this.listeTuile = listeTuile;
            this.InformationCarte = new InfoCarte();
            this.InformationCarte.NomCarte = nomCarte;
            this.InformationCarte.NbColonne = nbColonne;
            this.InformationCarte.NbRange = nbRange;
            this.InformationCarte.NbJoueurMin = nbJoueurMin;
            this.InformationCarte.NbJoueurMax = nbJoueurMax;
        }

        [Serializable]
        public class InfoCarte : Dimension
        {
            public string NomCarte { get; set; }

            public int NbJoueurMin { get; set; }

            public int NbJoueurMax { get; set; }

        }

        [Serializable]
        public class Dimension
        {
            public int NbColonne { get; set; }

            public int NbRange { get; set; }
        }
    }
}
