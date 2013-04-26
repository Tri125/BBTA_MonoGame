using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BBTA.Classe.IA.Navigation;
using Microsoft.Xna.Framework.Input;

namespace BBTA.Classe.Outils
{
    public class TesteGraphe
    {
        public Graphe Graph;

        public TesteGraphe()
        {
            Graph = new Graphe(false);
        }
    }
}
