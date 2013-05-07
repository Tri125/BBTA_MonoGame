using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BBTA.Elements;

namespace BBTA.Carte
{
    public class BlocBooleen
    {
        private Vector2 position;
        private bool estSolide;


        public BlocBooleen()
        {
        }

        public BlocBooleen(Vector2 position)
        {
            this.estSolide = false;
            this.position = position;
        }

        public BlocBooleen(int[] idNonSolide, Bloc blocJeu)
        {
            this.position = blocJeu.Position;
            this.estSolide = DeterminerSoliditer(idNonSolide, blocJeu.Type);
        }

        public bool DeterminerSoliditer(int[] idNonSolide, TypeBloc typeBloc)
        {
            foreach (int identifiant in idNonSolide)
            {
                if ((int)typeBloc == identifiant)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
