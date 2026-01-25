using UnityEngine;
using UnityEngine.InputSystem;

public class PacmanMover : MonoBehaviour
{
    public float speed = 6f;
    public float checkDistance = 0.6f;
    public LayerMask wallLayer;

    Vector3 currentDir = Vector3.zero;
    Vector3 desiredDir = Vector3.zero;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame)
            desiredDir = Vector3.forward;
        if (Keyboard.current.sKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame)
            desiredDir = Vector3.back;
        if (Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame)
            desiredDir = Vector3.left;
        if (Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame)
            desiredDir = Vector3.right;
    }

    void FixedUpdate()
    {
        if (desiredDir != Vector3.zero && CanMove(desiredDir))
            currentDir = desiredDir;

        if (!CanMove(currentDir))
            currentDir = Vector3.zero;

        rb.MovePosition(rb.position + currentDir * speed * Time.fixedDeltaTime);
    }

    bool CanMove(Vector3 dir)
    {
        if (dir == Vector3.zero) return false;
        return !Physics.Raycast(transform.position, dir, checkDistance, wallLayer);
    }
}