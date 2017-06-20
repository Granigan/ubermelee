using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePlayerStats : MonoBehaviour {

    GameObject[] players;
    private float playerNumber;
    private float scorePlayer1 = 0f;
    private float scorePlayer2 = 0f;

    // Use this for initialization
    void Start () {

        players = GameObject.FindGameObjectsWithTag("CameraObject"); // FindObjectsOfType(typeof(ShipDetails)) as ShipDetails[];
        playerNumber = 1f;
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("In UpdatePlayerStats");
		foreach(GameObject currPlayer in players) {
            if (playerNumber == 1)
            {
                GameObject textGO = GameObject.Find("Player" + playerNumber + "Stats");
                UnityEngine.UI.Text text = textGO.GetComponentInChildren<UnityEngine.UI.Text>();
                text.text = "PLAYER" + Mathf.Round(playerNumber).ToString() + "\n";
                text.text += "Crew: " + Mathf.Round(currPlayer.GetComponent<ShipHandling>().getCurrentCrew()).ToString() + "\n";
                text.text += "Battery: " + Mathf.Round(currPlayer.GetComponent<ShipHandling>().getCurrentBattery()).ToString() + "\n";
                text.text += "Score: " + Mathf.Round(currPlayer.GetComponent<ShipHandling>().shipDetails.score).ToString() + "\n";
                playerNumber = 2;
            }
            else
            {
                GameObject textGO = GameObject.Find("Player" + playerNumber + "Stats");
                UnityEngine.UI.Text text = textGO.GetComponentInChildren<UnityEngine.UI.Text>();
                text.text = "PLAYER" + Mathf.Round(playerNumber).ToString() + "\n";
                text.text += "Crew: " + Mathf.Round(currPlayer.GetComponent<ShipHandling>().getCurrentCrew()).ToString() + "\n";
                text.text += "Battery: " + Mathf.Round(currPlayer.GetComponent<ShipHandling>().getCurrentBattery()).ToString() + "\n";
                text.text += "Score: " + Mathf.Round(currPlayer.GetComponent<ShipHandling>().shipDetails.score).ToString() + "\n";
                playerNumber = 1;
            }
        }
	}

    public void AddScore(float playerNumber)
    {
        if(playerNumber == 1f)
        {
            scorePlayer1++;
        }
        if (playerNumber == 2f)
        {
            scorePlayer2++;
        }
    }
}
