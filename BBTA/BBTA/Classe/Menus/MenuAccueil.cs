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
        //Lettrage
        private Texture2D lettrage;

        //Boutons
        private Bouton btnJouer;
        private Bouton btnOptions;
        private Bouton btnQuitter;
        private EtatJeu prochainEtat;

        /// <summary>
        /// Constructeur de base du menu d'accueil
        /// </summary>
        /// <param name="game"></param>
        public MenuAccueil(Game game)
            : base(game)
        {
            prochainEtat = EtatJeu.Accueil;
        }

        /// <summary>
        /// Chargement des textures pour le menu accueil
        /// </summary>
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

        //Évênement du bouton quitter -> Permet de quitter le jeu
        void btnQuitter_Clic(object sender, EventArgs e)
        {
            Game.Exit();
        }

        //Évênement du bouton option -> Transition au menu configuration
        void btnOptions_Clic(object sender, EventArgs e)
        {
            prochainEtat = EtatJeu.Options;
        }

        //Évênement du bouton jouer -> Transition au menu configuration
        void btnJouer_Clic(object sender, EventArgs e)
        {
            prochainEtat = EtatJeu.Configuration;
        }

        /// <summary>
        /// Mise à jour des éléments du menu accueil
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //Arrière Plan
            base.Update(gameTime);

            //Boutons
            btnJouer.Update(null);
            btnOptions.Update(null);
            btnQuitter.Update(null);
        }

        //Retourne le prochain état que Game1 ira
        public EtatJeu ObtenirEtat()
        {
            return prochainEtat;
        }
        //Laisse 
        public void RemiseAZeroEtat()
        {
            prochainEtat = EtatJeu.Accueil;
        }

        /// <summary>
        /// Affichage des éléments du menu accueil
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            //Arrière plan
            base.Draw(gameTime);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
            
            //Lettrage
            spriteBatch.Draw(lettrage, Vector2.Zero, Color.White);
            
            //Boutons
            btnJouer.Draw(spriteBatch);
            btnOptions.Draw(spriteBatch);
            btnQuitter.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
