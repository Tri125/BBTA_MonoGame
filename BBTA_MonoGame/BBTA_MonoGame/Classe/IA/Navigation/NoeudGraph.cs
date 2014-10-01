using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBTA.IA
{

    //-----------------------------------------------------------------------------
    //
    //  Name:   GraphNodeTypes.h
    //
    //  Author: Mat Buckland (www.ai-junkie.com)
    //
    //  Desc:   Node classes to be used with graphs
    //-----------------------------------------------------------------------------
    //Adapté de C++ en C# par Tristan Savaria
    //tristan@twisted-ip.com

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
