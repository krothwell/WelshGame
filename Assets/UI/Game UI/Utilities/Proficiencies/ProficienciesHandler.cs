using System.Collections.Generic;
using DbUtilities;

public abstract class ProficienciesHandler {

    protected List<string[]> proficiencyList, acquiredProficiencies;


    public ProficienciesHandler() {
        proficiencyList = new List<string[]>();
        DbCommands.GetDataStringsFromQry(DbQueries.GetWelshThresholdsQry(), out proficiencyList);
    }

    public void GetProficiencyDetailsFromTally(int tally, out string name, out int threshold) {
        name = "";
        threshold = 10;
        for (int i = 0; i < proficiencyList.Count; i++) {
            string[] proficiencyArray = proficiencyList[i];
            threshold = int.Parse(proficiencyArray[1]);
            name = proficiencyArray[0];
            if (threshold >= tally) {
                if (!isProficiencyAcquired(name)) {
                    break;
                }
            }
        }
    }

    public string GetProficiencyNameFromTally(int tallyCorrect) {
        string proficiencyName = "";
        int proficiencyThreshold = 10;
        for (int i = 0; i < proficiencyList.Count; i++) {
            string[] proficiencyArray = proficiencyList[i];
            proficiencyThreshold = int.Parse(proficiencyArray[1]);
            proficiencyName = proficiencyArray[0];
            if (proficiencyThreshold > tallyCorrect) {
                break;
            }
        }
        return proficiencyName;
    }

    public bool isProficiencyAcquired(string name) {
        bool acquired = false;
        foreach (string[] stringArray in acquiredProficiencies) {
            if (stringArray[0] == name) {
                acquired = true;
                break;
            }
        }
        return acquired;
    }

}