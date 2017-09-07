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
    public Material mesh;
    //public GameObject[] howMany;
    //public int i;
    //bool shipDestroyed;

    // Use this for initialization
    void Start()
    {

        ringColor = new Color[5];
        ringColor[0] = new Color(0,0,0,0);
        ringColor[1] = new Color32(0, 181, 255, 50);
        ringColor[2] = new Color32(255, 88, 71, 50);
        ringColor[3] = new Color32(101, 255, 8, 50);
        ringColor[4] = new Color32(255, 69, 220, 50);
        rings();
    }


    // Update is called once per frame
    void rings()
    {
        mine = this.gameObject;
        //playerNumber = GetComponent<BulletCollision>().bulletOwnerPlayerNumber;
        playerNumber = GetComponentInParent<BulletCollision>().bulletOwnerPlayerNumber;
        player = (int)playerNumber;
        GameObject childObject = Instantiate(ring) as GameObject;
        childObject.transform.parent = mine.transform;
        childObject.transform.localPosition = Vector3.zero;
        childObject.GetComponent<Renderer>().material.color = ringColor[player];

        mesh = GetComponent<MeshRenderer>().material;
        mesh.color = ringColor[player];
    }
}

