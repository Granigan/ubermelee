using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBuilder : MonoBehaviour {
    public GameObject Planet;
    public GameObject[] Players;
    private List<GameObject> PlayerInstances = new List<GameObject>();
    public int NumberOfPlanets = 10;

    void Awake() {
        //Instantiate(Planet);
        BuildPlanets();
        BuildShips();
        

    }

    void BuildShips() {
        //Debug.Log("BuildShips!!!");
        int playerNumber = 1;
        foreach(GameObject currPlayer in Players )
        {
            PlayerInstances.Add(Instantiate(currPlayer));
            currPlayer.GetComponent<ShipHandling>().playerNumber = playerNumber;
            //currPlayer.GetComponent<ShipHandling>().AIEnabled = true;
            //Debug.Log("playerNumber: "+ playerNumber);
            playerNumber++;
            currPlayer.transform.position = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), -3);
            currPlayer.transform.Rotate(Vector3.forward, Random.Range(-180, 180));
            {
                Collider[] hitColliders = Physics.OverlapSphere(currPlayer.transform.position, 1);
                //Debug.Log("ship1colliders" + hitColliders.Length);
                while (hitColliders.Length > 0)
                {
                    currPlayer.transform.position = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), -3);
                    hitColliders = Physics.OverlapSphere(currPlayer.transform.position, 1);
                    //Debug.Log("ship1colliders2" + hitColliders.Length);
                }

            }

            // currPlayer.GetComponent<ShipDetails>().score = 0f;

            
        }
    }


    void BuildPlanets()
    {
        for(int i = 0; i < NumberOfPlanets; i++)
        {
            //Debug.Log("Buildign planet " + i);
            GameObject currPlanet =Instantiate(Planet);
            currPlanet.transform.position = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), -3);
            float scale = Random.Range(1.3f, 2.9f);
            currPlanet.transform.localScale = new Vector3(scale, scale, scale);

        }
    }
    


public void ResetScene() {
        BuildPlanets();
        BuildShips();
    }
}
