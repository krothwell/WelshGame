using UnityEngine;
using System.Collections;

public class CombatUI : MonoBehaviour {
    public enum CombatAbilities { passive, strike }
    public CombatAbilities currentAbility;
    LowerUI lowerUI;
    GameObject abilitiesPanel;
    ExplorerUI explorerUI;
    public Texture2D[] cursors;
    public bool CombatUIactive;
    CloseRangeEnemyPlacements closeRangeEnemyPlacements;
    MainCharacter mainChar;
    // Use this for initialization
    void Start () {
        lowerUI = FindObjectOfType<LowerUI>();
        abilitiesPanel = transform.FindChild("AbilitiesPanel").gameObject;
        explorerUI = FindObjectOfType<ExplorerUI>();
        currentAbility = CombatAbilities.passive;
        closeRangeEnemyPlacements = FindObjectOfType<CloseRangeEnemyPlacements>();
        mainChar = FindObjectOfType<MainCharacter>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!lowerUI.components.activeSelf && mainChar.playerStatus == MainCharacter.PlayerStatus.inCombat) {
            if (Input.GetKeyUp(KeyCode.Space)) {
                ToggleCombatMode();
            }
        }
    }

    public void ToggleCombatMode() {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        ShowAbilities();
    }

    private void ShowAbilities() {
        if (abilitiesPanel.activeSelf) {
            abilitiesPanel.SetActive(false);
            explorerUI.ShowMe();
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            CombatUIactive = false;
            currentAbility = CombatAbilities.passive;
        }
        else {
            abilitiesPanel.SetActive(true);
            explorerUI.HideMe();
            CombatUIactive = true;
        }
    }

    public void SetStrikeAbility() {
        currentAbility = CombatAbilities.strike;
    }

    public void SetAttackCursor() {
        Texture2D cursorTexture = cursors[0];
        CursorMode cursorMode = CursorMode.ForceSoftware;
        Cursor.SetCursor(cursorTexture, Vector2.zero, cursorMode);
    }
}
