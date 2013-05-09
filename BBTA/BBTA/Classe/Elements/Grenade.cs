using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using BBTA.Outils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using System.Timers;

namespace BBTA.Elements
{
    /// <summary>
    /// Une grande dans BBTA est un dispositif explosif qui explose après un certain délai.
    /// </summary>
    public class Grenade:Projectile
    {
        //Constantes----------------------------------------------------------------------------------
        public const int RAYON_EXPLOSION = 4;

        //Variables reliées au processus d'explosion de la grenade------------------------------------
        private Timer compteReboursExplosion = new Timer(4000);
        private bool estAusol = false;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="mondePhysique">Monde physique Farseer dans lequel évoluera la grenade</param>
        /// <param name="positionSpriteSheet">Position et taille de l'image de la grenade dans la spritesheet</param>
        /// <param name="texture">Texture qui sera affichée à l'écran</param>
        /// <param name="positionDepart">Position initiale de la grenade lorsqu'elle est lancée</param>
        /// <param name="vitesse">Vitesse de la grenade</param>
        public Grenade(World mondePhysique, Texture2D texture, Rectangle positionSpriteSheet, Vector2 positionDepart, Vector2 vitesse)
            : base(mondePhysique, new CircleShape(Conversion.PixelAuMetre(9), 5), texture, positionSpriteSheet, positionDepart, vitesse, RAYON_EXPLOSION)
        {
            corpsPhysique.Restitution = 0.5f; //Coefficient de rebond
            corpsPhysique.Rotation = Conversion.ValeurAngle(vitesse);  // L'angle de la grande jusqu'à ce qu'elle touche un objet
            if (corpsPhysique.Rotation > MathHelper.PiOver2 && corpsPhysique.Rotation < MathHelper.PiOver2 * 3)
            {
                retourner = SpriteEffects.FlipVertically;
            }
            compteReboursExplosion.Start(); //Compte à rebours avant l'explosion
            compteReboursExplosion.Elapsed += new ElapsedEventHandler(compteReboursExplosion_Elapsed);
            corpsPhysique.OnCollision += new OnCollisionEventHandler(corpsPhysique_OnCollision);
            corpsPhysique.OnSeparation += new OnSeparationEventHandler(corpsPhysique_OnSeparation);
        }

        /// <summary>
        /// Met à jour la grenade. Ralenti la grenade lorsqu'elle est au sol.
        /// </summary>
        /// <param name="gameTime">Temps du jeu</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //Ralentissement de la grenade au sol.
            if (estAusol)
            {
                corpsPhysique.LinearDamping = 2;
            }
        }


        /// <summary>
        /// Lorsque la grenade cesse d'être en contact avec le sol, le jeu cesse de la ralentir.
        /// Une variable est modifiée pour indiquer cet état de fait.
        /// </summary>
        /// <param name="fixtureA">Fixture Farseer de la grenade</param>
        /// <param name="fixtureB">Fixture de l'objet qu'elle touche</param>
        void corpsPhysique_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            estAusol = false;
        }

        /// <summary>
        /// Lorsqu'elle entre en collision avec le sol, le jeu ralentit à partir de cet instant la grenade.
        /// En somme, cette fonction remplace la propriété Friction du moteur Farseer qui n'est pas utilisée par le jeu pour
        /// empêcher les joueurs de "coller" aux parois.
        /// De plus, un moment de force est donné à la grenade pour lui donner une certaine vitesse angulaire et ainsi améliorer le réalisme.
        /// </summary>
        /// <param name="fixtureA">Fixture Farseer de la grenade.</param>
        /// <param name="fixtureB">Fixture Farseer de l'objet en collision.</param>
        /// <param name="contact">Données Farseer de la collision.</param>
        /// <returns></returns>
        bool corpsPhysique_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            estAusol = true;
            corpsPhysique.ApplyTorque(1);
            return true; //La collision a bel et bien lieu.
        }

        /// <summary>
        /// Lorsque le compte à rebours vient à échéance, la grenade explose.
        /// Une variable est mise à jour pour indiquer cet état de fait à la classe lors de la phase de mise à jour ("Update")
        /// </summary>
        /// <param name="sender">Compte à rebours</param>
        /// <param name="e"></param>
        void compteReboursExplosion_Elapsed(object sender, ElapsedEventArgs e)
        {
            explose = true;
        }
    }
}
