using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadPlayer : MonoBehaviour
{
    private Player player;
    // Start is called before the first frame update
    void OnEnable()
    {
        GetComponent<Animator>().playbackTime = 0;
    }

    public void SetPlayer(Player p)
    {
        player = p;
    }

    void Update()
    {
        if (Input.GetButtonDown("Confirm"))
        {
            player.gameObject.SetActive(true);
            LevelManager.Instance.Restart();
        }
    }
}
