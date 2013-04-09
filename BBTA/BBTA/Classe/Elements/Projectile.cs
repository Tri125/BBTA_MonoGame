using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using BBTA.Partie_De_Jeu;

namespace BBTA.Elements
{
    public abstract class Projectile:ObjetPhysique
    {
        private float energieExplosion;
        private World mondePhysique;
        public bool Explose { get; protected set; }

        public Projectile(World mondePhysique, Shape forme, Vector2 vitesseDepart, Vector2 positionDepart, Texture2D texture, float energieExplosion)
            : base(texture, mondePhysique, forme)
        {
            this.mondePhysique = mondePhysique;
            this.energieExplosion = energieExplosion;
            corpsPhysique.BodyType = BodyType.Dynamic;
            corpsPhysique.Rotation = (float)Math.Atan2(vitesseDepart.Y, vitesseDepart.X);
            Explose = false;
        }

        public void Update(GameTime gameTime)
        {
            corpsPhysique.Rotation = (float)Math.Atan2(corpsPhysique.LinearVelocity.Y, corpsPhysique.LinearVelocity.X);
        }

        public void Explosion(Carte carte, List<Acteur> Acteurs)
        {
            carte.Explosion(corpsPhysique.Position*40, energieExplosion);
            for (int compteurJoueur = 0; compteurJoueur < Acteurs.Count; compteurJoueur++)
            {
                Acteurs[compteurJoueur].RecevoirDegat(energieExplosion, corpsPhysique.Position);
            }
        }

    }
}
