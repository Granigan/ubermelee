using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class InputController : MonoBehaviour
{
    public ShipDetails shipDetails;
    
    //public float thrustForce = 15f;
    public Rigidbody rb;

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

    void Start()
    {
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
       
        bool fireButton = false;
        if (joystick != null)
            fireButton = joystick.Action1;

   
        if (Input.GetButtonDown(shoot) == true)
        {
            fireButton = Input.GetButtonDown(shoot);
        }

        if ( fireButton == true)
        {
            Fire();
        }

 
    }

    void Fire()
    {      
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 15;

        // Destroy the bullet after 5 seconds
        Destroy(bullet, 2.0f);
        
    }
    
}


