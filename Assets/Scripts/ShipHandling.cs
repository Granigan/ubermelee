using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHandling : MonoBehaviour {

    public float hitPoints = 20;
    public GameObject explosionPrefab;
    public float respawnVariance = 30f;

    private IEnumerator coroutine;
    private float currHitPoints = 20;

    // Use this for initialization
    void Start () {
        currHitPoints = hitPoints;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DoDamage(float Damage)
    {
        this.currHitPoints = this.currHitPoints - Damage;
        Debug.Log("Hitpoints left " + this.currHitPoints);
        if(this.currHitPoints <= 0)
        {
            explodeShip();
            currHitPoints = hitPoints;
        }

    }

    private void explodeShip() { 
    
        Debug.Log("Ship Explosion!!!");
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 3.0f);
        StartCoroutine(DieAndRespawn());
    }


    private IEnumerator DieAndRespawn()
    {
        Debug.Log("Player just died!!");
        //this.gameObject.SetActive(false);
        //GetComponent(Rigidbody).enabled = false;
        this.GetComponentInChildren<MeshRenderer>().enabled = false;
        this.GetComponentInChildren<Transform>().localScale = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(2.0f);
        Debug.Log("Player just respawned!!");
        //transform.position = new Vector3(0.04833326f, 3.980667f, 0.0f);
        //transform.rotation = Quaternion.identity;
        //GetComponent<Renderer>().enabled = true;
        Vector3 respawnPoint = new Vector3(Random.Range(-respawnVariance, respawnVariance), Random.Range(-respawnVariance, respawnVariance), -3);  // Fix up the static -3 Z-axis later?
        //this.gameObject.SetActive(true);
        this.GetComponentInChildren<Transform>().position = respawnPoint;
        this.GetComponentInChildren<MeshRenderer>().enabled = true;
        this.GetComponentInChildren<Transform>().localScale = new Vector3(1, 1, 1);
    }
}
