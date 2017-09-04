using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour {

    public GameObject[] players;
    public Color[] ringColor;
    public GameObject ring;
    GameObject ship;
    public int player;
    public float playerNumber;
    //public GameObject[] howMany;
    //public int i;
    //bool shipDestroyed;

    // Use this for initialization
    void Start () {
         
    ringColor = new Color[5];
    ringColor[0] = Color.white;
    ringColor[1] = Color.cyan;
    ringColor[2] = Color.red;
    ringColor[3] = Color.green;
    ringColor[4] = Color.magenta;
    rings();
    }
    //void Awake(){

        //howMany = GameObject.FindGameObjectsWithTag("CameraObject"); 
        //Debug.Log("monta" + howMany.Length);

        //i = howMany.Length;
        //Debug.Log(i);
        //if (i <= 3)
        //{
        //  i++;
        //  rings();
        
        //rings();
        //}

    //}

    // Update is called once per frame
    void rings () {
        ship = this.gameObject;
        playerNumber = GetComponent<ShipHandling>().playerNumber;
        player = (int)playerNumber;
        GameObject childObject = Instantiate(ring) as GameObject;
        childObject.transform.parent = ship.transform;
        childObject.transform.localPosition = Vector3.zero;
        childObject.GetComponent<Renderer>().material.color = ringColor[player];
        //childObject.GetComponentInChildren<Renderer>().material.color = ringColor[player];
        //players = GameObject.FindGameObjectsWithTag("CameraObject");
        //float playerNumber;
        //int player;

        //foreach (GameObject ship in players)

        //{

        //ship.GetComponent<ShipHandling>().playerNumber = playerNumber;
        //playerNumber = ship.GetComponent<ShipHandling>().playerNumber;
        //player = (int)playerNumber;
        //GameObject childObject = Instantiate(ring) as GameObject;
        //childObject.transform.parent = ship.transform;
        //childObject.transform.localPosition = Vector3.zero;
        //childObject.GetComponentInChildren<Renderer>().material.color = ringColor[player];
        //player++;
        //  i++;
        //}
    }
}

