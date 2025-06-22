using UnityEngine;

public class ArmController : MonoBehaviour
{
    public Transform[] armSegments;
    public float rotationSpeed = 30f;
    public GameObject segmentPrefab;
    private Transform lastSegment;
    public float spinSpeed = 90f;
    public float minScaleY = 2.2f;
    public float maxScaleY = 10f;

    void Start()
    {
        if (armSegments.Length > 0)
        {
            lastSegment = armSegments[armSegments.Length - 1];
        }
    }

    void Update()
    {
        if (armSegments.Length > 0)
        {
            // Control 1st element with A or D
            if (Input.GetKey(KeyCode.A))
            {
                armSegments[0].Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                armSegments[0].Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            }
        }

        if (armSegments.Length > 1)
        {
            // Control 2nd element with W or S
            if (Input.GetKey(KeyCode.W))
            {
                armSegments[1].Rotate(Vector3.down, -rotationSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                armSegments[1].Rotate(Vector3.down, rotationSpeed * Time.deltaTime);
            }
        }

        // Spins the screwdriver
        if (armSegments.Length > 2)
        {
            armSegments[2].Rotate(Vector3.up, spinSpeed * Time.deltaTime);
        }

        // Instantiate new robotic arms
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (segmentPrefab != null && lastSegment != null)
            {
                // Spawn new segment as a child of the last one
                GameObject newSegmentObj = Instantiate(segmentPrefab, lastSegment.position, lastSegment.rotation);
                newSegmentObj.transform.SetParent(lastSegment);

                newSegmentObj.transform.localPosition = new Vector3(0, 1, 0);

                lastSegment = newSegmentObj.transform;
            }
        }

        // Changes color of the RoboticArm (all of the segments)
        if (Input.GetKeyDown(KeyCode.C))
        {
            ColorAllSegments();
        }

        // Make a specific segment bigger(F) or smaller(G)
        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.G))
        {
            string pathToArm = "Base/LowerShoulder/UprightArm/UpperShoulder/DownrightArm";
            Transform foundArm = transform.Find(pathToArm);

            if (foundArm != null)
            {
                // Make the segment a little bit BIGGER
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (foundArm.localScale.y < maxScaleY)
                    {
                        foundArm.localScale += new Vector3(0, 0.2f, 0);
                    }
                }

                // Make the segment a little bit SMALLER
                if (Input.GetKeyDown(KeyCode.G))
                {
                    if (foundArm.localScale.y > minScaleY)
                    {
                        foundArm.localScale -= new Vector3(0, 0.2f, 0); 
                    }
                }
            }
        }
    }

    void ColorAllSegments()
    {
        // Loops through every child of the object
        foreach (Transform segment in transform)
        {
            // Gets every renderer component in this and its children
            Renderer[] allRenderers = GetComponentsInChildren<Renderer>();

            // Loop through the array of renderers
            foreach (Renderer rend in allRenderers)
            {
                // Changes the color to a random bright color
                rend.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }
        }
    }
}