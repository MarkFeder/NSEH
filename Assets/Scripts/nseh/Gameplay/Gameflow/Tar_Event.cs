using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tar_Event : Event {

    //List<EventComponent> _tarComponents;
    float eventDuration = 10.0f;
    bool eventFinished = false;
    float elapsedTime;
    bool isUp = false;
    public delegate bool TarHandler();
    public static event TarHandler TarUp;
    public static event TarHandler TarDown;
    //Setup the event providing the current game instance. The event is not active here yet.
    override public void Setup(Game myGame, LevelManager lvlManager)
    {
        base.Setup(myGame, lvlManager);
        //_tarComponents = new List<EventComponent>();
    }

    //Activate the event execution.
    override public void ActivateEvent()
    {
        IsActivated = true;
    }

    
    //Event execution.
    override public void EventTick()
    {
        elapsedTime += Time.deltaTime;
        //Controls when the tar should go up
        if (elapsedTime >= 5.0f && elapsedTime < (5.0f + eventDuration) && !isUp)
        {
            //foreach(EventComponent tarComponent in _tarComponents)
            //{
                isUp = TarUp();
            //}
            
        }
        //Controls when the tar should go down
        else if (elapsedTime >= (5.0f + eventDuration) && isUp)
        {
                isUp = TarDown();
        }
        //Controls when the event cycle is completed and resets the involved variables
        else if (elapsedTime >= (5.0f + eventDuration) && !isUp)
        {
            if (eventDuration != 45.0f)
            {
                eventDuration += 5.0f;
            }
            elapsedTime = 0;
            Debug.Log("Variables are reset and tar will remain up next time: " + eventDuration + " seconds.");
        }
    }

    //Deactivates the event
    override public void EventRelease()
    {
        IsActivated = false;
    }
/*
    public void RegisterLight(MonoBehaviour componentToRegister)
    {
        _tarComponents.Add(componentToRegister);
    }
*/
}
