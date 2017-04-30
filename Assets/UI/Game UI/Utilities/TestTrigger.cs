using UnityEngine;

public class TestTrigger {
    public enum TriggerType {
        Ability,
        Quest
    }
    private string triggerName;
    private Sprite iconSprite;
    private TriggerType triggerType;

    public TestTrigger(string name, Sprite sprite, TriggerType typeIn) {
        triggerName = name;
        iconSprite = sprite;
        triggerType = typeIn;
    }
    
    public string GetTriggerName() {
        return triggerName;
    }

    public Sprite GetTriggerSprite() {
        return iconSprite;
    }

    public string GetTriggerLabel() {
        string labelStr = "";
        switch (triggerType) { 
            case TriggerType.Ability:
                labelStr = "Ability attempted";
                break;
            case TriggerType.Quest:
                labelStr = "Part of quest";
                break;
        }
        return labelStr;
    }

}
