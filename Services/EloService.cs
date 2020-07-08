using System;
namespace CatMash.Services
{
    public static class EloService
    {
        // The value of k affects the volatility of the scores.
        // If k is large, the scores are very volatile, i.e a win gives a lot of elo, and a loss make you lose a lot as well.
        public const int k = 32;

        // The probabilty that player 1 wins against player 2.
        public static double ExpectationToWin(int catOneRating, int catTwoRating)
        {
            return 1 / (1 + Math.Pow(10, (catTwoRating - catOneRating) / 400.0));
        }

        // The amount of score that player 1 steals to player 2 if he wins.
        public static int CalculateDelta(int playerOneRating, int playerTwoRating)
        {
            return (int)(k * ExpectationToWin(playerOneRating, playerTwoRating));
        }
    }
}
