using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
namespace BBTA.Elements
{
    public class Bloc: SpriteAnimer
    {
        private Body corpsPhysique;
        private const float DENSITE = 1f;

        public Bloc(World mondePhysique, Vector2 position, Texture2D texture, float tailleCote, int nbColonnes, int nbRangees, int milliSecParImage = 50)
            :base(texture, nbColonnes, nbRangees, milliSecParImage) 
        {
            this.Position = position;
            corpsPhysique = BodyFactory.CreateRectangle(mondePhysique, tailleCote, tailleCote, DENSITE);
            corpsPhysique.IsStatic = true;
            corpsPhysique.Friction = 0.3f;
        }
    }
}
