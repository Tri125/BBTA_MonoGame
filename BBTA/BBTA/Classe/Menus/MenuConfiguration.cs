using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using BBTA.Interface;
using Microsoft.Xna.Framework;
using IndependentResolutionRendering;

namespace BBTA.Classe.Menus
{
    public class MenuConfiguration : MenuArrierePlan
    {
        private Texture2D lettrage;
        private Bouton btnRetour;
        private Bouton btnConfirmer;
        private EtatJeu prochainEtat;

        //Incrémenteur/Déccrémenteur nb soldats
        private int nbSoldatsJ1;
        private Bouton btnBasJ1;
        private Bouton btnHautJ1;

        private int nbSoldatsJ2;
        private Bouton btnBasJ2;
        private Bouton btnHautJ2;

        private SpriteFont police;

        public MenuConfiguration(Game game)
            : base(game)
        {
            prochainEtat = EtatJeu.Configuration;
            //nbSoldatsJ1 = Game1.chargeurCarte.InformationCarte().NbJoueurMin/2;
            //nbSoldatsJ2 = Game1.chargeurCarte.InformationCarte().NbJoueurMin/2;
        }

        protected override void LoadContent()
        {
            lettrage = Game.Content.Load<Texture2D>(@"Ressources\Menus\Configuration\lettrageConfiguration");
            
            btnRetour = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnRetour"), new Vector2(1150, 800), null);
            btnRetour.Clic += new EventHandler(btnRetour_Clic);
            
            btnConfirmer = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Configuration\btnConfirmer"), new Vector2(1150, 700), null);
            btnConfirmer.Clic += new EventHandler(btnConfirmer_Clic);

            //Boutons Incré/Déccré
            btnBasJ1 = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Configuration\btnBas"), new Vector2(960, 448), null);
            btnBasJ1.Clic += new EventHandler(btnBasJ1_Clic);

            btnHautJ1 = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Configuration\btnHaut"), new Vector2(1100, 448), null);
            btnHautJ1.Clic += new EventHandler(btnHautJ1_Clic);

            btnBasJ2 = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Configuration\btnBas"), new Vector2(960, 518), null);
            btnBasJ2.Clic += new EventHandler(btnBasJ2_Clic);

            btnHautJ2 = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Configuration\btnHaut"), new Vector2(1100, 518), null);
            btnHautJ2.Clic += new EventHandler(btnHautJ2_Clic);

            police = Game.Content.Load<SpriteFont>(@"PoliceIndicateur");

            base.LoadContent();
        }

        void btnConfirmer_Clic(object sender, EventArgs e)
        {
            prochainEtat = EtatJeu.Jeu;
        }
        void btnRetour_Clic(object sender, EventArgs e)
        {
            prochainEtat = EtatJeu.Accueil;
        }

        void btnBasJ1_Clic(object sender, EventArgs e)
        {
            if (nbSoldatsJ1 > Game1.chargeurCarte.InformationCarte().NbJoueurMin / 2)
            {
                nbSoldatsJ1--;
            }
        }
        void btnHautJ1_Clic(object sender, EventArgs e)
        {
            if (nbSoldatsJ1 < Game1.chargeurCarte.InformationCarte().NbJoueurMax / 2)
            {
                nbSoldatsJ1++;
            }
        }

        void btnBasJ2_Clic(object sender, EventArgs e)
        {
            if (nbSoldatsJ2 > Game1.chargeurCarte.InformationCarte().NbJoueurMin / 2)
            {
                nbSoldatsJ2--;
            }
        }
        void btnHautJ2_Clic(object sender, EventArgs e)
        {
            if (nbSoldatsJ2 < Game1.chargeurCarte.InformationCarte().NbJoueurMax / 2)
            {
                nbSoldatsJ2++;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            btnConfirmer.Update(null);
            btnRetour.Update(null);
            btnBasJ1.Update(null);
            btnHautJ1.Update(null);
            btnBasJ2.Update(null);
            btnHautJ2.Update(null);
        }

        public EtatJeu ObtenirEtat()
        {
            return prochainEtat;
        }

        public void RemiseAZeroEtat()
        {
            prochainEtat = EtatJeu.Configuration;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
            spriteBatch.Draw(lettrage, Vector2.Zero, Color.White);
            btnRetour.Draw(spriteBatch);
            btnConfirmer.Draw(spriteBatch);

            btnBasJ1.Draw(spriteBatch);
            btnHautJ1.Draw(spriteBatch);
            spriteBatch.DrawString(police, nbSoldatsJ1.ToString(), new Vector2(1010, 407), Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);

            btnBasJ2.Draw(spriteBatch);
            btnHautJ2.Draw(spriteBatch);
            spriteBatch.DrawString(police, nbSoldatsJ2.ToString(), new Vector2(1010, 483), Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);

            spriteBatch.End();
        }
    }
}
