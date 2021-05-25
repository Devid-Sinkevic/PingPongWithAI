using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour{
    public float moveSpeed = 8.0f;
    public float topBorder = 9.6f;
    public float bottomBorder = -9.6f;
    public Vector2 startPosition = new Vector2(18.0f, 0.0f);

    private GameObject ball = null;
    private Vector2 ballPos;


    private Game game;
    // Start is called before the first frame update
    void Start(){
        game = GameObject.Find("Game").GetComponent<Game>();

        transform.localPosition = (Vector3)startPosition;
    }

    // Update is called once per frame
    void Update(){
        if(game.gameState == Game.GameState.playing){
            move();
        }
    }

    void move(){
        if(!ball)
          ball = GameObject.FindGameObjectWithTag("p-ball");

        if(ball.GetComponent<Ball>().ballDirection != Vector2.left){
            ballPos = ball.transform.localPosition;

            if(transform.localPosition.y > bottomBorder && ballPos.y < transform.localPosition.y){
                transform.localPosition += new Vector3(0, -1 * moveSpeed * Time.deltaTime, 0);
            }

            if(transform.localPosition.y < topBorder && ballPos.y > transform.localPosition.y){
                transform.localPosition += new Vector3(0, moveSpeed * Time.deltaTime, 0);
            }
        }
    }
}
