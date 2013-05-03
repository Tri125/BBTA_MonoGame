using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BBTA.Interface;

namespace BBTA.Classe.Interface
{
    public class AffichageNom:MenuDeployable
    {
        private SpriteFont police;

        public AffichageNom(SpriteFont police, Texture2D panneau)
            :base(panneau, null, 200)
        {
            this.police = police;
        }

        public void Draw(SpriteBatch spriteBatch, Color couleur, Vector2 position, string vie)
        {
            base.Draw(spriteBatch);
            if (estDeploye == true && estOuvert == true)
            {
                spriteBatch.DrawString(police, vie, new Vector2(position.X - police.MeasureString(vie).X / 2, position.Y - 50), couleur);
            }
        }


    }
}
