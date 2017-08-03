using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour {

    public float timeToLive = 3.0f;
    public float damage = 2.0f;
    public GameObject explosionPrefab;
    public GameObject explosionPrefabBig;
    public float bulletOwnerPlayerNumber = 0f;
    public float bulletHitPoints = 1f;
<<<<<<< HEAD
=======
    public bool isMine = false;
>>>>>>> f02afbea1adc735de60e7110bd1b529f2774cb51

    // Use this for initialization
    void Start () {
        //Destroy(this.gameObject, timeToLive);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        ShipHandling shipHandling = col.gameObject.GetComponent<ShipHandling>();
        //Debug.Log("OnCollisionEnter");

        // Check for own deployed mines first
        if (col.gameObject.tag == "CameraObject" && isMine == true && bulletOwnerPlayerNumber == shipHandling.playerNumber)
        {
            // Do nothing, just push it on collision.

<<<<<<< HEAD
            var shipHandling = col.gameObject.GetComponent<ShipHandling>();
            bool shipDestroyed = shipHandling.DoDamage(damage);
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 3.0f);
            Destroy(this.gameObject);
=======
        } else {
>>>>>>> f02afbea1adc735de60e7110bd1b529f2774cb51

            if (col.gameObject.tag == "CameraObject")
            {
<<<<<<< HEAD
                GameObject playerStats = GameObject.FindGameObjectWithTag("Player1Stats");
                //Debug.Log("Killer was " + bulletOwnerPlayerNumber);

                // Don't add points if killed by itself!
                if(bulletOwnerPlayerNumber != shipHandling.playerNumber)
                {
                    playerStats.GetComponent<UpdatePlayerStats>().playerScores[(int)bulletOwnerPlayerNumber - 1]++;
                }

            }

        } else if(col.gameObject.tag == "Bullet")
        {
            //Debug.Log("Bullet Collision!!");
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            
            // Exchange damage
            bulletHitPoints = bulletHitPoints - col.transform.GetComponent<BulletCollision>().damage;
            col.transform.GetComponent<BulletCollision>().bulletHitPoints = col.transform.GetComponent<BulletCollision>().bulletHitPoints - damage;

            //Debug.Log("Bullet hitpoints after collision: " + bulletHitPoints + " and " + col.transform.GetComponent<BulletCollision>().bulletHitPoints);

            // Check for casualties
            if (bulletHitPoints <= 0)
            {
                Destroy(explosion, 3.0f);
                Destroy(this.gameObject);
            }
            if (col.transform.GetComponent<BulletCollision>().bulletHitPoints <= 0)
            {
                Destroy(explosion, 3.0f);
                Destroy(col.gameObject);
=======
                bool shipDestroyed = shipHandling.DoDamage(damage);
                GameObject explosion;

                if (isMine)
                    explosion = Instantiate(explosionPrefabBig, transform.position, Quaternion.identity);
                else
                    explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

                //GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(explosion, 3.0f);
                Destroy(this.gameObject);

                if (shipDestroyed == true)
                {
                    GameObject playerStats = GameObject.FindGameObjectWithTag("Player1Stats");
                    //Debug.Log("Killer was " + bulletOwnerPlayerNumber);

                    // Don't add points if killed by itself!
                    if (bulletOwnerPlayerNumber != shipHandling.playerNumber)
                    {
                        playerStats.GetComponent<UpdatePlayerStats>().playerScores[(int)bulletOwnerPlayerNumber - 1]++;
                    }

                }

            }
            else if (col.gameObject.tag == "Bullet")
            {
                //Debug.Log("Bullet Collision!!");
                GameObject explosion;


                // Exchange damage
                bulletHitPoints = bulletHitPoints - col.transform.GetComponent<BulletCollision>().damage;
                col.transform.GetComponent<BulletCollision>().bulletHitPoints = col.transform.GetComponent<BulletCollision>().bulletHitPoints - damage;

                //Debug.Log("Bullet hitpoints after collision: " + bulletHitPoints + " and " + col.transform.GetComponent<BulletCollision>().bulletHitPoints);

                // Check for casualties
                if (bulletHitPoints <= 0)
                {
                    if(isMine)
                         explosion = Instantiate(explosionPrefabBig, transform.position, Quaternion.identity);
                    else
                        explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

                    Destroy(explosion, 3.0f);
                    Destroy(this.gameObject);
                }
                if (col.transform.GetComponent<BulletCollision>().bulletHitPoints <= 0)
                {
                    if (col.transform.GetComponent<BulletCollision>().isMine)
                        explosion = Instantiate(explosionPrefabBig, transform.position, Quaternion.identity);
                    else
                        explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

                    Destroy(explosion, 3.0f);
                    Destroy(col.gameObject);
                }



            } else
            {
                // On any other object, just destroy the bullet
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(explosion, 3.0f);
                Destroy(this.gameObject);
>>>>>>> f02afbea1adc735de60e7110bd1b529f2774cb51
            }



        }

       
    }


    public void setDamage( float newDamage)
    {
        damage = newDamage;
    }


}
