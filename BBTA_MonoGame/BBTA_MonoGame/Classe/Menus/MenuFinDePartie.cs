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
        //Couleur des équipes gagnantes et perdantes
        private Color equipePerdante;
        private Texture2D equipeGagnante;

        //Lettrage
        private Texture2D lettrage;
        private Texture2D lettrageBleu;
        private Texture2D lettrageRouge;

        //Boutons
        private Bouton btnAccueil;
        private Bouton btnRecommencer;

        private EtatJeu prochainEtat;

        /// <summary>
        /// Constructeur de base pour le menu fin de partie
        /// </summary>
        /// <param name="game"></param>
        /// <param name="equipePerdante"></param>
        public MenuFinDePartie(Game game, Color equipePerdante)
            : base(game)
        {
            prochainEtat = EtatJeu.FinDePartie;
            this.equipePerdante = equipePerdante;
        }

        /// <summary>
        /// Chargement des textures pour le menu fin de partie
        /// </summary>
        protected override void LoadContent()
        {
            //Chargement des lettrages du texte
            lettrage = Game.Content.Load<Texture2D>(@"Ressources\Menus\FinDePartie\LettrageFinDePartie");
            lettrageBleu = Game.Content.Load<Texture2D>(@"Ressources\Menus\FinDePartie\LettrageBleusPNG");
            lettrageRouge = Game.Content.Load<Texture2D>(@"Ressources\Menus\FinDePartie\LettrageRougePNG");

            //Chargement des boutons
            btnRecommencer = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\FinDePartie\BoutonRecommencerPNG"), new Vector2(1175, 600), null);
            btnRecommencer.Clic += new EventHandler(btnRecommencer_Clic);
            btnAccueil = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\FinDePartie\BoutonAccueilPNG"), new Vector2(1175, 700), null);
            btnAccueil.Clic += new EventHandler(btnAccueil_Clic);
            base.LoadContent();
        }

        //Évènement du bouton recommencer
        void btnRecommencer_Clic(object sender, EventArgs e)
        {
            //Lors
            prochainEtat = EtatJeu.Jeu;
        }

        //Évènement du bouton accueil
        void btnAccueil_Clic(object sender, EventArgs e)
        {
            prochainEtat = EtatJeu.Accueil;
        }

        /// <summary>
        /// Mise à jour des éléments du menu fin de partie
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Boutons
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


        /// <summary>
        /// Affichage des éléments du menu fin de partie
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
            
            //Lettrage
            spriteBatch.Draw(lettrage, Vector2.Zero, Color.White);
            spriteBatch.Draw(equipeGagnante, new Vector2(510, 200), Color.White);
            
            //Boutons
            btnAccueil.Draw(spriteBatch);
            btnRecommencer.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
