using UnityEngine.UI;

namespace DataUI {
    namespace ListItems {
        public class TaskPartDefeatCharTag : TaskPart {

            private string charTag;
            public string CharTag {
                get { return charTag; }
                set { charTag = value; }
            }

            public void InitialiseMe(string charTag_, string partID) {
                SetTagText(charTag_);
                MyID = partID;
                charTag = charTag_;
            }

            public void SetTagText(string description) {
                transform.Find("DescriptionLbl").GetComponent<Text>().text = description;
            }
        }
    }
}