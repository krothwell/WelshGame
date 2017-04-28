using DbUtilities;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTestDataController {
    public enum TestType {
        read,
        write
    };
    string[] intro, vocab;
    int highestTallyPossible;
    List<string[]> relatedGrammarList;
    Dictionary<int, string[]> grammarDetailsDict;
    TestType testType;
    int answerCorrectPercent;
    int tallyModifier, readTally, writeTally, skillPointsGainedTotal, tallyShiftTotal;
    bool vocabSkillIncremented;
    string resultString, playerAnswer;
    public DialogueTestDataController() {
        tallyShiftTotal = skillPointsGainedTotal = 0;
        answerCorrectPercent = -1; //-1 when not set
        SetTestData();
        SetVocabIntro(vocab);
        grammarDetailsDict = new Dictionary<int, string[]>();
        highestTallyPossible = DbCommands.GetMaxFromTable("Proficiencies", "Thresholds");
    }

    private void SetVocabIntro(string[] vocab) {
        VocabIntroGetter introGetter = new VocabIntroGetter(vocab);
        intro = introGetter.GetIntroNodeArray();
        SetGrammar();
    }

    private void SetTestData() {
        string[] vocabDetails = DbCommands.GetRandomTupleFromTable("DiscoveredVocab");
        string[] vocabKey = new string[2];
        vocabKey[0] = vocabDetails[0];
        vocabKey[1] = vocabDetails[1];
        vocab = vocabKey;
        System.Random rnd = new System.Random();
        int num = rnd.Next(0, 1);
        testType = (num < 1) ? TestType.read : TestType.write;
    }

    public string[] GetVocab() {
        string[] vocabRet = new string[2];
        if (testType == TestType.read) {
            vocabRet[0] = vocab[1];
            vocabRet[1] = vocab[0];
        } else {
            vocabRet = vocab;
        }
        return vocabRet;
    }

    public void SetGrammar() {
        relatedGrammarList = new List<string[]>();
        DbCommands.GetDataStringsFromQry(DbQueries.GetDiscoveredGrammarRelatedToVocab(vocab[0], vocab[1]), out relatedGrammarList, vocab[0], vocab[1]);
    }

    public List<string[]> GetGrammar() {
        return relatedGrammarList;
    }

    public string[] GetVocabIntro() {
        return intro;
    }

    public string GetAnswer() {
        return vocab[1];
    }

    public void SetAnswerResults(string playerAnswerIn) {
        playerAnswer = playerAnswerIn;
        string correctAnswer = GetAnswer();
        int answerLength = correctAnswer.Length;
        answerCorrectPercent = 0;
        int countCorrect = 0;
        for (int i = 0; i < answerLength; i++) {
            if (i < playerAnswer.Length) {
                if (playerAnswer[i] == correctAnswer[i]) {
                    countCorrect++;
                }
            }
            else { break; }
        }
        answerCorrectPercent = (int)Math.Round((100f / answerLength) * countCorrect);
        if (answerCorrectPercent >= 95) {
            tallyModifier = 1;
        }
        else if (answerCorrectPercent < 65) {
            tallyModifier = -1;
        }
        else {
            tallyModifier = 0;
        }
    }

    public int GetAnswerPercentageCorrect() {
        return answerCorrectPercent;
    }

    public string GetResultString() {
        return resultString;
    }

    public void SetResultsData(string answer) {
        SetAnswerResults(answer);
        UpdateWelshSkillsTallies();
        UpdateResultString();
    }

    public void UpdateResultString() {
        if (answerCorrectPercent == 100) {
            resultString = "Perfect!";
        } else if (tallyModifier == 1) {
            resultString = "Success!";
        } else if (tallyModifier == 0) {
            resultString = "Keep working on it!";
        } else if (tallyModifier == -1) {
            resultString = "Whoops!";
        }

    }

    public void UpdateWelshSkillsTallies() {
        if (tallyModifier != 0) {
            if (testType == TestType.read) {
                UpdateReadTally();
                UpdateReadSkillPoints();
            } else if (testType == TestType.write) {
                UpdateWriteTally();
                UpdateWriteSkillPoints();
            }
            UpdateGrammarTallies();
            UpdateGrammarSkillPoints();
        }
        
    }

    private void UpdateReadTally() {
        readTally = int.Parse(DbCommands.GetFieldValueFromTable(
            "DiscoveredVocab",
            "ReadCorrectTallies",
            "SaveIDs = 0 " +
                "AND EnglishText = " + DbCommands.GetParameterNameFromValue(vocab[0]) + " " +
                "AND WelshText = " + DbCommands.GetParameterNameFromValue(vocab[1]),
            vocab[0],
            vocab[1]
            ));
        readTally += tallyModifier;
        tallyShiftTotal += tallyModifier;
        if (readTally >= 0) {
            if (readTally <= highestTallyPossible) {
                DbCommands.UpdateTableField(
                    "DiscoveredVocab",
                    "ReadCorrectTallies",
                    readTally.ToString(),
                    "SaveIDs = 0 " +
                        "AND EnglishText = " + DbCommands.GetParameterNameFromValue(vocab[0]) + " " +
                        "AND WelshText = " + DbCommands.GetParameterNameFromValue(vocab[1]),
                    vocab[0],
                    vocab[1]
                    );
            }
        } else {
            readTally = 0;
        }
    }

    private void UpdateWriteTally() {
        writeTally = int.Parse(DbCommands.GetFieldValueFromTable(
            "DiscoveredVocab",
            "WriteCorrectTallies",
            "SaveIDs = 0 " +
                "AND EnglishText = " + DbCommands.GetParameterNameFromValue(vocab[0]) + " " +
                "AND WelshText = " + DbCommands.GetParameterNameFromValue(vocab[1]),
            vocab[0],
            vocab[1]
            ));
        writeTally += tallyModifier;
        tallyShiftTotal += tallyModifier;
        if (writeTally >= 0) {
            if (writeTally <= highestTallyPossible) {
                DbCommands.UpdateTableField(
                "DiscoveredVocab",
                "WriteCorrectTallies",
                writeTally.ToString(),
                "SaveIDs = 0 " +
                    "AND EnglishText = " + DbCommands.GetParameterNameFromValue(vocab[0]) + " " +
                    "AND WelshText = " + DbCommands.GetParameterNameFromValue(vocab[1]),
                vocab[0],
                vocab[1]
                );
            }
        } else {
            writeTally = 0;
        }
    }

    private void UpdateGrammarTallies() {
        foreach (string[] grammarArray in relatedGrammarList) {
            int grammarID = int.Parse(grammarArray[0]);
            int currentTally = int.Parse(DbCommands.GetFieldValueFromTable(
                "DiscoveredVocabGrammar",
                "CorrectTallies",
                "SaveIDs = 0 " +
                    "AND RuleIDs = " + grammarID.ToString()
            ));

            currentTally += tallyModifier;
            tallyShiftTotal += tallyModifier;
            if (currentTally >= 0) {
                if (currentTally <= highestTallyPossible) {
                    DbCommands.UpdateTableField(
                        "DiscoveredVocabGrammar",
                        "CorrectTallies",
                        currentTally.ToString(),
                        "SaveIDs = 0 " +
                            "AND RuleIDs = " + grammarID.ToString()
                    );
                }
            } else {
                currentTally = 0;
            }
            if (!grammarDetailsDict.ContainsKey(grammarID)) {
                string[] details = new string[2];
                details[0] = currentTally.ToString();
                grammarDetailsDict.Add(grammarID, details);
            }
        }
    }

    private void UpdateReadSkillPoints() {
        
        ReadProficienciesHandler profienciesHandler = new ReadProficienciesHandler(vocab[0],vocab[1]);
        string readTallyProficiency;
        int threshold;
        profienciesHandler.GetProficiencyDetailsFromTally(writeTally, out readTallyProficiency, out threshold);
        int readProficiencyAcquired = DbCommands.GetCountFromTable(
            "AcquiredVocabReadSkills",
            "EnglishText = " + DbCommands.GetParameterNameFromValue(vocab[0]) +
                " AND WelshText = " + DbCommands.GetParameterNameFromValue(vocab[1]) +
                " AND ProficiencyNames = " + DbCommands.GetParameterNameFromValue(readTallyProficiency) +
                " AND SaveIDs = 0",
                vocab[0],
                vocab[1],
                readTallyProficiency
                );
        if (readProficiencyAcquired < 1) {
            if (readTally >= threshold) {
                DbCommands.InsertTupleToTable("AcquiredVocabReadSkills",
                    vocab[0],
                    vocab[1],
                    readTallyProficiency,
                    "0");
                skillPointsGainedTotal++;
                vocabSkillIncremented = true;
                if (highestTallyPossible > readTally) {
                    DbCommands.UpdateTableField("DiscoveredVocab", "ReadCorrectTallies", "0",
                    "SaveIDs = 0 AND EnglishText = " + DbCommands.GetParameterNameFromValue(vocab[0]) + " " +
                    "AND WelshText = " + DbCommands.GetParameterNameFromValue(vocab[1]),
                    vocab[0], vocab[1]);
                }
            }
        }
    }

    private void UpdateWriteSkillPoints() {
        WriteProficienciesHandler profienciesHandler = new WriteProficienciesHandler(vocab[0], vocab[1]);
        string writeTallyProficiency;
        int threshold;
        profienciesHandler.GetProficiencyDetailsFromTally(writeTally, out writeTallyProficiency, out threshold);
        int writeProficiencyAcquired = DbCommands.GetCountFromTable(
            "AcquiredVocabWriteSkills",
            "EnglishText = " + DbCommands.GetParameterNameFromValue(vocab[0]) +
                " AND WelshText = " + DbCommands.GetParameterNameFromValue(vocab[1]) +
                " AND ProficiencyNames = " + DbCommands.GetParameterNameFromValue(writeTallyProficiency) +
                " AND SaveIDs = 0",
            vocab[0],
            vocab[1],
            writeTallyProficiency
            );
        if (writeProficiencyAcquired < 1) {
            if (writeTally >= threshold) {
                DbCommands.InsertTupleToTable("AcquiredVocabWriteSkills",
                    vocab[0],
                    vocab[1],
                    writeTallyProficiency,
                    "0");
                skillPointsGainedTotal++;
                vocabSkillIncremented = true;
                if (highestTallyPossible > writeTally) {
                    DbCommands.UpdateTableField("DiscoveredVocab", "WriteCorrectTallies", "0",
                    "SaveIDs = 0 " +
                    "AND EnglishText = " + DbCommands.GetParameterNameFromValue(vocab[0]) + " " +
                    "AND WelshText = " + DbCommands.GetParameterNameFromValue(vocab[1]),
                    vocab[0], vocab[1]);
                }
            }
        }
    }

    private void UpdateGrammarSkillPoints() {
        
        foreach (KeyValuePair<int, string[]> pair in grammarDetailsDict) {
            int currentTally = int.Parse(pair.Value[0]);
            int threshold;
            string grammarProficiency;
            GrammarProficienciesHandler grammarProficienciesHandler = new GrammarProficienciesHandler(pair.Key.ToString());
            grammarProficienciesHandler.GetProficiencyDetailsFromTally(currentTally, out grammarProficiency, out threshold);
            int grammarProficiencyAcquired = DbCommands.GetCountFromTable(
                "AcquiredGrammarSkills",
                "RuleIDs = " + pair.Key.ToString() +
                    " AND ProficiencyNames = " + DbCommands.GetParameterNameFromValue(grammarProficiency) +
                    " AND SaveIDs = 0",
                grammarProficiency);
            if (grammarProficiencyAcquired < 1) {
                if (currentTally >= threshold) {
                    DbCommands.InsertTupleToTable("AcquiredGrammarSkills",
                        pair.Key.ToString(),
                        grammarProficiency,
                        "0");
                    skillPointsGainedTotal++;
                    pair.Value[1] = "+";
                    if (highestTallyPossible > currentTally) {
                        DbCommands.UpdateTableField("DiscoveredVocabGrammar", "CorrectTallies", "0",
                        "SaveIDs = 0 " +
                        "AND RuleIDs = " + pair.Key.ToString() +
                        ";");
                        pair.Value[0] = "0";
                    }
                }
            }
        } 
    }



    public int GetTallyShiftTotal() {
        return tallyShiftTotal;
    }

    public string GetTestTypeString() {
        return testType.ToString();
    }

    public TestType GetTestType() {
        return testType;
    }

    public int GetSkillPointsGainedTotal() {
        return skillPointsGainedTotal;
    }

    public string GetPlayerAnswer() {
        return playerAnswer;
    }

    public string[] GetVocabUpdateData() {
        string[] data = new string[3];
        data[0] = vocab[0];
        data[1] = vocab[1];
        data[2] = (testType == TestType.read) ? readTally.ToString() : writeTally.ToString();
        return data;
    }

    public int GetTallyModifier() {
        return tallyModifier;
    }

    public bool IsVocabSkillIncremented() {
        return vocabSkillIncremented;
    }

    public Dictionary<int, string[]> GetGrammarUpdateDict() {
        return grammarDetailsDict;
    }
}




