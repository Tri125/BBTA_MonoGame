using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BBTA.Interface;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BBTA.Classe.Interface
{
    public class BoutonClicEtEcris : Bouton
    {
        private String touche;
        private SpriteFont police;

        public BoutonClicEtEcris(Texture2D texture, Vector2 position, Rectangle? positionDansSpriteSheet, string touche, SpriteFont police)
            : base(texture, position, positionDansSpriteSheet)
        {
            this.touche = touche;
            this.police = police;
        }
    }
}
