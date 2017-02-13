using UnityEngine;

namespace DataUI {
    namespace Utilities {
        /* The name colours sounds quite generic but colour tends to be spelt Colour in the built
         *  in namespace classes that Unity tends to use so I think it is unlikely to clash anywhere.*/
        public class Colours : MonoBehaviour {
            public static Color32 colorDataUIbtn = new Color32(69, 135, 240, 255);
            public static Color32 colorDataUItxt = new Color32(0, 0, 0, 255);
            public static Color32 colorDataUIinactive = new Color32(33, 33, 33, 255);
            public static Color32 colorDataUIInputSelected = new Color32(204, 217, 255, 255);
            public static Color32 colorDataUIPanelInactive = new Color32(184, 184, 184, 100);
        }
    }
}