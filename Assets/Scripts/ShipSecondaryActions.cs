using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All the ship secondary actions go here. They are invoked by shipID.
public class ShipSecondaryActions : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    //Ship31 Secondary
    public void Ship31Secondary()
    {
        List<Transform> bulletSpecialSpawnPoints = new List<Transform>();
        int i = 0;
        foreach (Transform child in transform)
        {
            if (child.CompareTag("BulletSpawn") && child.name.Contains("BulletSpawnPointSpecial"))
            {
                bulletSpecialSpawnPoints.Add(child.transform);
                i++;
            }
        }

        ShipHandling shipHandling = this.GetComponentInParent<ShipHandling>();
        ShipDetails shipDetails = shipHandling.shipDetails;


        if (shipHandling.currentBattery >= shipDetails.Special.BatteryCharge)
        {
            List<Transform> usedSpawnPoints = new List<Transform>();

            usedSpawnPoints = bulletSpecialSpawnPoints;



            foreach (Transform currBulletSpawnPoint in usedSpawnPoints)
            {
                GameObject bullet = (GameObject)Instantiate(
                shipDetails.WeaponMain.bulletPrefab,
                currBulletSpawnPoint.position,
                currBulletSpawnPoint.rotation);

                Transform transform = bullet.GetComponentInChildren<Transform>();
                transform.localScale = new Vector3(shipDetails.Special.Scale, shipDetails.Special.Scale, shipDetails.Special.Scale);


                // Add velocity to the bullet
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * shipDetails.Special.Speed;

                BulletCollision bulletCol = bullet.GetComponentInChildren<BulletCollision>();

                bulletCol.setDamage(shipDetails.Special.Damage);

                // Destroy the bullet after X seconds
                Destroy(bullet, shipDetails.Special.TimeToLive);

            }


            shipHandling.currentBattery = shipHandling.currentBattery - shipDetails.Special.BatteryCharge;



            shipHandling.lastSpecialUsed = Time.time;
        }



    }

    //Ship47 Special
    public void Ship47Secondary()
    {
        //Transform currTransform = GetComponent<Transform>();
        transform.Rotate(0, 0, 180f, Space.Self);

        //transform.RotateAround(transform.position, transform.up, 180f);
        //Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * 10f * Time.deltaTime * -1);

    }


}
