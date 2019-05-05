using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    private AudioSource musicPlayer;

    public AudioClip titleMusic;
    // Start is called before the first frame update
    void Start()
    {
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

        musicPlayer.clip = titleMusic;
        musicPlayer.loop = true;
        musicPlayer.Play();
    }

    void Update()
    {
        if (Input.GetButton("Submit"))
        {
            Debug.Log("submit");
            SceneManager.LoadSceneAsync("OwenScene");
        }
    }
}
