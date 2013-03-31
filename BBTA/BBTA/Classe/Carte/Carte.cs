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
        //Je provoque un conflit de merge.
        private Texture2D textureArrierePlan;
        private Texture2D texturesBlocs;

        private Bloc[] blocs;
        public Carte(int[] donneesBlocs, int largeurCarte, Texture2D arrierePlan, Texture2D texturesBlocs, World mondePhysique)
        {
            this.textureArrierePlan = arrierePlan;
            this.texturesBlocs = texturesBlocs;

            blocs = new Bloc[donneesBlocs.Length];
            for(int compteurBlocs = 0; compteurBlocs < donneesBlocs.Length; compteurBlocs++)
            {
                blocs[compteurBlocs] = new Bloc(mondePhysique, new Vector2(compteurBlocs * TAILLE_BLOC, compteurBlocs / largeurCarte * TAILLE_BLOC)
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureArrierePlan, Vector2.Zero, Color.White);
            foreach (Body item in blocs)
            {
                spriteBatch.Draw(texturesBlocs, item.Position, Color.White);
            }
        }
    }
}
