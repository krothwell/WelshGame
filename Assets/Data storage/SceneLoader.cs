using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	public void LoadNextScene() {
		int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;

		SceneManager.LoadScene(activeSceneIndex+1);

	}
	public void LoadSceneByIndex(int i) {
		SceneManager.LoadScene(i);
	}

    public void LoadSceneByName(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public string GetCurrentSceneName() {
        return SceneManager.GetActiveScene().name;
    }
}
