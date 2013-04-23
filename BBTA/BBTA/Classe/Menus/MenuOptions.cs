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
        private Bouton btnRetour;
        private EtatJeu prochainEtat;

        private Slider sliderEffet;
        private int pourcentageEffet;

        private Slider sliderMusique;
        private int pourcentageMusique;

        private BBTA.Classe.Option.Option OptionJeu;

        public MenuOptions(Game game)
            : base(game)
        {
            prochainEtat = EtatJeu.Options;
            OptionJeu = Game1.chargeurOption.OptionUtilisateur;
        }

        protected override void LoadContent()
        {
            lettrage = Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\lettrageOption");
            btnRetour = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnRetour"), new Vector2(1200, 800), null);
            btnRetour.Clic += new EventHandler(btnRetour_Clic);

            sliderEffet = new Slider(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\ArrierePlanSlider"), new Vector2(900, 225),
                                       Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\BarreSlider"),
                                       Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnSlider"),
                                       (float)OptionJeu.InformationSonore.EffetSonore / 100);

            sliderMusique = new Slider(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\ArrierePlanSlider"), new Vector2(900, 325),
                                        Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\BarreSlider"),
                                        Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnSlider"),
                                        (float)OptionJeu.InformationSonore.Musique / 100);
            base.LoadContent();
        }

        void btnRetour_Clic(object sender, EventArgs e)
        {
            Enregistrement(); //Teste pour enregistrer les nouveaux paramètres dans les fichiers
            prochainEtat = EtatJeu.Accueil;
        }

        public override void Update(GameTime gameTime)
        {
            btnRetour.Update(null);
            base.Update(gameTime);
            sliderEffet.Deplacement();
            pourcentageEffet = sliderEffet.ObtenirPourcentage();

            sliderMusique.Deplacement();
            pourcentageMusique = sliderMusique.ObtenirPourcentage();
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
            btnRetour.Draw(spriteBatch);
            sliderEffet.Draw(spriteBatch);
            sliderMusique.Draw(spriteBatch);
            spriteBatch.End();
        }

        private void Enregistrement()
        {
            OptionJeu.InformationSonore.EffetSonore = sliderEffet.ObtenirPourcentage();
            OptionJeu.InformationSonore.Musique = sliderMusique.ObtenirPourcentage();
            Game1.chargeurOption.EnregistrementUtilisateur(ref OptionJeu);
        }
    }
}
