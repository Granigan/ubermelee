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
            if (child.CompareTag("BulletSpawn") && child.name.Contains("BulletSpawnPointMain"))
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


    private void genericPrimaryShoot()
    {
        ShipHandling shipHandling = this.GetComponentInParent<ShipHandling>();
        ShipDetails shipDetails = shipHandling.shipDetails;
        float usedFireRate = shipDetails.WeaponMain.FireRate;


        if (Time.time > usedFireRate + shipHandling.lastShot)
        {
            if (shipHandling.currentBattery >= shipDetails.WeaponMain.BatteryCharge)
            {
                List<Transform> usedSpawnPoints = new List<Transform>();

                usedSpawnPoints = bulletSpawnPoints;

                foreach (Transform currBulletSpawnPoint in usedSpawnPoints)
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

                shipHandling.currentBattery = shipHandling.currentBattery - shipDetails.WeaponMain.BatteryCharge;
                shipHandling.lastShot = Time.time;
            }


        }
    }

}
