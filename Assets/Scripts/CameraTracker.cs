using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraTracker : MonoBehaviour
{
    public Transform whatToTrack;

    public float leftEdgeDistance = 0.3f;
    public float rightEdgeDistance = 0.3f;

    private new Camera camera;

    private void Start()
    {
        camera = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (!whatToTrack) return;
        Vector3 newPos = transform.position;

        var posOnCamera = camera.WorldToViewportPoint(whatToTrack.position);

        float leftLimit = leftEdgeDistance;
        float rightLimit = 1.0f - rightEdgeDistance;
        float diff;

        if (posOnCamera.x < leftLimit)
        {
            diff = leftLimit - posOnCamera.x;
        }
        else if (posOnCamera.x > rightLimit)
        {
            diff = rightLimit - posOnCamera.x;
        }
        else
        {
            diff = 0;
        }

        posOnCamera.x = 0.5f - diff;
        newPos.x = camera.ViewportToWorldPoint(posOnCamera).x;
        transform.position = newPos;
    }
}
