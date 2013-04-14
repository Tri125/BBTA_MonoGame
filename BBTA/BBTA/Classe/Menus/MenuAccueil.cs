using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BBTA.Interface;
using BBTA.Elements;
using Microsoft.Xna.Framework.Graphics;
using IndependentResolutionRendering;
using BBTA.Classe.Menus;

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
            btnJouer = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Accueil\btnJouer"), new Vector2(Resolution.getVirtualViewport().Width / 2f, 500));
            btnOptions = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Accueil\btnOptions"), new Vector2(Resolution.getVirtualViewport().Width / 2f, 625));
            btnQuitter = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Accueil\btnQuitter"), new Vector2(Resolution.getVirtualViewport().Width / 2f, 750));
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //Clic sur btnJouer -> Configuration en temps normal, mais pour la phase de développement, aller directement au jeu
            if (btnJouer.ClicComplet())
            {
                prochainEtat = EtatJeu.Jeu;
            }
            //Clic btnOptions -> Options
            if (btnOptions.ClicComplet())
            {
                prochainEtat = EtatJeu.Options;
            }
            //Clic btnQuitter -> Quitter
            if (btnQuitter.ClicComplet())
            {
                Game.Exit();
            }
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
