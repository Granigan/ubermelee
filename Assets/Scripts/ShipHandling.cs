using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHandling : MonoBehaviour {

    public enum AILevels
    {
        IDIOT,
        ROOKIE,
        CYBORG
    };

    public ShipDetails shipDetails;

    //public float hitPoints = 20;
    public GameObject explosionPrefab;
    public float respawnVariance = 30f;
    private new Transform transform;
    public float shipID = 0;

    private IEnumerator coroutine;
    [HideInInspector]
    public float currentCrew;
    [HideInInspector]
    public float currentBattery;
    private List<Transform> bulletSpawnPoints;
    private ShipPrimaryActions shipPrimaryActions;
    private ShipSecondaryActions shipSecondaryActions;
    private GameObject primaryScript;

    public GameObject thruster;
    public TrailRenderer trail;
    public Vector3 eulerAngleVelocity;
    private Rigidbody rb;
    private float lastFrameTime = 0;
    [HideInInspector]
    public float lastShot = 0.0f;
    public float lastSecondaryUsed = 0.0f;
    public float playerNumber = 0f;

    public bool AIEnabled = false;
    public AILevels AILevel = AILevels.IDIOT;
    private float AILastExecuted = 1f;
    float AITurnDirection = 0f; // = Random.Range(-1.0f, 1.0f);

    public bool shipIsDead = false;

    // Use this for initialization
    void Start () {
        currentCrew = shipDetails.Crew;
        currentBattery = shipDetails.Battery;
        transform = GetComponentInChildren<Transform>();
        transform.localScale = new Vector3( shipDetails.Scale, shipDetails.Scale, transform.localScale.z);
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.mass = shipDetails.Mass;
        rb.drag = shipDetails.Drag;
        rb.angularDrag = shipDetails.AngularDrag;
        shipIsDead = false;
        //primaryScript = getComponent

        trail = GetComponentInChildren<TrailRenderer>();
        
        bulletSpawnPoints = new List<Transform>();
        int i = 0;
        //bulletSpawnPoints = getChi getChildGameObject(transform.gameObject, "BulletSpawnPoint").transform;
        foreach (Transform child in transform)
        {
            //Debug.Log("child.name = " + child.name);
            if (child.CompareTag("BulletSpawn") && child.name.Contains("BulletSpawnPointMain"))
            {
                //Debug.Log("Selected child.name = " + child.name);
                //bulletSpawnPoints[i] = child.transform;
                bulletSpawnPoints.Add(child.transform);
                i++;
            }
        }

        shipPrimaryActions = gameObject.AddComponent<ShipPrimaryActions>();
        shipSecondaryActions = gameObject.AddComponent<ShipSecondaryActions>();

        GameObject.FindGameObjectWithTag("Player1Stats").GetComponent<UpdatePlayerStats>().SetShipName((int)playerNumber, shipDetails.ShipName);
    }
	
	// Update is called once per frame
	void Update () {
        RechargeBattery();

        //disable trail
        //this.GetComponentInChildren<TrailRenderer>().enabled = false;

        transform.position = new Vector3(transform.position.x, transform.position.y, -3.0f);


        


        if (AIEnabled == true)
        {
            getClosestEnemy();
            executeAI();
        }

        GameObject.FindGameObjectWithTag("Player1Stats").GetComponent<UpdatePlayerStats>().SetShipName((int)playerNumber, shipDetails.ShipName);
    }

    public bool DoDamage(float Damage)
    {
        this.currentCrew = this.currentCrew - Damage;
        //Debug.Log("Hitpoints left " + this.currCrew);
        if(this.currentCrew <= 0)
        {
            ExplodeShip();
            //shipDetails.score++;
            return true;
        }

        return false;
    }

    private void ExplodeShip() { 
    
        //Debug.Log("Ship Explosion!!!");
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 3.0f);
        GameObject deadShip = this.gameObject;

        if(AIEnabled == true)
        {
            StartCoroutine(DieAndRespawn());
        }
        else
        {
            StartCoroutine(DieAndEnableShipSelectionUI());
        }

        currentCrew = shipDetails.Crew;
        //Instantiate(deadShip);
        
    }

    
    private IEnumerator DieAndRespawn()
    {
        shipIsDead = true;

        this.gameObject.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        this.gameObject.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;
        
        for (int j = 0; j < this.transform.childCount; j++)
        {
            this.transform.GetChild(j).gameObject.SetActive(false);
        }
                
        yield return new WaitForSeconds(3.0f);

        GameObject UICanvas = GameObject.FindGameObjectWithTag("Player" + playerNumber + "UIPanel");
        UICanvas.GetComponent<ShipSelection>().SelectionEnabled = true;

        int randomShipID = UICanvas.GetComponent<ShipSelection>().getRandomShipID();
        GameObject.FindGameObjectWithTag("Camera").GetComponent<SceneBuilder>().InstantiateShip(playerNumber, randomShipID, true) ;
        //UICanvas.GetComponent<CanvasGroup>().alpha = 1f;
        /*
        Vector3 respawnPoint = new Vector3(Random.Range(-respawnVariance, respawnVariance), Random.Range(-respawnVariance, respawnVariance), -3);  // Fix up the static -3 Z-axis later?

        this.GetComponentInChildren<Transform>().position = respawnPoint;

        this.GetComponentInChildren<Transform>().localScale = new Vector3(shipDetails.Scale, shipDetails.Scale, shipDetails.Scale);
        for (int j = 0; j < this.transform.childCount; j++)
        {
            this.transform.GetChild(j).gameObject.SetActive(true);
        }
        */
        //shipIsDead = false;
        //clone.GetComponent<ShipHandling>().playerNumber = playerNumber;

        Destroy(this.transform.gameObject);
    }

    private IEnumerator DieAndEnableShipSelectionUI()
    {
        //Debug.Log("Player just died!!");
        shipIsDead = true;

        this.gameObject.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        this.gameObject.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;
        
        for (int j = 0; j < this.transform.childCount; j++)
        {
            this.transform.GetChild(j).gameObject.SetActive(false);
        }
        
        yield return new WaitForSeconds(1.0f);

        //Debug.Log("Player" + playerNumber + "UIPanel");
        GameObject UICanvas = GameObject.FindGameObjectWithTag("Player" + playerNumber + "UIPanel");
        UICanvas.GetComponent<CanvasGroup>().alpha = 1f;
        UICanvas.GetComponent<ShipSelection>().SelectionEnabled = true;

        Destroy(this.transform.gameObject);

    }




    public bool RotateShip(float shipTurn) {
        if (shipIsDead == true) return false;

        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime * -1 * shipTurn * shipDetails.RotationRate);

        eulerAngleVelocity.y = 0;
        rb.MoveRotation(rb.rotation * deltaRotation);
        return true;
    }

    public bool MoveShip(float throttle)
    {
        if (shipIsDead == true) return false;

        rb.AddForce(transform.right * shipDetails.Acceleration * throttle);

        // when moving display trail
        if(this.GetComponentInChildren<TrailRenderer>())
            this.GetComponentInChildren<TrailRenderer>().enabled = true;

        if (throttle == 0)
        {
            // no thrus no trail
            if (this.GetComponentInChildren<TrailRenderer>())
                this.GetComponentInChildren<TrailRenderer>().enabled = false;

            //rb.freezeRotation = true;
            //thruster.GetComponent<TrailRenderer>().enabled = false;
            //trail.time = 0;
        }

        if (rb.velocity.magnitude > shipDetails.MaxSpeed)
        {
            rb.velocity = rb.velocity.normalized * shipDetails.MaxSpeed;
        }


        return true;
    }

    private bool RechargeBattery()
    {
        if (shipIsDead == true) return false;

        currentBattery = currentBattery + ((Time.time - lastFrameTime) * shipDetails.BatteryRechargeRate);
        lastFrameTime = Time.time;
        if (currentBattery > shipDetails.Battery)
        {
            currentBattery = shipDetails.Battery;
        }

        return true;
    }

    public bool UsePrimary(List<Transform> paramSpawnPoints = null, float paramBatteryCharge = -1, float paramFireRate = -1)
    {
        if (shipIsDead == true) return false;

        if (Time.time > shipDetails.Secondary.FireRate + lastSecondaryUsed)
        {
            shipPrimaryActions.Invoke("Ship" + shipID + "Primary", 0);
            lastSecondaryUsed = Time.time;
        }

        return true;
    }

    public bool UseSecondary()
    {
        if (shipIsDead == true) return false;

        if (Time.time > shipDetails.Secondary.FireRate + lastSecondaryUsed)
        {
            shipSecondaryActions.Invoke("Ship" + shipID + "Secondary", 0);
            lastSecondaryUsed = Time.time;
        }

        return true;
    }
    
    static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }

    public float getCurrentCrew()
    {
        return currentCrew;
    }

    void setCurrentCrew(float newCrew)
    {
        currentCrew = newCrew;
    }

    public float getCurrentBattery()
    {
        return currentBattery;
    }

    void setCurrentBattery(float newBattery)
    {
        currentBattery = newBattery;
    }

    Transform getClosestEnemy()
    {
        float closestEnemyDistance = Mathf.Infinity;
        Transform targetPlayer = null;



        if (AILevel == AILevels.ROOKIE)
        {
            int i = 1;
            foreach (GameObject currShip in GameObject.FindGameObjectsWithTag("CameraObject"))
            {
                float distance = (currShip.transform.position - transform.position).sqrMagnitude;
                //Debug.Log("i=" + i + " distance = " + distance);
                i++;

                if (distance < closestEnemyDistance && distance > 0.1)
                {
                    closestEnemyDistance = distance;
                    //Debug.Log("closestEnemyDistance = " + closestEnemyDistance);
                    targetPlayer = currShip.gameObject.transform;
                }
            }

            // Turn the ship according to closest enemy
            float targetAngle = 0f;
            float aimRotation = 0f;
            float myX = Mathf.Abs(this.transform.position.x);
            float myY = Mathf.Abs(this.transform.position.y);

            if(targetPlayer == null)
            {
                // All other players are dead. Return null for now...
                return null;
            }

            float targetPlayerX = Mathf.Abs(targetPlayer.position.x);
            float targetPlayerY = Mathf.Abs(targetPlayer.position.y);
            float diffY = 0f;
            float diffX = 0f;

            // Case 1: Enemy top right from player
            if (targetPlayer.position.y >= this.transform.position.y && targetPlayer.position.x >= this.transform.position.x)
            {
                if ((targetPlayer.position.y > 0 && this.transform.position.y > 0) || (targetPlayer.position.y < 0 && this.transform.position.y < 0))
                {
                    diffY = targetPlayerY - myY;
                }
                else
                {
                    diffY = targetPlayerY + myY;
                }

                if ((targetPlayer.position.x > 0 && this.transform.position.x > 0) || (targetPlayer.position.x < 0 && this.transform.position.x < 0))
                {
                    diffX = targetPlayerX - myX;
                }
                else
                {
                    diffX = targetPlayerX + myX;
                }

                targetAngle = Mathf.Abs(Mathf.Atan((diffY) / (diffX)) * Mathf.Rad2Deg);
                aimRotation = targetAngle * -1;
            }
            // Case 2: Enemy top left from player
            else if (targetPlayer.position.y >= this.transform.position.y && targetPlayer.position.x <= this.transform.position.x)
            {
                if ((targetPlayer.position.y > 0 && this.transform.position.y > 0) || (targetPlayer.position.y < 0 && this.transform.position.y < 0))
                {
                    diffY = targetPlayerY - myY;
                }
                else
                {
                    diffY = targetPlayerY + myY;
                }

                if ((targetPlayer.position.x > 0 && this.transform.position.x > 0) || (targetPlayer.position.x < 0 && this.transform.position.x < 0))
                {
                    diffX = myX - targetPlayerX;
                }
                else
                {
                    diffX = myX + targetPlayerX;
                }

                targetAngle = Mathf.Abs(Mathf.Atan((diffX) / (diffY)) * Mathf.Rad2Deg) + 90f;
                aimRotation = targetAngle * -1;

            }
            // Case 3: Enemy down right from player
            else if (targetPlayer.position.y <= this.transform.position.y && targetPlayer.position.x >= this.transform.position.x)
            {
                if ((targetPlayer.position.y > 0 && this.transform.position.y > 0) || (targetPlayer.position.y < 0 && this.transform.position.y < 0))
                {
                    diffY = myY - targetPlayerY;
                }
                else
                {
                    diffY = myY + targetPlayerY;
                }

                if ((targetPlayer.position.x > 0 && this.transform.position.x > 0) || (targetPlayer.position.x < 0 && this.transform.position.x < 0))
                {
                    diffX = targetPlayerX - myX;
                }
                else
                {
                    diffX = targetPlayerX + myX;
                }

                targetAngle = Mathf.Abs(Mathf.Atan((diffX) / (diffY)) * Mathf.Rad2Deg) + 270f;
                aimRotation = 360 - targetAngle;

            }
            // Case 4: Enemy down right from player
            else 
            {
                if ((targetPlayer.position.y > 0 && this.transform.position.y > 0) || (targetPlayer.position.y < 0 && this.transform.position.y < 0))
                {
                    diffY = myY - targetPlayerY;
                }
                else
                {
                    diffY = myY + targetPlayerY;
                }

                if ((targetPlayer.position.x > 0 && this.transform.position.x > 0) || (targetPlayer.position.x < 0 && this.transform.position.x < 0))
                {
                    diffX = myX - targetPlayerX;
                }
                else
                {
                    diffX = myX + targetPlayerX;
                }

                targetAngle = Mathf.Abs(Mathf.Atan((diffY) / (diffX)) * Mathf.Rad2Deg) + 180f;
                aimRotation = targetAngle -90;

            }

            // Start the turning according to the target
            if(aimRotation > gameObject.transform.rotation.eulerAngles.z -180f)
            {
                RotateShip(1);
            } else if (aimRotation == gameObject.transform.rotation.eulerAngles.z - 180f)
            {
                RotateShip(0);
            }
            else
            {
                RotateShip(-1);
            }


            //Debug.Log("targetPlayer = " + targetPlayer.ToString() + " this.transform =  " + this.transform.ToString() + " targetAngle = " + targetAngle);
            //Debug.Log("this.transform.rotation.z = " + (gameObject.transform.rotation.eulerAngles.z -180f) + " targetAngle = " + targetAngle + " aimRotation = " + aimRotation);
            

        }



        return targetPlayer;
    }


    void executeAI()
    {
        if(AIEnabled == true)
        {
        

            //Debug.Log("Time.time " + Time.time);
            float AIExecTime = Random.Range(1.0f, 5.0f);
            
            if (Time.time < AIExecTime + AILastExecuted)
            {
                //float execTurn = Random.Range(0.0f, 1.0f);
                //Debug.Log(turnDirection + " " + turnTime + " " + turnLastExecuted);

                if (AILevel == AILevels.IDIOT)
                {
                    RotateShip(AITurnDirection);


                    MoveShip(Random.Range(-0.0f, 0.2f));

                    if (Random.Range(0.0f, 1.0f) > 0.9f)
                    {
                        UsePrimary();
                    }
                    if (Random.Range(0.0f, 1.0f) > 0.95f)
                    {
                        UseSecondary();
                    }

                } else if (AILevel == AILevels.ROOKIE)
                {
                    MoveShip(Random.Range(-0.0f, 0.2f));

                    if (Random.Range(0.0f, 1.0f) > 0.85f)
                    {
                        UsePrimary();
                    }
                    if (Random.Range(0.0f, 1.0f) > 0.98f)
                    {
                        UseSecondary();
                    }
                }
            }
            else {
                /*
                if (AILevel == AILevels.ROOKIE)
                {
                    AITurnDirection = 0f;
                   // Quaternion rotationAngle = Quaternion.LookRotation(targetPlayer.position - transform.position);
                   // Debug.Log(rotationAngle.eulerAngles.z);

                    //transform.Rotate(0, 0, rotationAngle.z*10);
                    //transform.rotation = Quaternion.Euler(rotationAngle.x, rotationAngle.y, 0);


                    transform.LookAt(targetPlayer.position);

                
                }
                */
                    //transform.localEulerAngles
                    /*
                    var lookDir = targetPlayer.position - transform.position;
                    lookDir.y = 0; // keep only the horizontal direction
                    //lookDir.x = 180; // keep only the horizontal direction
                    Debug.Log("lookDir = " + lookDir);

                    transform.rotation = Quaternion.LookRotation(lookDir);
                    */
    /*
    Quaternion rotationAngle = Quaternion.LookRotation(targetPlayer.position - transform.position, Vector3.up);
    //rotationAngle.SetEulerAngles()
    transform.rotation = Quaternion.Slerp(transform.rotation, rotationAngle, Time.deltaTime * 10); // we rotate the rotationAngle 
    */
    //transform.localEulerAngles = new Vector3(180, 0, transform.localEulerAngles.z);


    //Vector3 dirToFace = targetPlayer.position - transform.position;
    //transform.rotation = Quaternion.LookRotation(dirToFace);




    /*
    Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime * -1 * shipTurn * shipDetails.RotationRate);

    eulerAngleVelocity.y = 0;
    rb.MoveRotation(rb.rotation * deltaRotation);
    */
    /*
    Quaternion rotationAngle = Quaternion.LookRotation(targetPlayer.position - transform.position); // we get the angle has to be rotated

    Debug.Log("rotationAngle = " + rotationAngle.ToString());

    transform.rotation = Quaternion.Slerp(transform.rotation, rotationAngle, Time.deltaTime * 10); // we rotate the rotationAngle 
    RotateShip(0f);
    */
    /*
    Vector3 targetPoint = targetPlayer.position;
    targetPoint.x = 0.0f;
    targetPoint.y = 0.0f;
    transform.LookAt(targetPoint);
    */
    /*
    Quaternion newRotation = Quaternion.LookRotation(transform.position - targetPoint);
    //newRotation.x = 0.0;
    //newRotation.y = 0.0;
    //transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 800);
    Debug.Log("targetPoint = " + targetPoint.ToString());

    Quaternion deltaRotation = Quaternion.Euler(0, 0, Time.deltaTime * newRotation.z * shipDetails.RotationRate * 1000);


    //rb.MoveRotation(rb.rotation * deltaRotation);

    //targetRotation. = 0.0f;
    //AITurnDirection = targetRotation.y;
    //rb.MoveRotation(targetRotation);



    //targetRotation = 
    //transform.rotation.set(180.0f, 0.0f, transform.rotation.z);// = 180.0f;
    //transform.y = 180.0f;

    float step = 1000f * Time.deltaTime;
    //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetPlayer.rotation, step);
    transform.right = transform.position - targetPlayer.position;
    */

    /*
    if (targetPlayer.position.x >= transform.position.x && targetPlayer.position.y >= transform.position.y)
    {
        AITurnDirection = -0.5f;
        Debug.Log("Enemy top right!");
    } else if (targetPlayer.position.x <= transform.position.x && targetPlayer.position.y >= transform.position.y)
    {
        AITurnDirection = 0.5f;
        Debug.Log("Enemy bottom right!");
    }
    else if (targetPlayer.position.x >= transform.position.x && targetPlayer.position.y <= transform.position.y)
    {
        AITurnDirection = -0.5f;
        Debug.Log("Enemy top right!");
    }
    else if (targetPlayer.position.x <= transform.position.x && targetPlayer.position.y <= transform.position.y)
    {
        AITurnDirection = 0.5f;
        Debug.Log("Enemy top left!");
    }
    */


    AITurnDirection = Random.Range(-0.5f, 0.5f);
                
                
                //Debug.Log("AI changed his mind!");
                AILastExecuted = Time.time;

            }
            
        }
    }








}



