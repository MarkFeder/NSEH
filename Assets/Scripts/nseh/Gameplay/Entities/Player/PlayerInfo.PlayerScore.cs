using System;
using UnityEngine;

namespace nseh.Gameplay.Entities.Player
{
    public partial class PlayerInfo : MonoBehaviour
    {

        #region Public Methods

        public void IncreaseScore(int amount)
        {
            if (amount > 0)
            {
                _currentScore += amount;
            }
        }

        public void IncreaseScore(Func<bool> condition, int amount)
        {
            if (condition())
            {
                IncreaseScore(amount);
            }
        }

        public void DecreaseScore(int amount)
        {
            if (amount > 0)
            {
                _currentScore -= amount;
            }
        }

        #endregion

    }
}
