using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField]
    protected Transform target;

    // Move in the y axis by 3 units
    float initY;
    [SerializeField] float xOffset = 0;
    [SerializeField] float yOffset = 2;
    [SerializeField] float xMin = -7.5f;
    [SerializeField] float xMax = 26.5f;
    [SerializeField] float yMin = 0;
    [SerializeField] float yMax = 10;
    [SerializeField] float cameraSpeed = 1f;
    [SerializeField] float cameraDistance = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
//        initY = target.position.y + yOffset;
        initY = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float y;
        float x = Mathf.Clamp(target.position.x + xOffset, xMin, xMax);

        if (target.position.y > 4)
        {
            y = Mathf.Clamp(initY, target.position.y - 4, yMax);
        }
        else
        {
            y = Mathf.Clamp(initY, yMin, yMax);
        }

        // Camera position
        transform.position = new Vector3(x, y, transform.position.z);
        // Update xOffset gradually to reach the camera distance
        xOffset = Mathf.Lerp(xOffset, cameraDistance, Time.deltaTime * cameraSpeed);
    }
}
