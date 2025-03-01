using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxFactor = 0.5f;
    public float minX; 
    public float maxX; 

    private Vector3 _previousCameraPosition;

    void Start()
    {
        _previousCameraPosition = cameraTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 cameraPosition = cameraTransform.position;

        Vector3 delta = cameraPosition - _previousCameraPosition;
        float parallaxX = delta.x * parallaxFactor;

        float newX = Mathf.Clamp(transform.position.x + parallaxX, minX, maxX);

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        _previousCameraPosition = cameraPosition;
    }
}