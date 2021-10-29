using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        // Singleton
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddToScore(int score)
    {
        this.score += score;
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetGame()
    {
        score = 0;
    }
    
}
