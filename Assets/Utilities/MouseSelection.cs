using UnityEngine;
using System.Collections;

namespace UnityUtilities {
    /// <summary>
    /// Ad hoc methods related to using the mouse in the game.
    /// </summary>
    public class MouseSelection : MonoBehaviour {
        private static bool active;
        private static float delayTime;


        void Update() {
            SetActiveIfInactiveAtEndOfDelay();
        }
        
        private void SetActiveIfInactiveAtEndOfDelay() {
            if (!active) {
                if (delayTime >= 0f) { delayTime -= Time.deltaTime; }
                else { active = true; }
            }
        }

        public static void DelayNextClickSelect() {
            delayTime = 0.2f;
            active = false;
        }

        //This method returns the game object that was clicked using Raycast 2D
        public static void ClickSelect(out GameObject objClicked) {
            objClicked = null;
            //Converting Mouse Pos to 2D (vector2) World Pos
            if (active) {
                Vector2 rayPos = new Vector2(
                                    Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                                    Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
                RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

                if (hit) {
                    objClicked = hit.transform.gameObject;
                }
            } 
        }

        public static bool IsClickedDifferentGameObjectTo(GameObject gameObj) {
            GameObject selected;
            ClickSelect(out selected);
            return TypeComparisons.AreDifferentGameObjects(selected, gameObj);
        }

        public static bool IsClickedGameObjectName(string gameObjectName) {
            GameObject selected;
            ClickSelect(out selected);
            return TypeComparisons.IsGameObjectName(selected, gameObjectName);
        }

        public static bool IsClickedGameObjectTag(string gameObjectTag) {
            GameObject selected;
            ClickSelect(out selected);
            return TypeComparisons.IsGameObjectTag(selected, gameObjectTag);
        }
    }
}