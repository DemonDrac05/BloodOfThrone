using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;

    public Vector2 minBorder;
    public Vector2 maxBorder;

    public static CameraMovement Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        float horizontalDis = Mathf.Clamp(target.position.x, minBorder.x, maxBorder.x);
        float verticalDis = Mathf.Clamp(target.position.y, minBorder.y, maxBorder.y);

        this.transform.position = new Vector3(horizontalDis, verticalDis, -10f);
    }
}