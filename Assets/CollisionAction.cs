
using UnityEngine;

public class CollisionAction : MonoBehaviour {

    public GameObject explosionPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        //Debug.Log("OnCollisionEnter");
        if (col.gameObject.tag == "CameraObject")
        {
            Destroy(col.gameObject);
            Destroy(this.gameObject);
            var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }

}
