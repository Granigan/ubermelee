
using UnityEngine;
using System.Collections;

public class CollisionAction : MonoBehaviour
{

    public GameObject explosionPrefab;
    private IEnumerator coroutine;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision col)
    {
        //Debug.Log("OnCollisionEnter");
        if (col.gameObject.tag == "Bullet")
        {
            //Destroy(col.gameObject);
            StartCoroutine(DieAndRespawn());

            Destroy(col.gameObject);
            var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }

    private IEnumerator DieAndRespawn()
    {
        Debug.Log("Player just died!!");
        //this.gameObject.SetActive(false);
        //GetComponent(Rigidbody).enabled = false;
        GetComponentInChildren<MeshRenderer>().enabled = false;
        GetComponent<Transform>().localScale = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(2.0f);
        Debug.Log("Player just respawned!!");
        //transform.position = new Vector3(0.04833326f, 3.980667f, 0.0f);
        //transform.rotation = Quaternion.identity;
        //GetComponent<Renderer>().enabled = true;

        //this.gameObject.SetActive(true);
        GetComponentInChildren<MeshRenderer>().enabled = true;
        GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
    }

}
