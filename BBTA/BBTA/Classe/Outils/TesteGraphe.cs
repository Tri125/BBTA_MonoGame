using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BBTA.Classe.IA.Navigation;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace BBTA.Classe.Outils
{
    public class TesteGraphe
    {
        public Graphe Graph;
        Stopwatch timePerParse;
        long temps;
        public TesteGraphe()
        {
            timePerParse = Stopwatch.StartNew();
            //Graph = new Graphe(false);
            //Graph.AddNode(new NavNoeudGraph(0, new Vector2(0, 0)));
            //Graph.AddNode(new NavNoeudGraph(1, new Vector2(1, 0)));
            //Graph.AddNode(new NavNoeudGraph(2, new Vector2(2, 0)));
            //Graph.AddNode(new NavNoeudGraph(3, new Vector2(3, 0)));
            //Graph.AddNode(new NavNoeudGraph(4, new Vector2(4, 0)));
            //Graph.AddNode(new NavNoeudGraph(5, new Vector2(5, 0)));
            //Graph.AddNode(new NavNoeudGraph(6, new Vector2(6, 0)));
            //Graph.AddNode(new NavNoeudGraph(7, new Vector2(7, 0)));
            //Graph.AddNode(new NavNoeudGraph(8, new Vector2(8, 0)));
            //Graph.AddNode(new NavNoeudGraph(9, new Vector2(9, 0)));
            //Graph.AddNode(new NavNoeudGraph(10, new Vector2(10, 0)));

            //Graph.AddEdge(new NavArcGraph(0, 1, 1));
            //Graph.AddEdge(new NavArcGraph(1, 2, 1));
            //Graph.AddEdge(new NavArcGraph(2, 3, 1));
            //Graph.AddEdge(new NavArcGraph(3, 5, 1));
            //Graph.AddEdge(new NavArcGraph(5, 4, 1));
            //Graph.AddEdge(new NavArcGraph(4, 6, 1));
            //Graph.AddEdge(new NavArcGraph(6, 7, 1));
            //Graph.AddEdge(new NavArcGraph(7, 5, 1));
            //Graph.AddEdge(new NavArcGraph(3, 8, 1));
            //Graph.AddEdge(new NavArcGraph(8, 9, 1));
            //Graph.AddEdge(new NavArcGraph(9, 1, 100));
            //Graph.AddEdge(new NavArcGraph(9, 10, 1));
            //Graph.AddEdge(new NavArcGraph(1, 10, 1));

            //NavArcGraph arc = Graph.GetEdge(1, 9);
            //int i = Graph.GetNextFreeNodeIndex;
            //bool b = Graph.isDigraph();

            //bool d = Graph.isEdgePresent(1, 10);
            //bool vide = Graph.isEmpty();

            //bool n = Graph.isNodePresent(10);
            //int edge2 = Graph.NumEdges();
            //Graph.RemoveNode(1);

            //int actif = Graph.NumActiveNodes();
            //int all = Graph.NumNodes();
            //int edge = Graph.NumEdges();
            //Graph.RemoveEdge(9, 10);
            Graph = new Graphe(true);
            Graph.AddNode(new NavNoeudGraph(0, Vector2.Zero));
            Graph.AddNode(new NavNoeudGraph(1, Vector2.Zero));
            Graph.AddNode(new NavNoeudGraph(2, Vector2.Zero));
            Graph.AddNode(new NavNoeudGraph(3, Vector2.Zero));
            Graph.AddNode(new NavNoeudGraph(4, Vector2.Zero));
            Graph.AddNode(new NavNoeudGraph(5, Vector2.Zero));


            Graph.AddEdge(new NavArcGraph(0, 4, 2.9f));
            Graph.AddEdge(new NavArcGraph(4, 1, 1.9f));
            Graph.AddEdge(new NavArcGraph(1, 2, 3.1f));
            Graph.AddEdge(new NavArcGraph(2, 4, 0.8f));
            Graph.AddEdge(new NavArcGraph(3, 2, 3.7f));
            Graph.AddEdge(new NavArcGraph(5, 3, 1.1f));
            Graph.AddEdge(new NavArcGraph(4, 5, 3.0f));
            Graph.AddEdge(new NavArcGraph(0, 5, 1.0f));


            timePerParse.Stop();
            temps = timePerParse.ElapsedMilliseconds;



            Console.WriteLine(temps);
            Console.WriteLine("J'aime les tacos");
        }
    }
}
