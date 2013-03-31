using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BBTA.Elements

namespace BBTA
{
    /// <summary>
    /// La classe contient
    /// </summary>
    public class Carte
    {
        private Texture2D textureArrierePlan;
        private Texture2D texturesBlocs;
        private const float TAILLE_BLOC = 1f;
        private Bloc[] blocs;
        public Carte(int[] donneesBlocs, int largeurCarte, Texture2D arrierePlan, Texture2D texturesBlocs, World mondePhysique, float MetrePixel)
        {
            this.textureArrierePlan = arrierePlan;
            this.texturesBlocs = texturesBlocs;

            blocs = new Bloc[donneesBlocs.Length];
            for(int compteurBlocs = 0; compteurBlocs < donneesBlocs.Length; compteurBlocs++)
            {
                if(donneesBlocs[compteurBlocs] != 0)
                {
                    Vector2 positionBloc = new Vector2(compteurBlocs%largeurCarte*MetrePixel, compteurBlocs/largeurCarte*MetrePixel);
                    blocs[compteurBlocs] = new Bloc(mondePhysique, positionBloc, texturesBlocs, TAILLE_BLOC);
                }                
            }
        }

        public void Explosion(Vector2 lieu, float rayon, float puissance)
        {
            float pente = -puissance/rayon;
            for (int compteurBloc = 0; compteurBloc < blocs.Length; compteurBloc++)
	        {
                if(blocs[compteurBloc].Explosetil(puissance, rayon, lieu))
                {
                    blocs[compteurBloc] = null;
                }
	        }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureArrierePlan, Vector2.Zero, Color.White);
            foreach (Bloc item in blocs)
            {
                spriteBatch.Draw(texturesBlocs, item.Position, Color.White);
            }
        }
    }
}
