using UnityEngine;
using System.Collections.Generic;

public class GhostAI : MonoBehaviour
{
    public float tileSize = 1f;
    public float speed = 5f;
    public float checkDistance = 0.9f;
    public LayerMask wallLayer;
    public Transform pacman;

    Vector3 currentDir = Vector3.left;
    Vector3 chaseOffset;

    void Start()
    {
        if (pacman == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) pacman = p.transform;
        }

        chaseOffset = new Vector3(
            Random.Range(-3f, 3f),
            0f,
            Random.Range(-3f, 3f)
        );

        Vector3 p0 = transform.position;
        p0.x = Mathf.Round(p0.x / tileSize) * tileSize;
        p0.z = Mathf.Round(p0.z / tileSize) * tileSize;
        transform.position = p0;
    }

    void FixedUpdate()
    {
        ChooseDirection();

        Vector3 pos = transform.position;
        pos += currentDir * speed * Time.fixedDeltaTime;

        if (Mathf.Abs(currentDir.x) > 0.5f)
            pos.z = Mathf.Round(pos.z / tileSize) * tileSize;

        if (Mathf.Abs(currentDir.z) > 0.5f)
            pos.x = Mathf.Round(pos.x / tileSize) * tileSize;

        transform.position = pos;
    }

    void ChooseDirection()
    {
        Vector3[] dirs = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
        List<Vector3> options = new List<Vector3>();

        for (int i = 0; i < dirs.Length; i++)
        {
            if (CanMove(dirs[i]))
                options.Add(dirs[i]);
        }

        if (options.Count == 0)
        {
            currentDir = Vector3.zero;
            return;
        }

        List<Vector3> filtered = new List<Vector3>();
        for (int i = 0; i < options.Count; i++)
        {
            if (options[i] != -currentDir)
                filtered.Add(options[i]);
        }

        List<Vector3> pool = filtered.Count > 0 ? filtered : options;

        if (pacman == null)
        {
            int i = Random.Range(0, pool.Count);
            currentDir = pool[i];
            return;
        }

        Vector3 best = pool[0];
        float bestDist = DistanceInDir(best);

        for (int i = 1; i < pool.Count; i++)
        {
            float d = DistanceInDir(pool[i]);
            if (d < bestDist)
            {
                bestDist = d;
                best = pool[i];
            }
        }

        currentDir = best;
    }

    float DistanceInDir(Vector3 dir)
    {
        Vector3 next = transform.position + dir * tileSize;
        Vector3 target = pacman != null ? pacman.position + chaseOffset : transform.position;
        return (next - target).sqrMagnitude;
    }

    bool CanMove(Vector3 dir)
    {
        if (dir == Vector3.zero) return false;
        return !Physics.Raycast(transform.position, dir, checkDistance, wallLayer);
    }
}
