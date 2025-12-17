using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PacmanGridMover : MonoBehaviour
{
    [Header("Grid")]
    public float tileSize = 1f;

    [Header("Walls")]
    public LayerMask wallMask;
    public float blockCheckRadius = 0.23f;

    [Header("Speed")]
    public float tilesPerSecond = 8f;

    Vector3 currentDir = Vector3.zero;
    Vector3 desiredDir = Vector3.zero;

    void Awake()
    {
        transform.position = SnapToGrid(transform.position);
    }

    void Start()
    {
        StartCoroutine(MoveLoop());
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // Buffer direction (last input wins)
        if (Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame)
            desiredDir = Vector3.forward;
        if (Keyboard.current.downArrowKey.wasPressedThisFrame || Keyboard.current.sKey.wasPressedThisFrame)
            desiredDir = Vector3.back;
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame || Keyboard.current.aKey.wasPressedThisFrame)
            desiredDir = Vector3.left;
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame || Keyboard.current.dKey.wasPressedThisFrame)
            desiredDir = Vector3.right;
    }

    IEnumerator MoveLoop()
    {
        while (true)
        {
            transform.position = SnapToGrid(transform.position);

            // turn ASAP if possible
            if (desiredDir != Vector3.zero && CanMove(desiredDir))
                currentDir = desiredDir;

            // stop if forward blocked
            if (currentDir != Vector3.zero && !CanMove(currentDir))
                currentDir = Vector3.zero;

            // if stopped, wait for a valid direction
            if (currentDir == Vector3.zero)
            {
                yield return null;
                continue;
            }

            Vector3 start = transform.position;
            Vector3 target = start + currentDir * tileSize;

            float duration = 1f / Mathf.Max(0.01f, tilesPerSecond);
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                transform.position = Vector3.Lerp(start, target, Mathf.Clamp01(t));
                yield return null;
            }

            transform.position = target; // exact center
        }
    }

    bool CanMove(Vector3 dir)
    {
        Vector3 from = SnapToGrid(transform.position);
        Vector3 checkPos = from + dir * tileSize;

        // Find what we're colliding with at the next tile
        Collider[] hits = Physics.OverlapSphere(
            checkPos,
            blockCheckRadius,
            wallMask,
            QueryTriggerInteraction.Ignore
        );

        if (hits.Length > 0)
        {
            Debug.Log("BLOCKED by:");
            foreach (var h in hits)
                Debug.Log($" - {h.name} (layer {LayerMask.LayerToName(h.gameObject.layer)})");
            return false;
        }

        return true;
    }


        if (hits.Length > 0)
        {
            Debug.Log("BLOCKED by:");
            foreach (var h in hits)
                Debug.Log($" - {h.name} (layer {LayerMask.LayerToName(h.gameObject.layer)})");
            return false;
        }

        return true;
    }


    Vector3 SnapToGrid(Vector3 p)
    {
        float x = Mathf.Round(p.x / tileSize) * tileSize;
        float z = Mathf.Round(p.z / tileSize) * tileSize;
        return new Vector3(x, p.y, z);
    }
}
