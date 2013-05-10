using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BBTA.Elements;

namespace BBTA.Interface
{
    /// <summary>
    /// SelectionArme est un menu qui permet au joueur de choisir une arme parmi celles offertes dans une perspective de tir.
    /// C'est un menu déployable.
    /// </summary>
    public class SelectionArme: MenuDeployable
    {
        //Variable-----------------------------------------------------------------------------------------------------------
        private Texture2D texturesArmes;
        private List<IndicateurArmeRestante> Armes = new List<IndicateurArmeRestante>();
        private Armement munitions;

        //Propriétés---------------------------------------------------------------------------------------------------------
        public Armement Munitions
        {
            get
            {
                return munitions;
            }
            set
            {
                foreach (IndicateurArmeRestante arme in Armes)
                {
                    arme.nbArmeRestantes = value.nbMunitionsRestante(arme.ObtenirType());
                }
                munitions = value;
            }
        }

        //Événements et délégués----------------------------------------------------------------------------------------------
        public delegate void DelegateArmeSelectionnee(Armes armeSelectionnee);
        public event DelegateArmeSelectionnee ArmeSelectionnee;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="texturePanneau">Spritesheet dans laquelle on trouve les textures des panneaux et des boutons</param>
        /// <param name="texturesArmes">Texture des armes (bazooka, grenade avec la main, etc.)</param>
        /// <param name="police">Police d'écriture pour les pastilles des boutons.</param>
        /// <param name="delaiDeploiement">Temps pour l'ouverture/fermeture du menu</param>
        public SelectionArme(Texture2D texturePanneau, Texture2D texturesArmes, SpriteFont police, int delaiDeploiement = 500)
            :base(texturePanneau, new Rectangle(0,0,528,309), delaiDeploiement)
        {
            this.texturesArmes = texturesArmes;
            //Initialisation des boutons pour choisir une arme
            for(int compteur = 0; compteur < texturesArmes.Height/30; compteur++)
            {
                int hauteur = compteur/4;
                Armes.Add(new IndicateurArmeRestante(texturePanneau, new Rectangle(0, 310 + 93*(compteur), 185, 93),
                                                      new Vector2(Position.X - texturePanneau.Width / 2f + 100 * (compteur + 1), Position.Y - tailleBouton.Height + 60),
                                                      (Armes) compteur,
                                                      police));
                Armes[compteur].Clic += new EventHandler(SelectionArme_Clic);
            }
        }

        /// <summary>
        /// Anime le menu lors de l'ouverture/fermeture.
        /// Ferme le menu si la souris clique à l'extérieur du menu
        /// </summary>
        /// <param name="gameTime">Temps de jeu</param>
        /// <param name="matriceCamera">Matrice de caméra</param>
        public void Update(GameTime gameTime, Matrix matriceCamera)
        {
            base.Update(gameTime);
            //Les boutons ne sont affichés que si le menu est pleinement déployé (ouvert)
            if (estDeploye == true)
            {
                //Mise à jour de la position de tous les boutons;
                for (int compteur = 0; compteur < Armes.Count; compteur++)
                {
                    int hauteur = compteur / 4;
                    Armes[compteur].Position = new Vector2(Position.X - texturePanneau.Width / 2f + 20 + 100 * (compteur+1), Position.Y - tailleBouton.Height + 60);
                    Armes[compteur].Update(matriceCamera);
                }

                //Si le joueurs clique à l'extérieur du menu, ce dernier se ferme
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && !aireOccupee.Contains(IndependentResolutionRendering.Resolution.MouseHelper.PositionSourisCamera(matriceCamera)))
                {
                    estOuvert = false;
                }
            }
        }

        /// <summary>
        /// Cette fonction est appellée si l'un des boutons est cliqué.
        /// Elle déclanche un événement pour indiquer la sélection d'une arme.
        /// Elle entame le processus de fermeture du menu.
        /// Elle ajuste l'armement du joueur.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SelectionArme_Clic(object sender, EventArgs e)
        {
            //S'il n'y a plus d'arme, rien ne se produit.
            if ((sender as IndicateurArmeRestante).nbArmeRestantes > 0)
            {
                if (ArmeSelectionnee != null)
                {
                    ArmeSelectionnee((sender as IndicateurArmeRestante).ObtenirType());
                }
                estOuvert = false;
                Munitions.MunitionUtilisee((sender as IndicateurArmeRestante).ObtenirType());
            }
        }

        /// <summary>
        /// Affiche le menu
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
                base.Draw(spriteBatch);
                //Les boutons sont affichés seulement lorsque le menu est pleinement déployé.
                if (estDeploye == true && estOuvert == true)
                {
                    foreach (IndicateurArmeRestante item in Armes)
                    {
                        item.Draw(spriteBatch);
                    }
                }
        }

    }
}
