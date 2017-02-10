using UnityEngine;
//using System.Collections.Generic;

namespace UnityUtilities {
    /// <summary>
    /// Ad hoc methods for Game object type comparisons
    /// </summary>
    public static class TypeComparisons {

        public static bool AreDifferentGameObjects(GameObject go1, GameObject go2) {
            if ((go1 == null && go2 != null) || go1 != null && go2 == null) {
                return true;
            }
            else if (go1 != go2) {
                return true;
            }
            else {
                return false;
            }

        }

        public static bool IsGameObjectName(GameObject go, string nameTest) {
            if (go == null) {
                return false;
            }
            else if (go.name == nameTest) {
                return true;
            }
            else {
                return false;
            }

        }

        public static bool IsGameObjectTag(GameObject go, string gameObjectTag) {
            if (go == null) {
                return false;
            }
            else if (go.tag == gameObjectTag) {
                return true;
            }
            else if (go.tag != gameObjectTag) {
                return false;
            }
            else {
                return true;
            }
        }
        
        public static bool HasComponent <T>(this GameObject obj) where T:Component {
            return obj.GetComponent<T>() != null;
        }
    }
}