using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All the ship secondary actions go here. They are invoked by shipID.
public class ShipSecondaryActions : MonoBehaviour
{
    private bool Ship47SecondaryActive = false;
    float Ship47SpeedReduction = 100f;
    float Ship47RotationSpeed = 0f;
    float Ship47InitialRotationSpeed = 25.3f;
    // Audio SFX
    private AudioSource source47;
    float ShipXXLaserLength = 12f;
    float ShipXXLaserDuration = 0.05f;
    float ShipXXLaserDurationLeft = 0f;
    bool ShipXXLaserActive = false;
    private List<GameObject> createdBullets;
    
    // Use this for initialization
    void Start()
    {
        LineRenderer laserBeamRenderer = this.GetComponentInChildren<LineRenderer>();
        if(laserBeamRenderer != null)
        {
            laserBeamRenderer.SetPosition(0, new Vector3(0f, 0f, 0f));
        }

        createdBullets = new List<GameObject>();
        Ship47RotationSpeed = Ship47InitialRotationSpeed;

        // Audio SFX
        //source47 = GetComponentInParent<AudioSource>();
        source47 = gameObject.AddComponent<AudioSource>();
        //Debug.Log("what " + source47);
        
        // Audio SFX
    }

    // Update is called once per frame
    void Update()
    {
        if(Ship47SecondaryActive == true)
        {
            RotateShip47();
        }

        if (ShipXXLaserActive == true)
        {
            ShipXXLaserDurationLeft = ShipXXLaserDurationLeft - Time.deltaTime;
            if(ShipXXLaserDurationLeft <= 0)
            {
                // Destroy laser
                this.GetComponentInChildren<LineRenderer>().enabled = false;
                ShipXXLaserActive = false;
            }
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
                shipDetails.Secondary.SecondaryPrefab,
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

                createdBullets.Add(bullet);

                // Destroy the bullet after X seconds
                Destroy(bullet, shipDetails.Secondary.TimeToLive);

                CheckForMaxInstances();
            }


            shipHandling.currentBattery = shipHandling.currentBattery - shipDetails.Secondary.BatteryCharge;



            shipHandling.lastSecondaryUsed = Time.time;
        }



    }
    //Ship17 Secondary
    public void Ship17Secondary()
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
                shipDetails.Secondary.SecondaryPrefab,
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

                createdBullets.Add(bullet);

                // Destroy the bullet after X seconds
                Destroy(bullet, shipDetails.Secondary.TimeToLive);

                CheckForMaxInstances();
            }


            shipHandling.currentBattery = shipHandling.currentBattery - shipDetails.Secondary.BatteryCharge;



            shipHandling.lastSecondaryUsed = Time.time;
        }



    }

   
    public void Ship47Secondary()
    {
        ShipHandling shipHandling = this.GetComponentInParent<ShipHandling>();
        ShipDetails shipDetails = shipHandling.shipDetails;
        


        if (shipHandling.currentBattery >= shipDetails.Secondary.BatteryCharge && Ship47SecondaryActive == false)
        {
            Ship47SecondaryActive = true;
            shipHandling.currentBattery -= shipDetails.Secondary.BatteryCharge;

            AudioClip Woosh = shipDetails.Secondary.SecondarySound;
            //Debug.Log("Woosh is " + Woosh.ToString());
            //Debug.Log("source47 is " + source47.ToString());

            source47.PlayOneShot(Woosh);
        } 

    }

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

    }




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
            bullet.GetComponent<BulletCollision>().isMine = true;
            bullet.transform.SetPositionAndRotation(currBulletSpawnPoint.position, Quaternion.identity);
            
            BulletCollision bulletCol = bullet.GetComponentInChildren<BulletCollision>();

            bulletCol.setDamage(shipDetails.Secondary.Damage);
            createdBullets.Add(bullet);

            Destroy(bullet, shipDetails.Secondary.TimeToLive);

            CheckForMaxInstances();
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
                shipDetails.Secondary.SecondaryPrefab,
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
                createdBullets.Add(bullet);
                // Destroy the bullet after X seconds
                Destroy(bullet, shipDetails.Secondary.TimeToLive);
                CheckForMaxInstances();
            }
            
            shipHandling.currentBattery = shipHandling.currentBattery - shipDetails.Secondary.BatteryCharge;
            
            shipHandling.lastSecondaryUsed = Time.time;
        }
        
    }



    private void CheckForMaxInstances()
    {
        ShipHandling shipHandling = this.GetComponentInParent<ShipHandling>();
        ShipDetails shipDetails = shipHandling.shipDetails;
        //Debug.Log("createdBullets.Count = " + createdBullets.Count + " shipDetails.Secondary.MaxInstances = " + shipDetails.Secondary.MaxInstances);
        if (createdBullets.Count > shipDetails.Secondary.MaxInstances)
        {
            for (int i = (createdBullets.Count - shipDetails.Secondary.MaxInstances); i > 0; i--)
            {
                GameObject oldestBullet = createdBullets[0];
                Destroy(oldestBullet);
                createdBullets.RemoveAt(0);
            }
        }
    }

    //ShipXX Laser temporarily here
    public void ShipXXSecondary()
    {
        if (ShipXXLaserActive == true)
        {
            return;
        }

        ShipHandling shipHandling = this.GetComponentInParent<ShipHandling>();
        ShipDetails shipDetails = shipHandling.shipDetails;
        LineRenderer laserBeamRenderer = this.GetComponentInChildren<LineRenderer>();

        List<Transform> bulletSpecialSpawnPoints = new List<Transform>();
        int i = 0;
        foreach (Transform child in transform)
        {
            if (child.CompareTag("BulletSpawn") && child.name.Contains("BulletSpawnPointPrimary"))
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

            Vector3 endPoint = currBulletSpawnPoint.transform.position + currBulletSpawnPoint.transform.forward * ShipXXLaserLength;

            Vector3 fwd = currBulletSpawnPoint.transform.TransformDirection(Vector3.forward);


            if (Physics.Raycast(currBulletSpawnPoint.transform.position, fwd, out hit, ShipXXLaserLength))
            {
                //print("There is something in front of the object! " + hit.distance);
                endPoint = hit.point;

                GameObject instance = Resources.Load("Prefabs/ShrapnelExplosionMedium") as GameObject;

                GameObject explosion = Instantiate(instance, hit.point, Quaternion.identity);
                Destroy(explosion, 3.0f);

                ShipHandling hitShipHandling = hit.collider.gameObject.GetComponentInParent<ShipHandling>();
                // Do damage 

                if (hitShipHandling != null)
                    hitShipHandling.DoDamage(shipDetails.Secondary.Damage);
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


    }

}
