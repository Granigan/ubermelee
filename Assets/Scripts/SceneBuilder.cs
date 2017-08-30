using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBuilder : MonoBehaviour
{
    public GameObject Planet;
    public GameObject[] Players;
    //private List<GameObject> PlayerInstances = new List<GameObject>();
    public int NumberOfPlanets = 10;
    public int StartPos;

    void Awake()
    {
        //Instantiate(Planet);
        BuildPlanets();
        BuildShips();


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
            //if(playerNumber != 1)
            //currPlayer.GetComponent<ShipHandling>().AIEnabled = true;
            //Debug.Log("playerNumber: "+ playerNumber);

            //currPlayer.transform.position = new Vector3(Random.Range(-StartPos, StartPos), Random.Range(-StartPos, StartPos), -3);
            //currPlayer.transform.Rotate(Vector3.forward, Random.Range(-180, 180));
            clone.transform.Rotate(Vector3.forward, Random.Range(-180, 180));
            clone.transform.position = new Vector3(Random.Range(-StartPos, StartPos), Random.Range(-StartPos, StartPos), -3);
            playerNumber++;
            clone.layer = 2;
            
            {
                clone.layer = 2;
                Collider[] hitColliders = Physics.OverlapSphere(clone.transform.position, 2);
                Debug.Log("shipcolliders" + hitColliders.Length);
                while (hitColliders.Length > 4)
                {
                    clone.layer = 2;
                    clone.transform.position = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), -3);
                    hitColliders = Physics.OverlapSphere(clone.transform.position, 2);
                    Debug.Log("shipcolliders2" + hitColliders.Length);
                  
                }
                //clone.layer = 0;
            }

            // currPlayer.GetComponent<ShipDetails>().score = 0f;


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
    }
    
}