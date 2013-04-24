using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BBTA.Elements;

namespace BBTA.Outils
{
    public class Camera2d
    {
        protected float zoom; // Camera Zoom
        public Matrix transform; // Matrix Transform
        public Vector2 pos; // Camera Position
        protected float rotation; // Camera Rotation
        private int hauteurCarte;
        public ObjetPhysique ObjetSuivi { get; set; }
        private bool doitSeDeplacer = false;
        public event EventHandler Verouiller;
        private Vector2 distanceParcourir;
        private const int VITESSE_TRANSITION = 1;

        public Camera2d(int hauteurCarte)
        {
            this.hauteurCarte = hauteurCarte;
            zoom = 1;
            rotation = 0.0f;
            pos = Vector2.Zero;
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        // Auxiliary function to move the camera
        public void Move(Vector2 amount)
        {
            pos += amount;
        }
        // Get set position
        public Vector2 Pos
        {
            get { return pos; }
            set { pos = value; }
        }

        public Matrix get_transformation(GraphicsDevice graphicsDevice)
        {
            transform =       // Thanks to o KB o for this solution
              Matrix.CreateTranslation(new Vector3(-pos.X, -pos.Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(IndependentResolutionRendering.Resolution.getVirtualViewport().Width * 0.5f, IndependentResolutionRendering.Resolution.getVirtualViewport().Height * 0.5f, 0));
            return transform;
        }

        public void SeDirigerVers(ObjetPhysique objet)
        {
            doitSeDeplacer = true;
            ObjetSuivi = objet;
            distanceParcourir = Vector2.Subtract(ObjetSuivi.ObtenirPosition(), pos);
        }

        public void Update(GameTime gameTime)
        {
            Vector2 positionIntermediaire = ObjetSuivi.ObtenirPosition();
            if (doitSeDeplacer)
            {
                Vector2 deplacement = distanceParcourir;
                deplacement.Normalize();
                deplacement *= gameTime.ElapsedGameTime.Milliseconds * VITESSE_TRANSITION;
                if (distanceParcourir.Length() > deplacement.Length())
                {
                    distanceParcourir -= deplacement;
                    positionIntermediaire = ObjetSuivi.ObtenirPosition() - distanceParcourir;
                }
                else
                {
                    doitSeDeplacer = false;
                    pos = positionIntermediaire;
                    if (Verouiller != null)
                    {
                        Verouiller(this, new EventArgs());
                    }
                }
            }
            pos = positionIntermediaire;

            if (ObjetSuivi.ObtenirPosition().Y + IndependentResolutionRendering.Resolution.getVirtualViewport().Height / 2f >= hauteurCarte)
            {
                Pos = new Vector2((int)pos.X, (int)hauteurCarte - IndependentResolutionRendering.Resolution.getVirtualViewport().Height / 2f);
            }
            else
            {
                Pos = new Vector2((int)pos.X, (int)pos.Y);
            }
        }
    }
}
