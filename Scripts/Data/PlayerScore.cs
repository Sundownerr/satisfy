using System;

namespace Satisfy.Variables
{
    [Serializable]
    public class PlayerScore
    {
        public float finishCoins;
        public float finishMults = 1;
        public float coins;

        public float Total => (finishCoins + coins) * finishMults;

        public void Reset()
        {
            finishMults = 1;
            finishCoins = 0;
            coins = 0;
        }
    }
}