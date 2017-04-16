using nseh.Gameplay.Base.Abstract;
using UnityEngine;

namespace nseh.Gameplay.Combat.Defense
{
    public enum DefenseType
    {
        None = 0,
        NormalDefense = 1
    }

    public class CharacterDefense : HandledAction
    {
        #region Public C# Properties

        public DefenseType CurrentMode { get; private set; }

        #endregion

        public CharacterDefense(DefenseType defenseType, int hashAnimation, string stateName, Animator animator,
            KeyCode keyToPress = KeyCode.None,
            string buttonToPress = null)
            : base(hashAnimation, stateName, animator)
        {
            this.KeyToPress = keyToPress;
            this.ButtonToPress = buttonToPress;
            this.CurrentMode = defenseType;
        }
    }
}
