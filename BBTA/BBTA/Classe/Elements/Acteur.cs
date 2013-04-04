using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BBTA.Elements;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;

namespace BBTA.Elements
{
    public abstract class Acteur:ObjetPhysiqueAnimer
    {
        //Variables-----------------------------------------------------------------------------------------------
        private float pointDeVie = 100;

        //Constantes----------------------------------------------------------------------------------------------
        private const float DENSITE = 1;

        /// <summary>
        /// Constructeur de base pour la classe acteur
        /// *****Modifications possibles si nécessaires*****
        /// L'objet acteur est pris en charge comme étant un rectangle définit par les dimensions de sa texture d'un seul frame
        /// </summary>
        /// <param name="mondePhysique"></param>
        /// <param name="pointDeVie"></param>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="nbColonnes"></param>
        /// <param name="nbRangees"></param>
        /// <param name="milliSecParImage"></param>
        public Acteur(World mondePhysique, float pointDeVie, Texture2D texture, Vector2 position, 
                      int nbColonnes, int nbRangees, int milliSecParImage = 50)
            : base(texture, position, nbColonnes, nbRangees, milliSecParImage)
        {
            corpsPhysique = BodyFactory.CreateRectangle(mondePhysique, largeur, hauteur, DENSITE, corpsPhysique.Position);
            corpsPhysique.FixedRotation = true;
            corpsPhysique.Restitution = 0;
            corpsPhysique.Friction = 0.3f;
        }

        /*Même fonction Explostil de la classe bloc à la différence près que les acteurs perdent
         * des points de vie au lieu de vérifier le dépassement du seuil de résistance*/
        void RecevoirDegat(float puissance, float rayon, Vector2 lieu)
        {
            float pente = -puissance / rayon;
            float distance = Vector2.Distance(lieu, corpsPhysique.Position);

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
