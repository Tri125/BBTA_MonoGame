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
    /// <summary>
    ///La classe BlocBooleen construit un bool avec une position et un identifiant de type bool représentant si, oui ou non, le bloc est solide.
    /// </summary>
    public class BlocBooleen
    {
        private Vector2 position;
        private bool estSolide;


        /// <summary>
        /// Constructeur de BlocBooleen à partir d'un Vector2
        /// </summary>
        /// <param name="position">La position du BlocBooleen dans la carte</param>
        public BlocBooleen(Vector2 position)
        {
            this.estSolide = false;
            this.position = position;
        }


        /// <summary>
        /// Constructeur de BlocBooleen à partir d'une tableau d'identifiant non solide comme référence et un Bloc.
        /// Crée un Bloc en BlocBooleen.
        /// </summary>
        /// <param name="idNonSolide">Tableau avec les identifiants correspondants à des blocs non solides.</param>
        /// <param name="blocJeu">Bloc à partir du quel on extrait l'identifiant et la position.</param>
        public BlocBooleen(int[] idNonSolide, Bloc blocJeu)
        {
            this.position = blocJeu.Position;
            this.estSolide = DeterminerSoliditer(idNonSolide, blocJeu.Type);
        }


        /// <summary>
        /// Compare une valeure de l'enum TypeBloc à un tableau d'identifiant non solide.
        /// Retourne false si non solide, true si solide.
        /// </summary>
        /// <param name="idNonSolide">Tableau d'identifiant non solide.</param>
        /// <param name="typeBloc">Enum représentant le type du bloc à traduire.</param>
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
