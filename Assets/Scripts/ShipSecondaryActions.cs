﻿using System.Collections;
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


        if (shipHandling.currentBattery >= shipDetails.Secondary.BatteryCharge)
        {
            List<Transform> usedSpawnPoints = new List<Transform>();

            usedSpawnPoints = bulletSpecialSpawnPoints;



            foreach (Transform currBulletSpawnPoint in usedSpawnPoints)
            {
                GameObject bullet = (GameObject)Instantiate(
                shipDetails.Primary.bulletPrefab,
                currBulletSpawnPoint.position,
                currBulletSpawnPoint.rotation);
                bullet.GetComponent<BulletCollision>().bulletOwnerPlayerNumber = shipHandling.playerNumber;
                Transform transform = bullet.GetComponentInChildren<Transform>();
                transform.localScale = new Vector3(shipDetails.Secondary.Scale, shipDetails.Secondary.Scale, shipDetails.Secondary.Scale);


                // Add velocity to the bullet
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * shipDetails.Secondary.Speed;

                BulletCollision bulletCol = bullet.GetComponentInChildren<BulletCollision>();

                bulletCol.setDamage(shipDetails.Secondary.Damage);

                // Destroy the bullet after X seconds
                Destroy(bullet, shipDetails.Secondary.TimeToLive);

            }


            shipHandling.currentBattery = shipHandling.currentBattery - shipDetails.Secondary.BatteryCharge;



            shipHandling.lastSecondaryUsed = Time.time;
        }



    }

    //Ship47 Special
    public void Ship47Secondary()
    {
        ShipHandling shipHandling = this.GetComponentInParent<ShipHandling>();
        ShipDetails shipDetails = shipHandling.shipDetails;
        if(shipHandling.currentBattery >= shipDetails.Secondary.BatteryCharge)
        {
            transform.Rotate(0, 0, 180f, Space.Self);

            shipHandling.currentBattery -= shipDetails.Secondary.BatteryCharge;

        }
        //Transform currTransform = GetComponent<Transform>();


        //transform.RotateAround(transform.position, transform.up, 180f);
        //Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * 10f * Time.deltaTime * -1);

    }


}
