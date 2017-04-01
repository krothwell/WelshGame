using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameUI.Utilities;

namespace GameUI.ListItems {
    public class AbilityButton : MonoBehaviour, ISelectableUI {
        public string AbilityName;
        public GameObject AbilityPrefab;
        CombatUI combatUI;
        // Use this for initialization
        void Start() {
            combatUI = FindObjectOfType<CombatUI>();
            DeselectSelf();
        }

        public void SelectSelf() {
            GetComponent<Image>().color = Colours.selectedAbility;
            combatUI.SetAbility(AbilityPrefab);
            combatUI.EndCurrentSelection();
        }

        public void DeselectSelf() {
            GetComponent<Image>().color = Colours.deselectedAbility;
            combatUI.RemoveCurrentAbility();
        }

        void OnMouseUpAsButton() {
            if ((combatUI.GetSelectedItemFromGroup(combatUI.SelectedAbilityOption) as AbilityButton) == this) {
                combatUI.ToggleSelectionTo(null, combatUI.SelectedAbilityOption);
            }
            else {
                combatUI.ToggleSelectionTo(this, combatUI.SelectedAbilityOption);
            }
        }
    }
}