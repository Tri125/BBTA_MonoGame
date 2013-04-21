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
        protected bool EstEnMain = true;

        public Projectile(Game jeu, World mondePhysique, Shape forme, Vector2 positionDepart, float energieExplosion)
            : base(jeu, mondePhysique, forme)
        {
            this.mondePhysique = mondePhysique;
            this.energieExplosion = energieExplosion;
            corpsPhysique.IsBullet = true;
            corpsPhysique.Position = positionDepart;
            corpsPhysique.BodyType = BodyType.Dynamic;
            Explose = false;
        }

        public override void Update(GameTime gameTime)
        {
            corpsPhysique.Rotation = (float)Math.Atan2(corpsPhysique.LinearVelocity.Y, corpsPhysique.LinearVelocity.X);
            base.Update(gameTime);
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
