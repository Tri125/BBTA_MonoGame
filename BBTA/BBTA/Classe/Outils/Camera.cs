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
    /* =========================================================================================
     * 
     * La base de cette caméra provient d'un tutoriel trouvé sur le web.
     * Auteur: David Amador
     * Adresse URL: http://www.david-amador.com/2009/10/xna-camera-2d-with-zoom-and-rotation/
     * Elle fut ensuite modifiée pour satisfaire nos besoins.
     * 
     * ========================================================================================= */


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

        /// <summary>
        /// Permet de définir un point vers lequel la caméra se dirigera graduellement.
        /// </summary>
        /// <param name="objet">Objet vers lequel se diriger</param>
        public void SeDirigerVers(ObjetPhysique objet)
        {
            doitSeDeplacer = true;
            ObjetSuivi = objet;
            distanceParcourir = Vector2.Subtract(ObjetSuivi.ObtenirPosition(), pos);
        }

        /// <summary>
        /// Permet le déplacement de la caméra.
        /// Suit un joueur en jeu.
        /// Permet de déplacer graduellement la caméra vers un point défini.
        /// Repositionne la caméra pour empêcher de voir à l'extérieur de la carte.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            Vector2 positionIntermediaire = ObjetSuivi.ObtenirPosition(); 
            if (doitSeDeplacer)
            {
                Vector2 deplacement = distanceParcourir;
                deplacement.Normalize();
                deplacement *= gameTime.ElapsedGameTime.Milliseconds * VITESSE_TRANSITION;
                /* Si la distance à parcourir est encore plus grande que le déplacement possible durant l'exécution de cette méthode, 
                 * alors o se déplace simplement et on réajuste la distance totale à parcourir. */
                if (distanceParcourir.Length() > deplacement.Length())
                {
                    distanceParcourir -= deplacement;
                    positionIntermediaire = ObjetSuivi.ObtenirPosition() - distanceParcourir;
                }
                /* Si la distance de déplacement possible suffit à se rendre au point désirer, on se positionne immédiatement dessus et
                 * et on déclanche un événement pour indiquer que la caméra a terminé son déplacement */
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

            //Si la caméra est trop près des extrémités, on réajuste sa position pour ne pas voir à l'extérieur de la carte.
            if (positionIntermediaire.Y + IndependentResolutionRendering.Resolution.getVirtualViewport().Height / 2f >= hauteurCarte)
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
