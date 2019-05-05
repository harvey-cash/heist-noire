using System.Collections;
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


    public AudioClip stealthMusic;
    public AudioClip actionMusic;
    private AudioSource musicPlayer;

    private Player player;
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

        player = FindObjectOfType<Player>();
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
        player.gameObject.SetActive(false);
        Invoke("ResetPlayer", 0.05f);
        
        
        GameObject obj = GameObject.Find("musicplayer");
        if (!obj)
        {
            musicPlayer = new GameObject("musicplayer").AddComponent<AudioSource>();
            DontDestroyOnLoad(musicPlayer.gameObject);
        }
        else
        {
            musicPlayer = obj.GetComponent<AudioSource>();
        }
        ActivateStealthMusic();
        
    }

    public void PlayActionMusic()
    {
        if (musicPlayer)
        {
            StopAllCoroutines();
            if (musicPlayer.clip != actionMusic)
            {
                Debug.Log("play action music");
                float oldtime = musicPlayer.time;
                musicPlayer.clip = actionMusic;
                musicPlayer.time = 0;
                musicPlayer.volume = 0.2f;
                musicPlayer.Play();
            }
        }
        
    }
    
    public void PlayStealthMusic()
    {
        
        Invoke("ActivateStealthMusic", 3f);
        
    }

    void ActivateStealthMusic()
    {
        
        StopAllCoroutines();
        if (musicPlayer)
        {

            if (musicPlayer.clip != stealthMusic)
            {
                float oldtime = musicPlayer.time;
                musicPlayer.clip = stealthMusic;
                musicPlayer.time = oldtime;
                musicPlayer.Play();
            }
        }
    }

    void ResetPlayer()
    {
        player.Reset();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Confirm"))
        {
            if (hasWon)    
                CompleteLevel();
                
        }

        if (musicPlayer)
        {
            if (musicPlayer.volume < 1)
            {
                musicPlayer.volume += Time.deltaTime/3;
            }
        }
    }

    public void CompleteLevel()
    {
        hasWon = false;
        currentLevelIndex++;
        if (currentLevelIndex >= Levels.Length)
        {
            SceneManager.LoadSceneAsync("TitleScreen");
        }
        else
        {
            StartLevel(currentLevelIndex);
        }
    }

    public void EndGame()
    {
        continueText.SetActive(true);
        int score = FindObjectOfType<Player>().score;
        gameOver = true;
        continueText.GetComponentInChildren<TextMeshProUGUI>().text = "Thanks for playing! You made $" + score;
        
    }
    
    
}
