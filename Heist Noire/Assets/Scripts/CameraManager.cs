using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Vector3 offset;
    public Player player;

    public float shakeDuration;

    public float dampingSpeed = 1;
    public float shakeMagnitude;
    
    
    public static CameraManager Instance;

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

    public void StartScreenShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
            return;
        GetComponent<Rigidbody>().AddForce((player.transform.position + offset) - transform.position);

        Vector3 initialPosition = transform.localPosition;
        
        if (shakeDuration > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
   
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = initialPosition;
        }
    }
}
