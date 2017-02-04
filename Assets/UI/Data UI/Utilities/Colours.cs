using UnityEngine;

namespace DataUI {
    namespace Utilities {
        /* The name colours sounds quite generic but colour tends to be spelt Colour in the built
         *  in namespace classes that Unity tends to use so I think it is unlikely to clash anywhere.*/
        public class Colours : MonoBehaviour {
            public static Color colorDataUIbtn;
            public static Color colorDataUItxt;
            public static Color colorDataUIinactive;
            public static Color colorDataUIInputSelected;
            void Awake() {
                colorDataUIbtn = new Color(0.27f, 0.53f, 0.94f, 1f);
                colorDataUItxt = new Color(0f, 0f, 0f, 1f);
                colorDataUIinactive = new Color(0.13f, 0.13f, 0.13f, 1f);
                colorDataUIInputSelected = new Color(0.8f, 0.85f, 1f, 1f);
            }
        }
    }
}