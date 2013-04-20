using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBTA.Classe.IA.Navigation
{
    public class Graphe
    {
        //Liste des noeuds fesant partie du graphe
        private List<NavNoeudGraph> NoeudGraphe;

        private LinkedList<NavArcGraph> LienArc;

        //Une liste d'arc adjacent. L'index d'un noeud est relié avec les arcs de cette liste
        private List<LinkedList<NavArcGraph>> ArcAdjacent;
        //Est-ce un graphe orienté?
        private bool graphe_orienter;
        //L'index du prochain noeud à être rajouté dans la liste
        private int prochainIndex;


        //-------------------------------- UniqueEdge ----------------------------
        //
        //  returns true if the edge is not present in the graph. Used when adding
        //  edges to prevent duplication
        //------------------------------------------------------------------------
        private bool ArcUnique(int debut, int fin)
        {
            foreach (var item in ArcAdjacent[debut])
            {
                if (item.IndexDest == fin)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
