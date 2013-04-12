using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BBTA.Elements
{
    public class JoueurHumain : Acteur
    {
        KeyboardState clavierAvant;
        KeyboardState clavierMaintenant;

        public JoueurHumain(World mondePhysique, Texture2D texture, Vector2 position, float pointsVie, int nbColonnes, int nbRangees, int milliSecParImage = 50)
            : base(mondePhysique, pointsVie, texture, position, nbColonnes, nbRangees, milliSecParImage)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (monTour == true)
            {
                clavierAvant = clavierMaintenant;
                clavierMaintenant = Keyboard.GetState();
            }
            base.Update(gameTime);

            if (clavierMaintenant.IsKeyDown(Keys.D))
            {
                BougerADroite();
            }

            if (clavierMaintenant.IsKeyDown(Keys.A))
            {
                BougerAGauche();
            }

            if (clavierMaintenant.IsKeyDown(Keys.Space) && !clavierAvant.IsKeyDown(Keys.Space) && estAuSol == true)
            {
                Sauter();
            }

            if (clavierMaintenant.IsKeyDown(Keys.T) && !clavierAvant.IsKeyDown(Keys.T))
            {
                CompletionTour();
            }
        }

    }
}
