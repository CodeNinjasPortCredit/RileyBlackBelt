using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class main_menu : MonoBehaviour
{
    public Button HowToPlay;
    public Button Play;
    public Button Credits;
    public Button RangerSelect;
    public Button FighterSelect;
    public Button WizardSelect;
    public Button Back;
    public GameObject HowToPlayPanel;
    public GameObject CreditPanel;
    public GameObject DialogBox;
    public GameObject PauseScreen;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseScreen != null) {
                PauseGame();
            }
        }
    }
    public static void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public static void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void PauseGame()
    {
        PauseScreen.SetActive(true);
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
