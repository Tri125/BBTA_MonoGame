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
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using BBTA.Classe.Outils;

namespace BBTA.Elements
{
    public abstract class Acteur : ObjetPhysiqueAnimer
    {
        //Évênements----------------------------------------------------------------------------------------------
        public event EventHandler TourCompleter;
        public event EventHandler Mort;
        public event EventHandler TirDemande;
        //Variables-----------------------------------------------------------------------------------------------
        private float pointDeVie = 100;
        protected const float VITESSE_LATERALE = 3f;
        protected const float FORCE_MOUVEMENT_VERTICAL = 8f;
        public bool estAuSol { get; private set; }
        private bool veutSeDeplacer = false;
        public bool monTour = false;
        public bool enModeTir { get; set; }
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
            : base(mondePhysique, new CircleShape(0.42f, DENSITE), texture, nbColonnes, nbRangees, milliSecParImage)
        {
            estAuSol = true;
            corpsPhysique.CollisionCategories = Category.Cat1;
            corpsPhysique.CollidesWith = Category.All & ~Category.Cat1;
            corpsPhysique.Position = position;
            corpsPhysique.BodyType = BodyType.Dynamic;
            corpsPhysique.FixedRotation = true;
            corpsPhysique.Restitution = 0f;
            corpsPhysique.SleepingAllowed = false;
            enModeTir = false;
            corpsPhysique.OnCollision += new OnCollisionEventHandler(corpsPhysique_OnCollision);
        }

        protected void CompletionTour()
        {
            if (monTour == true)
            {
                if (TourCompleter != null)
                {
                    TourCompleter(this, EventArgs.Empty);
                }
            }
        }

        /*Même fonction Explostil de la classe bloc à la différence près que les acteurs perdent
         * des points de vie au lieu de vérifier le dépassement du seuil de résistance*/
        public void RecevoirDegat(Vector2 lieuExplosion, int rayonExplosion)
        {
            Vector2 direction = Vector2.Subtract(Conversion.MetreAuPixel(corpsPhysique.Position), lieuExplosion);
            if (direction.Length() < rayonExplosion)
            {
                direction.Normalize();
                corpsPhysique.ApplyLinearImpulse(direction * rayonExplosion / Vector2.Distance(Conversion.MetreAuPixel(corpsPhysique.Position), lieuExplosion));
            }
        }

        public override void Update(GameTime gameTime)
        {
            corpsPhysique.Friction = 0;
            if (veutSeDeplacer == true && estAuSol == true)
            {
                Animer(gameTime, 0, 3);
            }

            if (veutSeDeplacer == false)
            {
                corpsPhysique.LinearVelocity = new Vector2(0, corpsPhysique.LinearVelocity.Y);
            }
            else
            {
                veutSeDeplacer = false;
            }
        }

        protected void BougerADroite()
        {
            if (monTour == true)
            {
                corpsPhysique.LinearVelocity = new Vector2(VITESSE_LATERALE, corpsPhysique.LinearVelocity.Y);
                veutSeDeplacer = true;
                effet = SpriteEffects.FlipHorizontally;
            }
        }

        protected void BougerAGauche()
        {
            if (monTour == true)
            {
                corpsPhysique.LinearVelocity = new Vector2(-VITESSE_LATERALE, corpsPhysique.LinearVelocity.Y);
                veutSeDeplacer = true;
                effet = SpriteEffects.None;
            }

        }

        protected void Sauter()
        {
            if (monTour == true)
            {
                estAuSol = false;
                corpsPhysique.ApplyLinearImpulse(new Vector2(0, -FORCE_MOUVEMENT_VERTICAL));
            }
        }

        bool corpsPhysique_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (!estAuSol)
            {
                if (contact.Manifold.LocalPoint.Y == -0.5f && contact.Manifold.LocalPoint.X == 0)
                {
                    estAuSol = true;
                }
            }
            else
            {
                corpsPhysique.LinearVelocity = Vector2.Zero;
            }
            return true;
        }

        protected void Tirer()
        {
            TirDemande(this, new EventArgs());
            enModeTir = true;
        }

    }
}