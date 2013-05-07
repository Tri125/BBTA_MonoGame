using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace EditeurCarteXNA
{
    [Serializable]
    public class BBTA_Carte
    {

        private List<TuileEditeur> listeTuile = new List<TuileEditeur>();

        [XmlElement("EnTete")]
        public InfoCarte InformationCarte { get; set; }

        public List<TuileEditeur> ListTuile { get { return listeTuile; } }

        public BBTA_Carte()
        {

        }

        public BBTA_Carte(InfoCarte infoCarte, List<TuileEditeur> listeTuile)
        {
            this.InformationCarte = infoCarte;
            this.listeTuile = listeTuile;
        }


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
