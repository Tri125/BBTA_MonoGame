using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IndependentResolutionRendering;
using BBTA.Interface;
using BBTA.Classe.Interface;

namespace BBTA.Classe.Menus
{
    public class MenuOptions : MenuArrierePlan
    {
        private Texture2D lettrage;

        //Boutons menu
        private Bouton btnValider;
        private Bouton btnDefaut;
        private Bouton btnAnnuler;

        //Bouton ClickNType
        private Bouton btnGauche;
        private Bouton btnDroite;
        private Bouton btnSaut;
        private Bouton btnTir;
        private Bouton btnPause;

        private EtatJeu prochainEtat;

        private Slider sliderEffet;
        private Slider sliderMusique;

        private BBTA.Classe.Option.Option OptionJeu;

        public MenuOptions(Game game)
            : base(game)
        {
            prochainEtat = EtatJeu.Options;
            OptionJeu = Game1.chargeurOption.OptionActive;
        }

        protected override void LoadContent()
        {
            lettrage = Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\lettrageOption");
            sliderEffet = new Slider(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\ArrierePlanSlider"), new Vector2(900, 225),
                                       Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\BarreSlider"),
                                       Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnSlider"),
                                       (float)OptionJeu.InformationSonore.EffetSonore / 100);

            sliderMusique = new Slider(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\ArrierePlanSlider"), new Vector2(900, 325),
                                        Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\BarreSlider"),
                                        Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnSlider"),
                                        (float)OptionJeu.InformationSonore.Musique / 100);

            btnValider = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnValider"), new Vector2(1175, 600), null);
            btnValider.Clic += new EventHandler(btnValider_Clic);

            btnDefaut = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnDefaut"), new Vector2(1175, 700), null);
            btnDefaut.Clic +=new EventHandler(btnDefaut_Clic);

            btnAnnuler = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnAnnuler"), new Vector2(1175, 800), null);
            btnAnnuler.Clic += new EventHandler(btnAnnuler_Clic);

            btnGauche = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnClickNTypeGauche"), new Vector2(226, 500), null);
            btnDroite = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnClickNTypeDroite"), new Vector2(655, 500), null);
            btnSaut = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnClickNTypeSaut"), new Vector2(226, 574), null);
            btnTir = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnClickNTypeTir"), new Vector2(655, 574), null);
            btnPause = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnClickNTypePause"), new Vector2(226, 648), null);

            base.LoadContent();
        }

        void btnValider_Clic(object sender, EventArgs e)
        {
            Enregistrement(); //Teste pour enregistrer les nouveaux paramètres dans les fichiers
            prochainEtat = EtatJeu.Accueil;
        }

        void btnDefaut_Clic(object sender, EventArgs e)
        {
            RetourDefaut();
        }

        void btnAnnuler_Clic(object sender, EventArgs e)
        {
            sliderEffet.DeplacementPourcentage((float)OptionJeu.InformationSonore.EffetSonore / 100);
            sliderMusique.DeplacementPourcentage((float)OptionJeu.InformationSonore.Musique / 100);
            prochainEtat = EtatJeu.Accueil;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            sliderEffet.Deplacement();
            sliderMusique.Deplacement();

            btnValider.Update(null);
            btnDefaut.Update(null);
            btnAnnuler.Update(null);

            btnGauche.Update(null);
            btnDroite.Update(null);
            btnSaut.Update(null);
            btnTir.Update(null);
            btnPause.Update(null);
        }

        public EtatJeu ObtenirEtat()
        {
            return prochainEtat;
        }

        public void RemiseAZeroEtat()
        {
            prochainEtat = EtatJeu.Options;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
            spriteBatch.Draw(lettrage, Vector2.Zero, Color.White);
            sliderEffet.Draw(spriteBatch);
            sliderMusique.Draw(spriteBatch);

            btnValider.Draw(spriteBatch);
            btnDefaut.Draw(spriteBatch);
            btnAnnuler.Draw(spriteBatch);

            btnGauche.Draw(spriteBatch);
            btnDroite.Draw(spriteBatch);
            btnSaut.Draw(spriteBatch);
            btnTir.Draw(spriteBatch);
            btnPause.Draw(spriteBatch);
            spriteBatch.End();
        }

        private void Enregistrement()
        {
            OptionJeu.InformationSonore.EffetSonore = sliderEffet.ObtenirPourcentage();
            OptionJeu.InformationSonore.Musique = sliderMusique.ObtenirPourcentage();
            Game1.chargeurOption.EnregistrementUtilisateur(ref OptionJeu);
        }

        private void RetourDefaut()
        {
            sliderEffet.DeplacementPourcentage ((float)Game1.chargeurOption.OptionDefaut.InformationSonore.EffetSonore/100);
            sliderMusique.DeplacementPourcentage( (float)Game1.chargeurOption.OptionDefaut.InformationSonore.Musique / 100);
        }
    }
}
