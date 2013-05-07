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
    public class GestionSon : DrawableGameComponent
    {
        private AudioEngine moteur;
        private SoundBank banqueSon;
        private WaveBank banqueVague;
        private Cue cue;


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


        public void ChangementVolume(object sender, EventArgs eventArgs)
        {
            if (sender as Option.Option.InfoSonore != null)
            {
                Option.Option.InfoSonore info = sender as Option.Option.InfoSonore;
                AudioCategory categorieAudio = moteur.GetCategory("EffetSonore");
                categorieAudio.SetVolume((float)info.EffetSonore / 100);

            }
        }


        public void SonExplosion(object sender, EventArgs eventArgs)
        {
            Cue nouveauCue = banqueSon.GetCue("explosion");
            nouveauCue.Play();
        }


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

        public void MineAttachement(object sender, EventArgs eventArgs)
        {
            Mine mine = sender as Mine;
            if (mine != null)
            {
                Cue nouveauCue = banqueSon.GetCue("attach_mine");
                nouveauCue.Play();
            }
        }

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
