using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        Rigidbody otherBody = other.attachedRigidbody;
        if (!otherBody)
            return;
        if (other.attachedRigidbody.GetComponent<Player>())
        {
            Debug.Log("goal trigger");
            LevelManager.Instance.continueText.SetActive(true);
            LevelManager.Instance.PlayCompleteMusic();
            LevelManager.Instance.hasWon = true;
            if (LevelManager.Instance.currentLevelIndex >= LevelManager.Instance.Levels.Length - 1)
            {
                LevelManager.Instance.EndGame();
            }
            FindObjectOfType<Player>().AddScore();
            gameObject.SetActive(false);
            //FindObjectOfType<Player>().OnDie();
        };
    }
}
