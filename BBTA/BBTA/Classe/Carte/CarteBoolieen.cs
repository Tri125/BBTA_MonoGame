using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BBTA.Elements;

namespace BBTA
{
    public class CarteBoolieen
    {
        private int[] ID_BLOC_NON_SOLIDE = { -1, 0 };
        private  List<BlocBooleen> doneesBlocs;
        private readonly int largeurCarte;
        private readonly int hauteurCarte;

        public CarteBoolieen()
        {
        }

        public CarteBoolieen(int largeurCarte, int hauteurCarte)
        {
            this.largeurCarte = largeurCarte;
            this.hauteurCarte = hauteurCarte;
            this.doneesBlocs = new List<BlocBooleen>(largeurCarte * hauteurCarte);
        }

        public void RajoutBloc(Bloc nouveauBloc, Vector2 position)
        {
            if (nouveauBloc == null)
            {
                this.doneesBlocs.Add(new BlocBooleen(position));
                return;
            }
            this.doneesBlocs.Add(new BlocBooleen (ID_BLOC_NON_SOLIDE, nouveauBloc));
        }


        public void ChangementSoliditer()
        {

        }

        private void TransformationTableau2D()
        {

        }

       


    }
}
