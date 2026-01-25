using UnityEngine;

public class GhostAI : MonoBehaviour
{
    public float speed = 5f;
    public MazeBuilder maze;
    public Transform pacman;
    public int personality = 0;

    public GhostMode mode = GhostMode.Normal;

    Vector3 currentDir = Vector3.left;
    Vector3 spawnPos;

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
            spawnPos = transform.position;
        }
    }

    void FixedUpdate()
    {
        if (maze == null) return;

        if (AtCellCenter())
        {
            if (mode == GhostMode.Frightened && pacman != null)
                UpdateFleeDirection();
            else if (pacman != null)
                UpdateChaseDirection();
        }

        Move();
    }

    bool AtCellCenter()
    {
        Vector2Int cell = maze.WorldToCell(transform.position);
        Vector3 center = maze.CellToWorld(cell.x, cell.y, transform.position.y);
        return Vector3.Distance(transform.position, center) < 0.05f;
    }

    void UpdateChaseDirection()
    {
        Vector2Int ghostCell = maze.WorldToCell(transform.position);
        Vector2Int pacCell = maze.WorldToCell(pacman.position);

        Vector2Int target = pacCell;

        if (personality == 1)
            target += new Vector2Int(2, 0);
        else if (personality == 2)
            target += new Vector2Int(0, 2);
        else if (personality == 3)
            target += new Vector2Int(-2, 0);

        ChooseBestDirection(ghostCell, target);
    }

    void UpdateFleeDirection()
    {
        Vector2Int ghostCell = maze.WorldToCell(transform.position);
        Vector2Int pacCell = maze.WorldToCell(pacman.position);
        Vector2Int target = ghostCell + (ghostCell - pacCell);

        ChooseBestDirection(ghostCell, target);
    }

    void ChooseBestDirection(Vector2Int ghostCell, Vector2Int targetCell)
    {
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

        foreach (Vector3 d in dirs)
        {
            if (d == Vector3.zero) continue;

            Vector2Int next = NextCell(ghostCell, d);
            if (maze.IsWalkableCell(next.x, next.y))
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
        if (dir == Vector3.right) cell.x++;
        else if (dir == Vector3.left) cell.x--;
        else if (dir == Vector3.forward) cell.y--;
        else if (dir == Vector3.back) cell.y++;
        return cell;
    }

    public void ResetGhost()
    {
        transform.position = spawnPos;
        mode = GhostMode.Normal;
    }

    public void EatGhost()
    {
        transform.position = spawnPos;
        mode = GhostMode.Normal;
    }
}