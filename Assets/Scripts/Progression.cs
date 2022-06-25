using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class Progression : MonoBehaviour
{
    [SerializeField]
    private CircleCollider2D hole;
    private CircleCollider2D ball;
    
    private bool levelCompleted = false;
    private float ballScale = 0.5f;

    public Controls controlsComposite;
    public Levels levelsComposite;

    private int currentLevel;

    private void Start()
    {
        currentLevel = levelsComposite.GetStartLevel;
        ball = controlsComposite.ball.GetComponent<CircleCollider2D>();
    }

    private bool onlyOnce = true;
    private void FixedUpdate()
    {
        if (ball.IsTouching(hole) 
            && controlsComposite.ballRb2d.velocity.magnitude < 3f)
        {
            levelCompleted = true;
        }

        if (levelCompleted)
        {
            if (onlyOnce)
            {
                controlsComposite.ballRb2d.velocity = Vector2.zero;
                AudioManager.instance.PlaySound("Ball in");
                onlyOnce = false;
                controlsComposite.levelOver = true;
            }

            var dirToHole = hole.gameObject.transform.position - ball.gameObject.transform.position;
            controlsComposite.ballRb2d.AddForce(
                dirToHole * Vector2.Distance(
                    ball.transform.position, hole.transform.position) * 50f, ForceMode2D.Force);

            ballScale = Mathf.Clamp(ballScale - 0.005f, min: 0f, max: 0.5f);
            controlsComposite.ball.transform.localScale = new Vector3(ballScale, ballScale);

            if(ballScale == 0f)
            {                
                levelsComposite.EndCurrentLevel();
                controlsComposite.ResetCounter();
                levelCompleted = false;
                controlsComposite.levelOver = false;
                onlyOnce = true;
                ballScale = 0.5f;                
                try
                {
                    levelsComposite.StartLevel(++currentLevel);
                }
                catch (ArgumentOutOfRangeException)
                {
                    currentLevel = 1;
                    levelsComposite.StartLevel(currentLevel);
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }
            }
        }        
    }
}