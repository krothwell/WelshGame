using UnityEngine;

public class TestTrigger {
    public enum TriggerType {
        Ability,
        Quest,
        DialogueNode,
        DialogueChoice
    }
    private string triggerName;
    private Sprite iconSprite;
    private TriggerType trigType;
    public TriggerType TrigType {
        get { return trigType; }
        set { trigType = value; }
    }

    public TestTrigger(string name, Sprite sprite, TriggerType typeIn) {
        triggerName = name;
        iconSprite = sprite;
        trigType = typeIn;
    }
    
    public string GetTriggerName() {
        return triggerName;
    }

    public Sprite GetTriggerSprite() {
        return iconSprite;
    }

    public string GetTriggerLabel() {
        string labelStr = "";
        switch (trigType) { 
            case TriggerType.Ability:
                labelStr = "Ability attempted";
                break;
            case TriggerType.Quest:
                labelStr = "Part of quest";
                break;
            case TriggerType.DialogueChoice:
                labelStr = "Conversation";
                break;
            case TriggerType.DialogueNode:
                labelStr = "Conversation";
                break;
        }
        return labelStr;
    }

}
