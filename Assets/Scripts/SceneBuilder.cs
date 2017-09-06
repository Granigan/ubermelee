using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBuilder : MonoBehaviour
{
    private MeleeManager meleeManager;
    public GameObject Planet;
    public GameObject[] Players;
    public int NumberOfPlanets = 8;
    public int StartPos;
    public float respawnVariance = 30f;
    public float randomMin = 0f;
    public float randomMax = 1f;

    public GameObject[] ShipTypes;
    public bool[] AIEnabled = new bool[5];
    
    void Awake()
    {
        meleeManager = GameObject.FindGameObjectWithTag("MeleeManager").GetComponent<MeleeManager>();
        //Instantiate(Planet);
        BuildPlanets();
        BuildShips();
        HidePlayerUIs();

        /*
        for(int i = 0; i < AIEnabled.Length; i++)
        {
            AIEnabled[i] = false;
        }
        */
    }


    void BuildShips()
    {
        //Debug.Log("BuildShips!!!");
        int playerNumber = 1;
        foreach (GameObject currPlayer in Players)
        {
            GameObject clone = Instantiate (currPlayer);
            //PlayerInstances.Add(Instantiate(currPlayer));
            //currPlayer.GetComponent<ShipHandling>().playerNumber = playerNumber;
            clone.GetComponent<ShipHandling>().playerNumber = playerNumber;
            clone.GetComponent<ShipHandling>().AIEnabled = meleeManager.GetAIEnabled(playerNumber);
            InitNewShip(clone);
            playerNumber++;
            

        }
    }


    void BuildPlanets()
    {


        for (int i = 0; i < NumberOfPlanets; i++)
        {
            Vector3 force = new Vector3(Random.Range(0f, 50f), Random.Range(0f, 50f), 0);
            Vector3 forcePosition = new Vector3(Random.Range(0.1f, 1f), Random.Range(0.1f, 1f), 0);
            Vector3 velocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            //Debug.Log("Buildign planet " + i);
            GameObject currPlanet = Instantiate(Planet);
            currPlanet.transform.position = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), -3);
            float scale = Random.Range(0.05f, 0.3f);
            currPlanet.transform.localScale = new Vector3(scale, scale, scale);
            currPlanet.GetComponent<Rigidbody>().velocity = velocity;
            currPlanet.GetComponent<Rigidbody>().AddForceAtPosition(force, forcePosition);
        }
    }




    public void ResetScene()
    {
        BuildPlanets();
        BuildShips();
        HidePlayerUIs();
    }
    
    
    public GameObject InstantiateShip(float playerNumber, int shipID, bool AIEnabled = false)
    {
        //Debug.Log("Prefabs/ship" + shipID);
        GameObject clone;
        //GameObject ship = Instantiate(Resources.Load<GameObject>("ship" + shipID));
        foreach (GameObject currShipType in ShipTypes )
        {
            if(currShipType.GetComponent<ShipHandling>().shipID == shipID)
            {
                clone = Instantiate(currShipType);
                //PlayerInstances.Add(Instantiate(currPlayer));
                //currPlayer.GetComponent<ShipHandling>().playerNumber = playerNumber;
                clone.GetComponent<ShipHandling>().playerNumber = playerNumber;
                //Vector3 respawnPoint = new Vector3(Random.Range(-respawnVariance, respawnVariance), Random.Range(-respawnVariance, respawnVariance), -3);  // Fix up the static -3 Z-axis later?

                //Camera mainCamera = GameObject.FindGameObjectWithTag("Camera").GetComponentInChildren<Camera>();
                //Vector3 randomPoint = new Vector3(Random.Range(randomMin, randomMax), Random.Range(randomMin, randomMax), -3f);
                //clone.transform.position = mainCamera.ViewportToWorldPoint(randomPoint);


                //this.GetComponentInChildren<Transform>().position = respawnPoint;
                if(AIEnabled == true)
                {
                    clone.GetComponent<ShipHandling>().AIEnabled = true;
                }

                // Found the right ship type...
                InitNewShip(clone);
                return clone;
            }
        }

        return null;
     
    }
    

    private void InitNewShip(GameObject ship)
    {

        ship.transform.Rotate(Vector3.forward, Random.Range(-180, 180));

        //ship.transform.position = new Vector3(Random.Range(-StartPos, StartPos), Random.Range(-StartPos, StartPos), -3);
        
        Camera mainCamera = GameObject.FindGameObjectWithTag("Camera").GetComponentInChildren<Camera>();
        Vector3 randomPoint = new Vector3(Random.Range(randomMin, randomMax), Random.Range(randomMin, randomMax), -3f);
        ship.transform.position = mainCamera.ViewportToWorldPoint(randomPoint);
      

        //Debug.Log("ship.transform.position = " + ship.transform.position);

        ship.layer = 2;
        Collider[] hitColliders = Physics.OverlapSphere(ship.transform.position, 2);
        while (hitColliders.Length > 4)
        {
            //Debug.Log("shipcolliders in loop" + hitColliders.Length);

            ship.layer = 2;
            //ship.transform.position = new Vector3(Random.Range(-StartPos, StartPos), Random.Range(-StartPos, StartPos), -3);

            randomPoint = new Vector3(Random.Range(randomMin, randomMax), Random.Range(randomMin, randomMax), -3f);
            ship.transform.position = mainCamera.ViewportToWorldPoint(randomPoint);
            
            hitColliders = Physics.OverlapSphere(ship.transform.position, 2);
            //Debug.Log("shipcolliders2" + hitColliders.Length);
        }
        ship.layer = 0;

    }

    private void HidePlayerUIs()
    {
        for(int playerNumber = 1; playerNumber <= 4; playerNumber++)
        {
            GameObject UICanvas = GameObject.FindGameObjectWithTag("Player" + playerNumber + "UIPanel");
            UICanvas.GetComponent<CanvasGroup>().alpha = 0f;
        }
    }
}