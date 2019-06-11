using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float Horizontal { get; private set; }
    public bool Jumping { get; private set; }

    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Jumping = Input.GetAxisRaw("Jump") > 0;
    }
}
