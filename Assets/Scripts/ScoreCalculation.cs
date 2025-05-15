using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCalculation : MonoBehaviour
{
    int playerScore=150;
    [SerializeField] TMP_Text displayScore;
    [SerializeField] GameObject scoreDisplayPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        scoreDisplayPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other) 
    {
        switch(other.gameObject.tag)
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
