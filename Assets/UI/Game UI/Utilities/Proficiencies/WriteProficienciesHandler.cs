using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DbUtilities;

public class WriteProficienciesHandler : ProficienciesHandler {

    public WriteProficienciesHandler(string vocabEn, string vocabCy) : base() {
        DbCommands.GetDataStringsFromQry(DbQueries.GetWriteSkillAcquiredForVocabQry(vocabEn, vocabCy), out acquiredProficiencies, vocabEn, vocabCy);
    }
}
