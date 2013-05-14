using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using BBTA.Outils;
using BBTA.Interface;

namespace BBTA.Elements
{
    /// <summary>
    /// Un acteur est en effet un joueur dans BBTA.  
    /// Cette classe sert de base aux classes gérants les joueurs humains et artificiels. 
    /// </summary>
    public abstract class Acteur : ObjetPhysiqueAnimer
    {
        //Variables reliées au nombre de points de vie restant----------------------------------------------------
        private float pointDeVie = 100;

        //Variables permettant de déterminer l'état dans lequel il est et l'exécution ou non de certaines actions-
        private bool veutSeDeplacer = false;
        private Vector2 vieillePosition;
        private bool seDeplace = false;
        public bool monTour = false;

        //Constantes----------------------------------------------------------------------------------------------
        private const float DENSITE = 1;
        protected const float VITESSE_LATERALE = 1.6f;
        protected const float FORCE_MOUVEMENT_VERTICAL = 4f;

        //Propriétés----------------------------------------------------------------------------------------------
        public bool enModeTir { get; set; }
        public string Nom { get; set; }
        public bool estAuSol { get; private set; }
        public float Vies { get { return pointDeVie; } }

        //Évênements----------------------------------------------------------------------------------------------
        public event EventHandler TirDemande;


        /// <summary>
        /// Constructeur de base pour la classe acteur
        /// L'objet acteur est pris en charge comme étant un rectangle définit par les dimensions de sa texture d'un seul frame
        /// </summary>
        /// <param name="mondePhysique">Monde physique Farseer dans lequel l'acteur évoluera</param>
        /// <param name="texture">Texture de l'objet qui sera affichée à l'écran </param>
        /// <param name="position">Position initiale du joueur</param>
        /// <param name="nbColonnes">Nombre d'images sur l'axe horizontal dans la spritesheet</param>
        /// <param name="nbRangees">Nombre d'images sur l'axe vertical dans la spritesheet</param>
        /// <param name="milliSecParImage">Délai entre l'affichage de chaque image</param>
        public Acteur(World mondePhysique, Texture2D texture, Vector2 position, 
                      int nbColonnes, int nbRangees, int milliSecParImage = 50)
            : base(mondePhysique, new CircleShape(0.41f, DENSITE), texture, nbColonnes, nbRangees, milliSecParImage)
        {
            estAuSol = true;
            corpsPhysique.CollisionCategories = Category.Cat1;
            corpsPhysique.CollidesWith = Category.All & ~Category.Cat1; //Il n'y a pas de collisions entre les joueurs.
            corpsPhysique.Position = position;
            corpsPhysique.BodyType = BodyType.Dynamic;
            corpsPhysique.FixedRotation = true;
            corpsPhysique.Restitution = 0f;
            corpsPhysique.SleepingAllowed = false;
            corpsPhysique.Friction = 0;
            enModeTir = false;
            corpsPhysique.OnCollision += new OnCollisionEventHandler(corpsPhysique_OnCollision);
        }

