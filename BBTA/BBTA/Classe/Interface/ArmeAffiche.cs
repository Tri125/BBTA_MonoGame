using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BBTA.Elements;

namespace BBTA.Interface
{
    /// <summary>
    /// Représente l'arme que le joueur tient dans ses bras lors du tir (ex: un bazooka dans le cas d'une roquette)
    /// Est dessiné selon la valeur de l'angle spécifié dans angleRotation.  
    /// Est un menu déplayable.
    /// </summary>
    public class ArmeAffiche:MenuDeployable
    {
        //Type de l'arme affichée-----------------------------------------------------------------------------------------------
        private Armes type;

        //Variables reliées à l'affichage---------------------------------------------------------------------------------------
        private SpriteEffects tournerLaTexture = SpriteEffects.None;

        //Propriétés------------------------------------------------------------------------------------------------------------
        public float angleRotation { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="texture">Spritesheet où se trouve l'image de l'arme.  Elles doivent êtres toutes sur la même ligne.</param>
        /// <param name="type">Type de l'arme (Grenade, roquette, etc.)</param>
        public ArmeAffiche(Texture2D texture, Armes type):base(texture, new Rectangle(0, (int)type*30, texture.Width, 30), 200)
        {
            this.type = type;
        }

        /// <summary>
        /// Met à jour l'angle de rotation de l'arme et retourne l'image lorsqu'elle pointe vers la droite.
        /// </summary>
        /// <param name="gameTime">Temps de jeu</param>
        public override void Update(GameTime gameTime)
        {
            //Si l'arme pointe dans le deuxième ou troisième quadran, on retourne l'image sur elle-même.  Autrement, elle sera affichée normalement.
            if (Math.Abs(angleRotation % MathHelper.TwoPi) > MathHelper.PiOver2 && Math.Abs(angleRotation % (2 * Math.PI)) < MathHelper.PiOver2 * 3)
            {
                tournerLaTexture = SpriteEffects.FlipVertically;
            }
            else
            {
                tournerLaTexture = SpriteEffects.None;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Affiche l'arme à l'écran
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texturePanneau, Position, new Rectangle(0, (int)type * 30, texturePanneau.Width, 30), 
                             Color.White * progressionDeploiement, angleRotation, new Vector2(texturePanneau.Width / 2, 15), progressionDeploiement, tournerLaTexture, 0);
        }

    }
}
