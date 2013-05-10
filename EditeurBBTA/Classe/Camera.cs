using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EditeurCarteXNA
{
    public class Camera
    {
        protected float zoom;
        protected Matrix transform;
        public Matrix ViewMatrix {get {return transform;} }
        public Vector2 Position { get; protected set; }
        public float Rotation { get; set; }

        public Camera()
        {
            zoom = 1.0f;
            Rotation = 0.0f;
            Position = Vector2.Zero;
        }

        public Camera(Vector2 position)
            : this()
        {
            Position = position;
        }

        public float Zoom
        {
            get { return zoom; }
            set
            {
                zoom = value;
                if (zoom < 0.1f)
                    zoom = 0.1f;
            }
        }


        public void Move(Vector2 displacement)
        {
            Position += displacement;
        }

        public Matrix CreateTransformationMatrix(GraphicsDevice device)
        {
            transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                                                  Matrix.CreateRotationZ(Rotation) *
                                                  Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                                                  Matrix.CreateTranslation(new Vector3(device.Viewport.Width * 0.5f, device.Viewport.Height * 0.5f, 0));

            return transform;
        }

    }
}
