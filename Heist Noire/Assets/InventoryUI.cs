using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{

    public Transform IconHolder;
    public Transform IconHighlight;
    public static InventoryUI Instance;
    private Player player;
    public GameObject LootIcon;

    private List<Image> IconImages;

    void Awake()
    {
        if (!Instance || Instance == this)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("ERROR: Duplicate InventoryUI");
            Destroy(gameObject);
        }
        
        IconImages = new List<Image>();
    }

    public void Init(Player p)
    {
        player = p;
        for (int i = 0; i < player.InventorySize; i++)
        {
            GameObject IconObject = Instantiate(LootIcon);
            IconObject.transform.SetParent(IconHolder);
            Image IconImage = IconObject.GetComponent<Image>();
            IconImages.Add(IconImage);
        }
        UpdateIcons();
    }

    public void UpdateIcons()
    {
        for (int i = 0; i < player.InventorySize; i++)
        {
           UpdateIcon(IconImages[i], player.CurrentLoot[i]);
        }
        
        IconHighlight.SetParent(IconImages[player.InventoryIndex].transform, false);
    }

    void UpdateIcon(Image Icon, Loot loot)
    {
        if (loot)
        {
            Icon.GetComponentInChildren<Text>().text ="$" + loot.value;
            Icon.sprite = loot.Icon;
        }
        else
        {
            Icon.sprite = null;
            Icon.GetComponentInChildren<Text>().text ="";
        }
    }
    
}
