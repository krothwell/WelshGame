using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WelshVocab : NewWelshVocab {
    Slider readProgressSlider;
    Slider writeProgressSlider;
    Text readProgressLbl;
    Text writeProgressLbl;
    Transform extraInfo;
    //WelshSkillsListUI welshSkillListUI;
    public void InitialiseMe(string engVocab, string cymVocab, string readTally, string writeTally) {
        base.InitialiseMe(engVocab, cymVocab);
        extraInfo = transform.Find("ExtraInformation");
        //welshSkillListUI = FindObjectOfType<WelshSkillsListUI>();
        readProgressSlider = extraInfo.Find("ReadProgressSlider").GetComponent<Slider>();
        writeProgressSlider = extraInfo.Find("WriteProgressSlider").GetComponent<Slider>();
        readProgressLbl = extraInfo.Find("ReadProgressLbl").GetComponent<Text>();
        writeProgressLbl = extraInfo.Find("WriteProgressLbl").GetComponent<Text>();
        int readTallyInt = int.Parse(readTally);
        int writeTallyInt = int.Parse(writeTally);
        int readThreshold;
        int writeThreshold;
        string readProficiencyString;
        string writeProficiencyString;
        ReadProficienciesHandler readProficienciesHandler = new ReadProficienciesHandler(engVocab, cymVocab);
        readProficienciesHandler.GetProficiencyDetailsFromTally(readTallyInt, out readProficiencyString, out readThreshold);
        WriteProficienciesHandler writeProficienciesHandler = new WriteProficienciesHandler(engVocab, cymVocab);
        writeProficienciesHandler.GetProficiencyDetailsFromTally(writeTallyInt, out writeProficiencyString, out writeThreshold);
        readProgressSlider.maxValue = readThreshold;
        writeProgressSlider.maxValue = writeThreshold;
        readProgressSlider.value = readTallyInt;
        writeProgressSlider.value = writeTallyInt;
        readProgressLbl.text = readProficiencyString;
        writeProgressLbl.text = writeProficiencyString;
        
    }
}
