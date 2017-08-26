using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using nseh.Managers.Main;

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

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            // GameManager.Instance.SoundManager.PlayAudioFX(gameObject.transform.GetComponent<EventSystemProvider>().clipSelection, 1f, false, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0);
            if (gameObject.transform.GetComponent<EventSystemProvider>().selectingButton)
                gameObject.transform.GetChild(0).GetChild(eventSystem.IndexOf(eventData.currentInputModule.transform.GetComponent<MyEventSystem>())).GetComponent<Image>().enabled = true;
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            if (gameObject.transform.GetComponent<EventSystemProvider>().selectingButton)
                gameObject.transform.GetChild(0).GetChild(eventSystem.IndexOf(eventData.currentInputModule.transform.GetComponent<MyEventSystem>())).GetComponent<Image>().enabled = false;
        }

        #endregion

    }
}
