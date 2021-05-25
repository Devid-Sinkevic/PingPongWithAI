using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour{

    public float moveSpeed = 12.0f;
    public Vector2 ballDirection = Vector2.right;

    public float topBorder = 9.6f,
                 bottomBorder = -9.6f;

    public int speedIncreaseInterval = 20;
    private float speedIncreaseTimer = 0f;
    public float speedIncreaseBy = 1.0f;

    
    private float playerHeight,
                  playerWidth,
                  AIHeight,
                  AIWidth,
                  playerMaxX,
                  playerMaxY,
                  playerMinX,
                  playerMinY,
                  AIMaxX,
                  AIMaxY,
                  AIMinX,
                  AIMinY,
                  ballWidth,
                  ballHeight;
    
    
    private GameObject player, AI;

    private float bounceAngle;
    private float vx, vy;
    private float maxAngl = 45.0f;


    private bool collideWithPlayer, collidedWithAI, collidedWithWall;


    private Game game;


    private bool assignedpoint;

    // Start is called before the first frame update
    void Start(){
        game = GameObject.Find("Game").GetComponent<Game>();
        moveSpeed = 12.0f;
        topBorder = 10.6f;
        bottomBorder = -10.6f;
        if(moveSpeed > 0){
            moveSpeed = -1 * moveSpeed;
        }

        player = GameObject.Find("Player");
        AI = GameObject.Find("AI");

        playerHeight = player.transform.GetComponent<SpriteRenderer> ().bounds.size.y;
        playerWidth = player.transform.GetComponent<SpriteRenderer> ().bounds.size.x;

        AIHeight = AI.transform.GetComponent<SpriteRenderer> ().bounds.size.y;
        AIWidth = AI.transform.GetComponent<SpriteRenderer> ().bounds.size.x;

        ballHeight = transform.GetComponent<SpriteRenderer> ().bounds.size.y;
        ballWidth = transform.GetComponent<SpriteRenderer> ().bounds.size.x;

        playerMaxX = player.transform.localPosition.x + playerWidth / 2;
        playerMinX = player.transform.localPosition.x - playerWidth / 2;

        AIMaxX = AI.transform.localPosition.x - AIWidth / 2;
        AIMinX = AI.transform.localPosition.x + AIWidth / 2;

        bounceAngle = getRandomBounceAngle();

        vx = moveSpeed * Mathf.Cos(bounceAngle);
        vy = moveSpeed * -Mathf.Sin(bounceAngle);

    }

    // Update is called once per frame
    void Update(){
        if(game.gameState != Game.GameState.paused){
            move();
            updateSpeed();
        }
        
    }

    void updateSpeed(){
        if(speedIncreaseTimer >= speedIncreaseInterval){
            speedIncreaseTimer = 0;
            if(moveSpeed > 0){
                moveSpeed += speedIncreaseBy;
            }
            else{
                moveSpeed -= speedIncreaseBy;
            }
        }
        else{
            speedIncreaseTimer += Time.deltaTime;
        }
    }

    bool checkCollision(){
        playerMaxY = player.transform.localPosition.y + playerHeight / 2;
        playerMinY = player.transform.localPosition.y - playerHeight / 2;

        AIMaxY = AI.transform.localPosition.y + AIHeight / 2;
        AIMinY = AI.transform.localPosition.y - AIHeight / 2;


        // Collision with player:
        if(transform.localPosition.x - ballWidth / 2 < playerMaxX && 
            transform.localPosition.x + ballWidth  / 2 > playerMinX){

                if(transform.localPosition.y - ballHeight / 2 < playerMaxY &&
                    transform.localPosition.y + ballHeight / 2 > playerMinY){
                        collideWithPlayer = true;
                        ballDirection = Vector2.right;
                        transform.localPosition = new Vector3(playerMaxX + ballWidth / 2, transform.localPosition.y, transform.localPosition.z);
                        return true;
                }
                else{
                    if(!assignedpoint){
                        assignedpoint = true;
                        game.aiPoint();
                    }
                }
        }
        
        // Collision with AI:
        if(transform.localPosition.x + ballWidth / 2 > AIMaxX &&
            transform.localPosition.x - ballWidth / 2 < AIMinX){

                if(transform.localPosition.y - ballHeight / 2 < AIMaxY &&
                    transform.localPosition.y + ballHeight / 2 > AIMinY){
                        collidedWithAI = true;
                        ballDirection = Vector2.left;
                        transform.localPosition = new Vector3 (AIMaxX - ballWidth / 2, transform.localPosition.y, transform.localPosition.z);
                        return true;
                }
                else{
                    if(!assignedpoint){
                        assignedpoint = true;
                        game.playerPoint();
                    }
                }
        }

        // Collision with up and bottom:
        if(transform.localPosition.y > topBorder){
            transform.localPosition = new Vector3(transform.localPosition.x, topBorder, transform.localPosition.z);
            collidedWithWall = true;
            return true;
        }

        if(transform.localPosition.y < bottomBorder){
            transform.localPosition = new Vector3(transform.localPosition.x, bottomBorder, transform.localPosition.z);
            collidedWithWall = true;
            return true;
        }

        return false;

    }

    void move(){
        if(!checkCollision()){

            vx = moveSpeed * Mathf.Cos(bounceAngle);
            if(moveSpeed > 0){
                vy = moveSpeed * -Mathf.Sin(bounceAngle);
            }
            else{
                vy = moveSpeed * Mathf.Sin(bounceAngle);
            }

            transform.localPosition += new Vector3(ballDirection.x * vx * Time.deltaTime, vy * Time.deltaTime, 0);
        }else{
            if(moveSpeed < 0){
                moveSpeed = -1 * moveSpeed;
            }

            if(collideWithPlayer){
                collideWithPlayer = false;
                float relativeIntersectY = player.transform.localPosition.y - transform.localPosition.y;
                float normalizedRelativeIntersectionY = (relativeIntersectY / (playerHeight / 2));

                bounceAngle = normalizedRelativeIntersectionY * (maxAngl * Mathf.Deg2Rad);
            }
            else if (collidedWithAI){
                collidedWithAI = false;
                float relativeIntersectY = AI.transform.localPosition.y - transform.localPosition.y;
                float normalizedRelativeIntersectionY = (relativeIntersectY / (AIHeight / 2));

                bounceAngle = normalizedRelativeIntersectionY * (maxAngl * Mathf.Deg2Rad);
            }
            else if (collidedWithWall){
                collidedWithWall = false;
                bounceAngle = -1 * bounceAngle;
            }
        }
    }


    float getRandomBounceAngle(float minDegrees = 160f, float maxDegrees = 260f){
        float minRad = minDegrees * Mathf.PI / 180;
        float maxRad = maxDegrees * Mathf.PI / 180;

        return Random.Range(minRad, maxRad);
    }
}
