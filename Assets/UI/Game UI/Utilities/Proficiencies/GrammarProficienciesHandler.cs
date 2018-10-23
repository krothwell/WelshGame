using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DbUtilities;

public class GrammarProficienciesHandler : ProficienciesHandler {

    public GrammarProficienciesHandler(string grammarID) : base() {
        DbCommands.GetDataStringsFromQry(DbQueries.GetGrammarSkillAcquiredForVocabQry(grammarID), out acquiredProficiencies);
    }
}
