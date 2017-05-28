using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public GameObject player;       //Public variable to store a reference to the player game object
    public GameObject playerPrefab;

    private Vector3 offset;         //Private variable to store the offset distance between the player and camera
    public float respawnTimer;

    // Use this for initialization
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        if(player != null)
        {
            // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
            transform.position = player.transform.position + offset;

        } else
        {
            // Player is dead! Time to respawn
            respawnTimer += Time.deltaTime;
            if (respawnTimer > 2)
            {
                //Action
                respawnTimer = 0;
                Quaternion quat = new Quaternion();
                quat.Set(180f, 0f, 0f, 0f);
                
                player = Instantiate(playerPrefab, new Vector3(transform.position.x, transform.position.y, -3.5f), quat);
                
            }
        }

    }
}