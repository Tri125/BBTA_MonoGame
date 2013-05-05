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
using FarseerPhysics.Common;
using BBTA.Classe.Outils;
using BBTA.Classe.Interface;

namespace BBTA.Elements
{
    public abstract class Acteur : ObjetPhysiqueAnimer
    {
        //Évênements----------------------------------------------------------------------------------------------

        public event EventHandler TirDemande;
        //Variables-----------------------------------------------------------------------------------------------
        private float pointDeVie = 100;
        public float Vies { get { return pointDeVie; } }
        protected const float VITESSE_LATERALE = 1.6f;
        protected const float FORCE_MOUVEMENT_VERTICAL = 4f;
        public bool estAuSol { get; private set; }
        private bool veutSeDeplacer = false;
        public bool monTour = false;
        public bool enModeTir { get; set; }
        public string Nom { get; set; }
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
        public Acteur(World mondePhysique, Texture2D texture, Vector2 position, 
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
            corpsPhysique.Friction = 0;
            enModeTir = false;
            corpsPhysique.OnCollision += new OnCollisionEventHandler(corpsPhysique_OnCollision);
        }

        /*Même fonction Explostil de la classe bloc à la différence près que les acteurs perdent
         * des points de vie au lieu de vérifier le dépassement du seuil de résistance*/
        public void RecevoirDegat(Vector2 lieuExplosion, int rayonExplosion)
        {
            Vector2 direction = Vector2.Subtract(Conversion.MetreAuPixel(corpsPhysique.Position), lieuExplosion);
            if (direction.Length() < rayonExplosion)
            {
                direction.Normalize();
                float distanceExplosion = rayonExplosion / Vector2.Distance(Conversion.MetreAuPixel(corpsPhysique.Position), lieuExplosion);
                corpsPhysique.ApplyLinearImpulse(direction * distanceExplosion);
                pointDeVie -= distanceExplosion*4;
                monTour = false;
                if (pointDeVie < 0)
                {
                    corpsPhysique.Dispose();
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (monTour == true)
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
            base.Update(gameTime);
            
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
        }

        bool corpsPhysique_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (!estAuSol)
            {
                if (fixtureB.Body.UserData is Bloc)
                {
                    if (contact.Manifold.LocalNormal.Y < 0 &&
                       fixtureA.Body.Position.X >= fixtureB.Body.Position.X - (fixtureB.Body.UserData as Bloc).Taille &&
                       fixtureA.Body.Position.X <= fixtureB.Body.Position.X + (fixtureB.Body.UserData as Bloc).Taille)
                    {
                        estAuSol = true;
                    }
                }
                else 
                {
                    return false;
                }
            }
            if (monTour == false && corpsPhysique.LinearVelocity.Length() != 0 && contact.Manifold.LocalPoint.Y < 0)
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