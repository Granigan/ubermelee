using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePlayerStats : MonoBehaviour {

    GameObject[] players;
    private float playerNumber;

	// Use this for initialization
	void Start () {

        players = GameObject.FindGameObjectsWithTag("CameraObject"); // FindObjectsOfType(typeof(ShipDetails)) as ShipDetails[];
        playerNumber = 1f;
    }
	
	// Update is called once per frame
	void Update () {
		foreach(GameObject currPlayer in players) {
            if(playerNumber == 1)
            {
                GameObject textGO = GameObject.Find("Player" + playerNumber + "Stats");
                UnityEngine.UI.Text text = textGO.GetComponentInChildren<UnityEngine.UI.Text>();
                text.text = "PLAYER" + playerNumber + "\n";
                text.text += "Crew: " + currPlayer.GetComponent<ShipHandling>().currentCrew + "\n";
                //text.text += "Battery: " + currPlayer.GetComponent<ShipDetails>().Battery + "\n";

                playerNumber = 2;
            } else {
                GameObject textGO = GameObject.Find("Player" + playerNumber + "Stats");
                UnityEngine.UI.Text text = textGO.GetComponentInChildren<UnityEngine.UI.Text>();
                text.text = "PLAYER" + playerNumber + "\n";
                text.text += "Crew: " + currPlayer.GetComponent<ShipHandling>().currentCrew + "\n";
                //text.text += "Battery: " + currPlayer.GetComponent<ShipDetails>().Battery + "\n";
                playerNumber = 1;
            }

        }
	}
}
