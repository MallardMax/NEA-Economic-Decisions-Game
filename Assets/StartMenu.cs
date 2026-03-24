using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // this just loads the unity libraries required for mehods like .loadscene() etc


public class StartMenu : MonoBehaviour {

    public void playGame () { //this function loads the main panel screen, and closes the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainPanel");
    }

    public void settingsMenu () { //this function loads the difficulty screen, and closes the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Settings Menu");
    }

    public void QuitGame() { //see method name
        Debug.Log("the quit game method was called");
        Application.Quit();
    }

    public void TestFunction() {
        Debug.Log("function called");
    }
}