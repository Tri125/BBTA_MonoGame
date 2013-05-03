using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BBTA.Elements;

namespace BBTA.Classe.Carte
{
    public class BlocBooleen
    {
        const int[] ID_BLOC_NON_SOLIDE = { -1, 0 };
        private Vector2 position;
        private bool estSolide;


        public BlocBooleen()
        {

        }

        public BlocBooleen(Bloc blocJeu)
        {
            this.position = blocJeu.Position;

        }

        public bool DeterminerSoliditer(TypeBloc typeB)
        {
            return false;
        }
    }
}
