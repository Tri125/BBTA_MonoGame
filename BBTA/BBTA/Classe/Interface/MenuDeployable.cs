﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BBTA.Interface
{
    public abstract class MenuDeployable
    {
        protected Texture2D texturePanneau;
        private Rectangle tailleBouton;
        private Vector2 position;
        public Vector2 Position 
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                aireOccupee = new Rectangle((int)value.X- texturePanneau.Width / 2, (int)value.Y - texturePanneau.Height, aireOccupee.Width, aireOccupee.Height);
            }
        }
        protected Rectangle aireOccupee;

        protected bool estDeploye = false;
        public bool estOuvert{get;set;}

        protected float progressionDeploiement = 0;
        int delaiOuvertureFermeture;

        public MenuDeployable(Texture2D texture, Rectangle? tailleBouton, int delaiOuvertureFermeture = 500)
        {
            this.texturePanneau = texture;
            this.delaiOuvertureFermeture = delaiOuvertureFermeture;
            if (!tailleBouton.HasValue)
            {
                aireOccupee = new Rectangle((int)Position.X, (int)Position.Y, (int)texture.Width, (int)texture.Height);
                this.tailleBouton = new Rectangle(0, 0, aireOccupee.Width, aireOccupee.Height);
            }
            else
            {
                aireOccupee = new Rectangle((int)Position.X, (int)Position.Y, tailleBouton.Value.Width, tailleBouton.Value.Height);
                this.tailleBouton = tailleBouton.Value;
            }
            estOuvert = false;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (estOuvert == true && estDeploye == false)
            {
                progressionDeploiement += (float)1 / delaiOuvertureFermeture * gameTime.ElapsedGameTime.Milliseconds;
                if (progressionDeploiement > 1)
                {
                    estDeploye = true;
                    progressionDeploiement = 1;
                }
            }

            if (estOuvert == false)
            {
                progressionDeploiement -= (float)1 / delaiOuvertureFermeture * gameTime.ElapsedGameTime.Milliseconds;
                if (progressionDeploiement < 0)
                {
                    estDeploye = false;
                    progressionDeploiement = 0;
                }
            }
        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texturePanneau, Position, tailleBouton, Color.White * progressionDeploiement, 0,
                             new Vector2(tailleBouton.Width/2, tailleBouton.Height), 
                             progressionDeploiement, SpriteEffects.None, 0);
        }
    }
}
