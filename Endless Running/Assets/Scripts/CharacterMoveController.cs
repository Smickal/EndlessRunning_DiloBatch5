using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{
    [Header("Movement")]
    public float moveAccel;
    public float moveSpeed;

    Rigidbody2D playerRigid;

    
    [Header("Jump")]
    public float jumpAccel;

    private bool isJumping;
    private bool isOnTheGround;

    [Header("Ground RayCast")]
    public float groundRayCastDistance;
    public LayerMask groundLayerMask;
    private Animator playerAnim;

    private CharacterSoundController sound;


    [Header("Scoring")]
    public ScoreController score;
    public float scoringRatio;
    private float lastPositionX;

    [Header("GameOver")]
    public GameObject gameOverScreen;
    public float fallPositionY;

    [Header("Camera")]
    public CameraMoveController gameCamera;

    private void Start() 
    {
        playerRigid = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        sound = GetComponent<CharacterSoundController>();
    }

    private void FixedUpdate() 
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRayCastDistance, groundLayerMask);
        if(hit)
        {
            if(!isOnTheGround && playerRigid.velocity.y <= 0)
            {
                isOnTheGround = true;
            }
        }
        else
        {
            isOnTheGround = false;
        }

        Vector2 velocityVector = playerRigid.velocity;
        if(isJumping)
        {
            velocityVector.y += jumpAccel;
            isJumping = false;
        }
        velocityVector.x = Mathf.Clamp(velocityVector.x + moveAccel * Time.deltaTime, 0f, moveSpeed);
        playerRigid.velocity = velocityVector;

        
    }



    private void Update() 
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(isOnTheGround)
            {
                isJumping = true;
                sound.PlayJumpSound();
            }
        }

        playerAnim.SetBool("isOnGround", isOnTheGround);

        int dinstancePassed = Mathf.FloorToInt(transform.position.x - lastPositionX);
        int scoreIncrement = Mathf.FloorToInt(dinstancePassed/ scoringRatio);

        if(scoreIncrement > 0)
        {
            score.IncreaseCurrentScore(scoreIncrement);
            lastPositionX += dinstancePassed;
        }

        if(transform.position.y < fallPositionY)
        {
            GameOver();
        }

        
    }


    void GameOver()
    {
        score.FinishScoring();
        gameCamera.enabled = false;
        gameOverScreen.SetActive(true);

        this.enabled = false;
    }
    private void OnDrawGizmos() 
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundRayCastDistance), Color.white);
    }




}
