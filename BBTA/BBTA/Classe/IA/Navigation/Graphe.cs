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


        ////iterates through all the edges in the graph and removes any that point
        ////to an invalidated node
        //private void CullInvalidEdges();




        //retrieves the next free node index
        public int GetNextFreeNodeIndex { get { return m_iNextNodeIndex; } }

        ////adds a node to the graph and returns its index
        //public int AddNode(NavNoeudGraph node);

        ////removes a node by setting its index to invalid_node_index
        //public void RemoveNode(int node);

        ////sets the cost of an edge
        //public void SetEdgeCost(int from, int to, float cost);

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




        //public bool Load(string nomFichier);

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
                    foreach (NavArcGraph arc in ArcAdjacent[to])
                    {
                        if (arc.IndexDest == from)
                        {
                            ArcAdjacent[to].Remove(arc);
                            //break;
                        }
                    }
                }

                foreach (NavArcGraph arc in ArcAdjacent[from])
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
//int AddNode(NavNoeudGraph node)
//{
//  if (node.NumIndex < NoeudGraphe.Count())
//  {
//    //make sure the client is not trying to add a node with the same ID as
//    //a currently active node
//    assert (m_Nodes[node.Index()].Index() == invalid_node_index &&
//      "<SparseGraph::AddNode>: Attempting to add a node with a duplicate ID");
    
//    m_Nodes[node.Index()] = node;

//    return m_iNextNodeIndex;
//  }
  
//  else
//  {
//    //make sure the new node has been indexed correctly
//    assert (node.Index() == m_iNextNodeIndex && "<SparseGraph::AddNode>:invalid index");

//    m_Nodes.push_back(node);
//    m_Edges.push_back(EdgeList());

//    return m_iNextNodeIndex++;
//  }
//}







        //-------------------------------- UniqueEdge ----------------------------
        //
        //  returns true if the edge is not present in the graph. Used when adding
        //  edges to prevent duplication
        //------------------------------------------------------------------------
        private bool UniqueEdge(int debut, int fin)
        {
            foreach (NavArcGraph arc in ArcAdjacent[debut])
            {
                if (arc.IndexDest == fin)
                {
                    return false;
                }
            }
            return true;
        }



        //-------------------------------- Save ---------------------------------------
        public bool Save(string nomFichier)
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
