using UnityEngine;
using System.Collections;
using UnityUtilities;

namespace GameUtilities {
    namespace Display {
        /// <summary>
        /// Ad hoc methods to change the layering of sprites in game.
        /// </summary>
        public class ImageLayerOrder : MonoBehaviour {

            public static void SetLayerOrder(GameObject go) {
                int layer = GetOrderInt(go);
                go.GetComponent<SpriteRenderer>().sortingOrder = layer;
            }

            public static void SetOrderOnGameObjectArray(GameObject[] arrayObjects, int order) {
                foreach (GameObject go in arrayObjects) {
                    go.GetComponent<SpriteRenderer>().sortingOrder = order;
                }
            }

            public static void SetOrderOnSpriteObjectArray(SpriteRenderer[] arrayObjects, int order) {
                foreach (SpriteRenderer go in arrayObjects) {
                    go.sortingOrder = order;
                }
            }

            public static void SetOrderOnTranformChildren(Transform parentTransform) {
                foreach (Transform childTransform in parentTransform) {
                    int orderInt = GetOrderInt(childTransform.gameObject);
                    childTransform.gameObject.GetComponent<SpriteRenderer>().sortingOrder = orderInt;
                }
            }

            public static void SetOrderOnParticleSystem(ParticleSystemRenderer particleSystem, int orderInt) {
                particleSystem.sortingOrder = orderInt; 
            }

            public static int GetOrderInt(GameObject go) {
                return (int)(-System.Math.Round(go.GetComponent<Transform>().position.y, 1) * 10);
            }

            public static void SetZ(GameObject go) {
                
                Vector3 goPos = go.GetComponent<Transform>().localPosition;
                //print("gameobject: " + go + ", current y position: " + goPos.y);
                float newZ = (float)((goPos.y % 10) * 0.2);
               // if (newZ >= 2f) {
               //     newZ = -2f + (float)(System.Math.Round((newZ % 20),2));
               // }
                //print("new z:" + newZ);
                go.GetComponent<Transform>().localPosition = new Vector3(goPos.x, goPos.y, newZ);
                //print(go.GetComponent<Transform>().localPosition);
            }
        }
    }
}