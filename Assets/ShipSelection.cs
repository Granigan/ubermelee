using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class ShipSelection : MonoBehaviour {

    public int playerNumber = 1;
    private string turnControl;
    private string thrustControl;
    private string primaryControl;
    private string secondaryControl;
    private InputDevice joystick;
    private UnityEngine.UI.Text textField;
    public KeyValuePair<string, int>[] shipArray;
    public List<KeyValuePair<string, int>> shipList = new List<KeyValuePair<string, int>>();

    private int currentSelectionIndex = 1;
    private bool axisInUse = false;
    public bool SelectionEnabled = false;

    // Use this for initialization
    void Start () {
        GameObject textGO = GameObject.Find("Player" + playerNumber + "UIPanel");
        textField = textGO.GetComponentInChildren<UnityEngine.UI.Text>();

        textField.text = "";

        shipList.Add(new KeyValuePair<string, int>("RANDOM", 0));
        shipList.Add(new KeyValuePair<string, int>("Daredevil", 31));
        shipList.Add(new KeyValuePair<string, int>("Gunship", 47));
        shipList.Add(new KeyValuePair<string, int>("The Mouse", 63));
        shipList.Add(new KeyValuePair<string, int>("Musketeer", 64));
        shipArray = shipList.ToArray();

    }
	
	// Update is called once per frame
	void Update () {
        AssignJoysticks();

        //if(Input.GetAxis(turnControl) > 0)
        if (Input.GetAxisRaw(turnControl) == 1 || joystick.LeftStickX > 0 || joystick.DPadLeft > 0)
        {
            if (axisInUse == false)
            {
                axisInUse = true;
                currentSelectionIndex++;
            }

        } //else if (Input.GetAxis(turnControl) < 0)
        if (Input.GetAxisRaw(turnControl) == -1 || joystick.LeftStickX < 0 || joystick.DPadLeft < 0)
        {
            if (axisInUse == false)
            {
                axisInUse = true;
                currentSelectionIndex--;
            }
        }

        if(currentSelectionIndex < 0)
        {
            currentSelectionIndex = 0;
        }

        if (currentSelectionIndex >= (shipArray.Length-1))
        {
            //Debug.Log("shipArray.Length = " + (shipArray.Length-1));
            currentSelectionIndex = (shipArray.Length-1);
        }

        textField.text = shipArray[currentSelectionIndex].Key.ToString();

        if (Input.GetAxisRaw(turnControl) == 0 )
        {
            axisInUse = false;
        }

        
        // Check if ship is selected by button press
        if ((Input.GetButton(primaryControl) == true || joystick.Action1 == true) && SelectionEnabled == true)
        {
            int selectedShipID = 0;
            if(shipArray[currentSelectionIndex].Value == 0)
            {
                selectedShipID = getRandomShipID();
            } else
            {
                selectedShipID = shipArray[currentSelectionIndex].Value;
            }
            GameObject clone = GameObject.FindGameObjectWithTag("Camera").GetComponent<SceneBuilder>().InstantiateShip(playerNumber, selectedShipID);
            clone.GetComponent<ShipHandling>().playerNumber = playerNumber;
            // Hide ShipSelection and disable it's input
            this.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
            SelectionEnabled = false;
            
        }



    }

    public int getRandomShipID()
    {
        return shipArray[Random.Range(1, shipArray.Length)].Value;

    }






    private void AssignJoysticks()
    {
        turnControl = "Horizontal" + playerNumber;
        thrustControl = "Vertical" + playerNumber;
        primaryControl = "Primary" + playerNumber;
        secondaryControl = "Secondary" + (playerNumber);
        joystick = null;

        InputDevice[] joysticks = new InputDevice[InputManager.Devices.Count];
        List<InputDevice> finalJoysticks = new List<InputDevice>();
        InputManager.Devices.CopyTo(joysticks, 0);

        for (int i = 0; i < joysticks.Length; i++)
        {
            if (joysticks[i].Name.Equals("PlayStation 4 Controller") == false)
            {
                //Debug.Log("Device type is " + joysticks[i].Name + ", joystick number is " + i + ", playerNumber is " + playerNumber);
                finalJoysticks.Add(joysticks[i]);
            }
        }

        //Debug.Log("finalJoysticks.Count " + finalJoysticks.Count);
        if (finalJoysticks.Count > (playerNumber - 1))
        {
            //Debug.Log("Assigning joystick to player " + playerNumber);
            joystick = finalJoysticks[(playerNumber - 1)];
        }
        else
        {
            //Debug.Log("Joystick for player " + playerNumber + " is NULL");
            joystick = null;
        }
    }

}
