using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using EventInput;

namespace BBTA.Classe.Interface
{
    /// <summary>
    /// Ce type de bouton en est un particulier qui affiche une touche.
    /// Il ne gère pas la saisie de touche.  Il ne fait que l'affichée.
    /// </summary>
    public class BoutonClicEtEcris : Bouton
    {
        //Variables d'affichage-------------------------------------------------------------------------------------------------------
        private SpriteFont police;

        //Variables reliées au clavier------------------------------------------------------------------------------------------------
        public KeyEventHandler EcouteTouche;

        //Propriétés reliées au clavier-----------------------------------------------------------------------------------------------
        public Keys Touche { get; set; }

        //Variable reliée à l'état du bouton (lorsqu'il est cliqué et qu'on attend la saisie d'une touche)----------------------------
        private ButtonState enAttente;

        /// <summary>
        /// Cosntructeur
        /// </summary>
        /// <param name="texture">Spritesheet contenant la texture du bouton</param>
        /// <param name="position">La position du bouton à l'écran</param>
        /// <param name="positionDansSpriteSheet">Position de la texture du bouton dans la spritesheet</param>
        /// <param name="touche">Touche en mémoire</param>
        /// <param name="police">Police pour écrire la touche</param>
        public BoutonClicEtEcris(Texture2D texture, Vector2 position, Rectangle? positionDansSpriteSheet, Keys touche, SpriteFont police)
            : base(texture, position, positionDansSpriteSheet)
        {
            this.Touche = touche;
            this.police = police;
        }

        /// <summary>
        /// Met à jour le bouton.  
        /// Change la texture à afficher lorsque l'utlisateur saisit une touche
        /// </summary>
        /// <param name="matriceCamera"></param>
        public override void Update(Matrix? matriceCamera)
        {
            base.Update(matriceCamera);
            if (EcouteTouche != null)
            {
                enAttente = ButtonState.Pressed;
            }
            else
            {
                enAttente = ButtonState.Released;
            }
        }

        /// <summary>
        /// Affiche le bouton à l'écran.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Si l'état est "Attente", alors la texture du bouton est standard. Autrement, la texture du bouton est celle indiquant le clic.
            Rectangle selection = new Rectangle((int)boutonDansSprite.X + (int)enAttente * rectangleCollisionBouton.Width, 
                                                (int)boutonDansSprite.Y, 
                                                (int)rectangleCollisionBouton.Width, 
                                                (int)rectangleCollisionBouton.Height);
            spriteBatch.Draw(texture, rectangleCollisionBouton, selection, Color.White);
            spriteBatch.DrawString(police, Touche.ToString(), new Vector2(Position.X + 255, Position.Y + 10), Color.AliceBlue);
        }
    }
}
