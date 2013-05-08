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
using BBTA.Interface;
using BBTA.Elements;

namespace BBTA.Classe.GestionAudio
{
    /// <summary>
    /// GestionSon est une composante qui charge les ressources audio et le lancement d'effet sonore selon des événements.
    /// </summary>
    public class GestionSon : DrawableGameComponent
    {
        private AudioEngine moteur;
        private SoundBank banqueSon;
        private WaveBank banqueVague;

        /// <summary>
        /// Constructeur de base de GestionSon.
        /// </summary>
        /// <param name="jeu">L'objet Game qui s'attache à la composante</param>
        public GestionSon(Game jeu)
            : base(jeu)
        {

        }


        protected override void LoadContent()
        {
            moteur = new AudioEngine(@"Content\Ressources\Audio\BBTA_Son.xgs");
            banqueVague = new WaveBank(moteur, @"Content\Ressources\Audio\Wave Bank2.xwb");
            banqueSon = new SoundBank(moteur, @"Content\Ressources\Audio\Sound Bank2.xsb");
            base.LoadContent();
        }


        public override void Update(GameTime gameTime)
        {
            moteur.Update();
        }


        /// <summary>
        /// Événement qui gère le changement de volume des effets sonores.
        /// </summary>
        public void ChangementVolume(object sender, EventArgs eventArgs)
        {
            //Vérifie si c'est bien un objet InfoSonore qui est reçut.
            Option.Option.InfoSonore info = sender as Option.Option.InfoSonore;
            if (info != null)
            {
                AudioCategory categorieAudio = moteur.GetCategory("EffetSonore");
                //Change le volume des sons de la catégorie "EffetSonore".
                //Échelle de 0 à 1.
                categorieAudio.SetVolume((float)info.EffetSonore / 100);

            }
        }

        /// <summary>
        /// Événement lance un son du type "explosion" pour les explosions.
        /// </summary>
        public void SonExplosion(object sender, EventArgs eventArgs)
        {
            Cue nouveauCue = banqueSon.GetCue("explosion");
            nouveauCue.Play();
        }

        /// <summary>
        /// Événement lance un son unique pour chaque arme lorsqu'un projectile est lancé.
        /// </summary>
        public void SonLancement(object sender, EventArgs eventArgs)
        {
            if (sender is Armes)
            {
                Armes arme = (Armes)sender;
                if (arme == Armes.Roquette)
                {
                    Cue nouveauCue = banqueSon.GetCue("fire_rpg");
                    nouveauCue.Play();
                }
                else
                    if (arme == Armes.Grenade)
                    {
                        Cue nouveauCue = banqueSon.GetCue("drop_gun");
                        nouveauCue.Play();
                    }
            }
        }

        /// <summary>
        /// Événement lance un son pour l'attachement d'une mine à un bloc.
        /// </summary>
        public void MineAttachement(object sender, EventArgs eventArgs)
        {
            Mine mine = sender as Mine;
            if (mine != null)
            {
                Cue nouveauCue = banqueSon.GetCue("attach_mine");
                nouveauCue.Play();
            }
        }

        /// <summary>
        /// Événement lance un son pour l'activation d'une mine.
        /// </summary>
        public void MineActivation(object sender, EventArgs eventArgs)
        {
            Mine mine = sender as Mine;
            if (mine != null)
            {
                Cue nouveauCue = banqueSon.GetCue("ativation_mine");
                nouveauCue.Play();
            }
        }

    }
}
