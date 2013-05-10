using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BBTA.Interface;
using IndependentResolutionRendering;

namespace BBTA.Menus
{
    public class MenuFinDePartie : MenuArrierePlan
    {
        private Texture2D lettrage;
        private Texture2D lettrageBleu;
        private Texture2D lettrageRouge;
        private Bouton btnAccueil;
        private Bouton btnRecommencer;

        
        private EtatJeu prochainEtat;
        private Color equipePerdante;
        private Texture2D equipeGagnante;

        public MenuFinDePartie(Game game, Color equipePerdante)
            : base(game)
        {
            prochainEtat = EtatJeu.FinDePartie;
            this.equipePerdante = equipePerdante;
        }

        protected override void LoadContent()
        {
            lettrage = Game.Content.Load<Texture2D>(@"Ressources\Menus\FinDePartie\LettrageFinDePartie");
            lettrageBleu = Game.Content.Load<Texture2D>(@"Ressources\Menus\FinDePartie\LettrageBleusPNG");
            lettrageRouge = Game.Content.Load<Texture2D>(@"Ressources\Menus\FinDePartie\LettrageRougePNG");

            btnAccueil = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\FinDePartie\BoutonAccueilPNG"), new Vector2(1175, 600), null);
            btnAccueil.Clic += new EventHandler(btnAccueil_Clic);

            btnRecommencer = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\FinDePartie\BoutonRecommencerPNG"), new Vector2(1175, 800), null);
            btnRecommencer.Clic += new EventHandler(btnRecommencer_Clic);

            base.LoadContent();
        }

        //Évènement du bouton accueil
        void btnAccueil_Clic(object sender, EventArgs e)
        {
            prochainEtat = EtatJeu.Accueil;
        }

        //Évènement du bouton recommencer
        void btnRecommencer_Clic(object sender, EventArgs e)
        {
            prochainEtat = EtatJeu.Jeu;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            btnAccueil.Update(null);
            btnRecommencer.Update(null);

            //Déterminer l'équipe gagnante
            //Si l'équipe rouge est perdante, alors équipe bleue est gagnante
            if (equipePerdante == Color.Firebrick)
            {
                equipeGagnante = lettrageBleu;
            }
            else
            {
                equipeGagnante = lettrageRouge;
            }
        }

        public EtatJeu ObtenirEtat()
        {
            return prochainEtat;
        }

        public void RemiseAZeroEtat()
        {
            prochainEtat = EtatJeu.FinDePartie;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
            spriteBatch.Draw(lettrage, Vector2.Zero, Color.White);
            spriteBatch.Draw(equipeGagnante, new Vector2(510, 200), Color.White);
            btnAccueil.Draw(spriteBatch);
            btnRecommencer.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
