using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BBTA.Classe.IA.Navigation;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace BBTA.Classe.Outils
{
    public class TesteGraphe
    {
        public Graphe Graph;
        public TesteGraphe()
        {
            Graph = new Graphe(false);
            Graph.AddNode(new NavNoeudGraph(0, new Vector2(0, 0)));
            Graph.AddNode(new NavNoeudGraph(1, new Vector2(1, 0)));
            Graph.AddNode(new NavNoeudGraph(2, new Vector2(2, 0)));
            Graph.AddNode(new NavNoeudGraph(3, new Vector2(3, 0)));
            Graph.AddNode(new NavNoeudGraph(4, new Vector2(4, 0)));
            Graph.AddNode(new NavNoeudGraph(5, new Vector2(5, 0)));
            Graph.AddNode(new NavNoeudGraph(6, new Vector2(6, 0)));
            Graph.AddNode(new NavNoeudGraph(7, new Vector2(7, 0)));
            Graph.AddNode(new NavNoeudGraph(8, new Vector2(8, 0)));
            Graph.AddNode(new NavNoeudGraph(9, new Vector2(9, 0)));
            Graph.AddNode(new NavNoeudGraph(10, new Vector2(10, 0)));

            Graph.AddEdge(new NavArcGraph(0, 1, 1));
            Graph.AddEdge(new NavArcGraph(1, 2, 1));
            Graph.AddEdge(new NavArcGraph(2, 3, 1));
            Graph.AddEdge(new NavArcGraph(3, 5, 1));
            Graph.AddEdge(new NavArcGraph(5, 4, 1));
            Graph.AddEdge(new NavArcGraph(4, 6, 1));
            Graph.AddEdge(new NavArcGraph(6, 7, 1));
            Graph.AddEdge(new NavArcGraph(7, 5, 1));
            Graph.AddEdge(new NavArcGraph(3, 8, 1));
            Graph.AddEdge(new NavArcGraph(8, 9, 1));
            Graph.AddEdge(new NavArcGraph(9, 1, 1));
            Graph.AddEdge(new NavArcGraph(9, 11, 1));
            Graph.AddEdge(new NavArcGraph(1, 10, 1));
            Console.WriteLine("J'aime les tacos");
        }
    }
}
