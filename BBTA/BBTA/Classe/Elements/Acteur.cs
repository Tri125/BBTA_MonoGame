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
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;

namespace BBTA.Elements
{
    public abstract class Acteur:ObjetPhysiqueAnimer
    {
        //Variables-----------------------------------------------------------------------------------------------
        private float pointDeVie = 100;
        protected const float VITESSE_LATERALE = 5f;
        protected const float FORCE_MOUVEMENT_VERTICAL = 6f;
        public bool estAuSol { get; private set; }
        private bool veutSeDeplacer = false;

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
            corpsPhysique.Position = position; 
            corpsPhysique.BodyType = BodyType.Dynamic;
            corpsPhysique.FixedRotation = true;
            corpsPhysique.Restitution = 0f;
            corpsPhysique.Friction = 0;
        }

        /*Même fonction Explostil de la classe bloc à la différence près que les acteurs perdent
         * des points de vie au lieu de vérifier le dépassement du seuil de résistance*/
        public void RecevoirDegat(float puissance, float rayon, Vector2 lieu)
        {
            float pente = -puissance / rayon;
            float distance = Vector2.Distance(lieu, corpsPhysique.Position);

            this.pointDeVie -= pente * distance + puissance;
        }

        /*Détermine si un acteur est mort*/
        private bool KillMe()
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

        public override void Update(GameTime gameTime)
        {
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
            corpsPhysique.LinearVelocity = new Vector2(VITESSE_LATERALE, corpsPhysique.LinearVelocity.Y);
            veutSeDeplacer = true;
            effet = SpriteEffects.FlipHorizontally;
        }

        protected void BougerAGauche()
        {
            corpsPhysique.LinearVelocity = new Vector2(-VITESSE_LATERALE, corpsPhysique.LinearVelocity.Y);
            veutSeDeplacer = true;
            effet = SpriteEffects.None;

        }

        protected void Sauter()
        {
            estAuSol = false;
            corpsPhysique.ApplyLinearImpulse(new Vector2(0, -FORCE_MOUVEMENT_VERTICAL));
            corpsPhysique.OnCollision += new OnCollisionEventHandler(corpsPhysique_OnCollision);
        }

        bool corpsPhysique_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (contact.Manifold.LocalPoint.Y == -0.5f && contact.Manifold.LocalPoint.X == 0)
            {
                estAuSol = true;
            }
            return true;
        }

        public Vector2 ObtenirPosition()
        {
            return corpsPhysique.Position * 40;
        }
    }
}
