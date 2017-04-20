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

        public EventSystem eventSystem;
        public GameObject selectedGameObject;

        private bool _buttonSelected;
        private float horizontal;
        private float vertical;


        // Use this for initialization
        void Start ()
        {
            eventSystem = FindObjectOfType<EventSystem>();
	    }
	
	    // Update is called once per frame
	    void Update ()
        {
            Debug.Log(Input.GetAxisRaw(String.Format("{0}{1}", Inputs.AXIS_HORIZONTAL_GAMEPAD, 1)));
            if ((Input.GetAxis(String.Format("{0}{1}", Inputs.AXIS_HORIZONTAL_GAMEPAD, 1)) != 0 || Input.GetAxisRaw(String.Format("{0}{1}", Inputs.AXIS_VERTICAL_GAMEPAD, 1)) != 0) 
                && !_buttonSelected && selectedGameObject)
            {
                eventSystem.SetSelectedGameObject(selectedGameObject);
                _buttonSelected = true;
            }
	    }

        private void OnDisable()
        {
            _buttonSelected = false;
        }
    }
}
