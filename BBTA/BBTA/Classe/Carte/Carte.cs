using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BBTA
{
    public class Carte
    {
        private Texture2D arrierePlan;
        private Texture2D texturesBlocs;

        private Body[] blocs;
        private const float TAILLE_BLOC = 1f;
        private const float DENSITE = 1f;
        public Carte(int[] donneesBlocs, int largeurCarte, Texture2D arrierePlan, Texture2D texturesBlocs, World mondePhysique)
        {
            this.arrierePlan = arrierePlan;
            this.texturesBlocs = texturesBlocs;

            blocs = new Body[donneesBlocs.Length];
            for(int compteurBlocs = 0; compteurBlocs < donneesBlocs.Length; compteurBlocs++)
            {
                blocs[compteurBlocs] = BodyFactory.CreateRectangle(mondePhysique, TAILLE_BLOC, TAILLE_BLOC, DENSITE,
                                                   new Vector2(compteurBlocs * TAILLE_BLOC, compteurBlocs / largeurCarte * TAILLE_BLOC));
                blocs[compteurBlocs].IsStatic = true;
                blocs[compteurBlocs].Friction = 0.5f;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Body item in blocs)
            {
                spriteBatch.Draw(texturesBlocs, item.Position, Color.White);
            }
        }
    }
}
