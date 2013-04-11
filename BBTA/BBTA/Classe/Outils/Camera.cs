using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BBTA.Outils
{
    public class Camera2d
    {
        protected float zoom; // Camera Zoom
        public Matrix transform; // Matrix Transform
        public Vector2 pos; // Camera Position
        protected float rotation; // Camera Rotation

        public Camera2d()
        {
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

        public void SuivreObjet(Vector2 positionJoueur, int longueurCarte, int hauteurCarte)
        {

            if (positionJoueur.X <= IndependentResolutionRendering.Resolution.getVirtualViewport().Width / 2f)
            {
                Pos = new Vector2((int)IndependentResolutionRendering.Resolution.getVirtualViewport().Width/2f, (int)Pos.Y);
            }

            else if (positionJoueur.X - IndependentResolutionRendering.Resolution.getVirtualViewport().Width / 2f > longueurCarte)
            {
                Pos = new Vector2((int)longueurCarte + 5 - IndependentResolutionRendering.Resolution.getVirtualViewport().Width / 2f, (int)Pos.Y);
            }

            else
            {
                Pos = new Vector2((int)positionJoueur.X, (int)Pos.Y);
            }

            if (positionJoueur.Y + IndependentResolutionRendering.Resolution.getVirtualViewport().Height / 2f < hauteurCarte)
            {
                Pos = new Vector2((int)Pos.X, (int)positionJoueur.Y);
            }

            else
            {
                Pos = new Vector2((int)Pos.X, (int)IndependentResolutionRendering.Resolution.getVirtualViewport().Height/2f);
            }
        }
    }
}
