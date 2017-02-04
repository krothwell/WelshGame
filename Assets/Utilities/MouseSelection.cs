using UnityEngine;
using System.Collections;

namespace UnityUtilities {
    public class MouseSelection : MonoBehaviour {
        public static GameObject selected;
        private static bool active;
        private static float delayTime;


        void Start() {
            active = true;
        }


        void Update() {
            //if (Input.GetMouseButtonUp(0)){
            //	SelectController.ClickSelect();
            //}
            if (!active) {
                if (delayTime >= 0f) { delayTime -= Time.deltaTime; }
                else { active = true; }
            }
        }

        public static void DelayNextSelection() {
            delayTime = 0.2f;
            active = false;
        }

        //This method returns the game object that was clicked using Raycast 2D
        public static void ClickSelect() {
            //Converting Mouse Pos to 2D (vector2) World Pos
            if (active) {
                Vector2 rayPos = new Vector2(
                                    Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                                    Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
                RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

                if (hit) {
                    selected = hit.transform.gameObject;
                    print(selected);
                }
                else { selected = null; }
            }
        }

        public static bool ClickedDifferentGameObjectTo(GameObject gameObj) {
            if (selected == null) {
                return true;
            }
            else if (selected != gameObj) {
                return true;
            }
            else {
                return false;
            }

        }

        public static bool IsClickedGameObjectName(string gameObjectName) {
            if (selected == null) {
                return false;
            }
            else if (selected.name.Contains(gameObjectName)) {
                return true;
            }
            else if (!selected.name.Contains(gameObjectName)) {
                return false;
            }
            else {
                return true;
            }

        }

        public static bool IsClickedGameObjectTag(string gameObjectTag) {
            if (selected == null) {
                return false;
            }
            else if (selected.tag == gameObjectTag) {
                return true;
            }
            else if (selected.tag != gameObjectTag) {
                return false;
            }
            else {
                return true;
            }
        }
    }
}