using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BBTA.Elements;
using BBTA.Option;

namespace BBTA.Interface
{
    /// <summary>
    /// ViseurVisuel est le viseur du joueur lorsqu'il tente de tirer.
    /// Il tourne selon les touches de déplacement.
    /// C'est un menu déployable.
    /// Il déclanche un événement lorsque le joueur a choisi son angle de tir.
    /// </summary>
    public class ViseurVisuel:MenuDeployable
    {
        //Variables-------------------------------------------------------------------------------------------------------
        private Texture2D texture;
        private float angleRotation;

        //Propriétés------------------------------------------------------------------------------------------------------
        public Armes armeChoisie { get; set; }

        //Événements------------------------------------------------------------------------------------------------------
        public event EventHandler Verouiller;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="texture">Texture qui servira de viseur pour le joueur et sera affichée à l'écran</param>
        public ViseurVisuel(Texture2D texture):base(texture, null, 200)
        {
            this.texture = texture;
            angleRotation = 0;
        }

        /// <summary>
        /// Tourne le viseur selon les touches appuyées.
        /// Déclanche un événement lorsque le joueur a choisi son angle de tir.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            KeyboardState clavier = Keyboard.GetState();

            //On maintient l'angle de rotation entre 0 et 2 pi
            //----------------------------------------------------------------------------------------
            if (clavier.IsKeyDown(Game1.chargeurOption.OptionActive.InformationTouche.Droite))
            {
                angleRotation += MathHelper.ToRadians(3);
            }
            else if (clavier.IsKeyDown(Game1.chargeurOption.OptionActive.InformationTouche.Gauche))
            {
                angleRotation -= MathHelper.ToRadians(3);
            }
            angleRotation %= MathHelper.TwoPi;
            //----------------------------------------------------------------------------------------

            //Lorsque le joueur appuie sur la touche qui vérouille le tir, tel qu'il l'a configuré, le viseur s'immobilise et déclanche un événement pour le faire savoir.
            //Il démarre aussi le processus de fermeture du menu.
            if (clavier.IsKeyDown(Game1.chargeurOption.OptionActive.InformationTouche.Tir))
            {
                if (Verouiller != null)
                {
                    Verouiller(this, new EventArgs());
                }
                estOuvert = false;
            }
        }

        /// <summary>
        /// Permet d'obtenir l'angle dans lequel pointe le viseur sur la forme d'un vecteur en deux dimensions.
        /// </summary>
        /// <returns>Angle</returns>
        public Vector2 ObtenirAngle()
        {
            Vector2 angle = new Vector2((float)Math.Cos(angleRotation), (float)Math.Sin(angleRotation));
            angle.Normalize(); //Le vecteur est désormais unitaire.
            return angle;
        }

        /// <summary>
        /// Affiche le viseur à l'écran.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White * progressionDeploiement, angleRotation, new Vector2(texture.Width / 2f, texture.Height / 2f), progressionDeploiement, SpriteEffects.None, 0);
        }
    }
}
