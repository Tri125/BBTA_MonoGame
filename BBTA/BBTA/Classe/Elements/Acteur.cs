﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BBTA.Elements;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;

namespace BBTA.Classe.Elements
{
    public class Acteur:SpriteAnimer
    {
        //Variables-----------------------------------------------------------------------------------------------
        private Body corpsPhysique;        
        private float pointDeVie = 100;

        private KeyboardState previousKeyboardState;
        private KeyboardState currentKeyboardState;

        //Constantes----------------------------------------------------------------------------------------------
        private const float DENSITE = 0;

        /// <summary>
        /// Constructeur de base pour la classe acteur
        /// *****Modifications possibles si nécessaires*****
        /// L'objet acteur est pris en charge comme étant un rectangle définit par les dimensions de sa texture d'un seul frame
        /// </summary>
        /// <param name="mondePhysique"></param>
        /// <param name="pointDeVie"></param>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="vitesse"></param>
        /// <param name="nbColonnes"></param>
        /// <param name="nbRangees"></param>
        /// <param name="milliSecParImage"></param>
        public Acteur(World mondePhysique, float pointDeVie, Texture2D texture, Vector2 position, Vector2 vitesse,
                        int nbColonnes, int nbRangees, int milliSecParImage = 50)
            : base(texture, position, vitesse, nbColonnes, nbRangees, milliSecParImage)
        {
            corpsPhysique = BodyFactory.CreateRectangle(mondePhysique, largeur, hauteur, DENSITE, Position);
            corpsPhysique.IsStatic = true;
            corpsPhysique.Friction = 0.3f;
        }

        /*Même fonction Explostil de la classe bloc à la différence près que les acteurs perdent
         * des points de vie au lieu de vérifier le dépassement du seuil de résistance*/
        void RecevoirDegat(float puissance, float rayon, Vector2 lieu)
        {
            float pente = -puissance / rayon;
            float distance = Vector2.Distance(lieu, Position);

            this.pointDeVie -= pente * distance + puissance;
        }

        /*Détermine si un acteur est mort*/
        bool KillMe()
        {
            if (pointDeVie <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
