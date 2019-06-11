using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterController2D : MonoBehaviour
{
    private BoxCollider2D boxCollider;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private LayerMask oneWayLayer;

    [SerializeField]
    [Range(0f, 0.3f)]
    private float collisionInset = 0.02f;

    [SerializeField]
    private bool flipForDirection = true;

    public CollisionState collisionState;
    public bool Grounded => collisionState.bottom;

    private RayOrigins rayOrigins;

    private Vector2[] topRays;
    private Vector2[] bottomRays;

    private float previousDirection;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        CalcRayOrigins();
    }

    public void CalcRayOrigins()
    {
        var bounds = boxCollider.bounds;
        bounds.Expand(-collisionInset * 2f);

        var extents = bounds.extents;

        rayOrigins.topLeft = new Vector2(-extents.x, extents.y);
        rayOrigins.top = new Vector2(0, extents.y);
        rayOrigins.topRight = new Vector2(extents.x, extents.y);
        rayOrigins.bottomLeft = new Vector2(-extents.x, -extents.y);
        rayOrigins.bottom = new Vector2(0, -extents.y);
        rayOrigins.bottomRight = new Vector2(extents.x, -extents.y);

        topRays = new[] {rayOrigins.topLeft, rayOrigins.top, rayOrigins.topRight};
        bottomRays = new[] {rayOrigins.bottomLeft, rayOrigins.bottom, rayOrigins.bottomRight};
    }

    public void LowerToGround()
    {
        Move(new Vector2(0, float.NegativeInfinity));
    }

    public void Move(Vector3 deltaMovement)
    {
        collisionState.Reset();

        if (deltaMovement.x != 0f)
        {
            MoveHorizontal(ref deltaMovement);
        }

        if (deltaMovement.y != 0f)
        {
            MoveVertical(ref deltaMovement);
        }

        transform.Translate(deltaMovement, Space.World);
    }

    private void MoveHorizontal(ref Vector3 delta)
    {
        if (flipForDirection && !Utils.SameSign(delta.x, previousDirection))
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            previousDirection = delta.x;
        }
    }

    private void MoveVertical(ref Vector3 delta)
    {
        var goingUp = delta.y > 0;
        var direction = goingUp ? Vector2.up : -Vector2.up;

        var distance = Mathf.Abs(delta.y) + collisionInset;
        var center = (Vector2) transform.position + boxCollider.offset;
        center.x += delta.x;

        LayerMask layerMask = groundLayer;
        if (goingUp)
        {
            layerMask &= ~oneWayLayer;
        }

        var rayList = goingUp ? topRays : bottomRays;
        foreach (var corner in rayList)
        {
            var origin = center + corner;

            Debug.DrawRay(origin, direction * distance, Color.red);

            var hit = Physics2D.Raycast(origin, direction, distance, layerMask);
            if (!hit) continue;

            delta.y = goingUp ? hit.distance : -hit.distance;

            if (goingUp)
            {
                delta.y -= collisionInset;
                collisionState.top = true;
            }
            else
            {
                delta.y += collisionInset;
                collisionState.bottom = true;
            }

            if (hit.distance < collisionInset + 0.001f)
                break;
        }
    }

    public struct CollisionState
    {
        public bool top, bottom, left, right;

        public void Reset()
        {
            top = bottom = left = right = false;
        }
    }

    struct RayOrigins
    {
        public Vector2 topLeft, top, topRight, bottomLeft, bottom, bottomRight;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        CalcRayOrigins();
    }
#endif
}