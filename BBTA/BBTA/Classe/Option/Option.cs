using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Input;

namespace BBTA.Classe.Option
{
    [Serializable]
    public class Option
    {
        private InfoSonore informationSonore = new InfoSonore();
        private ParametreTouche informationTouche = new ParametreTouche();

        [XmlElement("Son")]
        public InfoSonore InformationSonore { get { return informationSonore; } set { informationSonore = value; } }

        [XmlElement("ToucheJeu")]
        public ParametreTouche InformationTouche { get { return informationTouche; } set { informationTouche = value; } }

        public Option()
        {
        }


        public Option(InfoSonore paramSon, ParametreTouche paramTouche)
        {
            this.InformationSonore = paramSon;
            this.InformationTouche = paramTouche;
        }


        [Serializable]
        public class InfoSonore
        {
            public int musique { get; set; }
            public int effetSonore { get; set; }
        }

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
