using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBTA.Classe.IA.Navigation
{
    public class ArcGraph
    {
        public int IndexDest {get; set;}
        public int IndexProv { get; set; }

        public float Cout_traverse { get; set; }

        public ArcGraph(int prov, int dest, float cout = 1.0f)
        {
            this.IndexProv = prov;
            this.IndexDest = dest;
            this.Cout_traverse = cout;
        }

        public ArcGraph()
        {
            this.Cout_traverse = 1.0f;
            this.IndexProv = (int)Navigation.MessageNoeud.index_invalide;
        }

        public static bool operator ==(ArcGraph a1, ArcGraph a2)
        {
            if ( (Object)a1 == null && (Object)a2 == null)
            {
                return true;
            }

            if ((Object)a1 != null && (Object)a2 == null)
            {
                return false;
            }

            if ((Object)a1 == null && (Object)a2 != null)
            {
                return false;
            }
           
            return (a1.IndexProv == a2.IndexProv && a1.IndexDest == a2.IndexDest && a1.Cout_traverse == a2.Cout_traverse);
        }

        public static bool operator !=(ArcGraph a1, ArcGraph a2)
        {
            if ((Object)a1 == null && (Object)a2 == null)
            {
                return false;
            }

            if ((Object)a1 != null && (Object)a2 == null)
            {
                return true;
            }

            if ((Object)a1 == null && (Object)a2 != null)
            {
                return true;
            }

            return (a1.IndexProv != a2.IndexProv || a1.IndexDest != a2.IndexDest || a1.Cout_traverse != a2.Cout_traverse);
        }

        protected void Affichage()
        {
            Console.WriteLine("Noeud Provenance : " + this.IndexProv + "Noeud Destination : " + this.IndexDest + "Coût de traverse : " + this.Cout_traverse);
        }


    }
}
