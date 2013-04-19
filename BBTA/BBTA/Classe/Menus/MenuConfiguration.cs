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

        public MenuConfiguration(Game game)
            : base(game)
        {
            prochainEtat = EtatJeu.Configuration;
        }

        protected override void LoadContent()
        {
            lettrage = Game.Content.Load<Texture2D>(@"Ressources\Menus\Configuration\lettrageConfiguration");
            btnRetour = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnRetour"), new Vector2(1200, 800), null);
            btnRetour.Clic += new EventHandler(btnRetour_Clic);
            btnConfirmer = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Configuration\btnConfirmer"), new Vector2(1200, 700), null);
            btnConfirmer.Clic += new EventHandler(btnConfirmer_Clic);
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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            btnConfirmer.Update(null);
            btnRetour.Update(null);
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
            spriteBatch.End();
        }
    }
}
