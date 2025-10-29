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
    public static void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public static void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
