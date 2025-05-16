using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCalculation : MonoBehaviour
{
    int playerScore=150;
    TMP_Text displayScore;
    GameObject scoreDisplayPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        scoreDisplayPanel = GameObject.Find("ScoreDisplayPanel");
        displayScore=scoreDisplayPanel.GetComponentInChildren<TMP_Text>();
        scoreDisplayPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) 
    {
        switch(hit.gameObject.tag)
        {
            case "Obstacle":
            UpdatePlayerScore(-5);
            break;

            case "Ground":
            UpdatePlayerScore(-10);
            break;

            case "FinishLine":
            UpdatePlayerScore(20);
            DisplayScore();
            break;
        }    
    }

    void UpdatePlayerScore(int score)
    {
        playerScore+=score;
    }

    void DisplayScore()
    {
        scoreDisplayPanel.SetActive(true);
        displayScore.text="Your Score:" + playerScore.ToString();
    }


}
