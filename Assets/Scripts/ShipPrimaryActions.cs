using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All the ship ship primary actions go here go here. They are invoked by shipID.
public class ShipPrimaryActions : MonoBehaviour
{
    private List<Transform> bulletSpawnPoints;

    // Use this for initialization
    void Start()
    {
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
    }

    public void Ship31Primary()
    {
        genericPrimaryShoot();

    }

    public void Ship47Primary()
    {
        genericPrimaryShoot();
    }

    public void Ship63Primary()
    {
        genericPrimaryShoot();

    }

    public void Ship64Primary()
    {
        genericPrimaryShoot();

    }


    private void genericPrimaryShoot()
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
                    bullet.GetComponent<BulletCollision>().bulletOwnerPlayerNumber = shipHandling.playerNumber;

                    Transform transform = bullet.GetComponentInChildren<Transform>();
                    transform.localScale = new Vector3(shipDetails.Primary.Scale, shipDetails.Primary.Scale, shipDetails.Primary.Scale);


                    // Add velocity to the bullet
                    bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * shipDetails.Primary.Speed;

                    BulletCollision bulletCol = bullet.GetComponentInChildren<BulletCollision>(); //transform.Find("BulletCollision");
                                                                                                  //ScriptB other = (ScriptB)go.GetComponent(typeof(ScriptB));
                    bulletCol.setDamage(shipDetails.Primary.Damage);

                    // Destroy the bullet after X seconds
                    Destroy(bullet, shipDetails.Primary.TimeToLive);

                }

                shipHandling.currentBattery = shipHandling.currentBattery - shipDetails.Primary.BatteryCharge;
                shipHandling.lastShot = Time.time;
            }


        }
    }

}
