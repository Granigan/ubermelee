using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All the ship Specials go here. They are invoked by shipID.
public class ShipSpecials : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    //Ship31 Special
    public void Ship31Special()
    {
        //Debug.Log("Modelo 31 Especial!!!");
        List<Transform> bulletSpecialSpawnPoints = new List<Transform>();

        //bulletSpawnPoints = new List<Transform>();
        int i = 0;
        //bulletSpawnPoints = getChi getChildGameObject(transform.gameObject, "BulletSpawnPoint").transform;
        foreach (Transform child in transform)
        {
            //Debug.Log("child.name = " + child.name);
            if (child.CompareTag("BulletSpawn") && child.name.Contains("BulletSpawnPointSpecial"))
            {
                //Debug.Log("Selected child.name = " + child.name);
                //bulletSpawnPoints[i] = child.transform;
                bulletSpecialSpawnPoints.Add(child.transform);
                i++;
            }
        }

        //Debug.Log("bulletSpecialSpawnPoints length: " + bulletSpecialSpawnPoints.Count);

        ShipHandling shipHandling = this.GetComponentInParent<ShipHandling>();
        shipHandling.FireMainWeapon(bulletSpecialSpawnPoints);

}

    //Ship47 Special
    public void Ship47Special()
    {
        //Debug.Log("Modelo 47 Especial!!!");
        RaycastHit hit;
        float distance;

        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, forward, Color.green);

        if(Physics.Raycast(transform.position, (forward), out hit))
        {
            distance = hit.distance;
            //Debug.Log(distance + " " + hit.collider.gameObject.name);
        }

   
        //Debug.Log("transform " + transform.position.x + " " + transform.position.y);

    }

   
}
