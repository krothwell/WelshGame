using UnityEngine.UI;

namespace DataUI.ListItems {
    public class ExistingActivateWelshVocab : ExistingResult {
        public void InitialiseMe(string resultID, string engText, string welshText) {
            SetMyID(resultID);
            transform.Find("English").GetComponent<Text>().text = engText;
            transform.Find("Welsh").GetComponent<Text>().text = welshText;
        }
    }
}