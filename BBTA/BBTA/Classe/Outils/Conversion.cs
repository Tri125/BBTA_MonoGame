using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BBTA.Classe.Outils
{
    static class Conversion
    {
        private const int RATIO_METRE_PIXEL = 40;

        public static int MetreAuPixel(int metres)
        {
            return metres * RATIO_METRE_PIXEL;
        }

        public static float MetreAuPixel(float metres)
        {
            return metres * RATIO_METRE_PIXEL;
        }

        public static Vector2 MetreAuPixel(Vector2 metres)
        {
            return metres * RATIO_METRE_PIXEL;
        }

        public static float PixelAuMetre(int pixel)
        {
            return (float)pixel / RATIO_METRE_PIXEL;
        }

        public static float PixelAuMetre(float pixel)
        {
            return pixel / RATIO_METRE_PIXEL;
        }

        public static Vector2 PixelAuMetre(Vector2 pixel)
        {
            return pixel * RATIO_METRE_PIXEL;
        }

    }
}
