using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBTA.Classe.Outils
{
    static class Conversion
    {
        private const int RATIO_METRE_PIXEL = 40;

        static int MetreAuPixel(int metres)
        {
            return metres * RATIO_METRE_PIXEL;
        }

        static float MetreAuPixel(float metres)
        {
            return metres * RATIO_METRE_PIXEL;
        }

        static float PixelAuMetre(int pixel)
        {
            return (float)pixel / RATIO_METRE_PIXEL;
        }

        static float PixelAuMetre(float pixel)
        {
            return pixel / RATIO_METRE_PIXEL;
        }

    }
}
