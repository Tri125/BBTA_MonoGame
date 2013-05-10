using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using BBTA.Interface;
using Microsoft.Xna.Framework;
using IndependentResolutionRendering;

namespace BBTA.Menus
{
    public class MenuPause : DrawableGameComponent
    {
        protected SpriteBatch spriteBatch;

        //Lettrage
        private Texture2D lettrage;

        //Boutons
        private Bouton btnContinuer;
        private Bouton btnAccueil;
        private Bouton btnQuitter;

        private EtatJeu prochainEtat;
        /// <summary>
        /// Constructeur de base pour le menu fin de partie
        /// Met le module invisible par défaut
        /// </summary>
        /// <param name="game"></param>
        /// <param name="equipePerdante"></param>
        public MenuPause(Game game)
            : base(game)
        {
            prochainEtat = EtatJeu.Pause;
            this.Enabled = false;
            this.Visible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            lettrage = Game.Content.Load<Texture2D>(@"Ressources\Menus\Pause\LettragePausePNG");

            btnContinuer = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Pause\BoutonContinuerPNG"), new Vector2(720, 308), null);
            btnContinuer.Clic += new EventHandler(btnContinuer_Clic);
            btnAccueil = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\FinDePartie\BoutonAccueilPNG"), new Vector2(720, 408), null);
            btnAccueil.Clic += new EventHandler(btnAccueil_Clic);
            btnQuitter = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Accueil\btnQuitter"), new Vector2(720, 508), null);
            btnQuitter.Clic +=new EventHandler(btnQuitter_Clic);
            base.LoadContent();
        }

        //Évènement du bouton Continuer
        void btnContinuer_Clic(object sender, EventArgs e)
        {
            prochainEtat = EtatJeu.Jeu;
        }

        //Évènement du bouton accueil
        void btnAccueil_Clic(object sender, EventArgs e)
        {
            prochainEtat = EtatJeu.Accueil;
        }

        //Évènement du bouton quitter
        void btnQuitter_Clic(object sender, EventArgs e)
        {
            Game.Exit();
        }

        public override void Update(GameTime gameTime)
        {
            btnContinuer.Update(null);
            btnAccueil.Update(null);
            btnQuitter.Update(null);
        }

        public EtatJeu ObtenirEtat()
        {
            return prochainEtat;
        }

        public void RemiseAZeroEtat()
        {
            prochainEtat = EtatJeu.Pause;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
            spriteBatch.Draw(lettrage, Vector2.Zero, Color.White);

            btnContinuer.Draw(spriteBatch);
            btnAccueil.Draw(spriteBatch);
            btnQuitter.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
