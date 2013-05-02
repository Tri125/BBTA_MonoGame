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
    public class OutilGraphe : DrawableGameComponent
    {
        public Graphe Graph;
        Stopwatch timePerParse;
        long temps;
        public OutilGraphe(Game jeu)
            :base (jeu)
        {
            timePerParse = Stopwatch.StartNew();
            
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

            Graph_SearchDijkstra recherche = new Graph_SearchDijkstra(ref Graph, 4, 2);
            List<int> chemin = recherche.GetPathToTarget();
            timePerParse.Stop();
            temps = timePerParse.ElapsedMilliseconds;



            Console.WriteLine(temps);
            Console.WriteLine("J'aime les tacos");
        }
    }
}
