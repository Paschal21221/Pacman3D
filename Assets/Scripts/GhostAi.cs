using UnityEngine;

public class GhostAI : MonoBehaviour
{
    public float speed = 5f;
    public MazeBuilder maze;
    public Transform pacman;
    public int personality = 0;

    Vector3 currentDir = Vector3.left;

    void Start()
    {
        if (maze == null)
            maze = FindObjectOfType<MazeBuilder>();

        if (pacman == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) pacman = p.transform;
        }

        if (maze != null)
        {
            Vector2Int cell = maze.WorldToCell(transform.position);
            Vector3 center = maze.CellToWorld(cell.x, cell.y, transform.position.y);
            transform.position = center;
        }
    }

    void FixedUpdate()
    {
        if (maze == null) return;

        if (pacman != null)
            UpdateDirection();

        Move();
    }

    void UpdateDirection()
    {
        Vector2Int ghostCell = maze.WorldToCell(transform.position);
        Vector2Int pacCell = maze.WorldToCell(pacman.position);

        Vector2Int targetCell = pacCell;

        if (personality == 1)
            targetCell += new Vector2Int(2, 0);
        else if (personality == 2)
            targetCell += new Vector2Int(0, 2);
        else if (personality == 3)
            targetCell += new Vector2Int(-2, 0);

        int dx = targetCell.x - ghostCell.x;
        int dz = targetCell.y - ghostCell.y;

        Vector3[] dirs = new Vector3[4];
        int index = 0;

        if (Mathf.Abs(dx) >= Mathf.Abs(dz))
        {
            if (dx > 0) dirs[index++] = Vector3.right;
            else if (dx < 0) dirs[index++] = Vector3.left;

            if (dz > 0) dirs[index++] = Vector3.back;
            else if (dz < 0) dirs[index++] = Vector3.forward;
        }
        else
        {
            if (dz > 0) dirs[index++] = Vector3.back;
            else if (dz < 0) dirs[index++] = Vector3.forward;

            if (dx > 0) dirs[index++] = Vector3.right;
            else if (dx < 0) dirs[index++] = Vector3.left;
        }

        dirs[index++] = Vector3.right;
        dirs[index++] = Vector3.left;

        for (int i = 0; i < dirs.Length; i++)
        {
            Vector3 d = dirs[i];
            if (d == Vector3.zero) continue;

            Vector2Int nextCell = NextCell(ghostCell, d);
            if (maze.IsWalkableCell(nextCell.x, nextCell.y))
            {
                currentDir = d;
                return;
            }
        }
    }

    void Move()
    {
        Vector3 pos = transform.position;
        pos += currentDir * speed * Time.fixedDeltaTime;

        Vector2Int cell = maze.WorldToCell(pos);
        Vector3 center = maze.CellToWorld(cell.x, cell.y, pos.y);

        if (Mathf.Abs(currentDir.x) > 0.5f)
            pos.z = center.z;
        else if (Mathf.Abs(currentDir.z) > 0.5f)
            pos.x = center.x;

        transform.position = pos;
    }

    Vector2Int NextCell(Vector2Int cell, Vector3 dir)
    {
        int c = cell.x;
        int r = cell.y;

        if (dir == Vector3.right) c += 1;
        else if (dir == Vector3.left) c -= 1;
        else if (dir == Vector3.forward) r -= 1;
        else if (dir == Vector3.back) r += 1;

        return new Vector2Int(c, r);
    }
}
