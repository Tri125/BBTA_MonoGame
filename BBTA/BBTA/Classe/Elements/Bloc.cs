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
    public class Bloc: Sprite
    {
        private Body corpsPhysique;
        private const float DENSITE = 0;
        private const float seuilResistance = 45;

        public Bloc(World mondePhysique, Vector2 position, Texture2D texture, float tailleCote)
            :base(texture, position)
        {
            corpsPhysique = BodyFactory.CreateRectangle(mondePhysique, tailleCote, tailleCote, DENSITE);
            corpsPhysique.IsStatic = true;
            corpsPhysique.Friction = 0.3f;
        }

        public bool Explosetil(float puissance, float rayon, Vector2 lieu)
        {
            float pente = -puissance/rayon;            
            float distance = Vector2.Distance(lieu, Position);
            if (pente * distance + puissance > seuilResistance)
            {
                return true;
            }
            else
            {
                return false;
            }	        
        }
    }
}
