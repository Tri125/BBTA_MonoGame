using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BBTA.GestionAudio
{
    /// <summary>
    /// GestionMusique est une composante qui charge les ressources audio et le lancement de la musique de fond et son changement.
    /// </summary>
    public class GestionMusique : DrawableGameComponent
    {
        private AudioEngine moteur;
        private SoundBank banqueSon;
        private WaveBank banqueVague;
        private Cue cue;

        /// <summary>
        /// Constructeur de base de GestionMusique.
        /// </summary>
        /// <param name="jeu">L'objet Game qui s'attache à la composante</param>
        public GestionMusique(Game jeu)
            : base(jeu)
        {

        }


        protected override void LoadContent()
        {
            moteur = new AudioEngine(@"Content\Ressources\Audio\BBTA_Musique.xgs");
            banqueVague = new WaveBank(moteur, @"Content\Ressources\Audio\Wave Bank.xwb");
            banqueSon = new SoundBank(moteur, @"Content\Ressources\Audio\Sound Bank.xsb");
            base.LoadContent();
        }


        public override void Update(GameTime gameTime)
        {
            moteur.Update();
        }

        /// <summary>
        /// Retourne le Cue associé au bon état du jeu.
        /// </summary>
        /// <param name="num">Numéro associé à l'enum EtatJeu.</param>
        private Cue ObtenirCue(int num)
        {
            switch (num)
            {
                case ((int)BBTA.EtatJeu.Accueil):
                    return banqueSon.GetCue("menu");

                case ((int)BBTA.EtatJeu.Jeu):
                    return banqueSon.GetCue("jeu");

                case ((int)BBTA.EtatJeu.Pause):
                    return banqueSon.GetCue("pause");

                default:
                    return banqueSon.GetCue("menu");
            }
        }


        /// <summary>
        /// Événement qui gère le changement de volume des effets sonores.
        /// </summary>
        public void ChangementVolume(object sender, EventArgs eventArgs)
        {
            //Vérifie si c'est bien un objet de type InfoSonore qui est reçut.
            Option.Option.InfoSonore info = sender as Option.Option.InfoSonore;
            if (info != null)
            {
                AudioCategory categorieAudio = moteur.GetCategory("Music");
                //Change le volume des sons de la catégorie "Music".
                //Échelle de 0 à 1.
                categorieAudio.SetVolume((float)info.Musique / 100);

            }
        }


        /// <summary>
        /// Événement qui gère le changement de musique selon un changement d'état du jeu.
        /// </summary>
        public void ChangementEtatJeu(object sender, EventArgs eventArgs)
        {
            if (sender is BBTA.EtatJeu)
            {
                //Obtient la nouvelle musique selon l'état.
                Cue nouveauCue = ObtenirCue((int)(BBTA.EtatJeu)sender);

                if (cue == null)
                {
                    cue = nouveauCue;
                    cue.Play();
                }
                else
                {
                    //Si la musique n'est pas la même, alors on arrête celle qui joue et on lance la nouvelle.
                    if (cue.Name != nouveauCue.Name)
                    {
                        cue.Stop(AudioStopOptions.Immediate);
                        cue = nouveauCue;
                        cue.Play();
                    }
                }

            }

        }


    }
}