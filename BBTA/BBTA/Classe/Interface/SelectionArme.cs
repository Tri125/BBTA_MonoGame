using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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
        private List<Texture2D> texturesArmes;
        private Rectangle positionSouris = new Rectangle(0, 0, 1, 1);
        private List<IndiquateurArmeRestante> Armes = new List<IndiquateurArmeRestante>();
        public Armes armeChoisie {get;set;}

        public SelectionArme(Texture2D texturePanneau, List<Texture2D> texturesArmes, SpriteFont police, int delaiDeploiement = 500)
            :base(texturePanneau, delaiDeploiement)
        {
            this.texturesArmes = texturesArmes;
            YaTilUneArmeSelectionne = false;
            for(int compteur = 0; compteur < texturesArmes.Count; compteur++)
            {
                int hauteur = compteur/4;
                Armes.Add(new IndiquateurArmeRestante(texturesArmes[compteur],
                                                      new Vector2(Position.X - texturePanneau.Width/2f + 100, Position.Y + 50 + hauteur * 100),
                                                      police));
            }
        }

        public void Update(GameTime gameTime, Matrix matriceCamera)
        {
            if (estDeploye == true)
            {
                for (int compteur = 0; compteur < Armes.Count; compteur++)
                {
                    int hauteur = compteur / 4;
                    Armes[compteur].Position = new Vector2(Position.X - texturePanneau.Width / 2f + 20, Position.Y - texturePanneau.Height+20);

                    Console.WriteLine(Position);

                    if (Armes[compteur].ClicComplet(matriceCamera) == true)
                    {
                        armeChoisie = (Armes)compteur;
                        Fermer(gameTime);
                    }
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (estDeploye == true && estOuvert == true)
            {
                foreach (Bouton item in Armes)
                {
                    item.Draw(spriteBatch);
                }
            }
        }

    }
}
