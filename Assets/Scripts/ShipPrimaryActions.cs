using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All the ship ship primary actions go here go here. They are invoked by shipID.
public class ShipPrimaryActions : MonoBehaviour
{
    private List<Transform> bulletSpawnPoints;
    private List<GameObject> createdBullets;

    float Ship17LaserLength = 12f;
    float Ship17LaserMinLength = 6f;
    float Ship17LaserDuration = 0.30f;
    float Ship17LaserDurationLeft = 0f;
    float Ship17LaserDoDamageInterval = 5f;
    float Ship17LaserDrawCount = 0f;
    bool Ship17LaserActive = false;

    // Use this for initialization
    void Start()
    {
        LineRenderer laserBeamRenderer = this.GetComponentInChildren<LineRenderer>();
        if (laserBeamRenderer != null)
        {
            laserBeamRenderer.SetPosition(0, new Vector3(0f, 0f, 0f));
        }

        createdBullets = new List<GameObject>();
        bulletSpawnPoints = new List<Transform>();
        int i = 0;
        //bulletSpawnPoints = getChi getChildGameObject(transform.gameObject, "BulletSpawnPoint").transform;
        foreach (Transform child in transform)
        {
            //Debug.Log("child.name = " + child.name);
            if (child.CompareTag("BulletSpawn") && child.name.Contains("BulletSpawnPointPrimary"))
            {
                //Debug.Log("Selected child.name = " + child.name);
                //bulletSpawnPoints[i] = child.transform;
                bulletSpawnPoints.Add(child.transform);
                i++;
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        // Destroy laser


        if (Ship17LaserActive == true)
        {
            //this.GetComponentInChildren<LineRenderer>().enabled = false;
            Ship17LaserDurationLeft = Ship17LaserDurationLeft - Time.deltaTime;

            if (Ship17LaserDurationLeft <= 0)
            {
                this.GetComponentInChildren<LineRenderer>().enabled = false;
                Ship17LaserActive = false;
            }
            else
            {
                RedrawShip17Primary();
            }

        }
    }

    public void Ship31Primary()
    {
        GenericPrimaryShoot();

    }

    public void Ship47Primary()
    {
        GenericPrimaryShoot();
    }

    public void Ship63Primary()
    {
        GenericPrimaryShoot();

    }

    public void Ship64Primary()
    {
        GenericPrimaryShoot();
    }


    private void GenericPrimaryShoot()
    {

        ShipHandling shipHandling = this.GetComponentInParent<ShipHandling>();
        ShipDetails shipDetails = shipHandling.shipDetails;
        float usedFireRate = shipDetails.Primary.FireRate;


        if (Time.time > usedFireRate + shipHandling.lastShot)
        {
            if (shipHandling.currentBattery >= shipDetails.Primary.BatteryCharge)
            {
                List<Transform> usedSpawnPoints = new List<Transform>();

                usedSpawnPoints = bulletSpawnPoints;

                foreach (Transform currBulletSpawnPoint in usedSpawnPoints)
                {
                    GameObject bullet = (GameObject)Instantiate(
                    shipDetails.Primary.bulletPrefab,
                    currBulletSpawnPoint.position,
                    currBulletSpawnPoint.rotation);
                    bullet.transform.parent = gameObject.transform;
                    bullet.GetComponent<BulletCollision>().bulletOwnerPlayerNumber = shipHandling.playerNumber;
                    bullet.GetComponent<BulletCollision>().bulletHitPoints = shipDetails.Primary.HitPoints;
                    bullet.gameObject.tag = "Bullet";

                    Transform transform = bullet.GetComponentInChildren<Transform>();
                    transform.localScale = new Vector3(shipDetails.Primary.Scale, shipDetails.Primary.Scale, shipDetails.Primary.Scale);


                    // Add velocity to the bullet
                    bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * shipDetails.Primary.Speed;

                    BulletCollision bulletCol = bullet.GetComponentInChildren<BulletCollision>(); //transform.Find("BulletCollision");
                                                                                                  //ScriptB other = (ScriptB)go.GetComponent(typeof(ScriptB));
                    bulletCol.setDamage(shipDetails.Primary.Damage);

                    createdBullets.Add(bullet);

                    // Destroy the bullet after X seconds
                    Destroy(bullet, shipDetails.Primary.TimeToLive);

                    CheckForMaxInstances();

                }

                shipHandling.currentBattery = shipHandling.currentBattery - shipDetails.Primary.BatteryCharge;
                shipHandling.lastShot = Time.time;
            }


        }


    }

    private void CheckForMaxInstances()
    {
        ShipHandling shipHandling = this.GetComponentInParent<ShipHandling>();
        ShipDetails shipDetails = shipHandling.shipDetails;
        if (createdBullets.Count > shipDetails.Primary.MaxInstances)
        {
            for (int i = (createdBullets.Count - shipDetails.Primary.MaxInstances); i > 0; i--)
            {
                GameObject oldestBullet = createdBullets[0];
                Destroy(oldestBullet);
                createdBullets.RemoveAt(0);
            }
        }
    }


    public void RedrawShip17Primary()
    {
        ShipHandling shipHandling = this.GetComponentInParent<ShipHandling>();
        LineRenderer laserBeamRenderer = this.GetComponentInChildren<LineRenderer>();
        ShipDetails shipDetails = shipHandling.shipDetails;

        List<Transform> bulletPrimarySpawnPoints = new List<Transform>();
        int i = 0;
        foreach (Transform child in transform)
        {
            if (child.CompareTag("BulletSpawn") && child.name.Contains("BulletSpawnPointPrimary1"))
            {
                bulletPrimarySpawnPoints.Add(child.transform);
                i++;
            }
        }

        List<Transform> usedSpawnPoints = new List<Transform>();

        usedSpawnPoints = bulletPrimarySpawnPoints;

        foreach (Transform currBulletSpawnPoint in usedSpawnPoints)
        {
            RaycastHit hit;
            laserBeamRenderer.enabled = true;
            laserBeamRenderer.SetPosition(0, currBulletSpawnPoint.position);
            Vector3 direction = currBulletSpawnPoint.transform.forward;

            // Reduce laser length according to ship speed
            Rigidbody rb = GetComponent<Rigidbody>();
            Vector3 vel = rb.velocity;

            float finalLaserLength = Ship17LaserLength - vel.magnitude;
            //Debug.Log("finalLaserLength = " + finalLaserLength);

            if (finalLaserLength < Ship17LaserMinLength)
            {
                finalLaserLength = Ship17LaserMinLength;
            }

            Vector3 endPoint = currBulletSpawnPoint.transform.position + currBulletSpawnPoint.transform.forward * finalLaserLength;

            Vector3 fwd = currBulletSpawnPoint.transform.TransformDirection(Vector3.forward);


            if (Physics.Raycast(currBulletSpawnPoint.transform.position, fwd, out hit, finalLaserLength))
            {
                //print("There is something in front of the object! " + hit.distance);
                endPoint = hit.point;

                ShipHandling hitShipHandling = hit.collider.gameObject.GetComponentInParent<ShipHandling>();


                // Do damage 
                if (Ship17LaserDrawCount >= Ship17LaserDoDamageInterval)
                {
                    // Don't do damage every time laser is drawn
                    GameObject instance = Resources.Load("Prefabs/ShrapnelExplosionMedium") as GameObject;
                    GameObject explosion = Instantiate(instance, hit.point, Quaternion.identity);
                    Destroy(explosion, 3.0f);
                    Ship17LaserDrawCount = 0;

                    if (hitShipHandling != null)
                    {
                        hitShipHandling.DoDamage(shipDetails.Primary.Damage);
                    }
                }
                else
                {
                    Ship17LaserDrawCount++;
                }
            }

            laserBeamRenderer.SetPosition(1, endPoint);
        }

    }
    public void Ship17Primary()
    {
        ShipHandling shipHandling = this.GetComponentInParent<ShipHandling>();
        ShipDetails shipDetails = shipHandling.shipDetails;
        float usedFireRate = shipDetails.Primary.FireRate;

        if (Time.time > usedFireRate + shipHandling.lastShot)
        {
            if (shipHandling.currentBattery >= shipDetails.Primary.BatteryCharge)
            {
                Ship17LaserActive = true;
                // The laser drawing is done in Update...
                Ship17LaserDurationLeft = Ship17LaserDuration;
                shipHandling.currentBattery = shipHandling.currentBattery - shipDetails.Primary.BatteryCharge;
                shipHandling.lastShot = Time.time;

            }
        }
    }



}
