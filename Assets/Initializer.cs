using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Initializer : MonoBehaviour
{
    public GameObject FighterPrefab;
    public GameObject RangerPrefab;
    public GameObject WizardPrefab;

    public Slider HealthSlider;
    public Slider StaminaSlider;
    public Slider ManaSlider;
    


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
                WizardPrefab.SetActive(true);
                HealthSlider.maxValue = 50;
                HealthSlider.value = 50;
                StaminaSlider.maxValue = 50;
                StaminaSlider.value = 50;
                ManaSlider.maxValue = 100;
                ManaSlider.value = 100;
                return;
            case 2:
                RangerPrefab.SetActive(true);
                HealthSlider.maxValue = 50;
                HealthSlider.value = 50;
                StaminaSlider.maxValue = 75;
                StaminaSlider.value = 75;
                ManaSlider.maxValue = 75;
                ManaSlider.value = 75;
                return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
