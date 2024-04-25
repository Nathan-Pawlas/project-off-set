using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenuScript : MonoBehaviour
{

    public void PlayGame() {
        GridController.levelSelector = 0;
        SceneManager.LoadScene("PuzzleScene");
    }

    public void QuitGame() {
        Application.Quit();
    }
}
