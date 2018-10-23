using UnityEngine;
using System.Collections.Generic;
using UnityUtilities;
using GameUI;

/// <summary>
/// Responsible for high level members relating to world items.
/// </summary>
public class WorldItems : MonoBehaviour {
    public List<string[]> WorldItemList { get; private set; }
    public enum WorldItemTypes {
        HeadWearable,
        BodyWearable,
        HandsWearable,
        WeaponWearable,
        ShieldWearable,
        LegsWearable,
        FeetWearable
    };
    //PlayerInventoryUI playerInventory;
    PlayerEquipmentSlots playerEquipmentSlots;
    void Awake() {
        WorldItemList = new List<string[]>();
        SetWorldItemsList();
    }

    void Start() {
        //playerInventory = FindObjectOfType<PlayerInventoryUI>();
        //playerEquipmentSlots = FindObjectOfType<PlayerEquipmentSlots>();
        //playerEquipmentSlots.InitialiseEquippedItemsDict();
    }

    /// <summary>
    /// Creates a list with a string array holding all the details to save world item state
    /// in the database
    /// </summary>
    public void SetWorldItemsList() {
        WorldItemList.Clear();
        PlayerInventoryUI playerInventory;
        playerInventory = FindObjectOfType<PlayerInventoryUI>();
        playerInventory.OpenInventory();
        foreach (WorldItem worldItem in FindObjectsOfType<WorldItem>()) {
            string[] itemDetails = new string[6];
            Vector3 itemPosition = worldItem.GetComponent<Transform>().localPosition;
            itemDetails[0] = itemPosition.x.ToString();
            itemDetails[1] = itemPosition.y.ToString();
            itemDetails[2] = itemPosition.z.ToString();
            itemDetails[3] = worldItem.transform.GetPath();
            itemDetails[4] = worldItem.name;
            string prefabPath = "WorldItems/" + worldItem.name;
            itemDetails[5] = prefabPath;
            WorldItemList.Add(itemDetails);
        }
        //if (FindObjectOfType<GameMenuUI>().IsOn) {
        //    playerInventory.CloseInventory();
        //}

    }

    /// <summary>
    /// As a saved game will likely have included world items being moved, the state of all
    /// world items are saved to the database. When loading a game this method is responsible
    /// for destroying all world items before they're reloaded into the game from their saved position.
    /// </summary>
    public void DestroyWorldItems() {
        foreach (WorldItem worldItem in FindObjectsOfType<WorldItem>()) {
            Destroy(worldItem.gameObject);
        }
    }
}
