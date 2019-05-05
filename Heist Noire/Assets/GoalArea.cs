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
            LevelManager.Instance.continueText.SetActive(true);
            LevelManager.Instance.hasWon = true;
            FindObjectOfType<Player>().AddScore();
            //FindObjectOfType<Player>().OnDie();
        };
    }
}
