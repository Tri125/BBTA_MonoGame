using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBTA.Classe.IA.Navigation
{
    public class NavArcGraph : ArcGraph
    {

        public enum drapeau
        {
            normal = 0,
            saut = 1,
            tombe = 2
        }

        public int valDrapeau {get; set;}

        public int IDEntiteIntersection { get; set; }


        public NavArcGraph(int prov, int dest, float cout, int valDrapeau = 0, int id = -1)
            : base(prov, dest, cout)
        {
            this.IDEntiteIntersection = id;
            this.valDrapeau = valDrapeau;
        }
    }
}
