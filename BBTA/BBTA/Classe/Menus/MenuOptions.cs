using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IndependentResolutionRendering;
using BBTA.Interface;
using BBTA.GestionAudio;

namespace BBTA.Menus
{
    public class MenuOptions : MenuArrierePlan
    {
        private event EventHandler ChangementVolume;
        private bool initierAudio;

        private Texture2D lettrage;
        private SpriteFont police;
        //Boutons menu--------------------------------------------------------------------------------
        private Bouton btnValider;
        private Bouton btnDefaut;
        private Bouton btnAnnuler;

        //Bouton de selection des touches------------------------------------------------------------
        private BoutonClicEtEcris btnGauche;
        private BoutonClicEtEcris btnDroite;
        private BoutonClicEtEcris btnSaut;
        private BoutonClicEtEcris btnTir;
        private BoutonClicEtEcris btnPause;

        //Boutons de sélection du volume-------------------------------------------------------------
        private Slider sliderEffet;
        private Slider sliderMusique;

        //Si on est en train d'enregistrer une nouvelle touche---------------------------------------
        private bool enAttente;
        private BoutonClicEtEcris boutonEnAttente;

        private EtatJeu prochainEtat;

        private Option.Option OptionJeu;

        public void InitControlAudio(GestionMusique gestionnaireMusique, GestionSon gestionnaireSon)
        {
            if (initierAudio == false)
            {
                ChangementVolume += gestionnaireMusique.ChangementVolume;
                ChangementVolume += gestionnaireSon.ChangementVolume;
                ChangementVolume(Game1.chargeurOption.OptionActive.InformationSonore, EventArgs.Empty);

                initierAudio = true;
            }
        }

        public MenuOptions(Game game)
            : base(game)
        {
            prochainEtat = EtatJeu.Options;
            OptionJeu = Game1.chargeurOption.OptionActive;
        }

        protected override void LoadContent()
        {
            police = Game.Content.Load<SpriteFont>(@"Police\ComicSan");

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
            btnDefaut.Clic += new EventHandler(btnDefaut_Clic);

            btnAnnuler = new Bouton(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnAnnuler"), new Vector2(1175, 800), null);
            btnAnnuler.Clic += new EventHandler(btnAnnuler_Clic);

            btnGauche = new BoutonClicEtEcris(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnClickNTypeGauche"), new Vector2(226, 500),
                null, OptionJeu.InformationTouche.Gauche, police);
            btnGauche.Clic += new EventHandler(btnConfigTouche_Clic);

            btnDroite = new BoutonClicEtEcris(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnClickNTypeDroite"), new Vector2(655, 500),
                null, OptionJeu.InformationTouche.Droite, police);
            btnDroite.Clic += new EventHandler(btnConfigTouche_Clic);

            btnSaut = new BoutonClicEtEcris(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnClickNTypeSaut"), new Vector2(226, 574),
                null, OptionJeu.InformationTouche.Saut, police);
            btnSaut.Clic += new EventHandler(btnConfigTouche_Clic);

            btnTir = new BoutonClicEtEcris(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnClickNTypeTir"), new Vector2(655, 574),
                null, OptionJeu.InformationTouche.Tir, police);
            btnTir.Clic += new EventHandler(btnConfigTouche_Clic);

            btnPause = new BoutonClicEtEcris(Game.Content.Load<Texture2D>(@"Ressources\Menus\Options\btnClickNTypePause"), new Vector2(226, 648),
                null, OptionJeu.InformationTouche.Pause, police);
            btnPause.Clic += new EventHandler(btnConfigTouche_Clic);

            base.LoadContent();
        }


        void btnConfigTouche_Clic(object sender, EventArgs e)
        {
            if (!enAttente)
            {
                BoutonClicEtEcris boutonTouche = sender as BoutonClicEtEcris;
                if (boutonTouche != null)
                {
                    enAttente = true;
                    boutonEnAttente = boutonTouche;
                    boutonTouche.EcouteTouche += new EventInput.KeyEventHandler(EventInput_KeyDown);
                    EventInput.EventInput.KeyDown += new EventInput.KeyEventHandler(EventInput_KeyDown);
                }
            }
        }

        void EventInput_KeyDown(object sender, EventInput.KeyEventArgs e)
        {
            boutonEnAttente.touche = e.KeyCode;
            boutonEnAttente.EcouteTouche -= new EventInput.KeyEventHandler(EventInput_KeyDown);
            EventInput.EventInput.KeyDown -= new EventInput.KeyEventHandler(EventInput_KeyDown);
            boutonEnAttente = null;
            enAttente = false;

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

            btnDroite.touche = Game1.chargeurOption.OptionActive.InformationTouche.Droite;
            btnGauche.touche = Game1.chargeurOption.OptionActive.InformationTouche.Gauche;
            btnPause.touche = Game1.chargeurOption.OptionActive.InformationTouche.Pause;
            btnSaut.touche = Game1.chargeurOption.OptionActive.InformationTouche.Saut;
            btnTir.touche = Game1.chargeurOption.OptionActive.InformationTouche.Tir;

            prochainEtat = EtatJeu.Accueil;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            sliderEffet.Deplacement();
            sliderMusique.Deplacement();
            OptionJeu.InformationSonore.EffetSonore = sliderEffet.ObtenirPourcentage();
            OptionJeu.InformationSonore.Musique = sliderMusique.ObtenirPourcentage();

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

            OptionJeu.InformationTouche.Droite = btnDroite.touche;
            OptionJeu.InformationTouche.Gauche = btnGauche.touche;
            OptionJeu.InformationTouche.Pause = btnPause.touche;
            OptionJeu.InformationTouche.Saut = btnSaut.touche;
            OptionJeu.InformationTouche.Tir = btnTir.touche;

            Game1.chargeurOption.EnregistrementUtilisateur(ref OptionJeu);
            ChangementVolume(Game1.chargeurOption.OptionActive.InformationSonore, EventArgs.Empty);
        }

        private void RetourDefaut()
        {
            sliderEffet.DeplacementPourcentage((float)Game1.chargeurOption.OptionDefaut.InformationSonore.EffetSonore / 100);
            sliderMusique.DeplacementPourcentage((float)Game1.chargeurOption.OptionDefaut.InformationSonore.Musique / 100);

            btnDroite.touche = Game1.chargeurOption.OptionDefaut.InformationTouche.Droite;
            btnGauche.touche = Game1.chargeurOption.OptionDefaut.InformationTouche.Gauche;
            btnPause.touche = Game1.chargeurOption.OptionDefaut.InformationTouche.Pause;
            btnSaut.touche = Game1.chargeurOption.OptionDefaut.InformationTouche.Saut;
            btnTir.touche = Game1.chargeurOption.OptionDefaut.InformationTouche.Tir;
        }

    }
}
