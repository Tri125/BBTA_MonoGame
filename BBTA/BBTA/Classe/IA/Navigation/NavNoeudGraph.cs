using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BBTA.IA
{

    //-----------------------------------------------------------------------------
    //
    //  Author: Mat Buckland (www.ai-junkie.com)
    //
    //-----------------------------------------------------------------------------
    //Adapté de C++ en C# par Tristan Savaria
    //tristan@twisted-ip.com



    public class NavNoeudGraph : NoeudGraph
    {
        public Vector2 Position { get; set; }

        public NavNoeudGraph(int index, Vector2 pos)
            : base (index)
        {
            this.Position = pos;
        }

        protected void Affichage(NavNoeudGraph n)
        {
            Console.WriteLine("Index : " + n.NumIndex + "PosX : " + n.Position.X + "PosY : " + n.Position.Y);
        }
    }
}
