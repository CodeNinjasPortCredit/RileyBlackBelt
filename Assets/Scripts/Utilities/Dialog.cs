using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    List<string> dialog = new List<string>();

    public Button Character_1;
    public Button Character_2;
    public Button Character_3;

    public GameObject Dialog_object;
    private GameObject leftButton;
    private GameObject rightButton;

    private int Counter = 0;
    private bool finishReading = false;
    private int closeRequestId = 0; // Tracks close requests to prevent unwanted deactivation

    // Start is called before the first frame update
    void Start()
    {
        Dialog_object.transform.localScale = new Vector3(0, 0, 0);
        leftButton = Dialog_object.transform.Find("L_button").gameObject;
        rightButton = Dialog_object.transform.Find("R_button").gameObject;

        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            dialog.Add("In the game the three bars are the health bar, the stamina bar, and the mana bar.");
            dialog.Add("The health bar shows how much health you have.");
            dialog.Add("The stamina bar is for some attacks.");
            dialog.Add("The mana bar is for spells.");
            dialog.Add("Q,E,R are for your character's special abilities.");
            dialog.Add("The bar above the enemy's head is their health bar.");
            dialog.Add("Use arrow keys to move.");

            // Used for testing
            //Debug.Log(dialog[0]);
            OpenDialog(1.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Counter == 7)
        {
            finishReading = true;
        }

        if (finishReading)
        {
            CloseDialog(5f);
        }

    }

    public void NextDialog()
    {
        Counter++;
        if (Counter < 7)
        {
            Dialog_object.transform.Find("Dialog Area").gameObject.GetComponent<TMP_Text>().text = dialog[Counter];
            EnableButtons();
        }
    }

    // Will be explained by Sensei Jacky on Feb 9th
    IEnumerator WaitforSeconds(int numberOfSeconds)
    {
        yield return new WaitForSeconds(numberOfSeconds);
    }

    public void ShowDialog(string message, string button1_message, string button2_message, float time)
    {
        closeRequestId++; // Invalidate any pending close requests
        Dialog_object.transform.Find("Dialog Area").gameObject.GetComponent<TMP_Text>().text = message;
        Dialog_object.transform.Find("L_button").Find("Text (TMP)").gameObject.GetComponent<TMP_Text>().text = button1_message;
        Dialog_object.transform.Find("R_button").Find("Text (TMP)").gameObject.GetComponent<TMP_Text>().text = button2_message;
        Dialog_object.SetActive(true);
        Dialog_object.LeanScale(new Vector3(1f, 1f, 1f), time).setEaseOutElastic();

        // Enable both buttons when showing dialog
        EnableButtons();
    }

    public void CharacterSelectionDialog()
    {
        ShowDialog("Are you sure you want this character?", "Yes", "No", 1.5f);
    }

    public void OpenDialog(float time)
    {
        closeRequestId++; // Invalidate any pending close requests
        Dialog_object.transform.Find("Dialog Area").gameObject.GetComponent<TMP_Text>().text = dialog[Counter];
        Dialog_object.SetActive(true);
        Dialog_object.LeanScale(new Vector3(1f, 1f, 1f), time).setEaseOutElastic();

        // Enable both buttons when opening dialog
        EnableButtons();
    }

    public void CloseDialog(float time)
    {
        closeRequestId++; // New close request
        int thisCloseRequestId = closeRequestId;
        Dialog_object.LeanScale(new Vector3(0f, 0f, 0f), time).setEaseOutElastic().setOnComplete(() => SetDialogFalse(thisCloseRequestId));
        Dialog_object.transform.Find("Dialog Area").gameObject.GetComponent<TMP_Text>().text = "";
    }

    void SetDialogFalse(int requestId)
    {
        if (requestId == closeRequestId)
        {
            Dialog_object.SetActive(false);
        }
    }

    public void LoadScene(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }

    public void SetCharacter(int number)
    {
        PlayerPrefs.SetInt("Character", number);
    }

    // Add these new methods to handle button states
    public void DisableButtons()
    {
        leftButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        rightButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        Character_1.interactable = false;
        Character_2.interactable = false;
        Character_3.interactable = false;
    }

    private void EnableButtons()
    {
        leftButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
        rightButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
        Character_1.interactable = true;
        Character_2.interactable = true;
        Character_3.interactable = true;
    }

    // Add these methods to be called by the button click events
    public void OnLeftButtonClick()
    {
        DisableButtons();
        // Add your left button logic here
        CloseDialog(1.5f);
    }

    public void OnRightButtonClick()
    {
        DisableButtons();
        // Add your right button logic here
        CloseDialog(1.5f);
    }


}
