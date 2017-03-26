using System.Collections;
using System.Collections.Generic;
using DbUtilities;

public class VocabIntroGetter {

    string[] vocabIDArray;
    private enum grammarIDs {
        alphabet = 1,
    };
    Dictionary<int, string> vocabIntroDict;
	public VocabIntroGetter(string[] vocabID) {
        vocabIDArray = vocabID;
        vocabIntroDict = new Dictionary<int, string>();
        vocabIntroDict.Add((int)grammarIDs.alphabet, "Repeat the letter of the alphabet:");
    }

    public string[] GetIntroNodeArray() {
        string[] nodeArray = new string[2];
        nodeArray[0] = "-1";
        nodeArray[1] = GetIntroText();
        return nodeArray;
    }

    public string GetIntroText() {
        string introTxt = "Translate the following into Welsh:";
        List<string[]> grammarList = new List<string[]>();
        DbCommands.GetDataStringsFromQry(DbQueries.GetGrammarRuleDisplayQry(vocabIDArray[0], vocabIDArray[1]), out grammarList, vocabIDArray[0], vocabIDArray[1]);
        foreach (string[] grammarRule in grammarList) {
            string strGrammarID = grammarRule[0];
            int intGrammarID = int.Parse(strGrammarID);
            if (vocabIntroDict.ContainsKey(intGrammarID)) {
                introTxt = vocabIntroDict[intGrammarID];
                break;
            }
        }
        return introTxt;
    }


}
