using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BBTA.Elements;
using BBTA.Classe.Elements;

namespace BBTA.Interface
{
    public enum Armes
    {
        Roquette = 0, 
        Grenade,
        Mine
    }


    public class SelectionArme: MenuDeployable
    {
        private Texture2D texturesArmes;
        private Point positionSouris;

        public Armement Munitions
        {
            get
            {
                return new Armement(Armes[1].nbArmeRestantes, Armes[2].nbArmeRestantes, Armes[0].nbArmeRestantes);
            }
            set
            {
                foreach (IndicateurArmeRestante arme in Armes)
                {
                    arme.nbArmeRestantes = value[arme.ObtenirType()];
                }
            }
        }

        private List<IndicateurArmeRestante> Armes = new List<IndicateurArmeRestante>();
        public delegate void DelegateArmeSelectionnee(Armes armeSelectionnee);
        public event DelegateArmeSelectionnee ArmeSelectionnee;
        public event EventHandler SortieDuPanneau;

        public SelectionArme(Texture2D texturePanneau, Texture2D texturesArmes, SpriteFont police, int delaiDeploiement = 500)
            :base(texturePanneau, new Rectangle(0,0,528,309), delaiDeploiement)
        {
            this.texturesArmes = texturesArmes;
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

        void SelectionArme_Clic(object sender, EventArgs e)
        {
            if ((sender as IndicateurArmeRestante).nbArmeRestantes > 0)
            {
                if (ArmeSelectionnee != null)
                {
                    ArmeSelectionnee((sender as IndicateurArmeRestante).ObtenirType());
                }
                estOuvert = false;
                (sender as IndicateurArmeRestante).nbArmeRestantes--;
            }
        }

        public void Update(GameTime gameTime, Matrix matriceCamera)
        {
            base.Update(gameTime);
            positionSouris = IndependentResolutionRendering.Resolution.MouseHelper.PositionSourisCamera(matriceCamera);
            if (estDeploye == true)
            {
                for (int compteur = 0; compteur < Armes.Count; compteur++)
                {
                    int hauteur = compteur / 4;
                    Armes[compteur].Position = new Vector2(Position.X - texturePanneau.Width / 2f + 20 + 100 * (compteur+1), Position.Y - tailleBouton.Height + 60);

                    Armes[compteur].Update(matriceCamera);
                }

                if (Mouse.GetState().LeftButton == ButtonState.Pressed && !aireOccupee.Contains(positionSouris))
                {
                    estOuvert = false;
                    SortieDuPanneau(this, new EventArgs());
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
                base.Draw(spriteBatch);
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
