using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnityUtilities {
    public static class HeirarchyTraversal {
        public static string GetPath(this Transform current) {
            if (current.parent == null)
                return "/" + current.name;
            return current.parent.GetPath() + "/" + current.name;
        }
    }
}