using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All the ship ship primary actions go here go here. They are invoked by shipID.
public class ShipPrimaryActions : MonoBehaviour
{
    private List<Transform> bulletSpawnPoints;
    private List<GameObject> createdBullets;

    float ShipXXLaserLength = 12f;
    float ShipXXLaserMinLength = 4f;
    float ShipXXLaserDuration = 0.05f;
    float ShipXXLaserDurationLeft = 0f;
    bool ShipXXLaserActive = false;

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
        if (ShipXXLaserActive == true)
        {
            ShipXXLaserDurationLeft = ShipXXLaserDurationLeft - Time.deltaTime;
            if (ShipXXLaserDurationLeft <= 0)
            {
                // Destroy laser
                this.GetComponentInChildren<LineRenderer>().enabled = false;
                ShipXXLaserActive = false;
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
        if(createdBullets.Count > shipDetails.Primary.MaxInstances)
        {
            for(int i = (createdBullets.Count - shipDetails.Primary.MaxInstances); i > 0; i--) 
            {
                GameObject oldestBullet = createdBullets[0];
                Destroy(oldestBullet);
                createdBullets.RemoveAt(0);
            }
        }
    }





    public void Ship17Primary()
    {
        if (ShipXXLaserActive == true)
        {
            return;
        }

        ShipHandling shipHandling = this.GetComponentInParent<ShipHandling>();
        ShipDetails shipDetails = shipHandling.shipDetails;
        LineRenderer laserBeamRenderer = this.GetComponentInChildren<LineRenderer>();
        float usedFireRate = shipDetails.Primary.FireRate;

        if (Time.time > usedFireRate + shipHandling.lastShot)
        {
            if (shipHandling.currentBattery >= shipDetails.Primary.BatteryCharge)
            {

                List<Transform> bulletSpecialSpawnPoints = new List<Transform>();
                int i = 0;
                foreach (Transform child in transform)
                {
                    if (child.CompareTag("BulletSpawn") && child.name.Contains("BulletSpawnPointPrimary1"))
                    {
                        bulletSpecialSpawnPoints.Add(child.transform);
                        i++;
                    }
                }

                List<Transform> usedSpawnPoints = new List<Transform>();

                usedSpawnPoints = bulletSpecialSpawnPoints;

                foreach (Transform currBulletSpawnPoint in usedSpawnPoints)
                {
                    RaycastHit hit;
                    laserBeamRenderer.enabled = true;
                    laserBeamRenderer.SetPosition(0, currBulletSpawnPoint.position);
                    Vector3 direction = currBulletSpawnPoint.transform.forward;

                    // Reduce laser length according to ship speed
                    Rigidbody rb = GetComponent<Rigidbody>();
                    Vector3 vel = rb.velocity;

                    float finalLaserLength = ShipXXLaserLength - vel.magnitude;
                    //Debug.Log("finalLaserLength = " + finalLaserLength);

                    if(finalLaserLength < ShipXXLaserMinLength)
                    {
                        finalLaserLength = ShipXXLaserMinLength;
                    }

                    Vector3 endPoint = currBulletSpawnPoint.transform.position + currBulletSpawnPoint.transform.forward * finalLaserLength;

                    Vector3 fwd = currBulletSpawnPoint.transform.TransformDirection(Vector3.forward);


                    if (Physics.Raycast(currBulletSpawnPoint.transform.position, fwd, out hit, finalLaserLength))
                    {
                        //print("There is something in front of the object! " + hit.distance);
                        endPoint = hit.point;

                        GameObject instance = Resources.Load("Prefabs/ShrapnelExplosionMedium") as GameObject;

                        GameObject explosion = Instantiate(instance, hit.point, Quaternion.identity);
                        Destroy(explosion, 3.0f);

                        ShipHandling hitShipHandling = hit.collider.gameObject.GetComponentInParent<ShipHandling>();
                        // Do damage 

                        if (hitShipHandling != null)
                            hitShipHandling.DoDamage(shipDetails.Primary.Damage);
                    }


                    //if (Physics.Raycast(currBulletSpawnPoint.transform.position, direction, ShipXXLaserLength))
                    //    endPoint = hit.point;

                    laserBeamRenderer.SetPosition(1, endPoint);

                    //Debug.Log("Firing a laser..." + currBulletSpawnPoint.position.ToString() + " " + endPoint.ToString());

                    //endPoint.Set(currBulletSpawnPoint.position.x + ShipXXLaserLength, currBulletSpawnPoint.position.y, currBulletSpawnPoint.position.z);
                    //laserBeamRenderer.SetPosition(1, endPoint);
                    ShipXXLaserActive = true;

                    ShipXXLaserDurationLeft = ShipXXLaserDuration;
                }


                shipHandling.currentBattery = shipHandling.currentBattery - shipDetails.Primary.BatteryCharge;
                shipHandling.lastShot = Time.time;

            }
        }
    }



}
