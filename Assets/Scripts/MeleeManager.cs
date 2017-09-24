using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeleeManager : MonoBehaviour
{

    public GlobalSettings settings;
    public GameObject[] players;
    public SceneBuilder sceneBuilder;
    //public float GameOverScore = 10;

    // Use this for initialization
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = settings.GameSpeed;

        players = GameObject.FindGameObjectsWithTag("CameraObject");
        sceneBuilder = GameObject.FindGameObjectWithTag("Camera").GetComponent<SceneBuilder>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        players = GameObject.FindGameObjectsWithTag("CameraObject");

    }
    public bool GetAIEnabled(int playerNumber)
    {
        if (playerNumber == 1)
            return settings.Player1AIEnabled;
        if (playerNumber == 2)
            return settings.Player2AIEnabled;
        if (playerNumber == 3)
            return settings.Player3AIEnabled;
        if (playerNumber == 4)
            return settings.Player4AIEnabled;
        // Something went wrong here...
        return false;
    }

}
