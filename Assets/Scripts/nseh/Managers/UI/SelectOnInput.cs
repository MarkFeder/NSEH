using System;
using UnityEngine;
using System.Collections.Generic;
using Inputs = nseh.Utils.Constants.Input;

namespace nseh.Managers.UI
{ 
    public class SelectOnInput : MonoBehaviour
    {

        #region Private Properties

        private bool _buttonSelected;

        #endregion

        #region Public Properties

        public List<MyEventSystem> eventSystem;
        public List<GameObject> selectedGameObject;

		#endregion

		#region Private Methods

		private void Start()
		{
            foreach(MyEventSystem aux in eventSystem)
            {
                aux.SetSelectedGameObject(selectedGameObject[eventSystem.IndexOf(aux)]);
            }
			
			_buttonSelected = true;
		}

		private void Update()
		{
			if ((Input.GetAxis(String.Format("{0}{1}", Inputs.AXIS_HORIZONTAL_GAMEPAD, 1)) != 0 || Input.GetAxisRaw(String.Format("{0}{1}", Inputs.AXIS_VERTICAL_GAMEPAD, 1)) != 0)
				&& !_buttonSelected)
			{
                foreach (MyEventSystem aux in eventSystem)
                {
                    aux.SetSelectedGameObject(selectedGameObject[eventSystem.IndexOf(aux)]);
                }
                _buttonSelected = true;
			}
		}

        private void OnDisable()
        {
            _buttonSelected = false;
        }

        #endregion

    }
}
