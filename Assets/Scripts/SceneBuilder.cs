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
//if(playerNumber != 1)
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
<<<<<<< HEAD
        for(int i = 0; i < NumberOfPlanets; i++)
        {
            //Debug.Log("Buildign planet " + i);
            GameObject currPlanet =Instantiate(Planet);
            currPlanet.transform.position = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), -3);
            float scale = Random.Range(1.3f, 2.9f);
            currPlanet.transform.localScale = new Vector3(scale, scale, scale);

=======
        

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
>>>>>>> f02afbea1adc735de60e7110bd1b529f2774cb51
        }
    }
    


public void ResetScene() {
        BuildPlanets();
        BuildShips();
    }
}
