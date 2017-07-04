using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour {

    public float timeToLive = 3.0f;
    public float damage = 2.0f;
    public GameObject explosionPrefab;
    public float bulletOwnerPlayerNumber = 0f;

    // Use this for initialization
    void Start () {
        //Destroy(this.gameObject, timeToLive);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        //Debug.Log("OnCollisionEnter");
        if (col.gameObject.tag == "CameraObject")
        {

            var damageFunction = col.gameObject.GetComponent<ShipHandling>();
            bool shipDestroyed = damageFunction.DoDamage(damage);
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 3.0f);
            Destroy(this.gameObject);

            if(shipDestroyed == true)
            {
                GameObject playerStats = GameObject.FindGameObjectWithTag("Player1Stats");
                //Debug.Log("Killer was " + bulletOwnerPlayerNumber);
                playerStats.GetComponent<UpdatePlayerStats>().playerScores[(int)bulletOwnerPlayerNumber-1]++;
                
            }

        }
    }


    public void setDamage( float newDamage)
    {
        damage = newDamage;
    }

}
