using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Audio;

public class Controls : MonoBehaviour
{
    private Vector3 mousePosition;    
    public GameObject ball;
    private Transform ballTransform;
    private Collider2D ballCollider2D;
    [HideInInspector]
    public Rigidbody2D ballRb2d;
    [HideInInspector]
    public SpriteRenderer ballRenderer;
    [SerializeField]
    private GameObject pointerObj;
    private SpriteRenderer pointerSpriteRenderer;
    private Transform pointerTransform;
    [SerializeField]
    private Image strenghtBar;
    private Vector2 strikeVector;
    private int strikeStrength = 0;
    [SerializeField]
    private TMP_Text speedLabel;
    [SerializeField]
    private EdgeCollider2D boardCollider2D;
    [SerializeField]
    private LayerMask layerObstacles;
    [SerializeField]
    private GameObject hitEffect;
    private ParticleSystem particlesOfHit;
    private TrailRenderer ballTrail;
    [HideInInspector]
    public bool levelOver = false;
    private int howManyStrikes = 0;
    [SerializeField]
    private TMP_Text strikesText;

    private void Start()
    {
        ballTransform = ball.GetComponent<Transform>();
        ballRb2d = ball.GetComponent<Rigidbody2D>();
        ballRenderer = ball.GetComponent<SpriteRenderer>();
        ballCollider2D = ball.GetComponent<Collider2D>();

        particlesOfHit = hitEffect.GetComponent<ParticleSystem>();
        ballTrail = ball.GetComponent<TrailRenderer>();

        pointerSpriteRenderer = pointerObj.GetComponent<SpriteRenderer>();
        pointerTransform = pointerObj.GetComponent<Transform>();
    }

    bool hitEdge = false;
    bool onlyOnce = true;
    private void Update()
    {
        if (levelOver)
        {
            pointerSpriteRenderer.enabled = false;
            ballTrail.enabled = false;
        }
        else
        {
            ballTrail.enabled = true;
        }

        if(ballCollider2D.IsTouching(boardCollider2D) 
            || ballCollider2D.IsTouchingLayers(layerObstacles))
        {
            AudioManager.instance.StopSound("Obstacle Hit");
            AudioManager.instance.PlaySound("Obstacle Hit");

            hitEdge = true;          
        }
        else
        {
            hitEdge = false;
            onlyOnce = true;
        }

        if (hitEdge)
        {
            if (onlyOnce)
            {
                var hit = Instantiate(hitEffect);
                hit.transform.position = ballTransform.position;

                DestroyObject(hit, 3);

                onlyOnce = false;
            }
        }

        mousePosition = Input.mousePosition;
        mousePosition.z = 10.0f;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        strikeVector = (ballTransform.position - mousePosition).normalized;
        var radian = Mathf.Atan2(strikeVector.x, strikeVector.y);
        var degree = radian * 180f / Mathf.PI;
        pointerTransform.eulerAngles = new Vector3(0f, 0f, -degree + 90f);
    }

    private void FixedUpdate()
    {
        speedLabel.text = $"{ballRb2d.velocity.magnitude.ToString("0.00")}";

        bool ballStopped = ballRb2d.velocity.magnitude < 0.05f;

        if(ballStopped && !levelOver)
        {
            pointerSpriteRenderer.enabled = true;
        }
            

        if (Input.GetKey(KeyCode.Mouse0) && ballStopped && !levelOver)
        {
            strikeStrength++;
            strenghtBar.fillAmount = strikeStrength / 100f;
        }        
        else
        {
            if (strikeStrength != 0)
                strikeBall();
            strikeStrength = 0;
            strenghtBar.fillAmount = 0;
        }
        strenghtBar.color = Color.Lerp(Color.green, Color.red, strenghtBar.fillAmount);  
    }

    private void strikeBall()
    {
        strikesText.text = $"{++howManyStrikes}";
        var strength = strenghtBar.fillAmount;
        string sound = "none";

        if (strength < 1 / 3f)
            sound = "Small Hit";
        else if (strength > 1 / 3f && strength < 2 / 3f)
            sound = "Medium Hit";
        else if(strength > 2 / 3f && strength < 1f)
            sound = "Big Hit";
        else
            sound = "Max Hit";

        AudioManager.instance.PlaySound(sound);

        pointerSpriteRenderer.enabled = false;
        ballRb2d.AddForce(strikeVector * strenghtBar.fillAmount * 1000f * Time.deltaTime, ForceMode2D.Impulse);
    }
    public void ResetCounter()
    {
        howManyStrikes = 0;
        strikesText.text = $"{howManyStrikes}";
    }
}
