using nseh.Gameplay.Base.Abstract;
using nseh.Gameplay.Base.Interfaces;
using nseh.Gameplay.Combat;
using nseh.Gameplay.Combat.Defense;
using nseh.Gameplay.Combat.System;
using nseh.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Tags = nseh.Utils.Constants.Tags;

namespace nseh.Gameplay.Entities.Player
{
    [RequireComponent(typeof(PlayerInfo))]
    public class PlayerCombat : MonoBehaviour
    {
        #region Private Properties

        [SerializeField]
        private GameObject _actionsHolderGO;

        private List<IAction> _playerActions;
        private List<Collider> _colliders;
        private PlayerInfo _playerInfo;

        #endregion

        #region Public C# Properties

        public List<IAction> Actions
        {
            get { return _playerActions; }
        }

        public int CurrentHashAnimation
        {
            get { return (_playerInfo.Animator.GetCurrentAnimatorStateInfo(0).shortNameHash); }
        }

        public IAction CurrentAction
        {
            get { return _playerActions.Where(act => act.Hash == CurrentHashAnimation).FirstOrDefault(); }
        }

        #endregion

        #region Private Methods

        private void Awake()
        {
            _colliders = gameObject.GetSafeComponentsInChildren<Collider>().Where(c => c.tag.Equals(Tags.WEAPON)).ToList();
        }

        private void Start()
        {
            _playerInfo = GetComponent<PlayerInfo>();

            _playerActions = _actionsHolderGO.GetComponents<HandledAction>().Cast<IAction>().ToList();
        }

        /// <summary>
        /// Init all player actions. It is useful for making a gameobject 
        /// which contains all the player's actions on runtime.
        /// </summary>
        private void InitPlayerActions()
        {
            foreach (HandledAction baseAction in _playerActions)
            {
                // Grab info from prefabs references and attach them to real go holder actions

                Component co = _actionsHolderGO.AddComponent(baseAction.GetType());

                if (baseAction.GetType() == typeof(CharacterAttack))
                {
                    CharacterAttack ca = baseAction as CharacterAttack;
                    (co as CharacterAttack).InitAttackAction(ca.CurrentDamage, ca.InitialDamage, ca.AttackType);
                }

                if (baseAction.GetType() == typeof(CharacterDefense))
                {
                    CharacterDefense cd = baseAction as CharacterDefense;
                    (co as CharacterDefense).InitDefenseAction(cd.CurrentMode);
                }
            }
        }

        private void Update()
        {
            if (CurrentAction != null)
            {
                Debug.Log(string.Format("[{0}] - Current action is: {1}", _playerInfo.PlayerName, CurrentAction.ToString()));
            }
        } 

        #endregion

        #region Public Methods

        public void DeactivateSpecificCollider(int index)
        {
            if (_colliders != null && _colliders.Count > 0)
            {
                // Deactivate specific colliders
                for (int i = 0; i < _colliders.Count; i++)
                {
                    Collider collider = _colliders[i];
                    WeaponCollision weaponCollision = collider.GetComponent<WeaponCollision>();

                    if (weaponCollision.Index == index)
                    {
                        collider.enabled = false;
                        weaponCollision.enabled = false;
                    }
                }
            }
            else
            {
                Debug.Log(String.Format("DeactivateCollider({0}): colliders are 0 or null", index));
            }
        }

        #endregion

        #region Animation Events
        
        /// <summary>
        /// Activate the collider. This event is triggered by the animation.
        /// </summary>
        /// <param name="index">The weapon to be activated.</param>
        public void ActivateCollider(int index)
        {
            if (_colliders != null && _colliders.Count > 0)
            {
                // Deactivate other colliders
                for (int i = 0; i < _colliders.Count; i++)
                {
                    Collider collider = _colliders[i];
                    WeaponCollision weaponCollision = collider.GetComponent<WeaponCollision>();

                    if (weaponCollision.Index != index)
                    {
                        collider.enabled = false;
                        weaponCollision.enabled = false;
                    }
                    else
                    {
                        collider.enabled = true;
                        weaponCollision.enabled = true;
                    }
                }
            }
            else
            {
                Debug.Log(String.Format("ActivateCollider({0}): colliders are 0 or null", index));
            }
        }

        /// <summary>
        /// Deactivate the collider. This event is triggered by the animation.
        /// </summary>
        /// <param name="index">The weapon to be deactivated.</param>
        public void DeactivateCollider(int index)
        {
            if (_colliders != null && _colliders.Count > 0)
            {
                // Deactivate all the colliders
                for (int i = 0; i < _colliders.Count; i++)
                {
                    Collider collider = _colliders[i];
                    WeaponCollision weaponCollision = collider.GetComponent<WeaponCollision>();

                    collider.enabled = false;
                    weaponCollision.enabled = false;
                }
            }
            else
            {
                Debug.Log(String.Format("DeactivateCollider({0}): colliders are 0 or null", index));
            }
        }

        #endregion
    }
}
