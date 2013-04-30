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

        //private LinkedList<NavArcGraph> LienArc;

        //Une liste d'arc adjacent. L'index d'un noeud est relié avec les arcs de cette liste
        private List<LinkedList<NavArcGraph>> ArcAdjacent;
        //is this a directed graph?
        private bool m_bDigraph;

        //the index of the next node to be added
        private int m_iNextNodeIndex;

        public Graphe(bool m_bDigraph)
        {
            this.m_bDigraph = m_bDigraph;
            ArcAdjacent = new List<LinkedList<NavArcGraph>>();
            NoeudGraphe = new List<NavNoeudGraph>();
        }



        //retrieves the next free node index
        public int GetNextFreeNodeIndex { get { return m_iNextNodeIndex; } }


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
        public bool isDigraph() { return m_bDigraph; }

        //returns true if the graph contains no nodes
        public bool isEmpty()
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



        //clears the graph ready for new node insertions
        public void Clear() { m_iNextNodeIndex = 0; NoeudGraphe.Clear(); ArcAdjacent.Clear(); }

        public void RemoveEdges()
        {
            foreach (LinkedList<NavArcGraph> arc in ArcAdjacent)
            {
                arc.Clear();
            }
        }



        //--------------------------- isNodePresent --------------------------------
        //
        //  returns true if a node with the given index is present in the graph
        //--------------------------------------------------------------------------
        public bool isNodePresent(int nd)
        {
            if ((nd >= (int)NoeudGraphe.Count() || (NoeudGraphe[nd].NumIndex == (int)Navigation.MessageNoeud.index_invalide)))
            {
                return false;
            }
            else return true;
        }


        //--------------------------- isEdgePresent --------------------------------
        //
        //  returns true if an edge with the given from/to is present in the graph
        //--------------------------------------------------------------------------
        public bool isEdgePresent(int from, int to)
        {
            if (isNodePresent(from) && isNodePresent(to))
            {
                foreach (NavArcGraph arc in ArcAdjacent[from])
                {
                    if (arc.IndexDest == to)
                    {
                        return true;
                    }
                }
                return false;
            }
            else return false;
        }


        //------------------------------ GetNode -------------------------------------
        //
        //  const and non const methods for obtaining a reference to a specific node
        //----------------------------------------------------------------------------


        //non const version
        public NavNoeudGraph GetNode(int idx)
        {

            if (idx < NoeudGraphe.Count() && idx >= 0 && NoeudGraphe[idx].NumIndex == (int)Navigation.MessageNoeud.index_invalide)
            {
                return NoeudGraphe[idx];
            }
            return null;
        }



        //------------------------------ GetEdge -------------------------------------
        //
        //  const and non const methods for obtaining a reference to a specific edge
        //----------------------------------------------------------------------------

        //non const version
        public NavArcGraph GetEdge(int from, int to)
        {
            if (from < NoeudGraphe.Count() && from >= 0 && NoeudGraphe[from].NumIndex != (int)Navigation.MessageNoeud.index_invalide)
            {
                if (to < NoeudGraphe.Count() && to >= 0 && NoeudGraphe[to].NumIndex != (int)Navigation.MessageNoeud.index_invalide)
                {
                    foreach (NavArcGraph arc in ArcAdjacent[from])
                    {
                        if (arc.IndexDest == to)
                        {
                            return arc;
                        }
                    }
                }
            }
            return null;
        }



        //-------------------------- AddEdge ------------------------------------------
        //
        //  Use this to add an edge to the graph. The method will ensure that the
        //  edge passed as a parameter is valid before adding it to the graph. If the
        //  graph is a digraph then a similar edge connecting the nodes in the opposite
        //  direction will be automatically added.
        //-----------------------------------------------------------------------------
        public void AddEdge(NavArcGraph arc)
        {
            //first make sure the from and to nodes exist within the graph 
            if (arc.IndexProv < m_iNextNodeIndex && arc.IndexDest < m_iNextNodeIndex)
            {
                //make sure both nodes are active before adding the edge
                if ((NoeudGraphe[arc.IndexDest].NumIndex != (int)Navigation.MessageNoeud.index_invalide)
                    && (NoeudGraphe[arc.IndexProv].NumIndex != (int)Navigation.MessageNoeud.index_invalide))
                {

                    //add the edge, first making sure it is unique
                    if (UniqueEdge(arc.IndexProv, arc.IndexDest))
                    {
                        ArcAdjacent[arc.IndexProv].AddLast(arc);
                    }

                    //if the graph is undirected we must add another connection in the opposite
                    //direction
                    if (!m_bDigraph)
                    {
                        //check to make sure the edge is unique before adding
                        if (UniqueEdge(arc.IndexDest, arc.IndexProv))
                        {
                            NavArcGraph NewArc = new NavArcGraph(arc.IndexDest, arc.IndexProv, arc.Cout_traverse, arc.valDrapeau, arc.IDEntiteIntersection);
                            ArcAdjacent[arc.IndexDest].AddLast(NewArc);
                        }
                    }

                }
            }

        }



        //----------------------------- RemoveEdge ---------------------------------
        public void RemoveEdge(int from, int to)
        {
            if (from < NoeudGraphe.Count() && to < NoeudGraphe.Count())
            {
                if (!m_bDigraph)
                {
                    foreach (NavArcGraph arc in ArcAdjacent[to].ToList())
                    {
                        if (arc.IndexDest == from)
                        {
                            ArcAdjacent[to].Remove(arc);
                            //break;
                        }
                    }
                }

                foreach (NavArcGraph arc in ArcAdjacent[from].ToList())
                {
                    if (arc.IndexDest == to)
                    {
                        ArcAdjacent[from].Remove(arc);
                        //break;
                    }
                }
            }
        }



        //-------------------------- AddNode -------------------------------------
        //
        //  Given a node this method first checks to see if the node has been added
        //  previously but is now innactive. If it is, it is reactivated.
        //
        //  If the node has not been added previously, it is checked to make sure its
        //  index matches the next node index before being added to the graph
        //------------------------------------------------------------------------
        public int AddNode(NavNoeudGraph node)
        {
            if (node.NumIndex < NoeudGraphe.Count())
            {
                //make sure the client is not trying to add a node with the same ID as
                //a currently active node
                if (NoeudGraphe[node.NumIndex].NumIndex == (int)Navigation.MessageNoeud.index_invalide)
                {
                    NoeudGraphe[node.NumIndex] = node;
                }

                return m_iNextNodeIndex;
            }

            //make sure the new node has been indexed correctly
            if (node.NumIndex == m_iNextNodeIndex)
            {
                NoeudGraphe.Add(node);
                ArcAdjacent.Add(new LinkedList<NavArcGraph>());
                return m_iNextNodeIndex++;


            }
            return -1;
        }


        //----------------------- CullInvalidEdges ------------------------------------
        //
        //  iterates through all the edges in the graph and removes any that point
        //  to an invalidated node
        //-----------------------------------------------------------------------------
        public void CullInvalidEdges()
        {
            foreach (LinkedList<NavArcGraph> listeArc in ArcAdjacent.ToList())
            {
                foreach (NavArcGraph arc in listeArc.ToList())
                {
                    if (NoeudGraphe[arc.IndexDest].NumIndex == (int)Navigation.MessageNoeud.index_invalide ||
                        NoeudGraphe[arc.IndexProv].NumIndex == (int)Navigation.MessageNoeud.index_invalide)
                    {
                        listeArc.Remove(arc);
                    }
                }
            }

        }


        //------------------------------- RemoveNode -----------------------------
        //
        //  Removes a node from the graph and removes any links to neighbouring
        //  nodes
        //------------------------------------------------------------------------
        public void RemoveNode(int node)
        {
            if (node < NoeudGraphe.Count())
            {
                //set this node's index to invalid_node_index
                NoeudGraphe[node].NumIndex = (int)Navigation.MessageNoeud.index_invalide;
                //if the graph is not directed remove all edges leading to this node and then
                //clear the edges leading from the node
                if (!m_bDigraph)
                {
                    //visit each neighbour and erase any edges leading to this node
                    foreach (NavArcGraph arc in ArcAdjacent[node].ToList())
                    {

                        foreach (NavArcGraph arc2 in ArcAdjacent[arc.IndexDest].ToList())
                        {
                            if (arc2.IndexDest == node)
                            {
                                ArcAdjacent[arc.IndexDest].Remove(arc2);
                                break;
                            }
                        }

                    }

                    //finally, clear this node's edges
                    ArcAdjacent[node].Clear();
                }
            }
            //if a digraph remove the edges the slow way
            else
            {
                CullInvalidEdges();
            }
        }

        //-------------------------- SetEdgeCost ---------------------------------
        //
        //  Sets the cost of a specific edge
        //------------------------------------------------------------------------
        void SetEdgeCost(int from, int to, float NewCost)
        {
            //make sure the nodes given are valid
            if (from < NoeudGraphe.Count() && to < NoeudGraphe.Count())
            {
                //visit each neighbour and erase any edges leading to this node
                foreach (NavArcGraph arc in ArcAdjacent[from])
                {
                    if (arc.IndexDest == to)
                    {
                        arc.Cout_traverse = NewCost;
                        break;
                    }
                }
            }
        }



        //-------------------------------- UniqueEdge ----------------------------
        //
        //  returns true if the edge is not present in the graph. Used when adding
        //  edges to prevent duplication
        //------------------------------------------------------------------------
        private bool UniqueEdge(int from, int to)
        {

            foreach (NavArcGraph arc in ArcAdjacent[from])
            {
                if (arc.IndexDest == to)
                {
                    return false;
                }
            }
            return true;
        }



    }
}
