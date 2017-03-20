using System;
using UnityEngine;

public class ListDisplayInfo {

    Func<string, string> qryMethod;
    Func<string[], Transform> buildMethod;

    public ListDisplayInfo(Func<string, string> qryMethod, Func<string[], Transform> builder) {
        SetMyQryMethod(qryMethod);
        SetMyBuildMethod(builder);
    }

    private void SetMyQryMethod(Func<string, string> method) {
        qryMethod = method;
    }

    public string GetMySearchQuery(string searchStr) {
        return qryMethod(searchStr);
    }

    public string GetMyDefaultQuery() {
        return qryMethod("");
    }

    private void SetMyBuildMethod(Func<string[], Transform> builder) {
        buildMethod = builder;
    }

    public Func<string[], Transform> GetMyBuildMethod() {
        return buildMethod;
    }
}
