using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class InputController : MonoBehaviour
{
    private int playerNumber; // The order of the player regarding controller
    private ShipHandling shipHandling;
    private ShipDetails shipDetails;
    //input controls
    private string turnControl;
    private string thrustControl;
    private string primaryControl;
    private string secondaryControl;
    private InputDevice joystick;

    void Start()
    {
        //Debug.Log("Joystick count: " + InputManager.Devices.Count);
        AssignJoysticks();

    }

    private void AssignJoysticks()
    {
        shipHandling = GetComponent<ShipHandling>();
        playerNumber = (int)shipHandling.playerNumber;
        turnControl = "Horizontal" + playerNumber;
        thrustControl = "Vertical" + playerNumber;
        primaryControl = "Primary" + playerNumber;
        secondaryControl = "Secondary" + (playerNumber);
        joystick = null;

        InputDevice[] joysticks = new InputDevice[InputManager.Devices.Count];
        List<InputDevice> finalJoysticks = new List<InputDevice>();
        InputManager.Devices.CopyTo(joysticks,0);

        for(int i = 0; i < joysticks.Length; i++)
        {
            if(joysticks[i].Name.Equals("PlayStation 4 Controller") == false ) {
                //Debug.Log("Device type is " + joysticks[i].Name + ", joystick number is " + i + ", playerNumber is " + playerNumber);
                finalJoysticks.Add(joysticks[i]);
            }
        }

        //Debug.Log("finalJoysticks.Count " + finalJoysticks.Count);
        if(finalJoysticks.Count > (playerNumber - 1))
        {
            //Debug.Log("Assigning joystick to player " + playerNumber);
            joystick = finalJoysticks[(playerNumber - 1)];
        } else {
            //Debug.Log("Joystick for player " + playerNumber + " is NULL");
            joystick = null;
        }
    }

    private void Awake()
    {
        

    }

    void FixedUpdate()
    {
        AssignJoysticks();
        

        float shipTurn = 0;

        if (Input.GetAxis(turnControl) != 0)
        {
            //Debug.Log("turnControl = " + turnControl);
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


        if (Input.GetButton(secondaryControl) == true)
        {
            specialButton = Input.GetButton(secondaryControl);
        }

        if (specialButton == true)
        {
            //Debug.Log("specialButton!!");
            shipHandling.UseSecondary();
        }

        bool fireButton = false;
        if (joystick != null)
            fireButton = joystick.Action1;

   
        if (Input.GetButton(primaryControl) == true)
        {
            fireButton = Input.GetButton(primaryControl);
        }

        if ( fireButton == true)
        {
            shipHandling.UsePrimary();
        }

         

    }
}


