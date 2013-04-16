using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using IndependentResolutionRendering;

namespace BBTA.Classe.Interface
{
    enum EtatSlider
    {
        Attente,
        Clic
    }

    class Slider
    {
        //sliderArrierePlan
        private Texture2D sliderArrierePlan; //Texture of the slider background
        private Vector2 posArrierePlan; //position of the slider background
        private int largeurArrierePlan;
        private int hauteurArrierePlan;

        //barre slider
        private Texture2D barre; //Texture of the slider background
        private Vector2 posBarre; //position of the slider background
        private int largeurBarre;
        private int hauteurBarre;

        //slider
        private Texture2D btnSlider;
        private Vector2 posSlider;
        private int largeurBouton;
        private int hauteurBouton;

        private MouseState etatAvant;
        private MouseState etatMaintenant;
        private EtatSlider etat;

        //devider that we get a value between 0 - 100 
        private float divider;

        public Slider(Texture2D sliderArrierePlan, Vector2 posArrierePlan, Texture2D barre, Texture2D btnSlider)
        {
            this.sliderArrierePlan = sliderArrierePlan;
            this.posArrierePlan = posArrierePlan;
            hauteurArrierePlan = sliderArrierePlan.Height;
            largeurArrierePlan = sliderArrierePlan.Width;

            this.barre = barre;
            this.posBarre = posArrierePlan;
            hauteurBarre = barre.Height;
            largeurBarre = barre.Width;

            this.btnSlider = btnSlider;
            this.posSlider = posArrierePlan;
            hauteurBouton = btnSlider.Height;
            largeurBouton = btnSlider.Width;

            etat = EtatSlider.Attente;

            //later we want to get values between 0 and 100 from our slider
            divider = (sliderArrierePlan.Width - btnSlider.Width) / 100f;
        }

        public void Deplacement()
        {
            etat = EtatSlider.Attente;
            etatAvant = etatMaintenant;
            etatMaintenant = Mouse.GetState();

            if (Resolution.MouseHelper.CurrentMousePosition.X >= posSlider.X - largeurBouton / 2f && Resolution.MouseHelper.CurrentMousePosition.X <= posSlider.X + largeurBouton / 2f)
            {
                if (Resolution.MouseHelper.CurrentMousePosition.Y >= posSlider.Y - hauteurBouton / 2f && Resolution.MouseHelper.CurrentMousePosition.Y <= posSlider.Y + hauteurBouton / 2f)
                {
                    //Si clic il y a, mais sans relâchement, slider peut se déplacer
                    if (etatMaintenant.LeftButton == ButtonState.Pressed)
                    {
                        etat = EtatSlider.Clic;
                    }
                    //S'il y a un maintient, on peut déplacer le slider
                    if (etatMaintenant.LeftButton == ButtonState.Pressed && etatAvant.LeftButton == ButtonState.Pressed)
                    {
                        posSlider.X = Resolution.MouseHelper.CurrentMousePosition.X;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Arriere Plan
            spriteBatch.Draw(sliderArrierePlan, posArrierePlan, null, Color.White, 0, new Vector2(largeurArrierePlan / 2f, hauteurArrierePlan / 2f), 1, SpriteEffects.None, 0);
            //Barre
            spriteBatch.Draw(barre, posArrierePlan, null, Color.White, 0, new Vector2(largeurBarre / 2f, hauteurBarre / 2f), 1, SpriteEffects.None, 0);
            //Bouton
            spriteBatch.Draw(btnSlider, posSlider, null, Color.White, 0, new Vector2(largeurBouton / 2f, hauteurBouton / 2f), 1, SpriteEffects.None, 0);

        }
    }
}
