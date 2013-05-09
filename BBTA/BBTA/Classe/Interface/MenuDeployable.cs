using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BBTA.Classe.Interface
{
    /// <summary>
    /// MenuDeployable est une classe qui permet d'afficher un menu doté d'une animation à son lancement et à sa fermeture.
    /// L'animation touche à sa taille et à sa transparence.
    /// </summary>
    public abstract class MenuDeployable
    {
        //Variables----------------------------------------------------------------------------------------------------------
        private int delaiOuvertureFermeture;
        private Vector2 position;
        protected Texture2D texturePanneau;
        protected Rectangle tailleBouton;
        protected float progressionDeploiement = 0;
        protected Rectangle aireOccupee;

        //Propriétés---------------------------------------------------------------------------------------------------------
        public bool estDeploye { get; set; }
        public bool estOuvert { get; set; }
        public Vector2 Position 
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                aireOccupee = new Rectangle((int)value.X- aireOccupee.Width / 2, (int)value.Y - aireOccupee.Height, aireOccupee.Width, aireOccupee.Height);
            }
        }

        //Événements--------------------------------------------------------------------------------------------------------
        public event EventHandler PanneauFermer;

        /// <summary>
        /// Cosntructeur
        /// </summary>
        /// <param name="texture">Spritesheet dans laquelle se trouve la texture du menu</param>
        /// <param name="tailleBouton">Position de l'image du panneau du menu dans la spritesheet</param>
        /// <param name="delaiOuvertureFermeture">Temps nécessaire pour déployer/fermer complètement le menu</param>
        public MenuDeployable(Texture2D texture, Rectangle? tailleBouton, int delaiOuvertureFermeture = 500)
        {
            this.texturePanneau = texture;
            this.delaiOuvertureFermeture = delaiOuvertureFermeture;
            //Si on ne spécifie pas d'image dans la psritesheet, alors on cosidère que la spritesheet ne contient qu'une seule image: la texture du panneau du menu.
            if (!tailleBouton.HasValue)
            {
                aireOccupee = new Rectangle((int)Position.X, (int)Position.Y, (int)texture.Width, (int)texture.Height);
                this.tailleBouton = new Rectangle(0, 0, aireOccupee.Width, aireOccupee.Height);
            }
            //Autrement, on va chercher l'image indiquée.
            else
            {
                aireOccupee = new Rectangle((int)Position.X, (int)Position.Y, tailleBouton.Value.Width, tailleBouton.Value.Height);
                this.tailleBouton = tailleBouton.Value;
            }
            estOuvert = false;
            estDeploye = false;
        }

        /// <summary>
        /// Gère l'animation du déploiement et de la fermeture du menu.
        /// Déclanche un événement lorsque la fermeture est complète.
        /// </summary>
        /// <param name="gameTime">Temps de jeu</param>
        public virtual void Update(GameTime gameTime)
        {
            //Si le panneau est fermé et qu'on désire l'ouvrir...
            if (estOuvert == true && estDeploye == false)
            {
                //On augmente progressivement la taille et l'opacité de ce dernier.
                progressionDeploiement += (float)1 / delaiOuvertureFermeture * gameTime.ElapsedGameTime.Milliseconds;
                //Lorsque la variable dépasse 1, le panneau est complètement ouvert.
                if (progressionDeploiement > 1)
                {
                    estDeploye = true;
                    progressionDeploiement = 1;
                }
            }

            //Si le panneau est complètement ouvert et qu'on désire le fermer...
            if (estOuvert == false)
            {
                //On diminue progressivement la taille et l'opacité de ce dernier.
                progressionDeploiement -= (float)1 / delaiOuvertureFermeture * gameTime.ElapsedGameTime.Milliseconds;
                //Lorsque la variable est sous zéro, le panneau est complètement fermer.  Un événement l'indiquant est déclanché.
                if (progressionDeploiement < 0)
                {
                    estDeploye = false;
                    progressionDeploiement = 0;
                    if (PanneauFermer != null)
                    {
                        PanneauFermer(this, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// Affiche le menu à l'écran.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texturePanneau, Position, tailleBouton, Color.White * progressionDeploiement, 0,
                             new Vector2(tailleBouton.Width/2, aireOccupee.Height), 
                             progressionDeploiement, SpriteEffects.None, 0);
        }
    }
}
