using UnityEngine;
using UnityEngine.InputSystem;

public class PacmanMover : MonoBehaviour
{
    public float tileSize = 1f;
    public float speed = 6f;
    public float wallCheckRadius = 0.25f;

    Vector3 currentDir = Vector3.zero;
    Vector3 desiredDir = Vector3.zero;
    Vector3 targetPos;

    void Start()
    {
        transform.position = Snap(transform.position);
        targetPos = transform.position;
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
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            speed * Time.fixedDeltaTime
        );

        if (Vector3.Distance(transform.position, targetPos) < 0.001f)
        {
            transform.position = targetPos;
            ChooseNext();
        }
    }

    void ChooseNext()
    {
        if (CanMove(desiredDir))
            currentDir = desiredDir;
        else if (!CanMove(currentDir))
            currentDir = Vector3.zero;

        targetPos = transform.position + currentDir * tileSize;
        targetPos = Snap(targetPos);
    }

    bool CanMove(Vector3 dir)
    {
        if (dir == Vector3.zero) return false;

        Vector3 checkPos = Snap(transform.position) + dir * tileSize;
        Collider[] hits = Physics.OverlapSphere(checkPos, wallCheckRadius);

        foreach (var h in hits)
            if (h.CompareTag("Wall"))
                return false;

        return true;
    }

    Vector3 Snap(Vector3 p)
    {
        float x = Mathf.Round(p.x / tileSize) * tileSize;
        float z = Mathf.Round(p.z / tileSize) * tileSize;
        return new Vector3(x, p.y, z);
    }
}