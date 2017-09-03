using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpdatePlayerStats : MonoBehaviour {

    GameObject[] players;
    private int numberOfPlayers = 4;
    private float playerNumber;

    private string[] shipNames = new string[5];

    [HideInInspector]
    public float[] playerScores = new float[5];
    public float GameOverScore = 10;
    public float RestartSceneWaitTime = 5.0f;
    private bool MeleeInProgress = true;

    // GUI test stuff...
    //public GameObject[] HealthArray;
    //public GameObject[] EnergyArray;
    // GUI test stuff...

    // Use this for initialization
    void Start () {
        playerScores = new float[5];
        //players[0] = new GameObject;
        players = GameObject.FindGameObjectsWithTag("CameraObject");
        //Debug.Log("players = " + players.Length);
        /*
        playerScores[0] = 0f;
        playerScores[1] = 0f;
        playerScores[2] = 0f;
        playerScores[3] = 0f;
        playerScores[4] = 0f;
        */
        for (int i = 1; i <= numberOfPlayers; i++)
        {
            //Debug.Log("Hello i = " + i);
            playerScores[i] = 0f;
        }
        

        if (players.Length <= 3)
        {
            GameObject textGO = GameObject.Find("Player3Stats");
            UnityEngine.UI.Text text = textGO.GetComponentInChildren<UnityEngine.UI.Text>();
            text.text = "";
        }
        if (players.Length <= 4)
        {
            GameObject textGO = GameObject.Find("Player4Stats");
            UnityEngine.UI.Text text = textGO.GetComponentInChildren<UnityEngine.UI.Text>();
            text.text = "";
        }
        //Debug.Log("playerScores = " + playerScores.Length);
    }
	
	// Update is called once per frame
	bool Update () {

        if (MeleeInProgress == false)
        {
            return false;
        }

        players = GameObject.FindGameObjectsWithTag("CameraObject");

        //foreach (GameObject currPlayer in players) {
        for(int i = 1; i <= numberOfPlayers; i++) { 
            //i++;

            //Debug.Log("playerScores = "  + playerScores.Length + " and i = " + i );

            if (playerScores[i-1] >= GameOverScore)
            {
                //MeleeInProgress = false;
                KillAllOtherPlayers(players[i]);
                StartCoroutine(ShowWinnerStats());
                //GameObject.FindGameObjectWithTag("Camera").GetComponent<SceneBuilder>().ResetScene();
                // Display winner stats
                //

            } 
            // Update score UI as usual.
            //Debug.Log("i = " + i);
            GameObject textGO = GameObject.Find("Player" + i + "Stats");
            UnityEngine.UI.Text text = textGO.GetComponentInChildren<UnityEngine.UI.Text>();
            
            text.text = "";
            if(Mathf.Round(playerScores[i - 1]) >= GameOverScore)
            {
                text.text += "WINNER!!!\n";
                text.fontStyle = FontStyle.BoldAndItalic;
                text.fontSize = 12;
            }
            text.text += "PLAYER " + Mathf.Round(i).ToString() + "\n";
            text.text += shipNames[i] + "\n";
            //text.text += "Crew: " + Mathf.Round(currPlayer.GetComponent<ShipHandling>().getCurrentCrew()).ToString() + "/" + Mathf.Round(currPlayer.GetComponent<ShipHandling>().shipDetails.Crew).ToString() + "\n";
            //text.text += "Battery: " + Mathf.Round(currPlayer.GetComponent<ShipHandling>().getCurrentBattery()).ToString() + "/" + Mathf.Round(currPlayer.GetComponent<ShipHandling>().shipDetails.Battery).ToString() + "\n";
            text.text += "Score: " + Mathf.Round(playerScores[i - 1]).ToString() + "\n";

            // GUI Test Stuff...
            //GameObject CrewBar = GameObject.Find("HealthBar" + i);
            //GameObject EnergyBar = GameObject.Find("EnergyBar" + i);
            //CrewBar.transform.localScale = new Vector3(1, Mathf.Round(currPlayer.GetComponent<ShipHandling>().getCurrentCrew()), 1);
            //EnergyBar.transform.localScale = new Vector3(1, Mathf.Round(currPlayer.GetComponent<ShipHandling>().getCurrentBattery()), 1);
            // GUI Test Stuff...
        }
        return true;
	}

    


    private void KillAllOtherPlayers(GameObject winnerPlayer)
    {
        int i = 0;
        foreach (GameObject currPlayer in players)
        {
            i++;
            if(currPlayer != null && winnerPlayer != null)
            {
                if (currPlayer != winnerPlayer)
                {
                    for (int j = 0; j < currPlayer.transform.childCount; j++)
                    {
                        //currPlayer.transform.GetChild(j).gameObject.SetActive(false);
                        Destroy(currPlayer.transform.GetChild(j).gameObject);
                    }
                }
                //Destroy(currPlayer);
                currPlayer.GetComponent<ShipHandling>().shipIsDead = true;

            }

        }
    }

    private IEnumerator ShowWinnerStats()
    {
        //Debug.Log("ShowWinnerStats!!");
        

        yield return new WaitForSeconds(RestartSceneWaitTime);

        Debug.Log("Reset now!!");


        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //GameObject.FindGameObjectWithTag("Camera").GetComponent<SceneBuilder>().ResetScene();

        
    }

    public void SetShipName(int playerNumber, string shipName )
    {
        shipNames[playerNumber] = shipName;
    }
}
