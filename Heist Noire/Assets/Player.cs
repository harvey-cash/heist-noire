using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{


    public Rigidbody rb;
    public float speed = 4;
    private bool pickingUpLoot;
    private Loot[] lootInWorld;
    private int inventorySize = 6;
    public int InventorySize => inventorySize;
    public Sprite[] WalkCycleSprites;

    public float lootDistance = 10;
    private Loot[] currentLoot;
    public Loot[] CurrentLoot => currentLoot;
    public int InventoryIndex = 0;
    
    public Transform lootHolder;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Collider[] colliders;
    public GameObject DeadAnimation;

    public LineRenderer throwIndicator;


    public int score;
    
    private Vector3 startPos;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        currentLoot = new Loot[inventorySize];
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        InventoryUI.Instance.Init(this);
        colliders = GetComponentsInChildren<Collider>();
        LevelManager.Instance.Init();
    }

    public void OnDie()
    {
        gameObject.SetActive(false);
        DeadAnimation.transform.position = transform.position;
        DeadAnimation.SetActive(true);
        DeadAnimation.GetComponentInChildren<DeadPlayer>(true).SetPlayer(this);
    }

    public void Reset()
    {
        gameObject.SetActive(true);
        transform.position = GameObject.FindWithTag("SpawnPoint").transform.position;
        DeadAnimation.SetActive(false);
        Destroy(lootHolder.gameObject);
        lootHolder = new GameObject("loot holder").transform;
        lootHolder.SetParent(transform);
        if (currentLoot != null)
        {
            foreach (var loot in CurrentLoot)
            {
                RemoveLootFromInventory(loot);
            }
        }
        else
        {
            currentLoot = new Loot[InventorySize];
        }

    }
    
    public void OnHit()
    {
    }

    public void TakeDamage(int x)
    {
        LevelManager.Instance.retryText.SetActive(true);
        OnDie();
    }

    private bool InventoryHasSpace()
    {
        return Array.Exists(currentLoot, loot1 => loot1 == null);
    }
    
    // Update is called once per frames
    void Update()
    {
        InputScript();
        DrawThrowIndicator();
    }

    private void DrawThrowIndicator() {
        Vector3 direction = new Vector3(Input.GetAxisRaw("XAim"), 0, Input.GetAxisRaw("YAim")) * 12;
        
        Vector3 origin = new Vector3(transform.position.x, 0, transform.position.z);
        
        
        
        Vector3 end = origin + direction;

        if (!(currentLoot[InventoryIndex] as ThrowableLoot))
        {
            end = origin;
        }
        
        
        throwIndicator.widthCurve = new AnimationCurve(
         new Keyframe(0, 0.4f)
         , new Keyframe(0.999f - 0.2f, 0.4f)  // neck of arrow
         , new Keyframe(1 - 0.2f, 1f)  // max width of arrow head
         , new Keyframe(1, 0f));
        throwIndicator.SetPositions(new Vector3[] { origin, end });
    }

    private void IncreaseInventoryIndex()
    {
        InventoryIndex++;
        if (InventoryIndex >= inventorySize)
            InventoryIndex = 0;
        InventoryUI.Instance.UpdateIcons();
    }
    
    private void DecreaseInventoryIndex()
    {
        InventoryIndex--;
        if (InventoryIndex < 0)
            InventoryIndex = inventorySize - 1;
        InventoryUI.Instance.UpdateIcons();
    }

    public void Alert()
    {
        Debug.Log("ALert");
        GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().time = 0;
        
    }

    void InputScript()
    {
        Vector3 movement_vector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (movement_vector.magnitude > 0.3f)
        {
            animator.SetBool("Walk", true);
            if (movement_vector.x < 0)
                spriteRenderer.transform.localScale = new Vector3(-0.4f,0.4f,0.4f);
            else
                spriteRenderer.transform.localScale = new Vector3(0.4f,0.4f,0.4f);
            rb.MovePosition(rb.position + (movement_vector * speed) * Time.deltaTime);
            animator.speed = movement_vector.magnitude / 2f;
        }
        else
        {

            animator.SetBool("Walk", false);
            animator.speed = 0.2f;
        }



        if (Input.GetButtonDown("Inventory Up"))
        {
            IncreaseInventoryIndex();
        }
        if (Input.GetButtonDown("Inventory Down"))
        {
            DecreaseInventoryIndex();
        }

        if (!pickingUpLoot)
        {
            if (Input.GetButtonDown("Drop"))
            {
                DropLoot(currentLoot[InventoryIndex]);
            }
            else if (Input.GetAxis("Use") > 0.3f)
            {
                UseLoot(currentLoot[InventoryIndex]);
            }
        }

    }

    public void AddScore()
    {
        foreach (var loot in currentLoot)
        {
            if (loot)
                score += loot.value;
        }
    }
    
    private void DropLoot(Loot loot)
    {
        if (!loot)
            return;
        loot.transform.position = transform.position - Vector3.forward;
        loot.gameObject.SetActive(true);
        
        loot.OnDrop(this);
    }
    

    private void UseLoot(Loot loot)
    {
        if (loot)
            loot.OnUse(this);
    }

    public void RemoveLootFromInventory(Loot loot)
    {
        int index = Array.IndexOf(currentLoot, loot);
        if (index >= 0)
        {
            currentLoot[index] = null;
            InventoryUI.Instance.UpdateIcons();
        }
    }

    private void FixedUpdate()
    {

        if (Input.GetAxis("Pick Up") > 0.3f)
        {
            if (!pickingUpLoot)
            {
                pickingUpLoot = true;
                lootInWorld = FindObjectsOfType<Loot>();
                PickupLoot();
            }
            else
            {
                PickupLoot();
            }
        }
        else
        {
            pickingUpLoot = false;
        }
        
        
    }

    void PickupLoot()
    {
        pickingUpLoot = true;
        foreach (var lootObject in lootInWorld)
        {
            if (!lootObject)
                continue;
            if (!lootObject.PickedUp)
            {
                lootObject.rb.AddExplosionForce(-50,transform.position, lootDistance);
            }
        }
    }

    public void AddLoot(Loot loot)
    {
        if (InventoryHasSpace() && !Array.Exists(currentLoot, loot1 => loot1 == loot))
        {
            for (int i = 0; i < inventorySize; i++)
            {
                if (!currentLoot[i])
                {
                    currentLoot[i] = loot;
                    break;
                }
            }
            loot.transform.parent = lootHolder;
            InventoryUI.Instance.UpdateIcons();
        }
    }

    public void ThrowLoot(ThrowableLoot loot, float speed)
    {
        DropLoot(loot);
        
        Vector3 direction = new Vector3(Input.GetAxisRaw("XAim"), 0, Input.GetAxisRaw("YAim"));
        if (direction.magnitude > 0.3f)
        {
            RemoveLootFromInventory(loot);
            loot.transform.position = transform.position;
            foreach (var c in colliders)
            {
                Physics.IgnoreCollision(loot.GetComponentInChildren<Collider>(), c);
            }
            
            Debug.Log("throwing: " + direction);
            loot.HasBeenThrown = true;
            loot.rb.AddForce(direction * speed * direction.magnitude, ForceMode.VelocityChange);
            loot.OnLaunch();
        }
        
    }
    
    
    private void OnCollisionEnter(Collision other)
    {
        if (!other.rigidbody)
            return;
        if (pickingUpLoot)
        {
            Loot loot = other.rigidbody.gameObject.GetComponent<Loot>();
            if (loot)
            {
                if (!loot.PickedUp && InventoryHasSpace())
                {
                    loot.OnPickup(this);
                }
            }
        }
    }
}
