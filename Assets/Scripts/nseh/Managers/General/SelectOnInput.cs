using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace nseh.Managers.General
{ 
    public class SelectOnInput : MonoBehaviour
    {

        public EventSystem eventSystem;
        public GameObject selectedGameObject;

        private bool _buttonSelected;

	    // Use this for initialization
	    void Start ()
        {
            eventSystem = FindObjectOfType<EventSystem>();
	    }
	
	    // Update is called once per frame
	    void Update ()
        {
		    if ((Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0) 
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
