using UnityEngine;
namespace GameUI {
    namespace Utilities {
        /* The name colours sounds quite generic but colour tends to be spelt Colour in the built
        *  in namespace classes that Unity tends to use so I think it is unlikely to clash anywhere.*/
        public class Colours {
            public static Color32 colorSelectedQuestPanel = new Color32(174, 218, 208, 255);
            public static Color32 colorQuestPanel = new Color32(122, 163, 154, 255);
            public static Color32 colorCompletedQuestPanel = new Color32(150, 150, 150, 125);
            public static Color32 colorCompletedQuestTaskPart = new Color32(150, 150, 150, 125);
            public static Color32 colorCompletedQuestTask = new Color32(150, 150, 150, 125);
            public static Color32 selectedAbility = new Color32(255, 246, 231, 255);
            public static Color32 deselectedAbility = new Color32(255, 246, 231, 125);
        }
    }
}
