using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace BBTA.Classe.Interface
{
    class Slider
    {
        /*//slider background
        private Texture2D sliderBackground; //Texture of the slider background
        private Vector2 posBackground; //position of the slider background
        private Rectangle sliderBackgroundRectangle;

        //slider
        private Texture2D slider;
        private Vector2 posSlider;
        private Rectangle sliderRectangle;

        private Rectangle touchRectangle;     

        //says us if the player has touched the sliders background
        private bool touched = false;

        //devider that we get a value between 0 - 100 
        private float divider;

        public Slider(ContentManager content)
        {
            sliderBackground = content.Load<Texture2D>("ship/slider");
            posBackground = new Vector2(40, HelpfulGlobals.Instance.screenH
             - sliderBackground.Height);

            slider = content.Load<Texture2D>("ship/slider_button");
            posSlider = new Vector2(posBackground.X + sliderBackground.Width / 2,
            posBackground.Y);

            sliderRectangle = new Rectangle((int) posSlider.X,
            (int) posSlider.Y, slider.Width, slider.Height);

            touchRectangle = new Rectangle(0, 0, 30, 30);
            sliderBackgroundRectangle = new Rectangle((int) posBackground.X - 40,
            (int) posBackground.Y - 10, sliderBackground.Width + 80,
            sliderBackground.Height + 20);

            //later we want to get values between 0 and 100 from our slider
            divider = (sliderBackground.Width - slider.Width) / 100f;
        }

        public void Update(GameTime gameTime)
        {
            float distance = 0f;
            touched = false;

            TouchCollection touches = TouchPanel.GetState();       

            foreach (var touch in touches)
            {
                touchRectangle.X = (int) touch.Position.X - 15; // -15, because the finger is 30x30 pixel
                touchRectangle.Y = (int) touch.Position.Y - 15;          

                //finger touched on the sliders background
                if (touchRectangle.Intersects(sliderBackgroundRectangle))
                {
                    //Distance between finger and slider button
                   distance = posSlider.X - (touchRectangle.X);

                    //move the button to position of the finger 
                    if (distance < -5f)
                    {
                        posSlider.X += (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.40f;
                    }
                    if (distance > 5f)
                    {
                        posSlider.X -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.40f;
                    }

                    if (posSlider.X <= posBackground.X)
                    {
                        posSlider.X = posBackground.X;
                    }
                    if (posSlider.X >= posBackground.X + sliderBackground.Width - slider.Width)
                    {
                        posSlider.X = posBackground.X + sliderBackground.Width - slider.Width;
                    }    

                    sliderRectangle.X = (int)posSlider.X;

                    //player touched the slider
                    touched = true;
                }
            }

            if (touched == false)
            {
                //move back to the middle of the slider
                distance = posSlider.X - (posBackground.X + (sliderBackground.Width
                - slider.Width) / 2);
                if (distance < -5f)
                {
                    posSlider.X += (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.40f;
                }
                else if (distance > 5f)
                {
                    posSlider.X -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.40f;
                }
                else
                {
                    posSlider.X = posBackground.X + (sliderBackground.Width - slider.Width) / 2;
                }

                sliderRectangle.X = (int)posSlider.X;
            }

            HelpfulGlobals.Instance.sliderValue = (posSlider.X - posBackground.X) / divider;
           }
        public void Draw()
        {
            HelpfulGlobals.Instance.spriteBatch.Draw(sliderBackground, posBackground, Color.White);
            HelpfulGlobals.Instance.spriteBatch.Draw(slider, posSlider, Color.White);
        }*/

    }
}
