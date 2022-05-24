using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Levels : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField]
    private int debuggingLevelStart = 0;
    public int GetStartLevel { get => debuggingLevelStart; }

    private void Start()
    {
        if (levels.Count == 0)
            return;

        ballTrail = ball.GetComponent<TrailRenderer>();
        ballTrail.Clear();
        ballRb2d = ball.GetComponent<Rigidbody2D>();

        boardSpriteRenderer = board.GetComponent<SpriteRenderer>();
        boardEdgeCollider = board.GetComponent<EdgeCollider2D>();

        StartLevel(debuggingLevelStart);
    }

    [Header("Data")]

    [SerializeField]
    List<Level> levels;

    [SerializeField]
    public GameObject ball;
    private TrailRenderer ballTrail;
    private Rigidbody2D ballRb2d;

    [SerializeField]
    private GameObject hole;

    [SerializeField]
    private GameObject board;
    private SpriteRenderer boardSpriteRenderer;
    private EdgeCollider2D boardEdgeCollider;

    [SerializeField]
    public CameraBehaviour cameraComposite;

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

        boardSpriteRenderer.size = level.boardSize;
        board.transform.position = level.boardPosition;
        
        var size = level.boardSize;

        Vector2[] boardColliderPoints = new Vector2[5]
        {
            new Vector2(-size.x/2f, size.y/2f),
            new Vector2(size.x/2f, size.y/2f),
            new Vector2(size.x/2f, -size.y/2f),
            new Vector2(-size.x/2f, -size.y/2f),
            new Vector2(-size.x/2f, size.y/2f)
        };

        boardEdgeCollider.points = boardColliderPoints;

        var horizontalCamera = 
            new Vector2(level.boardPosition.x - size.x / 2f, level.boardPosition.x + size.x / 2f);
        var verticalCamera = 
            new Vector2(level.boardPosition.y - size.y / 2f, level.boardPosition.y + size.y / 2f);
       
        cameraComposite.cameraMovementArea = (horizontalCamera, verticalCamera);
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
    [HideInInspector]
    public static int _instanceCounter;
    [HideInInspector]
    public int levelNum;
    [SerializeField]
    public string name;
    [SerializeField]
    public List<GameObject> walls;
    [SerializeField]
    public Vector2 ballStartpoint;
    [SerializeField]
    public Vector2 holePosition;
    [SerializeField]
    public Vector2 boardSize;
    [SerializeField]
    public Vector2 boardPosition;
    static Level()
    {
        _instanceCounter = 0;
    }
    public Level()
    {
        levelNum = ++_instanceCounter;
    }
}