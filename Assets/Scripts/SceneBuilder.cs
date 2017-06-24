using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBuilder : MonoBehaviour {
    //public GameObject Camera;
    //public GameObject Light;
    //public GameObject Counter;
    public GameObject Planet;
    public GameObject[] Players;
    private List<GameObject> PlayerInstances = new List<GameObject>();
    //public GameObject Ship1;
    //public GameObject Ship2;

    //public int[] PosArray = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
    //public int[] NegArray = { -1, -2, -3, -4, -5, -6, -7, -8, -9, -10, -11, -12, -13, -14, -15 };


    //int x;
    //int y;

    // Use this for initialization
    void Awake() {
        //Instantiate(Camera);
        //Instantiate(Light);
        //Instantiate(Counter);
        Instantiate(Planet);
        BuildShips();
        //InitShipValues();
        
        //AttachScripts();
        // random vector3 values x, y. z is allways same.
        //Vector3 RandoPos = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), -3);
        //Vector3 RandoPos2 = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), -3);
        //Vector3 RandoPos2 = new Vector3(Random.Range(NegArray.Length, PosArray.Length), Random.Range(NegArray.Length, PosArray.Length), 0);
        //x = Random.Range(-15, 15);
        //y = Random.Range(-15, 15);
    }

    void BuildShips() {
        int playerNumber = 1;
        foreach(GameObject currPlayer in Players )
        {
            PlayerInstances.Add(Instantiate(currPlayer));
            currPlayer.GetComponent<ShipHandling>().playerNumber = playerNumber;
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
        /*
            Instantiate(Ship1);
            Ship1.transform.position = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), -3);
            Ship1.transform.Rotate(Vector3.forward, Random.Range(-180, 180));
            {
                Collider[] hitColliders = Physics.OverlapSphere(Ship1.transform.position, 1);
            //Debug.Log("ship1colliders" + hitColliders.Length);
                while (hitColliders.Length > 0)
                {
                    Ship1.transform.position = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), -3);
                    hitColliders = Physics.OverlapSphere(Ship1.transform.position, 1);
                //Debug.Log("ship1colliders2" + hitColliders.Length);
            }
                
            }
            {
                Instantiate(Ship2);
                Ship2.transform.position = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), -3);
                Ship2.transform.Rotate(Vector3.forward, Random.Range(-180, 180));
                {
                    Collider[] hitColliders2 = Physics.OverlapSphere(Ship2.transform.position, 1);
                //Debug.Log("ship2colliders" + hitColliders2.Length);
                while (hitColliders2.Length > 1)
                    {
                        Ship2.transform.position = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), -3);
                    hitColliders2 = Physics.OverlapSphere(Ship2.transform.position, 1);
                }
                    
                }
                 
            }
       */
    }

    /*
    void AttachScripts()
    {
        foreach (GameObject currPlayerInstance in PlayerInstances)
        {
            float shipID = currPlayerInstance.GetComponent<ShipHandling>().shipID;
            if(shipID == 31)
            {
                currPlayerInstance.gameObject.AddComponent<ship31Primary>();
                currPlayerInstance.gameObject.AddComponent<ship31Secondary>();
            } else if (shipID == 47)
            {
                currPlayerInstance.gameObject.AddComponent<ship47Primary>();
                currPlayerInstance.gameObject.AddComponent<ship47Secondary>();

            } else
            {
                currPlayerInstance.gameObject.AddComponent<defaultPrimary>();
                currPlayerInstance.gameObject.AddComponent<defaultSecondary>();

            }

        }

    }
    */

    private void InitShipValues()
    {
        foreach (GameObject currPlayer in PlayerInstances)
        {
            //currPlayer.GetComponent<ShipDetails>().score = 0;
        }
    }
}
