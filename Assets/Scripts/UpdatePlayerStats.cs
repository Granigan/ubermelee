using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePlayerStats : MonoBehaviour {

    GameObject[] players;
    private float playerNumber;
   
    public float[] playerScores = new float[6];

    // Use this for initialization
    void Start () {
       
        players = GameObject.FindGameObjectsWithTag("CameraObject");
        //Debug.Log("players = " + players.Length);
        for (int i = 0; i < (players.Length); i++)
        {
            //playerScores[i]= 0f;
            playerScores[i] = 0f;
        }
        //Debug.Log("playerScores = " + playerScores.Length);
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("In UpdatePlayerStats");
        int i = 0;
		foreach(GameObject currPlayer in players) {
            i++;
            //Debug.Log("i = " + i);
            GameObject textGO = GameObject.Find("Player" + i + "Stats");
            UnityEngine.UI.Text text = textGO.GetComponentInChildren<UnityEngine.UI.Text>();
            text.text = "PLAYER " + Mathf.Round(i).ToString() + "\n";
            text.text += currPlayer.GetComponent<ShipHandling>().shipDetails.ShipName + "\n";
            text.text += "Crew: " + Mathf.Round(currPlayer.GetComponent<ShipHandling>().getCurrentCrew()).ToString() + "/" + Mathf.Round(currPlayer.GetComponent<ShipHandling>().shipDetails.Crew).ToString() + "\n";
            text.text += "Battery: " + Mathf.Round(currPlayer.GetComponent<ShipHandling>().getCurrentBattery()).ToString() + "/" + Mathf.Round(currPlayer.GetComponent<ShipHandling>().shipDetails.Battery).ToString() + "\n";
            text.text += "Score: " + Mathf.Round(playerScores[i]).ToString() + "\n";
            
        }
	}

    
}
