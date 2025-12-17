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
    public int ghostCount = 4;

    [TextArea(10, 80)]
    public string map;

    void Awake()
    {
        Build();
    }

    void Build()
    {
        var lines = map.Replace("\r", "").Split('\n');
        int height = lines.Length;
        int width = lines[0].Length;

        float x0 = -(width / 2f) * tileSize + tileSize * 0.5f;
        float z0 = (height / 2f) * tileSize - tileSize * 0.5f;

        List<Vector3> ghostSpawns = new List<Vector3>();
        Vector3 pacPos = Vector3.zero;

        for (int r = 0; r < height; r++)
        {
            for (int c = 0; c < width; c++)
            {
                char ch = lines[r][c];
                float x = x0 + c * tileSize;
                float z = z0 - r * tileSize;

                if (ch == '#')
                    Instantiate(wallPrefab, new Vector3(x, wallY, z), Quaternion.identity, transform);
                else if (ch == '.')
                    Instantiate(pelletPrefab, new Vector3(x, pelletY, z), Quaternion.identity, transform);
                else if (ch == 'o')
                    Instantiate(powerPelletPrefab, new Vector3(x, pelletY, z), Quaternion.identity, transform);
                else if (ch == 'P')
                    pacPos = new Vector3(x, actorY, z);
                else if (ch == 'G')
                    ghostSpawns.Add(new Vector3(x, actorY, z));
            }
        }

        if (pacman)
            pacman.position = pacPos;

        if (GameManager.Instance)
            GameManager.Instance.RecountPellets();
    }
}
