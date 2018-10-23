using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DbUtilities;

public class ReadProficienciesHandler : ProficienciesHandler {

    public ReadProficienciesHandler(string vocabEn, string vocabCy):base() {
        DbCommands.GetDataStringsFromQry(DbQueries.GetReadSkillAcquiredForVocabQry(vocabEn, vocabCy), out acquiredProficiencies, vocabEn, vocabCy); 
    }
}
