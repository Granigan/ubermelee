using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHandling : MonoBehaviour {

    public ShipDetails shipDetails;

    //public float hitPoints = 20;
    public GameObject explosionPrefab;
    public float respawnVariance = 30f;
    private new Transform transform;
    public float shipID = 0;

    private IEnumerator coroutine;
    [HideInInspector]
    public float currentCrew;
    [HideInInspector]
    public float currentBattery;
    private List<Transform> bulletSpawnPoints;
    private ShipSpecials shipSpecials;

    public GameObject thruster;
    public TrailRenderer trail;
    public Vector3 eulerAngleVelocity;
    private Rigidbody rb;
    private float lastFrameTime = 0;
    [HideInInspector]
    public float lastShot = 0.0f;
    public float lastSpecialUsed = 0.0f;


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

        bulletSpawnPoints = new List<Transform>();
        int i = 0;
        //bulletSpawnPoints = getChi getChildGameObject(transform.gameObject, "BulletSpawnPoint").transform;
        foreach (Transform child in transform)
        {
            //Debug.Log("child.name = " + child.name);
            if (child.CompareTag("BulletSpawn") && child.name.Contains("BulletSpawnPointMain"))
            {
                //Debug.Log("Selected child.name = " + child.name);
                //bulletSpawnPoints[i] = child.transform;
                bulletSpawnPoints.Add(child.transform);
                i++;
            }
        }

        //Debug.Log("bulletSpawnPoints length: " + bulletSpawnPoints.Count);
        //bulletSpawn = transform.Find("BulletSpawnPoint");


        //shipSpecials = gameObject.AddComponent(ShipSpecials);
        shipSpecials = gameObject.AddComponent<ShipSpecials>();
        //shipSpecials = GameObject.FindObjectOfType(typeof(ShipSpecials)) as ShipSpecials;
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
            shipDetails.score++;
            /*
            if(shipID == 31)
            {
                //GameObject.FindGameObjectsWithTag("Player1Stats").;
                transform.parent.GetComponent.GetComponent <"Player1Stats" > ();

                GetComponentInParent<Frame>().GetComponent<UpdatePlayerStats>().AddScore(1f);
            } else
            {
                GetComponent<UpdatePlayerStats>().AddScore(2f);
            }
            */
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
        this.GetComponentInChildren<BoxCollider>().enabled = false;
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
        this.GetComponentInChildren<BoxCollider>().enabled = enabled;
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

    public void FireMainWeapon(List<Transform> paramSpawnPoints = null, float paramBatteryCharge = -1, float paramFireRate = -1)
    {
        float usedFireRate = 1f;
        if(paramFireRate == -1)
        {
            usedFireRate = shipDetails.WeaponMain.FireRate; 
        } else {
            usedFireRate = paramFireRate;
        }

        if (Time.time > usedFireRate + lastShot)
        {
            if (currentBattery >= shipDetails.WeaponMain.BatteryCharge)
            {
                List<Transform> usedSpawnPoints = new List<Transform>();
                if (paramSpawnPoints == null)
                {
                    usedSpawnPoints = bulletSpawnPoints;
                } else
                {
                    usedSpawnPoints = paramSpawnPoints;
                }


                foreach(Transform currBulletSpawnPoint in usedSpawnPoints)
                {
                    GameObject bullet = (GameObject)Instantiate(
                    shipDetails.WeaponMain.bulletPrefab,
                    currBulletSpawnPoint.position,
                    currBulletSpawnPoint.rotation); 

                    Transform transform = bullet.GetComponentInChildren<Transform>();
                    transform.localScale = new Vector3(shipDetails.WeaponMain.Scale, shipDetails.WeaponMain.Scale, shipDetails.WeaponMain.Scale);


                    // Add velocity to the bullet
                    bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * shipDetails.WeaponMain.Speed;

                    BulletCollision bulletCol = bullet.GetComponentInChildren<BulletCollision>(); //transform.Find("BulletCollision");
                                                                                                  //ScriptB other = (ScriptB)go.GetComponent(typeof(ScriptB));
                    bulletCol.setDamage(shipDetails.WeaponMain.Damage);

                    // Destroy the bullet after X seconds
                    Destroy(bullet, shipDetails.WeaponMain.TimeToLive);

                }

                if(paramBatteryCharge == -1)
                {
                    currentBattery = currentBattery - shipDetails.WeaponMain.BatteryCharge;
                } else
                {
                    currentBattery = currentBattery - paramBatteryCharge;
                }

                
                lastShot = Time.time;
            }


        }
    }

    public void UseSpecial()
    {
        // Just to test this special feature...
        //MoveShip(8f);
       
        if (Time.time > shipDetails.Special.FireRate + lastSpecialUsed)
        {
            shipSpecials.Invoke("Ship" + shipID + "Special", 0);
            lastSpecialUsed = Time.time;
        }

    }




    static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
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



