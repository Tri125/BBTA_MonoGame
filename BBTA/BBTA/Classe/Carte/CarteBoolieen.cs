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
        private int[] ID_BLOC_NON_SOLIDE = { -1 };
        private List<BlocBooleen> donneesBlocs;
        private BlocBooleen[,] donneesTab2D;
        private readonly int largeurCarte;
        private readonly int hauteurCarte;

        public CarteBoolieen()
        {
        }

        public CarteBoolieen(int largeurCarte, int hauteurCarte)
        {
            this.largeurCarte = largeurCarte;
            this.hauteurCarte = hauteurCarte;
            this.donneesBlocs = new List<BlocBooleen>(largeurCarte * hauteurCarte);
            this.donneesTab2D = new BlocBooleen[largeurCarte, hauteurCarte];
        }

        public void RajoutBloc(Bloc nouveauBloc, Vector2 position)
        {
            if (nouveauBloc == null)
            {
                this.donneesBlocs.Add(new BlocBooleen(position));
                return;
            }
            this.donneesBlocs.Add(new BlocBooleen(ID_BLOC_NON_SOLIDE, nouveauBloc));
        }


        public void ChangementSoliditer()
        {

        }

        private void TransformationTableau2D()
        {
            if (donneesBlocs.Count() == donneesTab2D.Length)
            {
                int rangee = 1;
                int colonne = 1;
                foreach (BlocBooleen bloc in donneesBlocs)
                {
                    donneesTab2D[rangee - 1, colonne - 1] = bloc;
                    rangee++;
                    if ((rangee % (largeurCarte + 1)) == 0)
                    {
                        colonne++;
                        rangee = 1;
                    }
                }
            }
            else
            {
                Console.WriteLine("Erreur CarteBoolieen::TransformationTableau2D: Taille de donneesBlocs plus grande que donneesTab2D");
            }
        }




    }
}
