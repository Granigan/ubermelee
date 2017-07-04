using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float dampTime = 0.2f;                 // Approximate time for the camera to refocus.
    public float screenEdgeBuffer = 4f;           // Space between the top/bottom most target and the screen edge.
    public float minSize = 6.5f;                  // The smallest orthographic size the camera can be.
                                                    //[HideInInspector] public Transform[] m_Targets; // All the targets the camera needs to encompass.
    public GameObject[] targets; // All the targets the camera needs to encompass.

    public float maxZoomLevel = 16f;

    private Camera mainCamera;                        // Used for referencing the camera.
    private float zoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
    private Vector3 moveVelocity;                 // Reference velocity for the smooth damping of the position.
    private Vector3 desiredPosition;              // The position the camera is moving towards.

    private float cameraLockedDuration = 0f;
    public float cameraLockedAfterJump = 1.0f;
    public float viewpointJumpLeftBottom = 1.2f;
    public float viewpointJumpRightTop = -0.2f;

    private void Awake()
    {
        mainCamera = GetComponentInChildren<Camera>();
    }


    private void FixedUpdate()
    {

        targets = GameObject.FindGameObjectsWithTag("CameraObject");

        // Move the camera towards a desired position.
        Move();
        // Change the size of the camera based.
        Zoom();

            
        CheckPlayAreaBoundaries();

        



        cameraLockedDuration -= Time.fixedDeltaTime;
        if (cameraLockedDuration < 0f) cameraLockedDuration = 0f;
    }


    private void Move()
    {
        // Find the average position of the targets.
        FindAveragePosition();

        // Smoothly transition to that position.
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref moveVelocity, dampTime);
    }


    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        // Go through all the targets and add their positions together.
        for (int i = 0; i < targets.Length; i++)
        {
            // If the target isn't active, go on to the next one.
            if (!targets[i].gameObject.activeSelf)
                continue;

            // Add to the average and increment the number of targets in the average.
            averagePos += targets[i].transform.position;
            numTargets++;
        }

        // If there are targets divide the sum of the positions by the number of them to find the average.
        if (numTargets > 0)
            averagePos /= numTargets;


        // The desired position is the average position;
        desiredPosition = averagePos;
    }


    private void Zoom()
    {
        // Find the required size based on the desired position and smoothly transition to that size.
        float requiredSize = FindRequiredSize();


        if(requiredSize > maxZoomLevel )
        {
            requiredSize = maxZoomLevel;
        }

        mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, requiredSize, ref zoomSpeed, dampTime);
        //Debug.Log(mainCamera.orthographicSize);
    }


    private float FindRequiredSize()
    {
        // Find the position the camera rig is moving towards in its local space.
        Vector3 desiredLocalPos = transform.InverseTransformPoint(desiredPosition);

        // Start the camera's size calculation at zero.
        float size = 0f;

        // Go through all the targets...
        for (int i = 0; i < targets.Length; i++)
        {
            // ... and if they aren't active continue on to the next target.
            if (!targets[i].gameObject.activeSelf)
                continue;

            // Otherwise, find the position of the target in the camera's local space.
            Vector3 targetLocalPos = transform.InverseTransformPoint(targets[i].transform.position);

            // Find the position of the target from the desired position of the camera's local space.
            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            // Choose the largest out of the current size and the distance of the tank 'up' or 'down' from the camera.
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

            // Choose the largest out of the current size and the calculated size based on the tank being to the left or right of the camera.
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / mainCamera.aspect);
            //size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x));
        }

        // Add the edge buffer to the size.
        size += screenEdgeBuffer;

        // Make sure the camera's size isn't below the minimum.
        size = Mathf.Max(size, minSize);

        return size;
    }


    public void SetStartPositionAndSize()
    {
        // Find the desired position.
        FindAveragePosition();

        // Set the camera's position to the desired position without damping.
        transform.position = desiredPosition;

        // Find and set the required size of the camera.
        mainCamera.orthographicSize = FindRequiredSize();
    }

    public bool CheckPlayAreaBoundaries()
    {
        foreach (GameObject currObject in targets)
        {
                        
            Vector3 viewPos = mainCamera.WorldToViewportPoint(currObject.GetComponentInChildren<Transform>().position);

            //Debug.Log("viewPos = " + viewPos);
            //Debug.Log("currObject.name " + currObject.name);

            if (viewPos.x < 0.0f)
            {
                GameObject cloneship = currObject;
                cloneship.GetComponentInChildren<TrailRenderer>().Clear();
                //Debug.Log(currObject);
                Vector3 newLocation = new Vector3(viewpointJumpLeftBottom, viewPos.y, mainCamera.nearClipPlane);
                newLocation = mainCamera.ViewportToWorldPoint(newLocation);

                currObject.transform.position = new Vector3(newLocation.x, currObject.transform.position.y, currObject.transform.position.z);
                //Debug.Log("Old = " + viewPos.x + " New = " + newLocation.x);
                //SetStartPositionAndSize();
                //lockCameraZoom(cameraLockedAfterJump);
                return true;
                
            }
            else if (viewPos.x > 1.0f)
            {
                GameObject cloneship = currObject;
                cloneship.GetComponentInChildren<TrailRenderer>().Clear();
                Vector3 newLocation = new Vector3(viewpointJumpRightTop, viewPos.y, mainCamera.nearClipPlane);
                newLocation = mainCamera.ViewportToWorldPoint(newLocation);

                currObject.transform.position = new Vector3(newLocation.x, currObject.transform.position.y, currObject.transform.position.z);
                //Debug.Log("Old = " + viewPos.x + " New = " + newLocation.x);
                //SetStartPositionAndSize();
                //lockCameraZoom(cameraLockedAfterJump);
                return true;
                
            }


            if (viewPos.y < 0.0f)
            {

                GameObject cloneship = currObject;

                if (cloneship.GetComponentInChildren<TrailRenderer>())
                    cloneship.GetComponentInChildren<TrailRenderer>().Clear();

                Vector3 newLocation = new Vector3(viewPos.x, viewpointJumpLeftBottom, mainCamera.nearClipPlane);
                newLocation = mainCamera.ViewportToWorldPoint(newLocation);

                currObject.transform.position = new Vector3(currObject.transform.position.x, newLocation.y, currObject.transform.position.z);
                //Debug.Log("Old = " + viewPos.x + " New = " + newLocation.x);
                //SetStartPositionAndSize();                
                //lockCameraZoom(cameraLockedAfterJump);
                return true;
                
            } else if (viewPos.y > 1.0f)
            {
                GameObject cloneship = currObject;

                if(cloneship.GetComponentInChildren<TrailRenderer>())
                    cloneship.GetComponentInChildren<TrailRenderer>().Clear();

                Vector3 newLocation = new Vector3(viewPos.x, viewpointJumpRightTop, mainCamera.nearClipPlane);
                newLocation = mainCamera.ViewportToWorldPoint(newLocation);

                currObject.transform.position = new Vector3(currObject.transform.position.x, newLocation.y, currObject.transform.position.z);
                //Debug.Log("Old = " + viewPos.x + " New = " + newLocation.x);
                //SetStartPositionAndSize();
                //lockCameraZoom(cameraLockedAfterJump);
                return true;
                
            }


        }
        return false;
    }

    private void lockCameraZoom(float seconds)
    {
        cameraLockedDuration += seconds;
        cameraLockedDuration = Mathf.Min(cameraLockedDuration, 1.0f);
    }


   
}