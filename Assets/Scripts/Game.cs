using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour{


    private GameObject ball;
    private int aiScore;
    private int playerScore;

    private GameObject hudCanvas;
    private Hud hud;

    private GameObject ai;

    public int winningScore = 2;


    public enum GameState{
        playing,
        gameOver,
        paused,
        launched
    }

    public GameState gameState = GameState.launched;
    // Start is called before the first frame update
    void Start(){
        ai = GameObject.Find("AI");
        hudCanvas = GameObject.Find("hud-canvas");
        hud = hudCanvas.GetComponent<Hud>();
        hud.playAgain.text = "PRESS SPACEBARE TO PLAY";
    }

    // Update is called once per frame
    void Update(){
        checkScore();
        checkInput();
    }

    void checkInput(){
        if(gameState == GameState.paused || gameState == GameState.playing){
            if(Input.GetKeyUp(KeyCode.Space)){
                pauseResumeGame();
            }
        }


        if(gameState == GameState.launched || gameState == GameState.gameOver){
            if(Input.GetKeyUp(KeyCode.Space)){
                startGame();
            }
        }
    }

    void checkScore(){
        if(playerScore >= winningScore || aiScore >= winningScore){
            if(playerScore >= winningScore && aiScore < playerScore - 1)
                playerWins();
            else if (aiScore >= winningScore && playerScore < aiScore - 1)
                aiWins();
        }
    }

    void spawnBall(){
        ball = GameObject.Instantiate ((GameObject)Resources.Load("Prefabs/p-ball", typeof(GameObject)));
        ball.transform.localPosition = new Vector3(0, 0, -2);
    }

    public void aiPoint(){
        aiScore++;
        hud.aiScore.text = aiScore.ToString();
        nextRound();
    }

    private void playerWins(){
        hud.playerWin.enabled = true;
        gameOver();
    }

    private void aiWins(){
        hud.aiWin.enabled = true;
        gameOver();
    }

    public void playerPoint(){
        playerScore++;
        hud.playerScore.text = playerScore.ToString();
        nextRound();
    }

    private void startGame(){
        playerScore = 0;
        aiScore = 0;
        hud.playerScore.text = "0";
        hud.aiScore.text = "0";
        hud.aiWin.enabled = false;
        hud.playerWin.enabled = false;

        hud.playAgain.enabled = false;

        gameState = GameState.playing;

        ai.transform.localPosition = new Vector3 (ai.transform.localPosition.x, 0, ai.transform.localPosition.z);
        spawnBall();
    }


    private void nextRound(){
        if(gameState == GameState.playing){
            ai.transform.localPosition = new Vector3(ai.transform.localPosition.x, 0, ai.transform.localPosition.z);
            GameObject.Destroy(ball.gameObject);

            spawnBall();
        }
    }

    private void gameOver(){
        GameObject.Destroy(ball.gameObject);
        hud.playAgain.text = "PRESS SPACEBAR TO PLAY AGAIN";
        hud.playAgain.enabled = true;
        gameState = GameState.gameOver;
    }

    private void pauseResumeGame(){
        if(gameState == GameState.paused){
            gameState = GameState.playing;
            hud.playAgain.enabled = false;
        }
        else{
            gameState = GameState.paused;
            hud.playAgain.text = "GAME IS PAUSED, PRESS SPACE TO CONTINUE PLAYING";
            hud.playAgain.enabled = true;
        }
    }
}
