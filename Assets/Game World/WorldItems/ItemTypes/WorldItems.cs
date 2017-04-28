using UnityEngine;
using System.Collections.Generic;
using UnityUtilities;
using UnityEditor;
using GameUI;

public class WorldItems : MonoBehaviour {

    private List<string[]> worldItemList;
    public enum WorldItemTypes {
        HeadWearable,
        BodyWearable,
        HandsWearable,
        WeaponWearable,
        ShieldWearable,
        LegsWearable,
        FeetWearable
    };

    void Awake() {
        worldItemList = new List<string[]>();
        SetWorldItemsList();
    }

    public void SetWorldItemsList() {
        //print(FindObjectsOfType<WorldItem>().Length);
        worldItemList.Clear();
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
            worldItemList.Add(itemDetails);
            //print(worldItem.name);
        }
        playerInventory.CloseInventory();

    }

    public List<string[]> GetWorldItemsList() {
        return worldItemList;
    }

    public void DestroyWorldItems() {
        foreach (WorldItem worldItem in FindObjectsOfType<WorldItem>()) {
            Destroy(worldItem.gameObject);
        }
    }
}
