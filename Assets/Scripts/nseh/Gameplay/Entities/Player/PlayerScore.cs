using System;
using UnityEngine;

namespace nseh.Gameplay.Entities.Player
{
    public class PlayerScore : MonoBehaviour
    {
        #region Private Properties

        private int _currentScore;
        private PlayerInfo _playerInfo;

        #endregion

        #region Public Properties

        public int Score
        {
            get { return _currentScore; }
        }

        #endregion

        #region Private Methods

        private void Start()
        {
            _playerInfo = GetComponent<PlayerInfo>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Increases the score.
        /// </summary>
        /// <param name="amount">The amount of score to increase.</param>
        public void IncreaseScore(int amount)
        {
            if (amount > 0)
            {
                _currentScore += amount;

                Debug.Log(string.Format("Score of {0} has been increased to: {1}", _playerInfo.PlayerName, _currentScore));
            }
        }

        /// <summary>
        /// Increases the score if condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition that must be satisfied.</param>
        /// <param name="amount">The amount of score to increase.</param>
        public void IncreaseScore(Func<bool> condition, int amount)
        {
            if (condition())
            {
                IncreaseScore(amount);
            }
        }

        /// <summary>
        /// Decreases the score.
        /// </summary>
        /// <param name="amount">The amount of score to decrease.</param>
        public void DecreaseScore(int amount)
        {
            if (amount > 0)
            {
                _currentScore -= amount;

                Debug.Log(string.Format("Score of {0} has been decreased to: {1}", _playerInfo.PlayerName, _currentScore));
            }
        }

        #endregion
    }
}
