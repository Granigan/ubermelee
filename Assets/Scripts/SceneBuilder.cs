using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBuilder : MonoBehaviour {
    public GameObject Planet;
    public GameObject[] Players;
    private List<GameObject> PlayerInstances = new List<GameObject>();

    void Awake() {
        Instantiate(Planet);
        BuildShips();      

    }

    void BuildShips() {
        Debug.Log("BuildShips!!!");
        int playerNumber = 1;
        foreach(GameObject currPlayer in Players )
        {
            PlayerInstances.Add(Instantiate(currPlayer));
            currPlayer.GetComponent<ShipHandling>().playerNumber = playerNumber;
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

    
}
