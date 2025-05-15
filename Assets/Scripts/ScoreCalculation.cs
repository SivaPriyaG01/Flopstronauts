using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculation : MonoBehaviour
{
    int playerScore=150;
    TMP_Text displayScore;
    
    // Start is called before the first frame update
    void Start()
    {
        
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
            break;
        }    
    }

    void UpdatePlayerScore(int score)
    {
        playerScore+=score;
    }

    void DisplayScore()
    {
        displayScore.text="Your Score:" + playerScore.ToString();
    }

}
