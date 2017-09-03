using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GameObject[] players = GameObject.FindGameObjectsWithTag("CameraObject");
        SceneBuilder sceneBuilder = GameObject.FindGameObjectWithTag("Camera").GetComponent<SceneBuilder>();


        if (Input.GetKeyDown(KeyCode.F1))
        {
            

            if (players[0].gameObject.GetComponent<ShipHandling>().AIEnabled == true)
            {
                players[0].gameObject.GetComponent<ShipHandling>().AIEnabled = false;
                sceneBuilder.AIEnabled[1] = false;
            }
            else
            {
                players[0].gameObject.GetComponent<ShipHandling>().AIEnabled = true;
                sceneBuilder.AIEnabled[1] = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (players[1].gameObject.GetComponent<ShipHandling>().AIEnabled == true)
            {
                players[1].gameObject.GetComponent<ShipHandling>().AIEnabled = false;
                sceneBuilder.AIEnabled[2] = false;
            }
            else
            {
                players[1].gameObject.GetComponent<ShipHandling>().AIEnabled = true;
                sceneBuilder.AIEnabled[2] = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            if (players.Length > 2) {
                if (players[2].gameObject.GetComponent<ShipHandling>().AIEnabled == true)
                {
                    players[2].gameObject.GetComponent<ShipHandling>().AIEnabled = false;
                    sceneBuilder.AIEnabled[3] = false;
                }
                else
                {
                    players[2].gameObject.GetComponent<ShipHandling>().AIEnabled = true;
                    sceneBuilder.AIEnabled[3] = true;
                }
            }
            
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            if (players.Length > 3) {
                if (players[3].gameObject.GetComponent<ShipHandling>().AIEnabled == true)
                {
                    players[3].gameObject.GetComponent<ShipHandling>().AIEnabled = false;
                    sceneBuilder.AIEnabled[4] = false;
                }
                else
                {
                    players[3].gameObject.GetComponent<ShipHandling>().AIEnabled = true;
                    sceneBuilder.AIEnabled[4] = true;
                }
            }
        }

    }

}
