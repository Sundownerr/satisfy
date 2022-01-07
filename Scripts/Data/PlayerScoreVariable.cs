using System;
using UnityEngine;

namespace Satisfy.Variables
{
    [CreateAssetMenu(fileName = "PlayerScore", menuName = "Variables/Player Score")]
    [Serializable]
    public class PlayerScoreVariable : Variable<PlayerScore>
    {
        public void AddCoins(int value)
        {
            Value.coins += value;
            // Changed.OnNext(this);
        }

        public void RemoveCoins(int value)
        {
            Value.coins -= value;
            // Changed.OnNext(this);
        }

        public void SetCoins(int value)
        {
            Value.coins = value;
            // Changed.OnNext(this);
        }

        public void SetMults(int value)
        {
            Value.finishMults = value;
            // Changed.OnNext(this);
        }

    }
}