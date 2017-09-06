using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour {

    MeleeManager meleeManager;
	// Use this for initialization
	void Start () {
        meleeManager = GameObject.FindGameObjectWithTag("MeleeManager").GetComponent<MeleeManager>();

    }
	
	// Update is called once per frame
	void Update () {
        GlobalSettings settings = meleeManager.settings;
        GameObject[] players = meleeManager.players;

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (players[0].gameObject.GetComponent<ShipHandling>().AIEnabled == true)
            {
                players[0].gameObject.GetComponent<ShipHandling>().AIEnabled = false;
                settings.Player1AIEnabled = false;

            }
            else
            {
                players[0].gameObject.GetComponent<ShipHandling>().AIEnabled = true;
                settings.Player1AIEnabled = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (players[1].gameObject.GetComponent<ShipHandling>().AIEnabled == true)
            {
                players[1].gameObject.GetComponent<ShipHandling>().AIEnabled = false;
                settings.Player2AIEnabled = false;
            }
            else
            {
                players[1].gameObject.GetComponent<ShipHandling>().AIEnabled = true;
                settings.Player2AIEnabled = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            if (players.Length > 2) {
                if (players[2].gameObject.GetComponent<ShipHandling>().AIEnabled == true)
                {
                    players[2].gameObject.GetComponent<ShipHandling>().AIEnabled = false;
                    settings.Player3AIEnabled = false;
                }
                else
                {
                    players[2].gameObject.GetComponent<ShipHandling>().AIEnabled = true;
                    settings.Player3AIEnabled = true;
                }
            }
            
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            if (players.Length > 3) {
                if (players[3].gameObject.GetComponent<ShipHandling>().AIEnabled == true)
                {
                    players[3].gameObject.GetComponent<ShipHandling>().AIEnabled = false;
                    settings.Player4AIEnabled = false;
                }
                else
                {
                    players[3].gameObject.GetComponent<ShipHandling>().AIEnabled = true;
                    settings.Player4AIEnabled = true;
                }
            }
        }

    }

}
