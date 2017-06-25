using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_DampTime = 0.2f;                 // Approximate time for the camera to refocus.
    public float m_ScreenEdgeBuffer = 4f;           // Space between the top/bottom most target and the screen edge.
    public float m_MinSize = 6.5f;                  // The smallest orthographic size the camera can be.
                                                    //[HideInInspector] public Transform[] m_Targets; // All the targets the camera needs to encompass.
    public GameObject[] m_Targets; // All the targets the camera needs to encompass.

    public float maxDistanceX = 16f;
    public float maxDistanceY = 9f;
    public float moveOffset = 5f;
    public float maxZoomLevel = 16f;

    private Camera mainCamera;                        // Used for referencing the camera.
    private float zoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
    private Vector3 moveVelocity;                 // Reference velocity for the smooth damping of the position.
    private Vector3 desiredPosition;              // The position the camera is moving towards.



    private void Awake()
    {
        mainCamera = GetComponentInChildren<Camera>();
    }


    private void FixedUpdate()
    {
        m_Targets = GameObject.FindGameObjectsWithTag("CameraObject");

        // Move the camera towards a desired position.
        Move();

        // Change the size of the camera based.
        Zoom();

        CheckPlayAreaBoundaries();
    }


    private void Move()
    {
        // Find the average position of the targets.
        FindAveragePosition();

        // Smoothly transition to that position.
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref moveVelocity, m_DampTime);
    }


    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        // Go through all the targets and add their positions together.
        for (int i = 0; i < m_Targets.Length; i++)
        {
            // If the target isn't active, go on to the next one.
            if (!m_Targets[i].gameObject.activeSelf)
                continue;

            // Add to the average and increment the number of targets in the average.
            averagePos += m_Targets[i].transform.position;
            numTargets++;
        }

        // If there are targets divide the sum of the positions by the number of them to find the average.
        if (numTargets > 0)
            averagePos /= numTargets;

        // Keep the same y value.
        //averagePos.y = transform.position.y;

        // The desired position is the average position;
        desiredPosition = averagePos;
    }


    private void Zoom()
    {
        // Find the required size based on the desired position and smoothly transition to that size.
        float requiredSize = FindRequiredSize();

        //if (requiredSize > 5) {
        requiredSize = requiredSize * 1.5f;
        //}

        if(requiredSize > maxZoomLevel )
        {
            requiredSize = maxZoomLevel;
        }

        mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, requiredSize, ref zoomSpeed, m_DampTime);
        //Debug.Log(mainCamera.orthographicSize);
    }


    private float FindRequiredSize()
    {
        // Find the position the camera rig is moving towards in its local space.
        Vector3 desiredLocalPos = transform.InverseTransformPoint(desiredPosition);

        // Start the camera's size calculation at zero.
        float size = 0f;

        // Go through all the targets...
        for (int i = 0; i < m_Targets.Length; i++)
        {
            // ... and if they aren't active continue on to the next target.
            if (!m_Targets[i].gameObject.activeSelf)
                continue;

            // Otherwise, find the position of the target in the camera's local space.
            Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].transform.position);

            // Find the position of the target from the desired position of the camera's local space.
            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            // Choose the largest out of the current size and the distance of the tank 'up' or 'down' from the camera.
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

            // Choose the largest out of the current size and the calculated size based on the tank being to the left or right of the camera.
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / mainCamera.aspect);
            //size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x));
        }

        // Add the edge buffer to the size.
        size += m_ScreenEdgeBuffer;

        // Make sure the camera's size isn't below the minimum.
        size = Mathf.Max(size, m_MinSize);

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
        foreach (GameObject currObject in m_Targets)
        {
            foreach (GameObject otherObject in m_Targets)
            {
                if(otherObject == currObject)
                {
                    continue;
                }

                Vector3 pos1 = currObject.transform.position;
                Vector3 pos2 = otherObject.transform.position;
                pos1.y = 0;
                pos2.y = 0;

                float distanceX = Vector3.Distance(pos1, pos2);

                if (distanceX > maxDistanceX)
                {
                    float newPositionX; // = currObject.transform.position.x + (maxDistanceX * 1) - moveOffset;

                    if (otherObject.transform.position.x > currObject.transform.position.x)
                    {
                        newPositionX = currObject.transform.position.x + (maxDistanceX * 2) - moveOffset;
                    }
                    else
                    {
                        newPositionX = currObject.transform.position.x + ((maxDistanceX * 2 - moveOffset) * -1);
                    }
                    //Debug.Log("Before X: "+ currObject.transform.position);
                    currObject.transform.position = new Vector3(newPositionX, currObject.transform.position.y, currObject.transform.position.z);
                    //Debug.Log("After X: " + currObject.transform.position);
                    return true;
                }





                Vector3 pos1y = currObject.transform.position;
                Vector3 pos2y = otherObject.transform.position;
                pos1y.x = 0;
                pos2y.x = 0;

                float distanceY = Vector3.Distance(pos1y, pos2y);

                if (distanceY > maxDistanceY)
                {
                    float newPositionY; 

                    if (otherObject.transform.position.y > currObject.transform.position.y)
                    {
                        newPositionY = currObject.transform.position.y + (maxDistanceY * 2) - moveOffset;
                    }
                    else
                    {
                        newPositionY = currObject.transform.position.y + ((maxDistanceY * 2 - moveOffset) * -1);
                    }
                    //Debug.Log("Before Y: " + currObject.transform.position);
                    currObject.transform.position = new Vector3(currObject.transform.position.x, newPositionY, currObject.transform.position.z);
                    //Debug.Log("After Y: " + currObject.transform.position);
                    return true;
                }



            }

            Vector3 viewPos = mainCamera.WorldToViewportPoint(currObject.GetComponentInChildren<Transform>().position);

            //Debug.Log("viewPos = " + viewPos);
            
            if (viewPos.x < 0.0f)
            {
                float newPositionX;
                newPositionX = currObject.transform.position.x + ((maxDistanceX * 1f - moveOffset) * 1);
                currObject.transform.position = new Vector3(newPositionX, currObject.transform.position.y, currObject.transform.position.z);
                if (currObject.name == "ship31") Debug.Log("viewPosX <= 0.0f = " + viewPos);
                SetStartPositionAndSize();
                return true;
            }
            else if (viewPos.x > 1.0f)
            {
                float newPositionX;
                newPositionX = currObject.transform.position.x + ((maxDistanceX * 1f - moveOffset) * -1);
                currObject.transform.position = new Vector3(newPositionX, currObject.transform.position.y, currObject.transform.position.z);
                if (currObject.name == "ship31") Debug.Log("viewPosX >= 1.0f = " + viewPos);
                SetStartPositionAndSize();
                return true;
            }


            if (viewPos.y < 0.0f)
            {
                float newPositionY;
                newPositionY = currObject.transform.position.y + ((maxDistanceY * 1f - moveOffset) * 1);
                currObject.transform.position = new Vector3(currObject.transform.position.x, newPositionY, currObject.transform.position.z);
                if(currObject.name == "ship31") Debug.Log("viewPosY <= 0.0f = " + viewPos);
                SetStartPositionAndSize();
                return true;
            } else if (viewPos.y > 1.0f)
            {
                float newPositionY;
                newPositionY = currObject.transform.position.y + ((maxDistanceY * 1f - moveOffset) * -1);
                currObject.transform.position = new Vector3(currObject.transform.position.x, newPositionY, currObject.transform.position.z);
                if (currObject.name == "ship31") Debug.Log("viewPosY >= 1.0f = " + viewPos);
                SetStartPositionAndSize();
                return true;
            }


        }
        return false;
    }


   
}