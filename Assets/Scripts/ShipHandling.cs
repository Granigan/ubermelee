using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHandling : MonoBehaviour {

    public ShipDetails shipDetails;

    //public float hitPoints = 20;
    public GameObject explosionPrefab;
    public float respawnVariance = 30f;
    private new Transform transform;

    private IEnumerator coroutine;
    private float currentCrew;
    private float currentBattery;
    private Transform bulletSpawn;


    public GameObject thruster;
    public TrailRenderer trail;
    public Vector3 eulerAngleVelocity;
    private Rigidbody rb;
    private float lastFrameTime = 0;
    private float lastShot = 0.0f;

    // Use this for initialization
    void Start () {
        currentCrew = shipDetails.Crew;
        currentBattery = shipDetails.Battery;
        transform = GetComponentInChildren<Transform>();
        transform.localScale = new Vector3( shipDetails.Scale, shipDetails.Scale, transform.localScale.z);
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.mass = shipDetails.Mass;
        rb.drag = shipDetails.Drag;
        rb.angularDrag = shipDetails.AngularDrag;
        
        trail = GetComponentInChildren<TrailRenderer>();


        bulletSpawn = getChildGameObject(transform.gameObject, "BulletSpawnPoint").transform;
        //bulletSpawn = transform.Find("BulletSpawnPoint");

    }
	
	// Update is called once per frame
	void Update () {
        RechargeBattery();

        transform.position = new Vector3(transform.position.x, transform.position.y, -3.0f);

	}

    public void DoDamage(float Damage)
    {
        this.currentCrew = this.currentCrew - Damage;
        //Debug.Log("Hitpoints left " + this.currCrew);
        if(this.currentCrew <= 0)
        {
            ExplodeShip();
            
        }

    }

    private void ExplodeShip() { 
    
        Debug.Log("Ship Explosion!!!");
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 3.0f);
        StartCoroutine(DieAndRespawn());
        currentCrew = shipDetails.Crew;     
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

    public void RotateShip(float shipTurn) {
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime * -1 * shipTurn * shipDetails.RotationRate);

        eulerAngleVelocity.y = 0;
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    public void MoveShip(float throttle)
    {
        rb.AddForce(transform.right * shipDetails.Acceleration * throttle);

        if (throttle == 0)
        {
            //rb.freezeRotation = true;
            //thruster.GetComponent<TrailRenderer>().enabled = false;
            //trail.time = 0;
        }
        /* TODO Trail code to shiphandling
        if (shipTurn <= 0.1)
        {
            //rb.freezeRotation = true;
            {
                trail.time = 0.5f;
            }

        }
        if (shipTurn == 0)
        {
            //rb.freezeRotation = true;
            {
                trail.time = 0.0f;
            }
        }
        */

        if (rb.velocity.magnitude > shipDetails.MaxSpeed)
        {
            rb.velocity = rb.velocity.normalized * shipDetails.MaxSpeed;
        }



    }

    private void RechargeBattery()
    {
        currentBattery = currentBattery + ((Time.time - lastFrameTime) * shipDetails.BatteryRechargeRate);
        lastFrameTime = Time.time;
        if (currentBattery > shipDetails.Battery)
        {
            currentBattery = shipDetails.Battery;
        }
    }

    public void FireMainWeapon()
    {
        if (Time.time > shipDetails.WeaponMain.FireRate + lastShot)
        {
            if (currentBattery >= shipDetails.WeaponMain.BatteryCharge)
            {
                GameObject bullet = (GameObject)Instantiate(
                    shipDetails.WeaponMain.bulletPrefab,
                    bulletSpawn.position,
                    bulletSpawn.rotation); // TODO: Maybe add bulletspawnpoint to WeaponDetails?

                Transform transform = bullet.GetComponentInChildren<Transform>();
                transform.localScale = new Vector3(shipDetails.WeaponMain.Scale, shipDetails.WeaponMain.Scale, shipDetails.WeaponMain.Scale);


                // Add velocity to the bullet
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * shipDetails.WeaponMain.Speed;

                BulletCollision bulletCol = bullet.GetComponentInChildren<BulletCollision>(); //transform.Find("BulletCollision");
                //ScriptB other = (ScriptB)go.GetComponent(typeof(ScriptB));
                bulletCol.setDamage(shipDetails.WeaponMain.Damage);

                // Destroy the bullet after X seconds
                Destroy(bullet, shipDetails.WeaponMain.TimeToLive);


                currentBattery = currentBattery - shipDetails.WeaponMain.BatteryCharge;
                lastShot = Time.time;
            }


        }
    }

    public void UseSpecial()
    {
        // Just to test this special feature...
        MoveShip(8f);
    }




    static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        //Author: Isaac Dart, June-13.
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }

    public float getCurrentCrew()
    {
        return currentCrew;
    }

    void setCurrentCrew(float newCrew)
    {
        currentCrew = newCrew;
    }

    public float getCurrentBattery()
    {
        return currentBattery;
    }

    void setCurrentBattery(float newBattery)
    {
        currentBattery = newBattery;
    }
}
