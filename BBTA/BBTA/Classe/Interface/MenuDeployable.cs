using System;
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
        public Vector2 Position { get; set; }

        protected bool estDeploye = false;
        protected bool estOuvert = false;

        float progressionDeploiement = 0;
        int delaiOuvertureFermeture;

        public MenuDeployable(Texture2D texture, int delaiOuvertureFermeture = 500)
        {
            this.texturePanneau = texture;
            this.delaiOuvertureFermeture = delaiOuvertureFermeture;
        }

        public void Ouvrir(GameTime gameTime)
        {
            estOuvert = true;
        }

        public void Fermer(GameTime gameTime)
        {
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

            if (estOuvert == false && estDeploye == true)
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
            spriteBatch.Draw(texturePanneau, Position, null, Color.White * progressionDeploiement, 0,
                             new Vector2(texturePanneau.Width/2f, texturePanneau.Height), 
                             progressionDeploiement, SpriteEffects.None, 0);
        }
    }
}
