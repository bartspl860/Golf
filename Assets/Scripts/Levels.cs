using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Levels : MonoBehaviour
{
    private void Start()
    {
        if (levels.Count == 0)
            return;

        ballTrail = ball.GetComponent<TrailRenderer>();
        ballTrail.Clear();
        ballRb2d = ball.GetComponent<Rigidbody2D>();

        StartLevel(1);
    }

    [SerializeField]
    List<Level> levels;

    [SerializeField]
    public GameObject ball;
    private TrailRenderer ballTrail;
    private Rigidbody2D ballRb2d;
    [SerializeField]
    public GameObject hole;
    [SerializeField]
    public GameObject wallPrefab;

    public void StartLevel(int i)
    {
        
        var level = levels.Where(w => w.levelNum == i).FirstOrDefault();

        if (level == null)
            throw new ArgumentOutOfRangeException();

        foreach (var wall in level.walls)
        {
            var tmpWall = Instantiate(wall);
        }
        
        ball.transform.position = level.ballStartpoint;
        hole.transform.position = level.holePosition;
        ballTrail.Clear();
    }
    public void EndCurrentLevel()
    {
        var walls = GameObject.FindGameObjectsWithTag("Wall");

        foreach (var wall in walls)
            DestroyObject(wall);

        ball.transform.localScale = new Vector2(0.5f, 0.5f);
        ball.transform.position = new Vector2(0f, 0f);
        ballTrail.Clear();
        ballRb2d.velocity = Vector2.zero;
    }
}
[Serializable]
public class Level
{
    [SerializeField]
    public string name;
    [SerializeField]
    public int levelNum;
    [SerializeField]
    public List<GameObject> walls;
    [SerializeField]
    public Vector2 ballStartpoint;
    [SerializeField]
    public Vector2 holePosition;
}