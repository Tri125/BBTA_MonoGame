using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BBTA.Interface;
using BBTA.Elements;
using Microsoft.Xna.Framework.Graphics;
using IndependentResolutionRendering;

namespace BBTA.Menus
{
    public class MenuAccueil : MenuArrierePlan
    {        
        //Propre à accueil
        private Texture2D lettrage;
        private Bouton btnJouer;
        private Bouton btnOptions;
        private Bouton btnQuitter;
        private EtatJeu prochainEtat;

        public MenuAccueil(Game game)
            : base(game)
        {
            prochainEtat = EtatJeu.Accueil;
        }

        protected override void LoadContent()
        {
            //Contenu propre au menu Accueil
            lettrage = Game.Content.Load<Texture2D>(@"Ressources\Menus\Accueil\lettrage");
            btnJouer = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Accueil\btnJouer"), new Vector2(Resolution.getVirtualViewport().Width / 2f, 500), null);
            btnJouer.Clic += new EventHandler(btnJouer_Clic);
            btnOptions = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Accueil\btnOptions"), new Vector2(Resolution.getVirtualViewport().Width / 2f, 625), null);
            btnOptions.Clic += new EventHandler(btnOptions_Clic);
            btnQuitter = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Accueil\btnQuitter"), new Vector2(Resolution.getVirtualViewport().Width / 2f, 750), null);
            btnQuitter.Clic += new EventHandler(btnQuitter_Clic);
            base.LoadContent();
        }

        void btnQuitter_Clic(object sender, EventArgs e)
        {
            Game.Exit();
        }

        void btnOptions_Clic(object sender, EventArgs e)
        {
            prochainEtat = EtatJeu.Options;
        }

        void btnJouer_Clic(object sender, EventArgs e)
        {
            prochainEtat = EtatJeu.Configuration;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            btnJouer.Update(null);
            btnOptions.Update(null);
            btnQuitter.Update(null);
            //Clic sur btnJouer -> Configuration en temps normal, mais pour la phase de développement, aller directement au jeu
        }

        public EtatJeu ObtenirEtat()
        {
            return prochainEtat;
        }

        public void RemiseAZeroEtat()
        {
            prochainEtat = EtatJeu.Accueil;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
            
            //Éléments
            spriteBatch.Draw(lettrage, Vector2.Zero, Color.White);
            btnJouer.Draw(spriteBatch);
            btnOptions.Draw(spriteBatch);
            btnQuitter.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
