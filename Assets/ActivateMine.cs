using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateMine : MonoBehaviour {

    public float radius = 1.2f;
    public float activationDelay = 0.5f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider col)
    {
        BulletCollision bc = this.GetComponentInParent<BulletCollision>();
        ShipHandling shipHandling = null;

        if(col.transform.parent != null)
        {
            if (col.transform.parent.GetComponent<ShipHandling>() != null)
            {
                shipHandling = col.transform.parent.GetComponent<ShipHandling>();
            }
        }
        //Debug.Log("OnTriggerEnter!! " + col.gameObject.tag + " " +  bc.bulletOwnerPlayerNumber + " " + shipHandling.playerNumber + " " + bc.isMine);

        if (shipHandling != null)
        {
            // Check for own deployed mines first
            if (col.transform.parent.tag == "CameraObject" && bc.isMine == true && bc.bulletOwnerPlayerNumber == shipHandling.playerNumber)
            {
                // Do nothing
            } else if (col.transform.tag == "Bullet")
            {
                // Do nothing
            }
            else
            {
                if (col.transform.parent.tag == "CameraObject")
                {
                    StartCoroutine(ExplodeMine(shipHandling, bc, activationDelay));
                }
            }
        }
    }


    IEnumerator ExplodeMine(ShipHandling shipHandling, BulletCollision bc, float waitForSeconds = 0)
    {
        yield return new WaitForSeconds(waitForSeconds);
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        List<ShipHandling> shipsInArea = new List<ShipHandling>();
        int i = 0;
        while (i < hitColliders.Length)
        {
            //Debug.Log(hitColliders[i].GetType() + " " + hitColliders[i].transform.parent.tag);

            if (hitColliders[i].transform.parent != null && hitColliders[i].transform.parent.tag == "CameraObject")
            {
                if (shipsInArea.IndexOf(hitColliders[i].transform.GetComponent<ShipHandling>()) < 0)
                {
                    shipsInArea.Add(hitColliders[i].transform.GetComponent<ShipHandling>());
                    //hitColliders[i].transform.GetComponent<Renderer>().GetComponent<Material>().color = Color.yellow;
                }
            }
            i++;
        }

        //Debug.Log("shipsInArea = "+shipsInArea.Count);

        //Do the damage to all players in the area
        i = 0;
        foreach(ShipHandling currShipHandling in shipsInArea)
        {
            bool shipDestroyed = shipHandling.DoDamage(bc.damage);
            if (shipDestroyed == true)
            {
                GameObject playerStats = GameObject.FindGameObjectWithTag("Player1Stats");

                // Don't add points if killed by itself!
                if (bc.bulletOwnerPlayerNumber != shipHandling.playerNumber)
                {
                    playerStats.GetComponent<UpdatePlayerStats>().playerScores[(int)bc.bulletOwnerPlayerNumber - 1]++;
                }

            }
        }

        // Do the big explosion
        GameObject explosion = Instantiate(bc.explosionPrefabBig, transform.position, Quaternion.identity);
        Destroy(explosion, 3.0f);
        Destroy(this.gameObject);
        Destroy(transform.parent.gameObject);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
     //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
     Gizmos.DrawWireSphere(transform.position, radius);
    }

}