        /// <summary>
        /// Met à jour l'acteur dépendemment du fait que c'est son tour ou non.  Le rend immobile s'il ne désire plus se déplacer
        /// </summary>
        /// <param name="gameTime">Temps du jeu</param>
        public override void Update(GameTime gameTime)
        {
            seDeplace = false;
            if (vieillePosition != ObtenirPosition())
            {
                seDeplace = true;
            }
            vieillePosition = ObtenirPosition();
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

        /// <summary>
        /// Permet de déplacer le joueur vers la droite à l'aide du moteur Farseer.  Ajuste l'affichage en conséquence.
        /// </summary>
        protected void BougerADroite()
        {
            corpsPhysique.LinearVelocity = new Vector2(VITESSE_LATERALE, corpsPhysique.LinearVelocity.Y);
            veutSeDeplacer = true;
            effet = SpriteEffects.FlipHorizontally;
        }

        /// <summary>
        /// Permet de déplacer le joueur vers la gauche à l'aide du moteur Farseer.  Ajuste l'affichage en conséquence.
        /// </summary>
        protected void BougerAGauche()
        {          
            corpsPhysique.LinearVelocity = new Vector2(-VITESSE_LATERALE, corpsPhysique.LinearVelocity.Y);
            veutSeDeplacer = true;
            effet = SpriteEffects.None;          
        }

        /// <summary>
        /// Permet à l'acteur de sauter à l'aide du moteur Farseer. Change la variable "estAuSol" pour indiquer qu'il est dans les airs.
        /// </summary>
        protected void Sauter()
        {
            estAuSol = false;
            corpsPhysique.ApplyLinearImpulse(new Vector2(0, -FORCE_MOUVEMENT_VERTICAL));
        }

        /// <summary>
        /// Lorsque l'objet entre en collision avec un autre objet dans le monde physique de Farseer
        /// </summary>
        /// <param name="fixtureA">Joint Farseer de l'acteur</param>
        /// <param name="fixtureB">Joint Farseer de l'objet en collision avec l'acteur</param>
        /// <param name="contact">Informations sur la collison</param>
        /// <returns></returns>
        bool corpsPhysique_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (!estAuSol) //S'il est dans les airs de manière volontaire
            {
                if (fixtureB.Body.UserData is Bloc) //Si l'objet avec lequel l'acteur est en collision est un Bloc
                {
                    //Alors on s'assure que le bloc est sous le joueur
                    if (contact.Manifold.LocalNormal.Y < 0 &&
                       fixtureA.Body.Position.X >= fixtureB.Body.Position.X - (fixtureB.Body.UserData as Bloc).Taille/2 &&
                       fixtureA.Body.Position.X <= fixtureB.Body.Position.X + (fixtureB.Body.UserData as Bloc).Taille/2)
                    {
                        estAuSol = true;
                    }
                }
            }
            //S'il est propulsé par une explosion, il cesse de bouger lorsqu'il touche au sol.
            if (monTour == false && corpsPhysique.LinearVelocity.Length() != 0 && contact.Manifold.LocalPoint.Y < 0)
            {
                corpsPhysique.LinearVelocity = Vector2.Zero;
            }
            return true;
        }

        /// <summary>
        /// Déclanche un événement annonçant un tir et en immobilisant le joueur jusqu'à ce que sa phase de tir soit terminée
        /// </summary>
        protected void Tirer()
        {
            if (!seDeplace)
            {
                TirDemande(this, new EventArgs());
                enModeTir = true;
            }
            else
            {
                Console.WriteLine("Acteur::Tirer() : Acteur est en déplacement, ne peux pas tirer.");
            }
        }

        /// <summary>
        /// Permet d'affecter des dégats et un impulsion à l'acteur s'il est suffisemment près de la source d'explosion.
        /// </summary>
        /// <param name="lieuExplosion">Coordonnées de l'explosion (en pixel)</param>
        /// <param name="rayonExplosion">Rayon de l'explosion (en mètres)</param>
        public void RecevoirDegat(Vector2 lieuExplosion, int rayonExplosion)
        {
            Vector2 direction = Vector2.Subtract(Conversion.MetreAuPixel(corpsPhysique.Position), lieuExplosion); //Détermine la distance entre l'explosion et l'acteur et son orientation
            //Si l'acteur est suffisemment proche
            if (direction.Length() < rayonExplosion)
            {
                direction.Normalize();
                //Détermine le ratio entre le rayon maximal et la distance de l'acteur
                float distanceExplosion = rayonExplosion / Vector2.Distance(Conversion.MetreAuPixel(corpsPhysique.Position), lieuExplosion);
                corpsPhysique.ApplyLinearImpulse(direction * distanceExplosion); //Un impulsion est appliquée
                pointDeVie -= distanceExplosion * 50; //Des points de vie sont perdus.
                monTour = false;
                if (pointDeVie < 0)
                {
                    corpsPhysique.Dispose(); //Le corps physique est supprimé du monde physique, ce qui aura pour conséquence de tuer le joueur.
                }
            }
        }

    }
}