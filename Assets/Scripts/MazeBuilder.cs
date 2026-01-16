using System.Collections.Generic;
using UnityEngine;

public class MazeBuilder : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject wallPrefab;
    public GameObject pelletPrefab;
    public GameObject powerPelletPrefab;
    public GameObject ghostPrefab;

    [Header("Scene References")]
    public Transform pacman;

    [Header("Grid")]
    public float tileSize = 1f;
    public float wallY = 0.5f;
    public float pelletY = 0.25f;
    public float actorY = 0.5f;

    [Header("Ghosts")]
    public int maxGhosts = 4;

    [TextArea(10, 80)]
    public string map;

    [HideInInspector] public string[] gridLines;
    [HideInInspector] public int width;
    [HideInInspector] public int height;
    [HideInInspector] public float originX;
    [HideInInspector] public float originZ;

    void Awake()
    {
        Build();
    }

    void Build()
    {
        gridLines = map.Replace("\r", "").Split('\n');
        height = gridLines.Length;
        width = 0;

        for (int i = 0; i < height; i++)
        {
            int len = gridLines[i].Length;
            if (len > width) width = len;
        }

        originX = -(width / 2f) * tileSize + tileSize * 0.5f;
        originZ = (height / 2f) * tileSize - tileSize * 0.5f;

        List<Vector3> ghostSpawns = new List<Vector3>();
        Vector3 pacPos = Vector3.zero;

        for (int r = 0; r < height; r++)
        {
            string line = gridLines[r];
            int rowWidth = line.Length;
            if (rowWidth == 0) continue;

            for (int c = 0; c < rowWidth; c++)
            {
                char ch = line[c];
                float x = originX + c * tileSize;
                float z = originZ - r * tileSize;

                Vector3 posWall = new Vector3(x, wallY, z);
                Vector3 posPellet = new Vector3(x, pelletY, z);
                Vector3 posActor = new Vector3(x, actorY, z);

                if (ch == '#')
                    Instantiate(wallPrefab, posWall, Quaternion.identity, transform);
                else if (ch == '.')
                    Instantiate(pelletPrefab, posPellet, Quaternion.identity, transform);
                else if (ch == 'o')
                    Instantiate(powerPelletPrefab, posPellet, Quaternion.identity, transform);
                else if (ch == 'P')
                    pacPos = posActor;
                else if (ch == 'G')
                    ghostSpawns.Add(posActor);
            }
        }

        if (pacman)
            pacman.position = pacPos;

        int count = 0;
        for (int i = 0; i < ghostSpawns.Count; i++)
        {
            if (ghostPrefab == null) break;
            if (maxGhosts > 0 && count >= maxGhosts) break;
            Instantiate(ghostPrefab, ghostSpawns[i], Quaternion.identity, transform);
            count++;
        }

        if (GameManager.Instance)
            GameManager.Instance.RecountPellets();
    }

    public Vector2Int WorldToCell(Vector3 world)
    {
        int c = Mathf.RoundToInt((world.x - originX) / tileSize);
        int r = Mathf.RoundToInt((originZ - world.z) / tileSize);
        return new Vector2Int(c, r);
    }

    public Vector3 CellToWorld(int c, int r, float y)
    {
        float x = originX + c * tileSize;
        float z = originZ - r * tileSize;
        return new Vector3(x, y, z);
    }

    public bool IsWalkableCell(int c, int r)
    {
        if (r < 0 || r >= height) return false;
        if (c < 0) return false;

        string line = gridLines[r];
        if (c >= line.Length) return false;

        char ch = line[c];
        return ch != '#';
    }
}
