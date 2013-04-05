﻿using System;
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
    /// <summary>
    /// La classe contient les blocs composant le relief ainsi que l'arrière-plan de la carte.
    /// -----------------------------------------------------------------------------------------------
    /// Affiche les blocs nécessaire.
    /// Affiche l'arrière-plan.
    /// Gère la destruction des blocs s'il y a lieu.
    /// -----------------------------------------------------------------------------------------------
    /// </summary>
    public class Carte
    {
        //Variables-----------------------------------------------------------------------------------------------
        private Texture2D textureArrierePlan;
        private Bloc[] blocs;
        private int largeur;

        //Constantes----------------------------------------------------------------------------------------------
        private const float TAILLE_BLOC = 1f;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="donneesBlocs">Données sur la nature des blocs</param>
        /// <param name="largeurCarte">Largeur de la carte (en blocs)</param>
        /// <param name="arrierePlan">Arrière-plan de la carte</param>
        /// <param name="textureBlocs">Texture des blocs</param>
        /// <param name="mondePhysique">World Farseer</param>
        /// <param name="MetrePixel">Valeur en pixel d'un metre</param>
        public Carte(int[] donneesBlocs, int largeurCarte, Texture2D arrierePlan, Texture2D textureBlocs, World mondePhysique, float metrePixel)
        {
            this.textureArrierePlan = arrierePlan;
            this.largeur = largeurCarte;
            blocs = new Bloc[donneesBlocs.Length];
            for(int compteurBlocs = 0; compteurBlocs < donneesBlocs.Length; compteurBlocs++)
            {                
                //Par convention, une case avec comme donnée "0" signifie une case vide.  En somme, il n'y a aucun bloc.
                if(donneesBlocs[compteurBlocs] != 0)
                {
                    //Position en mètres
                    Vector2 positionBloc = new Vector2(compteurBlocs%largeurCarte*TAILLE_BLOC+TAILLE_BLOC*0.5f, compteurBlocs/largeurCarte*TAILLE_BLOC+TAILLE_BLOC*0.5f);
                    blocs[compteurBlocs] = new Bloc(mondePhysique, positionBloc, textureBlocs, TAILLE_BLOC, metrePixel);
                }                
            }
        }

        /// <summary>
        /// Détruit les blocs nécessaires suite à une explosion
        /// </summary>Lieu d'origine de l'explosion</param>
        /// <param name="rayon">Rayon de l'explosion</param>
        /// <param name="puissance">Puissance déployée par l'explosion</param>
        public void Explosion(Vector2 lieu, float rayon, float puissance)
        {
            for (int compteurBloc = 0; compteurBloc < blocs.Length; compteurBloc++)
	        {
                if(blocs[compteurBloc].ExplosetIl(puissance, rayon, lieu))
                {
                    //Destruction du bloc
                    blocs[compteurBloc] = null;
                }
	        }
        }

        /// <summary>
        /// Affiche les blocs nécessaires à l'écran
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureArrierePlan, Vector2.Zero, Color.White);
            foreach (Bloc item in blocs)
            {
                if (item != null)
                {
                    item.Draw(spriteBatch);
                }
            }
        }
    }
}
