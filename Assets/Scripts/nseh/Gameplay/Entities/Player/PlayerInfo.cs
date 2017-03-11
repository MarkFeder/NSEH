using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inputs = nseh.Utils.Constants.Input;
using nseh.Gameplay.Movement;

public class PlayerInfo : MonoBehaviour {

    //Container class: Contains properties about players choices and level logic variables related to prefab
    private float horizontal;
    private float vertical;
    private int gamepadIndex;
    [Range(1, 4)]
    private int player;
    private bool teletransported;
    [SerializeField]
    private Sprite characterPortrait;

    public float Horizontal
    {
        get
        {
            return horizontal;
        }

        set
        {
            horizontal = value;
        }
    }

    public float Vertical
    {
        get
        {
            return vertical;
        }

        set
        {
            vertical = value;
        }
    }

    public int GamepadIndex
    {
        get
        {
            return gamepadIndex;
        }

        set
        {
            gamepadIndex = value;
        }
    }

    public int Player
    {
        get
        {
            return player;
        }

        set
        {
            player = value;
        }
    }

    public bool Teletransported
    {
        get
        {
            return teletransported;
        }

        set
        {
            teletransported = value;
        }
    }

    public Sprite CharacterPortrait
    {
        get
        {
            return (this.characterPortrait) ? this.characterPortrait : null;
        }
    }

    void Start()
    {
        this.GamepadIndex = this.gameObject.GetComponent<PlayerMovement>().gamepadIndex;
        this.Teletransported = false;
    }

    void Update()
    {
        this.Horizontal = Input.GetAxis(String.Format("{0}{1}", Inputs.AXIS_HORIZONTAL_GAMEPAD, this.GamepadIndex));
        this.Vertical = Input.GetAxis(String.Format("{0}{1}", Inputs.AXIS_VERTICAL_GAMEPAD, this.GamepadIndex));
    }             
}
