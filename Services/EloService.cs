using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatMash.Services
{

    public static class EloService
    {

        public const int k = 32;

        public static double ExpectationToWin(int catOneRating, int catTwoRating)
        {
            return 1 / (1 + Math.Pow(10, (catTwoRating - catOneRating) / 400.0));
        }
        public static int CalculateDelta(int playerOneRating, int playerTwoRating)
        {
            return (int)(k * ExpectationToWin(playerOneRating, playerTwoRating));
    
        }
    }

}
