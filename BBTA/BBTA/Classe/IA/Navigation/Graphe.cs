using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BBTA.Classe.IA.Navigation
{
    public class Graphe
    {
        //Liste des noeuds fesant partie du graphe
        private List<NavNoeudGraph> NoeudGraphe;

        private LinkedList<NavArcGraph> LienArc;

        //Une liste d'arc adjacent. L'index d'un noeud est relié avec les arcs de cette liste
        private List<LinkedList<NavArcGraph>> ArcAdjacent;
        //is this a directed graph?
        private bool m_bDigraph;

        //the index of the next node to be added
        private int m_iNextNodeIndex;


        //returns true if an edge is not already present in the graph. Used
        //when adding edges to make sure no duplicates are created.
        private bool UniqueEdge(int debut, int fin);

        //iterates through all the edges in the graph and removes any that point
        //to an invalidated node
        private void CullInvalidEdges();


        //returns the node at the given index
        public NavNoeudGraph GetNode(int idx);


        //non const version
        public NavArcGraph GetEdge(int from, int to);


        //retrieves the next free node index
        public int GetNextFreeNodeIndex { get { return m_iNextNodeIndex; } }

        //adds a node to the graph and returns its index
        public int AddNode(NavNoeudGraph node);

        //removes a node by setting its index to invalid_node_index
        public void RemoveNode(int node);

        //Use this to add an edge to the graph. The method will ensure that the
        //edge passed as a parameter is valid before adding it to the graph. If the
        //graph is a digraph then a similar edge connecting the nodes in the opposite
        //direction will be automatically added.
        public void AddEdge(NavArcGraph edge);

        //removes the edge connecting from and to from the graph (if present). If
        //a digraph then the edge connecting the nodes in the opposite direction 
        //will also be removed.
        public void RemoveEdge(int from, int to);

        //sets the cost of an edge
        public void SetEdgeCost(int from, int to, float cost);

        //returns the number of active + inactive nodes present in the graph
        public int NumNodes() { return NoeudGraphe.Count(); }


        //returns the number of active nodes present in the graph (this method's
        //performance can be improved greatly by caching the value)
        public int NumActiveNodes()
        {
            int count = 0;

            for (int n = 0; n < NoeudGraphe.Count; ++n)
            {
                if (NoeudGraphe[n].NumIndex != (int)Navigation.MessageNoeud.index_invalide)
                    ++count;
            }

            return count;
        }

        //returns the total number of edges present in the graph
        public int NumEdges()
        {
            int tot = 0;
            foreach (LinkedList<NavArcGraph> arc in ArcAdjacent)
            {
                tot += arc.Count();
            }
            return tot;
        }

        //returns true if the graph is directed
        bool isDigraph() { return m_bDigraph; }

        //returns true if the graph contains no nodes
        bool isEmpty()
        {
            if (NoeudGraphe.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //returns true if a node with the given index is present in the graph
        bool isNodePresent(int nd);

        //returns true if an edge connecting the nodes 'to' and 'from'
        //is present in the graph
        bool isEdgePresent(int from, int to);

        //methods for loading and saving graphs from an open file stream or from
        //a file name 
        bool Save(string nomFichier);

        bool Load(string nomFichier);

        //clears the graph ready for new node insertions
        void Clear() { m_iNextNodeIndex = 0; NoeudGraphe.Clear(); ArcAdjacent.Clear(); }



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



        //-------------------------------- Save ---------------------------------------
        private bool Sauvegarde(string nomFichier)
        {
            //save the number of nodes
            StreamWriter file = new System.IO.StreamWriter(nomFichier);
            file.WriteLine(NoeudGraphe.Count());
            //iterate through the graph nodes and save them
            foreach (var item in NoeudGraphe)
            {
                file.WriteLine(item);
            }

            //save the number of edges
            file.WriteLine(NumEdges());


            //iterate through the edges and save them
            for (int nodeIdx = 0; nodeIdx < NoeudGraphe.Count(); ++nodeIdx)
            {

                foreach (LinkedList<NavArcGraph> arc in ArcAdjacent)
                {
                    file.WriteLine(arc);
                }
            }
            file.Close();
            return true;
        }



    }
}
