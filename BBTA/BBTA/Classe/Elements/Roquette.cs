using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BBTA.Classe.Outils;

namespace BBTA.Classe.Elements
{
    /// <summary>
    /// Une roquette est un projectile qui explose au moment où il entre en collision avec un objet.
    /// </summary>
    public class Roquette : Projectile
    {
        //Constantes------------------------------------------------------------------------------------------------------------------
        private const int RAYON_EXPLOSION_MAX = 5;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="mondePhysique">Monde physique Farseer dans évoluera la roquette</param>
        /// <param name="texture">Spritesheet contenant la texture de la roquette</param>
        /// <param name="positionSpriteSheet">Position de l'image de la roquette dans la spritesheet</param>
        /// <param name="positionDepart">Position initiale de la roquette</param>
        /// <param name="vitesse">Vitesse initiale de la roquette</param>
        public Roquette(World mondePhysique, Texture2D texture, Rectangle positionSpriteSheet, Vector2 positionDepart, Vector2 vitesse)
            : base(mondePhysique, new PolygonShape(PolygonTools.CreateRectangle(Conversion.PixelAuMetre(positionSpriteSheet.Width), Conversion.PixelAuMetre(positionSpriteSheet.Height)), 1), 
                   texture, positionSpriteSheet, positionDepart, vitesse, RAYON_EXPLOSION_MAX)
        {
            corpsPhysique.FixedRotation = true;
            corpsPhysique.OnCollision += new OnCollisionEventHandler(corpsPhysique_OnCollision);
            //La roquette doit entrer en collision avec les joueurs
            corpsPhysique.CollidesWith = Category.All;
            corpsPhysique.CollisionCategories = Category.All;
        }

        /// <summary>
        /// Met à jour la position et l'angle de la roquette en fonction de sa vitesse
        /// </summary>
        /// <param name="gameTime">Temps du jeu</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            corpsPhysique.Rotation = (float)Math.Atan2(corpsPhysique.LinearVelocity.Y, corpsPhysique.LinearVelocity.X); //L'angle de rotation est déterminé par sa vitesse.
        }

        /// <summary>
        /// Lorsque la roquette entre en collision, une variable est mise à jour pour indiquer le déclanchement de l'explosion.
        /// Elle sera prise en compte dans le Update de la classe mère.
        /// </summary>
        /// <param name="fixtureA"></param>
        /// <param name="fixtureB"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        bool corpsPhysique_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            explose = true;
            return true;
        }
    }
}