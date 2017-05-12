using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Inputs = nseh.Utils.Constants.Input;
using InputUE = UnityEngine.Input;


namespace nseh.Managers.General
{ 
    public class SelectOnInput : MonoBehaviour
    {

        #region Private Properties
        private bool _buttonSelected;
        #endregion

        #region Public Properties
        public EventSystem eventSystem;
        public GameObject selectedGameObject;
        #endregion

        #region Public Methods
        // Use this for initialization
        public void Start ()
        {
            eventSystem = FindObjectOfType<EventSystem>();
            eventSystem.SetSelectedGameObject(selectedGameObject);
            _buttonSelected = true;
        }
	
	    // Update is called once per frame
	    public void Update ()
        {
            //Debug.Log(Input.GetAxisRaw(String.Format("{0}{1}", Inputs.AXIS_HORIZONTAL_GAMEPAD, 1)));
            if ((Input.GetAxis(String.Format("{0}{1}", Inputs.AXIS_HORIZONTAL_GAMEPAD, 1)) != 0 || Input.GetAxisRaw(String.Format("{0}{1}", Inputs.AXIS_VERTICAL_GAMEPAD, 1)) != 0) 
                && !_buttonSelected && selectedGameObject)
            {
                eventSystem.SetSelectedGameObject(selectedGameObject);
                _buttonSelected = true;
            }
	    }
        #endregion

        #region Private Methods
        private void OnDisable()
        {
            _buttonSelected = false;
        }
        #endregion
    }
}
