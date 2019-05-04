using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool IsLocked = true;

    private Vector3 startPos;

    void Awake()
    {
        startPos = transform.position;
    }
    public void Open()
    {
        IsLocked = false;
    }

    void Update()
    {
        if (!IsLocked)
        {
            transform.position -=Vector3.up * Time.deltaTime * 2;
        }
    }
}
