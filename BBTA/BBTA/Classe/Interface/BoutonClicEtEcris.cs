using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BBTA.Interface;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using EventInput;

namespace BBTA.Classe.Interface
{
    public class BoutonClicEtEcris : Bouton
    {
        private ButtonState enAttente;
        private SpriteFont police;
        public KeyEventHandler EcouteTouche;
        public Keys touche;

        public BoutonClicEtEcris(Texture2D texture, Vector2 position, Rectangle? positionDansSpriteSheet, Keys touche, SpriteFont police)
            : base(texture, position, positionDansSpriteSheet)
        {
            this.touche = touche;
            this.police = police;
        }

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

        public override void Draw(SpriteBatch spriteBatch)
        {
            //base.Draw(spriteBatch);
                        //Si l'état est "Attente", alors la texture du bouton est standard. Autrement, la texture du bouton est celle indiquant le clic.
            Rectangle selection = new Rectangle((int)boutonDansSprite.X + (int)enAttente * rectangleCollisionBouton.Width, 
                                                (int)boutonDansSprite.Y, 
                                                (int)rectangleCollisionBouton.Width, 
                                                (int)rectangleCollisionBouton.Height);
            spriteBatch.Draw(texture, rectangleCollisionBouton, selection, Color.White);
            spriteBatch.DrawString(police, touche.ToString(), new Vector2(Position.X + 255, Position.Y + 10), Color.AliceBlue);
        }
    }
}
