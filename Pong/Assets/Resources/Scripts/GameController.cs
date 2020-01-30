using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    [SerializeField]
    Text Player1ScoreText;
    [SerializeField]
    Text Player2ScoreText;

    [SerializeField]
    GameObject[] Paddles;
    
    public GameObject Ball;
    CameraShake CamShake;
    int Player1Score, Player2Score;

    TextManager TextManager;

	// Use this for initialization
	void Start () {
        TextManager = GetComponent<TextManager>();
        CamShake = GetComponent<CameraShake>();
        Reset();

        //Paddles[0].GetComponent<Paddle>().SetMoveStrategy(new MoveStrategy.AIMoveStrategy(Paddles[0], Ball));
        //Paddles[0].GetComponent<Paddle>().SetMoveStrategy(new MoveStrategy.PlayerMoveStrategy(0));
        //Paddles[1].GetComponent<Paddle>().SetMoveStrategy(new MoveStrategy.AIMoveStrategy());

        GameState.SetState(GameState.State.Playing);
	}

    private void Update()
    {
        if (GameState.IsState(GameState.State.WinScreen))
        {
            if (TextManager.IsState(TextManager.State.Finished)) {
                Reset();
                GameState.SetState(GameState.State.Playing);                
            }
        }
    }

    private void Reset()
    {
        Player1ScoreText.text = "";
        Player2ScoreText.text = "";
        Player1Score = 0;
        Player2Score = 0;
        UpdateScoreTexts();
        Ball.GetComponent<Ball>().Reset();

        foreach (GameObject p in Paddles)
        {
            p.GetComponent<Paddle>().Reset();
        }
    }

    public void BallHitEdge(GameObject edge, int playerID)
    {
        CamShake.Shake();

        if (GameState.IsState(GameState.State.Playing))
        {
            IncrementScore(playerID);
        }

        if (GameState.IsState(GameState.State.Playing) || GameState.IsState(GameState.State.TitleScreen))
        {
            StartCoroutine(ResetGameAfterDelay());
        }
    }

    void IncrementScore(int playerID)
    {
        if (playerID == 0)
        {
            Player1Score++;
        }
        else
        {
            Player2Score++;
        }
        UpdateScoreTexts();
        CheckGameOver();
    }

    IEnumerator ResetGameAfterDelay()
    {
        yield return new WaitForSeconds(1.0f);
        Ball.GetComponent<Ball>().Reset();
    }

    void UpdateScoreTexts()
    {
        string tmpText = "";

        for (var i = 0; i<Player1Score; ++i)
        {
            tmpText += "X";
        }
        Player1ScoreText.text = tmpText;

        tmpText = "";
        for (var i = 0; i < Player2Score; ++i)
        {
            tmpText += "X";
        }
        Player2ScoreText.text = tmpText;
    }

    void CheckGameOver()
    {
        if (Player1Score > 2 || Player2Score > 2)
        {
            GameState.SetState(GameState.State.WinScreen);

            if (Player1Score > 2)
            {
                TextManager.MessageItem[] messageQueue = {
                    new TextManager.MessageItem("LEFT", 0.6f), 
                    new TextManager.MessageItem("WINS", 0.6f),
                    new TextManager.MessageItem("LEFT", 0.6f),
                    new TextManager.MessageItem("WINS", 0.6f)
                };

                TextManager.SetQueueAndStart(messageQueue);
            } else
            {
                TextManager.MessageItem[] messageQueue = {
                    new TextManager.MessageItem("RIGHT", 0.6f),
                    new TextManager.MessageItem("WINS",  0.6f),
                    new TextManager.MessageItem("RIGHT", 0.6f),
                    new TextManager.MessageItem("WINS",  0.6f)
                };

                TextManager.SetQueueAndStart(messageQueue);
            }

        }
    }
}
