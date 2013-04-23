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

    public class Slider
    {
        //sliderArrierePlan
        private Texture2D sliderArrierePlan; //Texture of the slider background
        private Vector2 posArrierePlan; //position of the slider background
        private int largeurArrierePlan;
        private int hauteurArrierePlan;

        //barre slider
        private Texture2D barre; //Texture of the slider background
        private Vector2 posBarre; //position of the slider background
        private float largeurBarre;
        private float hauteurBarre;

        //slider
        private Texture2D btnSlider;
        private Vector2 posSlider;
        private float largeurBouton;
        private float hauteurBouton;

        private MouseState etatAvant;
        private MouseState etatMaintenant;
        private EtatSlider etat;

        private float pourcentage;

        public Slider(Texture2D sliderArrierePlan, Vector2 posArrierePlan, Texture2D barre, Texture2D btnSlider, float pourcentage)
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

            posSlider.X = pourcentage * (largeurBarre - 60) + (posArrierePlan.X - largeurBarre / 2f + 30);
        }

        public void Deplacement()
        {
            etat = EtatSlider.Attente;
            etatAvant = etatMaintenant;
            etatMaintenant = Mouse.GetState();

            pourcentage = (posSlider.X - (posArrierePlan.X - largeurBarre / 2f + 30)) / (largeurBarre - 60);

            if (Resolution.MouseHelper.CurrentMousePosition.Y >= posArrierePlan.Y - hauteurArrierePlan / 2f &&
                Resolution.MouseHelper.CurrentMousePosition.Y <= posArrierePlan.Y + hauteurArrierePlan / 2f)
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
                //Dépasse borne par la gauche
                if (posSlider.X < posArrierePlan.X - largeurBarre / 2f + 30)
                {
                    posSlider.X = posArrierePlan.X - largeurBarre / 2f + 30;
                }
                //Dépasse borne par la droite
                if (posSlider.X > posArrierePlan.X + largeurBarre / 2f - 30)
                {
                    posSlider.X = posArrierePlan.X + largeurBarre / 2f - 30;
                }
            }
        }

        public int ObtenirPourcentage()
        {
            return (int)(pourcentage * 100);
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
