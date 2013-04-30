using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BBTA.Classe.Option;

namespace BBTA.Elements
{
    public class JoueurHumain : Acteur
    {
        private KeyboardState clavierAvant;
        private KeyboardState clavierMaintenant;
        private BBTA.Classe.Option.Option.ParametreTouche optionClavier = Game1.chargeurOption.OptionActive.InformationTouche;


        public JoueurHumain(World mondePhysique, Texture2D texture, Vector2 position, int nbColonnes, int nbRangees, int milliSecParImage = 50)
            : base(mondePhysique, texture, position, nbColonnes, nbRangees, milliSecParImage)
        {
        }

        public override void Update(GameTime gameTime)
        {

            clavierAvant = clavierMaintenant;
            clavierMaintenant = Keyboard.GetState();


            if (Mouse.GetState().LeftButton == ButtonState.Pressed && enModeTir == false  && monTour == true && estAuSol == true)
            {
                Tirer();
            }

            base.Update(gameTime);

            if (!enModeTir && monTour == true)
            {

                if (clavierMaintenant.IsKeyDown(optionClavier.Droite))
                {
                    BougerADroite();
                }

                if (clavierMaintenant.IsKeyDown(optionClavier.Gauche))
                {
                    BougerAGauche();
                }

                if (clavierMaintenant.IsKeyDown(optionClavier.Saut) && !clavierAvant.IsKeyDown(optionClavier.Saut) && estAuSol == true)
                {
                    Sauter();
                }
            }
        }
    }
}