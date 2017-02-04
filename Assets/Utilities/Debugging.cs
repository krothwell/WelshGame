using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DbUtilities;

namespace UnityUtilities {
    public class Debugging : MonoBehaviour {

        public static void PrintListOfStrArrays(List<string[]> strArray) {
            foreach (string[] strItem in strArray) {
                string strRow = "";
                foreach (string str in strItem) {
                    strRow += str + "\t";
                }
                print(strRow);
            }
        }

        public static void PrintDbTable(string tblName) {
            List<string[]> strArray = new List<string[]>();
            DbCommands.GetDbTableListToPrint(tblName, out strArray);
            PrintListOfStrArrays(strArray);
        }
    }
}