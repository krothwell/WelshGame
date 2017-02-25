using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityUtilities;

public abstract class ExistingSaveGame : MonoBehaviour, ISelectableUI {
    private int id;
    public int ID {
        get { return id; }
        set { id = value; }
    }
    private string saveRef;
    public string SaveRef {
        get { return saveRef; }
        set { saveRef = value; }
    }
    private string charName;
    public string CharName {
        get { return charName; }
        set { charName = value; }
    }
    private string saveDate;
    public string SaveDate {
        get { return saveDate; }
        set { saveDate = value; }
    }

    protected PlayerSavesController playerSavesController;
    GameObject panel;

    protected PlayerSavesController GetPlayerSavesController () {
        return FindObjectOfType<PlayerSavesController>();
    }

    protected GameObject GetPanel() {
        return transform.FindChild("Panel").gameObject;
    }

    public abstract void SelectSelf();

    public void DeselectSelf() {
        GetPanel().GetComponent<Image>().color = new Color(0.82f, 0.94f, 0.9999f);
    }
}
