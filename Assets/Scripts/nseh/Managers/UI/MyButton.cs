using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

namespace nseh.Managers.UI
{

    public class MyButton : Button
    {

        #region Public Properties

        public List<MyEventSystem> eventSystem;
        public MyEventSystem eventCurrent;
        public int player;

        #endregion

        #region Protected Methods

        protected override void Awake()
        {
            base.Awake();
            eventSystem = GetComponent<EventSystemProvider>().eventSystem;
            player = 0;
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            player = eventSystem.IndexOf(eventData.currentInputModule.transform.GetComponent<MyEventSystem>());
            base.OnSubmit(eventData);
        }

        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
            //Debug.Log("AAAAAAAAAAAA " + eventData.selectedObject.name);
            //Debug.Log("BBBBBBBBBBBB " + eventData.currentInputModule.transform.name);
            //sonido de pasada
        }

        #endregion

    }
}
