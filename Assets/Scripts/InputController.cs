using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class InputController : MonoBehaviour
{
    private int playerNumber = 1; // The order of the player regarding controller
    private ShipHandling shipHandling;
    private ShipDetails shipDetails;
    //input controls
    private string turnControl = "Vertical1";
    private string thrustControl = "Horizontal1";
    private string shootControl = "Fire1";
    private string specialControl = "Special1";
    private InputDevice joystick;

    void Start()
    {
        shipHandling = GetComponent<ShipHandling>();
        playerNumber = (int)shipHandling.playerNumber; 
        turnControl = "Horizontal" + playerNumber;
        thrustControl = "Vertical" + playerNumber;
        shootControl = "Fire" + playerNumber;
        specialControl = "Special" + (playerNumber);
        
       
    }

    private void Awake()
    {
        foreach(InputDevice currDevice in InputManager.Devices)
        {
            Debug.Log("Device type is " + currDevice.Name);
        }

        Debug.Log("Joystick count: " + InputManager.Devices.Count);

        if(InputManager.Devices.Count >= playerNumber)
        {
            joystick = InputManager.Devices[(playerNumber-1)];
        } else
        {
            joystick = null;
        }


        //joystick = InputManager.ActiveDevice;
        
    }

    void Update()
    {
        float shipTurn = 0;


        if (Input.GetAxis(turnControl) != 0)
        {
            Debug.Log("turnControl = " + turnControl);
            shipTurn = Input.GetAxis(turnControl);
        }
        else
        {
            if(joystick != null)
                shipTurn = joystick.LeftStickX + (-1*joystick.DPadLeft) + joystick.DPadRight;
        }

        shipHandling.RotateShip(shipTurn);

        float speedValue = 0;
        if (joystick != null)
            speedValue = joystick.RightTrigger + (-1* joystick.LeftTrigger);

        if (Input.GetAxis(thrustControl) > 0)
        {
            speedValue = Input.GetAxis(thrustControl);
        }

        shipHandling.MoveShip(speedValue);


        bool specialButton = false;
        if (joystick != null)
            specialButton = joystick.Action3;


        if (Input.GetButton(specialControl) == true)
        {
            specialButton = Input.GetButton(specialControl);
        }

        if (specialButton == true)
        {
            //Debug.Log("specialButton!!");
            shipHandling.UseSecondary();
        }

        bool fireButton = false;
        if (joystick != null)
            fireButton = joystick.Action1;

   
        if (Input.GetButton(shootControl) == true)
        {
            fireButton = Input.GetButton(shootControl);
        }

        if ( fireButton == true)
        {
            //Debug.Log("fireButton!!");
            shipHandling.UsePrimary();
        }



       

    }

        
}


