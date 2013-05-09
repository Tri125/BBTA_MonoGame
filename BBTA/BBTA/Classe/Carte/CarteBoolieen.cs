using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BBTA.Classe.Elements;

namespace BBTA.Carte
{
    /// <summary>
    ///La classe est une représentation de la carte de jeu en remplançant les blocs par un BlocBooleen.
    ///Pour déterminer rapidement si à un emplacement le bloc est solide ou non.
    /// </summary>
    public class CarteBoolieen
    {
        //Tableau des identifiants non solide des blocs.
        private int[] ID_BLOC_NON_SOLIDE = { -1 };
        private List<BlocBooleen> donneesBlocs;
        private BlocBooleen[,] donneesTab2D;
        private readonly int largeurCarte;
        private readonly int hauteurCarte;

        public BlocBooleen[,] Tableau2DCarte{ get { return donneesTab2D;}}



        /// <summary>
        /// Constructeur de CarteBoolieen.
        /// La classe transforme l'identifiant d'objet Bloc d'après une liste de filtration.
        /// Pour représenté la carte de jeu en bloc solide ou non solide.
        /// </summary>
        /// <param name="largeurCarte">int de la largeur de la carte</param>
        /// <param name="hauteurCarte">int de la hauteur de la carte</param>
        public CarteBoolieen(int largeurCarte, int hauteurCarte)
        {
            this.largeurCarte = largeurCarte;
            this.hauteurCarte = hauteurCarte;
            this.donneesBlocs = new List<BlocBooleen>(largeurCarte * hauteurCarte);
            this.donneesTab2D = new BlocBooleen[largeurCarte, hauteurCarte];
        }


        /// <summary>
        /// Rajoute un nouveau BlocBooleen dans la liste. Permet de créer un BlocBooleen avec seulement un Vector2.
        /// </summary>
        /// <param name="nouveauBloc">Nouveau bloc à être convertie en BlocBooleen</param>
        /// <param name="position">vecteur de la position du bloc</param>
        public void RajoutBloc(Bloc nouveauBloc, Vector2 position)
        {
            if (donneesBlocs.Count == donneesBlocs.Capacity)
            {
                Console.WriteLine("Erreur CarteBoolieen::RajoutBloc: impossible de rajouter un Bloc, car nous avons atteint la taille pré-définie");
                return;
            }
            //Si la référence du Bloc est null, alors on créer un nouveau BlocBooleen avec la position voulue.
            if (nouveauBloc == null)
            {
                this.donneesBlocs.Add(new BlocBooleen(position));
                return;
            }
            //Si le Bloc a une référence, on rajoute un BlocBooleen créé à partir du Bloc dans la liste.
            this.donneesBlocs.Add(new BlocBooleen(ID_BLOC_NON_SOLIDE, nouveauBloc));
        }


        //public void ChangementSoliditer()
        //{

        //}


        /// <summary>
        /// Transforme la liste interne de BlocBooleen en un tableau en 2 dimension qui est enregistré.
        /// </summary>
        public void TransformationTableau2D()
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
