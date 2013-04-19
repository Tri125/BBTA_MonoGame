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
    public enum Armes
    {
        Roquette, 
        Grenade,
    }


    public class SelectionArme: MenuDeployable
    {
        public bool YaTilUneArmeSelectionne { get; set; }
        private Acteur joueur;
        private List<Texture2D> texturesArmes;
        private Point positionSouris;
        private MouseState sourisAvant;
        private MouseState sourisApres;
        private List<IndiquateurArmeRestante> Armes = new List<IndiquateurArmeRestante>();
        public Armes armeChoisie {get;set;}

        public SelectionArme(Texture2D texturePanneau, List<Texture2D> texturesArmes, SpriteFont police, int delaiDeploiement = 500)
            :base(texturePanneau, new Rectangle(0,0,528,309), delaiDeploiement)
        {
            this.texturesArmes = texturesArmes;
            YaTilUneArmeSelectionne = false;
            for(int compteur = 0; compteur < texturesArmes.Count; compteur++)
            {
                int hauteur = compteur/4;
                Armes.Add(new IndiquateurArmeRestante(texturePanneau, new Rectangle(0, 319, 183, 91),
                                                      new Vector2(Position.X - texturePanneau.Width/2f + 100, Position.Y + 50 + hauteur * 100),
                                                      police));
                Armes[compteur].Clic += new EventHandler(SelectionArme_Clic);
            }
        }

        void SelectionArme_Clic(object sender, EventArgs e)
        {
                
                estOuvert = false;
        }

        public void AssocierJoueur(Acteur joueur)
        {
            this.joueur = joueur;
            Position = new Vector2(joueur.ObtenirPosition().X, joueur.ObtenirPosition().Y - 10);
        }

        public void Update(GameTime gameTime, Matrix matriceCamera)
        {
            sourisAvant = sourisApres;
            sourisApres = Mouse.GetState();
            positionSouris = IndependentResolutionRendering.Resolution.MouseHelper.PositionSourisCamera(matriceCamera);
            if (estDeploye == true)
            {
                for (int compteur = 0; compteur < Armes.Count; compteur++)
                {
                    int hauteur = compteur / 4;
                    Armes[compteur].Position = new Vector2(Position.X - texturePanneau.Width / 2f + 20, Position.Y - texturePanneau.Height + 20);

                    Armes[compteur].Update(matriceCamera);
                }
            }

            if (sourisApres.LeftButton == ButtonState.Pressed && sourisAvant.LeftButton == ButtonState.Released && aireOccupee.Contains(positionSouris))
            {
                estOuvert = false;
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
                base.Draw(spriteBatch);
                foreach (Bouton item in Armes)
                {
                    item.Draw(spriteBatch);
                }
        }

    }
}
