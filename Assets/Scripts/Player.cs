using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Player : MonoBehaviour{


    public float moveSpeed = 8.0f;
    public float topBorder = 9.6f;
    public float bottomBorder = -9.6f;
    public Vector2 startPosition = new Vector2(-18.0f, 0.0f);

    private Game game;

    // Start is called before the first frame update
    void Start(){
        game = GameObject.Find("Game").GetComponent<Game>();
        transform.localPosition = (Vector3)startPosition;
    }

    // Update is called once per frame
    void Update(){
        if(game.gameState == Game.GameState.playing){
            checkInput();   
        }
    }

    void checkInput()
    {
        if (Input.GetKey(KeyCode.UpArrow)){
            if(transform.localPosition.y >= topBorder){
                transform.localPosition = new Vector3(transform.localPosition.x, topBorder, transform.localPosition.z);
            }
            else{
                transform.localPosition += Vector3.up * moveSpeed * Time.deltaTime;
            }
        }
        else if(Input.GetKey(KeyCode.DownArrow)){
            if(transform.localPosition.y <= bottomBorder){
                transform.localPosition = new Vector3(transform.localPosition.x, bottomBorder, transform.localPosition.z);
            }
            else{
                transform.localPosition += Vector3.down * moveSpeed * Time.deltaTime;
            }
        }
    }
}
