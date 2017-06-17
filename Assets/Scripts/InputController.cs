using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class InputController : MonoBehaviour
{
    public ShipDetails shipDetails;
    private WeaponDetails weaponDetails;
    
    //public float thrustForce = 15f;
    private Rigidbody rb;

    public GameObject thruster;
    public TrailRenderer trail;

    //public float maxSpeed = 200f;//Replace with your max speed

    public Vector3 eulerAngleVelocity;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    public int playerNumber = 1; // The order of the player regarding controller

    
    //input controls
    public string TurnControl = "Vertical1";
    public string ThrustControl = "Horizontal1";
    public string shoot = "Fire1";

    InputDevice joystick;

    private float lastShot = 0.0f;
    private float currBattery = 0;
    private float lastFrameTime = 0;

    

    void Start()
    {
        weaponDetails = shipDetails.WeaponMain;
        currBattery = shipDetails.Battery;
        //shipDetails = GetComponent<ShipDetails>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.mass = shipDetails.Mass;
        rb.drag = shipDetails.Drag;
        rb.angularDrag = shipDetails.AngularDrag;
        trail = GetComponentInChildren<TrailRenderer>();
    }

    private void Awake()
    {
        foreach(InputDevice currDevice in InputManager.Devices)
        {
            //Debug.Log("Device type is " + currDevice.ToString());
        }

        if(InputManager.Devices.Count > playerNumber)
        {
            joystick = InputManager.Devices[playerNumber];
        } else
        {
            joystick = null;
        }
       
        
    }

    void Update()
    {
        float shipTurn = 0;
        if (Input.GetAxis(TurnControl) != 0)
        {
            shipTurn = Input.GetAxis(TurnControl);
        }
        else
        {
            if(joystick != null)
                shipTurn = joystick.LeftStickX + (-1*joystick.DPadLeft) + joystick.DPadRight;
        }
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime * -1 * shipTurn * shipDetails.RotationRate);

        eulerAngleVelocity.y = 0;
        rb.MoveRotation(rb.rotation * deltaRotation);


        float speedValue = 0;
        if (joystick != null)
            speedValue = joystick.RightTrigger + (-1* joystick.LeftTrigger);

        if (Input.GetAxis(ThrustControl) > 0)
        {
            speedValue = Input.GetAxis(ThrustControl);
        }
        //Debug.Log("shipTurn = " + shipTurn + " speedvalue = " + speedValue);

        rb.AddForce(transform.right * shipDetails.Acceleration * speedValue);

        if (speedValue == 0)
        {
            //rb.freezeRotation = true;
            //thruster.GetComponent<TrailRenderer>().enabled = false;
            //trail.time = 0;
        }

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


        if (rb.velocity.magnitude > shipDetails.MaxSpeed)
        {
            rb.velocity = rb.velocity.normalized * shipDetails.MaxSpeed;
        }

        //float currVelocity = rb.velocity.x;

        currBattery = currBattery + ((Time.time - lastFrameTime) * shipDetails.BatteryRechargeRate);
        lastFrameTime = Time.time;
        if (currBattery > shipDetails.Battery)
        {
            currBattery = shipDetails.Battery;
        }
        //Debug.Log("currBattery=" + currBattery);


        bool fireButton = false;
        if (joystick != null)
            fireButton = joystick.Action1;

   
        if (Input.GetButton(shoot) == true)
        {
            fireButton = Input.GetButton(shoot);
        }

        if ( fireButton == true)
        {
            Fire();
        }

      
       

    }

    

    void Fire()
    {
        if (Time.time > weaponDetails.FireRate + lastShot)
        {
            if (currBattery >= weaponDetails.BatteryCharge)
            {
                GameObject bullet = (GameObject)Instantiate(
                    weaponDetails.bulletPrefab,
                    bulletSpawn.position,
                    bulletSpawn.rotation); // TODO: Maybe add bulletspawnpoint to WeaponDetails?

                Transform transform = bullet.GetComponentInChildren<Transform>();
                transform.localScale = new Vector3(weaponDetails.Scale, weaponDetails.Scale, weaponDetails.Scale);

                               
                // Add velocity to the bullet
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * weaponDetails.Speed;

                BulletCollision bulletCol = bullet.GetComponentInChildren<BulletCollision>(); //transform.Find("BulletCollision");
                //ScriptB other = (ScriptB)go.GetComponent(typeof(ScriptB));
                bulletCol.setDamage(weaponDetails.Damage);

                // Destroy the bullet after X seconds
                Destroy(bullet, weaponDetails.TimeToLive);

                
                currBattery = currBattery - weaponDetails.BatteryCharge;
                lastShot = Time.time;
            }


        }
        
    }
    
}


