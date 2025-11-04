using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour
{
    public GameObject FighterPrefab;
    public GameObject RangerPrefab;
    public GameObject WizardPrefab;

    public Slider HealthSlider;
    public Slider StaminaSlider;
    public Slider ManaSlider;

    public BackgroundController backController;
    public BackgroundController frontController;

    // Start is called before the first frame update
    void Start()
    {
        switch (PlayerPrefs.GetInt("Character"))
        {
            case 0:
                FighterPrefab.SetActive(true);
                HealthSlider.maxValue = 100;
                HealthSlider.value = 100;
                StaminaSlider.maxValue = 100;
                StaminaSlider.value = 100;
                ManaSlider.gameObject.SetActive(false);
                return;
            case 1:
                RangerPrefab.SetActive(true);
                HealthSlider.maxValue = 50;
                HealthSlider.value = 50;
                StaminaSlider.maxValue = 75;
                StaminaSlider.value = 75;
                ManaSlider.maxValue = 75;
                ManaSlider.value = 75;
                return;
            case 2:
                WizardPrefab.SetActive(true);
                HealthSlider.maxValue = 50;
                HealthSlider.value = 50;
                StaminaSlider.maxValue = 50;
                StaminaSlider.value = 50;
                ManaSlider.maxValue = 100;
                ManaSlider.value = 100;
                return;


        }

        Invoke("AssignCameras", 0.5f);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void AssignCameras() {
        if (backController != null)
        {
            backController.cam = GameObject.FindGameObjectWithTag("Player");
        }
        if (frontController != null)
        {
            frontController.cam = GameObject.FindGameObjectWithTag("Player");
        }
    }
}
