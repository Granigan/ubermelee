using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All the ship secondary actions go here. They are invoked by shipID.
public class ShipSecondaryActions : MonoBehaviour
{
    private bool Ship47SecondaryActive = false;
    float Ship47SpeedReduction = 100f;
    float Ship47RotationSpeed = 0f;
    float Ship47InitialRotationSpeed = 23.3f;
    // Use this for initialization
    void Start()
    {
        Ship47RotationSpeed = Ship47InitialRotationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(Ship47SecondaryActive == true)
        {
            RotateShip47();
        }

    }

    //Ship31 Secondary
    public void Ship31Secondary()
    {
        List<Transform> bulletSpecialSpawnPoints = new List<Transform>();
        int i = 0;
        foreach (Transform child in transform)
        {
            if (child.CompareTag("BulletSpawn") && child.name.Contains("BulletSpawnPointSecondary"))
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
                bullet.GetComponent<BulletCollision>().bulletHitPoints = shipDetails.Secondary.HitPoints;
                bullet.gameObject.tag = "Bullet";

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

   
    public void Ship47Secondary()
    {
        ShipHandling shipHandling = this.GetComponentInParent<ShipHandling>();
        ShipDetails shipDetails = shipHandling.shipDetails;
<<<<<<< HEAD
        if(shipHandling.currentBattery >= shipDetails.Secondary.BatteryCharge)
        {
            //This works as well, but not smoothly..
            //transform.Rotate(0, 0, Time.deltaTime * 3000, Space.Self);
            
            Vector3 eulerAngleVelocity = new Vector3(0,0,-180);
            Rigidbody rb = shipHandling.GetComponent<Rigidbody>();
            Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity *1);
            rb.MoveRotation(rb.rotation * deltaRotation);
=======
        

        if (shipHandling.currentBattery >= shipDetails.Secondary.BatteryCharge && Ship47SecondaryActive == false)
        {
            Ship47SecondaryActive = true;
            shipHandling.currentBattery -= shipDetails.Secondary.BatteryCharge;
        } 

    }
>>>>>>> f02afbea1adc735de60e7110bd1b529f2774cb51

    void RotateShip47()
    {

        //Debug.Log(Ship47RotationSpeed);
        if (Ship47RotationSpeed > 0)
        {
            Ship47RotationSpeed -= Ship47SpeedReduction * Time.deltaTime; 
            this.GetComponentInParent<ShipHandling>().RotateShip(Ship47RotationSpeed);
        }
        else
        {
            Ship47RotationSpeed = Ship47InitialRotationSpeed;
            Ship47SecondaryActive = false;
        }
<<<<<<< HEAD



        //}
        //Transform currTransform = GetComponent<Transform>();
=======
>>>>>>> f02afbea1adc735de60e7110bd1b529f2774cb51

    }


<<<<<<< HEAD

=======
>>>>>>> f02afbea1adc735de60e7110bd1b529f2774cb51


    public void Ship63Secondary()
    {
        ShipHandling shipHandling = this.GetComponentInParent<ShipHandling>();
        ShipDetails shipDetails = shipHandling.shipDetails;
        if (shipHandling.currentBattery >= shipDetails.Secondary.BatteryCharge)
        {
            DeployMine();
        }
    }


    public void DeployMine()
    {
        List<Transform> bulletSpecialSpawnPoints = new List<Transform>();
        int i = 0;
        foreach (Transform child in transform)
        {
            if (child.CompareTag("BulletSpawn") && child.name.Contains("BulletSpawnPointSecondary"))
            {
                bulletSpecialSpawnPoints.Add(child.transform);
                i++;
            }
        }

        ShipHandling shipHandling = this.GetComponentInParent<ShipHandling>();
        ShipDetails shipDetails = shipHandling.shipDetails;


        foreach (Transform currBulletSpawnPoint in bulletSpecialSpawnPoints)
        {
            
            GameObject bullet = (GameObject)Instantiate(
            shipDetails.Secondary.SecondaryPrefab,
            currBulletSpawnPoint.position,
            currBulletSpawnPoint.rotation);
            bullet.GetComponent<BulletCollision>().bulletOwnerPlayerNumber = shipHandling.playerNumber;
            Transform transform = bullet.GetComponentInChildren<Transform>();
            transform.localScale = new Vector3(shipDetails.Secondary.Scale, shipDetails.Secondary.Scale, shipDetails.Secondary.Scale);
            bullet.GetComponent<BulletCollision>().bulletHitPoints = shipDetails.Secondary.HitPoints;
            bullet.gameObject.tag = "Bullet";
<<<<<<< HEAD

=======
            bullet.GetComponent<BulletCollision>().isMine = true;
            bullet.transform.SetPositionAndRotation(currBulletSpawnPoint.position, Quaternion.identity);
>>>>>>> f02afbea1adc735de60e7110bd1b529f2774cb51
            // Add velocity to the bullet
            //bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * shipDetails.Secondary.Speed;

            BulletCollision bulletCol = bullet.GetComponentInChildren<BulletCollision>();

            bulletCol.setDamage(shipDetails.Secondary.Damage);

            Destroy(bullet, shipDetails.Secondary.TimeToLive);
            
        }


        shipHandling.currentBattery = shipHandling.currentBattery - shipDetails.Secondary.BatteryCharge;



        shipHandling.lastSecondaryUsed = Time.time;

    }



    //Ship64 Secondary
    public void Ship64Secondary()
    {
        List<Transform> bulletSpecialSpawnPoints = new List<Transform>();
        int i = 0;
        foreach (Transform child in transform)
        {
            if (child.CompareTag("BulletSpawn") && child.name.Contains("BulletSpawnPointSecondary"))
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
                bullet.GetComponent<BulletCollision>().bulletHitPoints = shipDetails.Secondary.HitPoints;
                bullet.gameObject.tag = "Bullet";

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

}
