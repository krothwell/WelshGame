using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityUtilities {
    public class PlayerPrefsManager {
        const string MASTER_VOLUME_KEY = "master_volume";
        const string DIFFICULTY_KEY = "difficulty";
        const string LEVEL_KEY = "level_unlocked_";
        const string SAVE_KEY = "save_game";

        public static void SetSaveGame(int id) {
            PlayerPrefs.SetInt(SAVE_KEY, id);
        }

        public static int GetSaveGame() {
            return PlayerPrefs.GetInt(SAVE_KEY);
        }
        public static void SetMasterVolume(float volume) {
            if (volume > 0f && volume < 1f) {
                PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);
            }
            else {
                Debug.LogError("Mater volume out of range");
            }
        }

        public static float GetMasterVolume() {
            return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY);
        }

        public static void UnlockLevel(int level) {
            if (level <= SceneManager.sceneCountInBuildSettings - 1) {
                PlayerPrefs.SetInt(LEVEL_KEY + level.ToString(), 1); //use 1 for true
            }
            else {
                Debug.LogError("Trying to unlock level not in build order");
            }

        }

        public static bool IsLevelUnlocked(int level) {
            int levelValue = PlayerPrefs.GetInt(LEVEL_KEY + level.ToString());
            bool isLevelUnlocked = (levelValue == 1);

            if (level <= SceneManager.sceneCountInBuildSettings - 1) {
                return isLevelUnlocked;
            }
            else {
                Debug.LogError("level queried not in build");
                return false;
            }
        }

        public static void SetDifficulty(int difficulty) {
            if (difficulty >= 1 && difficulty <= 3) {
                PlayerPrefs.SetInt(DIFFICULTY_KEY, difficulty);
            }
            else {
                Debug.LogError("attempted to set difficulty to "
                                 + difficulty + " but this number should be 1 to 3");
            }
        }

        public static int GetDifficulty() {
            return PlayerPrefs.GetInt(DIFFICULTY_KEY);
        }

    }
}