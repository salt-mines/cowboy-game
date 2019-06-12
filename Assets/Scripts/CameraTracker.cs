using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    public Transform whatToTrack;
    public float trackingTime = 0.2f;

    private Vector3 currentSmoothVel;

    void LateUpdate()
    {
        float origZ = transform.position.z;

        var newPos = Vector3.SmoothDamp(transform.position, whatToTrack.position, ref currentSmoothVel, trackingTime);
        newPos.z = origZ;

        transform.position = newPos;
    }
}
