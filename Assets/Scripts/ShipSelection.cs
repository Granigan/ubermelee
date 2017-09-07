using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class ShipSelection : MonoBehaviour {

    public int playerNumber = 1;
    private string turnControl;
    //private string thrustControl;
    private string primaryControl;
    //private string secondaryControl;
    private InputDevice joystick;
    private UnityEngine.UI.Text textField;
    public KeyValuePair<string, int>[] shipArray;
    public List<KeyValuePair<string, int>> shipList = new List<KeyValuePair<string, int>>();

    public int currentSelectionIndex = 1;
    private bool axisInUse = false;
    public bool SelectionEnabled = false;

    private float timeSinceLastInput = 0f;
    private float inputTimeout = 0.15f;

    private static float initCountdownValue = 3f;
    public float countdownSelect = initCountdownValue;
    private bool timerOn = false;

    public bool AIEnabled = false;


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

        if (timerOn)
            UpdateCountdownTimer();

        if (axisInUse == true)
        {
            UpdateInputTimeout();
            return;
        }

        if(joystick != null)
        {
            if (joystick.LeftStickX > 0.5 || joystick.DPadRight > 0)
            {
                if (axisInUse == false)
                {
                    axisInUse = true;
                    currentSelectionIndex++;
                    StartInputTimeout();
                }
            } 
        }

        if (Input.GetAxisRaw(turnControl) == 1 )
        {
            if (axisInUse == false)
            {
                axisInUse = true;
                currentSelectionIndex++;
                StartInputTimeout();
            }

        } 

        if(joystick != null)
        {
            if (joystick.LeftStickX < -0.5 || joystick.DPadLeft > 0)
            {
                if (axisInUse == false)
                {
                    axisInUse = true;
                    currentSelectionIndex--;
                    StartInputTimeout();
                }
            }

        }

        if (Input.GetAxisRaw(turnControl) == -1)
        {
            if (axisInUse == false)
            {
                axisInUse = true;
                currentSelectionIndex--;
                StartInputTimeout();
            }
        }

        if(currentSelectionIndex < 0)
        {
            currentSelectionIndex = 0;
        }

        if (currentSelectionIndex >= (shipArray.Length-1))
        {
            currentSelectionIndex = (shipArray.Length-1);
        }

        // Set the text here
        textField.text = shipArray[currentSelectionIndex].Key.ToString();
        textField.text += " " + Mathf.RoundToInt(countdownSelect).ToString();

        bool joystickButtonPressed = false;

        if(joystick != null)
        {
            if(joystick.Action1 == true)
            {
                joystickButtonPressed = true;
            }
        }

        // Check if ship is selected by button press
        if ((Input.GetButton(primaryControl) == true || joystickButtonPressed == true) && SelectionEnabled == true)
        {
            int selectedShipID = 0;
            if(shipArray[currentSelectionIndex].Value == 0)
            {
                selectedShipID = GetRandomShipID();
            } else
            {
                selectedShipID = shipArray[currentSelectionIndex].Value;
            }

            timerOn = false;
            CreateShip(selectedShipID);
            HideUI();

        }

        



    }

    private void CreateShip(int selectedShipID)
    {
        GameObject clone = GameObject.FindGameObjectWithTag("Camera").GetComponent<SceneBuilder>().InstantiateShip(playerNumber, selectedShipID, AIEnabled);
        clone.GetComponent<ShipHandling>().playerNumber = playerNumber;

    }

    private void HideUI()
    {
        // Hide ShipSelection and disable it's input
        this.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
        SelectionEnabled = false;
        currentSelectionIndex = 0;
    }

    private void StartInputTimeout()
    {
        timeSinceLastInput = inputTimeout;
        axisInUse = true;
    }

    private void UpdateInputTimeout()
    {
        timeSinceLastInput -= Time.deltaTime;
        if(timeSinceLastInput <= 0f)
        {
            axisInUse = false;
            timeSinceLastInput = 0f;
        }
    }


    public int GetRandomShipID()
    {
        return shipArray[Random.Range(1, shipArray.Length)].Value;

    }
    
    private void AssignJoysticks()
    {
        turnControl = "Horizontal" + playerNumber;
        //this.thrustControl = "Vertical" + playerNumber;
        primaryControl = "Primary" + playerNumber;
        //this.secondaryControl = "Secondary" + (playerNumber);
        joystick = null;

        InputDevice[] joysticks = new InputDevice[InputManager.Devices.Count];
        List<InputDevice> finalJoysticks = new List<InputDevice>();
        InputManager.Devices.CopyTo(joysticks, 0);

        //Debug.Log("Number of joysticks " + joysticks.Length);


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

    public void StartSelectionTimer()
    {
        countdownSelect = initCountdownValue;
        timerOn = true;
    }

    private void UpdateCountdownTimer()
    {
        countdownSelect -= Time.deltaTime;

        if(countdownSelect <= 0)
        {
            CreateShip(GetRandomShipID());
            HideUI();
            timerOn = false;

        }
    }

}

