using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBTA.Classe.IA.Navigation
{
    public enum MessageNoeud
    {
        index_invalide = -1
    }


    public class NoeudGraph
    {
        public int NumIndex { get; set; }

        public NoeudGraph()
        {
            this.NumIndex = (int)MessageNoeud.index_invalide;
        }

        public NoeudGraph(int index)
        {
            this.NumIndex = index;
        }

        protected void AffichageNoeud(NoeudGraph n)
        {
            Console.WriteLine("Index : " + n.NumIndex);
        }

    
    }
}
