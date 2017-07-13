using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class MyButton : Button
{
    public List<MyEventSystem> eventSystem;
    public MyEventSystem eventCurrent;
    public int player;

    protected override void Awake()
    {
        base.Awake();
        eventSystem = GetComponent<EventSystemProvider>().eventSystem;
        player = 0;
    }

    /*
    public void Update()
    {
        foreach (MyEventSystem aux in eventSystem)
        {

            eventCurrent = aux;
           
        }
    }*/


    public override void OnSubmit(BaseEventData eventData)
    {    
        player = eventSystem.IndexOf(eventData.currentInputModule.transform.GetComponent<MyEventSystem>());
        base.OnSubmit(eventData);
    }

    /*
    public override void OnPointerDown(PointerEventData eventData)
    {
        //player = eventSystem.IndexOf(eventCurrent);
        Debug.Log("AAAA" + player);
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        // Selection tracking
        if (IsInteractable() && navigation.mode != Navigation.Mode.None)
        {
            player = eventSystem.IndexOf(eventCurrent);
            Debug.Log("AAAA" + player);
            eventCurrent.SetSelectedGameObject(gameObject, eventData);
            
        }
            
        base.OnPointerDown(eventData);
    }
    
    public override void Select()
    {
        Debug.Log("AAAAAAAAAAAAAAA");
        if (eventCurrent.alreadySelecting)
            return;

        eventCurrent.SetSelectedGameObject(gameObject);
    }*/
}
