using UnityEngine.UI;

namespace DataUI.ListItems {
    public class ExistingActivateWelshVocab : ExistingResult {
        public void InitialiseMe(string resultID, string engText, string welshText) {
            transform.FindChild("English").GetComponent<Text>().text = engText;
            transform.FindChild("Welsh").GetComponent<Text>().text = welshText;
            SetMyID(resultID);
        }
    }
}