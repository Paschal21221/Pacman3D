using UnityEngine;
using System.Collections;

public class PlayerGhostCollision : MonoBehaviour
{
    bool dying;
    bool invulnerable;

    void OnTriggerEnter(Collider other)
    {
        GhostAI ghost = other.GetComponent<GhostAI>();
        if (ghost == null) return;

        if (dying || invulnerable) return;

        if (ghost.mode == GhostMode.Frightened)
        {
            GameManager.Instance.AddScore(200);
            ghost.EatGhost();
        }
        else
        {
            StartCoroutine(DieRoutine());
        }
    }

    IEnumerator DieRoutine()
    {
        dying = true;
        invulnerable = true;

        GameManager.Instance.LoseLife();

        yield return new WaitForSeconds(0.75f);

        if (!GameManager.Instance.IsGameOver())
        {
            MazeBuilder maze = FindFirstObjectByType<MazeBuilder>();
            if (maze != null && maze.pacman != null)
                transform.position = maze.pacman.position;

            foreach (GhostAI g in FindObjectsByType<GhostAI>(FindObjectsSortMode.None))
                g.ResetGhost();
        }

        yield return new WaitForSeconds(1.0f);

        invulnerable = false;
        dying = false;
    }
}