using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveShip : MonoBehaviour {


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
    float prevMagnitude = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        trail = GetComponentInChildren<TrailRenderer>();
    }

    
    void Update()
    {

        // Rotate the ship if necessary
        // This works kind a ok...
        //transform.Rotate(0, 0, -Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime);
        
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime * -Input.GetAxis("Horizontal") * rotationSpeed);
        eulerAngleVelocity.y = 0;
        rb.MoveRotation(rb.rotation * deltaRotation);
        

        // Thrust the ship if necessary
        rb.AddForce(transform.right * thrustForce *
                Input.GetAxis("Vertical"));

        //Debug.Log(Input.GetAxis("Horizontal"));

        if (Input.GetAxis("Horizontal") == 0)
        {
            //rb.freezeRotation = true;
            //thruster.GetComponent<TrailRenderer>().enabled = false;
            //trail.time = 0;
        }

        if (Input.GetAxis("Vertical") <= 0.1)
        {
            //rb.freezeRotation = true;
            {
                trail.time = 0.5f;
            }
            
        }
        if (Input.GetAxis("Vertical") == 0)
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
        Debug.Log(currVelocity);

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
        if (Input.GetKeyDown(KeyCode.Space))
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
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 20;

        // Destroy the bullet after 5 seconds
        Destroy(bullet, 5.0f);


        
    }

    void FixedUpdate()
    {

       

        if (Input.GetButtonDown("Jump"))
    {
        //rb.velocity = new Vector3(10, 0, 0);
        //rb.AddForce(new Vector3(10, 0, 0));
        //Debug.Log("Jump!!");
    }


    //rb.WakeUp();
    
}






}


