﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BBTA.Classe.GestionSon
{
    public class GestionMusique : DrawableGameComponent
    {
        //teste
        private AudioEngine moteur;
        private SoundBank banqueSon;
        private WaveBank banqueVague;
        private Cue cue;

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

        public void ChangementEtatJeu(object sender, EventArgs eventArgs)
        {
            if (sender is BBTA.EtatJeu)
            {

                Cue nouveauCue = ObtenirCue((int)(BBTA.EtatJeu)sender);
                if (cue == null)
                {
                    cue = nouveauCue;
                    cue.Play();
                }
                else
                {
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
