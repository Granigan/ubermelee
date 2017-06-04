using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MoveShip2Player : MonoBehaviour {


    public float rotationSpeed = 100.0f;
    public float thrustForce = 15f;
    public Rigidbody rb;

    public GameObject thruster;
    public TrailRenderer trail;

    public float maxSpeed = 200f;//Replace with your max speed

    public Vector3 eulerAngleVelocity;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    public float CameraMaxZoom = 6;
    public float CameraMinZoom = 4;
    public float CameraZoomSpeed = 3;
    public float CameraZoom = 5;
    public int playerId = 0; // The Rewired player id of this character

    float prevMagnitude = 0;


    //input controls
    public string TurnControl = "Vertical1";
    public string ThrustControl = "Horizontal1";
    public string shoot = "Fire1";

    private Player player; // The Rewired Player

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        trail = GetComponentInChildren<TrailRenderer>();
    }

    private void Awake()
    {
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(playerId);
        

    }

    void Update()
    {

        // Rotate the ship if necessary
        // This works kind a ok...
        //transform.Rotate(0, 0, -Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime);
        
        //Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime * -Input.GetAxis(ThrustControl) * rotationSpeed);

        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime * -player.GetAxis("Turn Ship") * rotationSpeed);

        eulerAngleVelocity.y = 0;
        rb.MoveRotation(rb.rotation * deltaRotation);

        //Debug.Log("This is player " + playerId);

        // Thrust the ship if necessary
        //rb.AddForce(transform.right * thrustForce * Input.GetAxis(TurnControl));

        rb.AddForce(transform.right * thrustForce * player.GetAxis("Move"));

        //Debug.Log(Input.GetAxis("Horizontal"));

        if (Input.GetAxis(ThrustControl) == 0)
        {
            //rb.freezeRotation = true;
            //thruster.GetComponent<TrailRenderer>().enabled = false;
            //trail.time = 0;
        }

        if (Input.GetAxis(TurnControl) <= 0.1)
        {
            //rb.freezeRotation = true;
            {
                trail.time = 0.5f;
            }
            
        }
        if (Input.GetAxis(TurnControl) == 0)
        {
            //rb.freezeRotation = true;
            {
                trail.time = 0.0f;
            }
        }


        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        float currVelocity = rb.velocity.x;

        if (currVelocity != 0.0f)
        {
            if(currVelocity < prevMagnitude)
            {
                CameraZoom -= currVelocity * CameraZoomSpeed;
               

            } else
            {
                CameraZoom += currVelocity * CameraZoomSpeed;
            }
            CameraZoom = Mathf.Clamp(CameraZoom, CameraMinZoom, CameraMaxZoom);
        }
        prevMagnitude = currVelocity;
        //Debug.Log(CameraZoom);
        //Debug.Log(currVelocity);

        //Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, CameraZoom, CameraZoomSpeed * Time.deltaTime);


        /*
        Camera.main.orthographicSize = rb.velocity.magnitude *1.5f;

        if(Camera.main.orthographicSize > CameraMaxZoom)
        {
            Camera.main.orthographicSize = CameraMaxZoom;
            
        }

        if (Camera.main.orthographicSize < CameraMinZoom)
        {
            Camera.main.orthographicSize = CameraMinZoom;
        }
        */
        if (/*Input.GetButtonDown(shoot) ||*/ player.GetButtonDown("Shoot Weapon"))
        {
            Fire();
        }

        //Debug.Log(bulletSpawn.position);
        //Debug.Log(bulletSpawn.rotation);

        //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);

    }

    void Fire()
    {
        //Debug.Log(bulletSpawn.position);
        //Debug.Log(bulletSpawn.rotation);

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


