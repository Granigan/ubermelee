using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineColor : MonoBehaviour
{

    public GameObject[] players;
    public Color[] ringColor;
    public GameObject ring;
    GameObject mine;
    public int player;
    public float playerNumber;
    //public GameObject[] howMany;
    //public int i;
    //bool shipDestroyed;

    // Use this for initialization
    void Start()
    {

        ringColor = new Color[5];
        ringColor[0] = Color.white;
        ringColor[1] = Color.cyan;
        ringColor[2] = Color.red;
        ringColor[3] = Color.green;
        ringColor[4] = Color.magenta;
        rings();
    }


    // Update is called once per frame
    void rings()
    {
        mine = this.gameObject;
        playerNumber = GetComponent<BulletCollision>().bulletOwnerPlayerNumber;
        player = (int)playerNumber;
        GameObject childObject = Instantiate(ring) as GameObject;
        childObject.transform.parent = mine.transform;
        childObject.transform.localPosition = Vector3.zero;
        childObject.GetComponent<Renderer>().material.color = ringColor[player];

    }
}

