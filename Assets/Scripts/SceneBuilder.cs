using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBuilder : MonoBehaviour {
    //public GameObject Camera;
    //public GameObject Light;
    //public GameObject Counter;
    public GameObject Planet;
    public GameObject Ship1;
    public GameObject Ship2;

    //public int[] PosArray = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
    //public int[] NegArray = { -1, -2, -3, -4, -5, -6, -7, -8, -9, -10, -11, -12, -13, -14, -15 };


    //int x;
    //int y;

    // Use this for initialization
    void Awake() {
        //Instantiate(Camera);
        //Instantiate(Light);
        //Instantiate(Counter);
        Instantiate(Planet);
        BuildShips();
        // random vector3 values x, y. z is allways same.
        //Vector3 RandoPos = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), -3);
        //Vector3 RandoPos2 = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), -3);
        //Vector3 RandoPos2 = new Vector3(Random.Range(NegArray.Length, PosArray.Length), Random.Range(NegArray.Length, PosArray.Length), 0);
        //x = Random.Range(-15, 15);
        //y = Random.Range(-15, 15);
    }

    void BuildShips()
        {
            Instantiate(Ship1);
            Ship1.transform.position = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), -3);
            Ship1.transform.Rotate(Vector3.forward, Random.Range(-180, 180));
            {
                Collider[] hitColliders = Physics.OverlapSphere(Ship1.transform.position, 1);
                while (hitColliders.Length > 1)
                {
                    Ship1.transform.position = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), -3);
                    hitColliders = Physics.OverlapSphere(Ship1.transform.position, 1);
                }
                
            }
            {
                Instantiate(Ship2);
                Ship2.transform.position = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), -3);
                Ship2.transform.Rotate(Vector3.forward, Random.Range(-180, 180));
                {
                    Collider[] hitColliders2 = Physics.OverlapSphere(Ship2.transform.position, 1);
                    while (hitColliders2.Length > 1)
                    {
                        Ship2.transform.position = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), -3);
                    hitColliders2 = Physics.OverlapSphere(Ship2.transform.position, 1);
                }
                    
                }
                 
            }
       
    }
}
