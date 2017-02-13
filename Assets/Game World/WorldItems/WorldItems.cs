using UnityEngine;
using System.Collections.Generic;
using UnityUtilities;

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

    private void SetWorldItemsList() {
        print(FindObjectsOfType<WorldItem>().Length);

        foreach (WorldItem worldItem in FindObjectsOfType<WorldItem>()) {
            string[] itemDetails = new string[5];
            Vector3 itemPosition = worldItem.GetComponent<Transform>().localPosition;
            itemDetails[0] = itemPosition.x.ToString();
            itemDetails[1] = itemPosition.y.ToString();
            itemDetails[2] = itemPosition.z.ToString();
            itemDetails[3] = worldItem.transform.GetPath();
            itemDetails[4] = worldItem.name;
            worldItemList.Add(itemDetails);
        }
    }

    public List<string[]> GetWorldItemsList() {
        return worldItemList;
    }
}
