using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endOfLevel : MonoBehaviour
{
    void Start() {
        gameObject.SetActive(false);
    }

    public void goToMainMenu() {
        SceneManager.LoadScene("mainMenu");
    }

    //public void endReached() {
        //gameObject.SetActive(true);
    //}

}
