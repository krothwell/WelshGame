using UnityEngine.UI;

namespace DataUI {
    namespace ListItems {
        public class TaskPartPrefab : TaskPart {

            private string prefabDescription;
            public string PrefabDescription {
                get { return prefabDescription; }
                set { prefabDescription = value; }
            }

            public void InitialiseMe(string description, string partID) {
                SetDescriptionText(description);
                MyID = partID;
                prefabDescription = description;
            }

            public void SetDescriptionText(string description) {
                transform.Find("DescriptionLbl").GetComponent<Text>().text = description;
            }
        }
    }
}
