﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject[] Levels;

    public int currentLevelIndex = 0;

    private GameObject levelInstance;

    public bool hasWon;

    public GameObject continueText;
    public GameObject retryText;

    private bool gameOver = false;
    public static LevelManager Instance;

    void Awake()
    {
        if (!Instance || Instance == this)
        {
            Instance = this;
        }
        else
        {
            print("ERROR: Duplicate Camera");
            Destroy(gameObject);
        }
        
        
    }


    public void Init()
    {
        StartLevel(currentLevelIndex);
    }

    public void Restart()
    {
        Destroy(levelInstance);
        StartLevel(currentLevelIndex);
    }

    void StartLevel(int x)
    {
        hasWon = false;
        continueText.SetActive(false);
        retryText.SetActive(false);
        Debug.Log("starting level");
        Destroy(levelInstance);
        levelInstance = Instantiate(Levels[x]);
        
        Debug.Log(levelInstance);
        levelInstance.SetActive(true);
        Invoke("ResetPlayer", 0.05f);
    }

    void ResetPlayer()
    {
        FindObjectOfType<Player>().Reset();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Confirm"))
        {
            if (hasWon)    
                CompleteLevel();
            else if (gameOver)
                SceneManager.LoadSceneAsync("OwenScene");
        }
    }

    public void CompleteLevel()
    {
        hasWon = false;
        currentLevelIndex++;
        if (currentLevelIndex >= Levels.Length)
        {
            EndGame();
        }
        else
        {
            StartLevel(currentLevelIndex);
        }
    }

    public void EndGame()
    {
        continueText.SetActive(false);
        int score = FindObjectOfType<Player>().score;
        gameOver = true;
        continueText.GetComponentInChildren<TextMeshProUGUI>().text = "Thanks for playing! You made $" + score;
        
    }
    
    
}
