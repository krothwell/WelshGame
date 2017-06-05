using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtilities {
    public class AnimationUtilities : MonoBehaviour {
        public static bool HasParameter(string paramName, Animator animator) {
            foreach (AnimatorControllerParameter param in animator.parameters) {
                if (param.name == paramName) return true;
            }
            return false;
        }

        public static void SetParameterIfExists(string paramName, Animator animator, bool switchBool) {
            if (HasParameter(paramName, animator)) {
                animator.SetBool(paramName, switchBool);
            }
        }

        public static void SetTriggerIfExists(string paramName, Animator animator) {
            if (HasParameter(paramName, animator)) {
                animator.SetTrigger(paramName);
            }
        }
    }
}