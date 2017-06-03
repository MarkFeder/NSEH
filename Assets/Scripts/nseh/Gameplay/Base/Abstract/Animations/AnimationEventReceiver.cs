using nseh.Gameplay.Combat;
using nseh.Gameplay.Combat.Defense;
using nseh.Gameplay.Entities.Player;
using nseh.Managers.Audio;
using nseh.Managers.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace nseh.Gameplay.Base.Abstract.Animations
{
    public class AnimationEventReceiver : MonoBehaviour
    {
        #region Private Properties

        private PlayerInfo _playerInfo;
        private List<AttackType> _attackTypes;
        private List<DefenseType> _defenseTypes;

        #endregion

        #region Virtual Methods

        public virtual void Start()
        {
            _playerInfo = GetComponent<PlayerInfo>();

            _attackTypes = Enum.GetValues(typeof(AttackType)).Cast<AttackType>().ToList();
            _defenseTypes = Enum.GetValues(typeof(DefenseType)).Cast<DefenseType>().ToList();
        }

        public virtual void OnPlayAttackSound(AnimationEvent animationEvent)
        {
            AttackType type = (AttackType)animationEvent.intParameter;

            if (_attackTypes.Contains(type))
            {
                AudioController controller = _playerInfo.GetAudioControllerForAttack(type);
                if (controller != null)
                {
                    GameManager.Instance.SoundManager.PlayAudio(controller);
                }
            }
            else
            {
                Debug.LogError(string.Format("Type: {0} does not exist", type));
            }
        }

        public virtual void OnPlayDefenseSound(AnimationEvent animationEvent)
        {
            DefenseType type = (DefenseType)animationEvent.intParameter;

            if (_defenseTypes.Contains(type))
            {
                AudioController controller = _playerInfo.GetAudioControllerForDefense(type);
                if (controller != null)
                {
                    GameManager.Instance.SoundManager.PlayAudio(controller);
                } 
            }
            else
            {
                Debug.LogError(string.Format("Type: {0} does not exist", type));
            }
        }

        public virtual void OnPlayHitSound(AnimationEvent animationEvent)
        {
            AudioController controller = _playerInfo.GetAudioControllerForHit();
            if (controller != null)
            {
                GameManager.Instance.SoundManager.PlayAudio(controller);
            }
        }

        public virtual void OnPlayJumpSound(AnimationEvent animationEvent)
        {
            AudioController controller = _playerInfo.GetAudioControllerForJump();
            if (controller != null)
            {
                GameManager.Instance.SoundManager.PlayAudio(controller);
            }
        }

        #endregion
    }
}
