using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EditeurCarteXNA
{
    public class TuileEditeur
    {
        private Rectangle bloc;
        private int tileID;

        public int TileId
        {
            get
            {
                return tileID;
            }
            set
            {
                tileID = value;
            }
        }

        public Rectangle Bloc
        {
            get
            {
                return bloc;
            }
        }

        public TuileEditeur()
        {
            tileID = 0;
        }

        public TuileEditeur(Rectangle bloc, int tileID = 0)
        {
            this.bloc = bloc;
            this.tileID = tileID;
        }
    }
}
