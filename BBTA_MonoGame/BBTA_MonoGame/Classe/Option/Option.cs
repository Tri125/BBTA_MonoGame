using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Input;

namespace BBTA.Option
{
    /// <summary>
    /// La classe contient les informations pour les configurations du jeu de l'utilisateur.
    /// </summary>
    [Serializable]
    public class Option
    {
        private InfoSonore informationSonore = new InfoSonore();
        private ParametreTouche informationTouche = new ParametreTouche();
        //Redéfini le nom de la balise XML. Par défault elle aurait porté le nom de l'attribut situé en dessous.
        [XmlElement("Son")]
        public InfoSonore InformationSonore { get { return informationSonore; } set { informationSonore = value; } }

        [XmlElement("ToucheJeu")]
        public ParametreTouche InformationTouche { get { return informationTouche; } set { informationTouche = value; } }
        /// <summary>
        /// Constructeur de base d'Option.
        /// La classe contient les informations pour les configurations du jeu de l'utilisateur.
        /// </summary>
        public Option()
        {
        }

        /// <summary>
        /// Constructeur d'Option à partir des paramètres de son et des touches de jeu.
        /// La classe contient les informations pour les configurations du jeu de l'utilisateur.
        /// </summary>
        /// <param name="paramSon">Objet des paramètres du volume sonore.</param>
        /// <param name="paramTouche">Objet des paramètres des touches de jeu.</param>
        public Option(InfoSonore paramSon, ParametreTouche paramTouche)
        {
            this.InformationSonore = paramSon;
            this.InformationTouche = paramTouche;
        }

        /// <summary>
        /// La classe InfoSonore contient les attributs pour le volume de la musique et des effets sonores.
        /// La valeur du volume est situé entre 0 et 100.
        /// </summary>
        [Serializable]
        public class InfoSonore
        {
            private int musique;
            private int effetSonore;
            public int Musique { get { return musique; } set { musique = LimiteSonore(value); } }
            public int EffetSonore { get { return effetSonore; } set { effetSonore = LimiteSonore(value); } }

            /// <summary>
            /// Détecte si le volume du son est à l'extérieur des bornes [0,100] et rammène la valeure à la plus proche borne.
            /// Retourne un int du volume validé.
            /// </summary>
            private int LimiteSonore(int volume)
            {
                if (volume > 100)
                {
                    volume = 100;
                }
                else if (volume < 0)
                {
                    volume = 0;
                }
                return volume;
            }
        }

        /// <summary>
        /// La classe ParametreTouche contient les Keys des différentes actions possibles dans le jeu.
        /// </summary>
        [Serializable]
        public class ParametreTouche
        {
            public Keys Gauche { get; set; }
            public Keys Droite { get; set; }
            public Keys Saut { get; set; }
            public Keys Tir { get; set; }
            public Keys Pause { get; set; }
        }
    }
}
