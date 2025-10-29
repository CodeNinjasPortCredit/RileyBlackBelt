using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    public Slider slider;
    public int Health;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        Health = health;
    }

    private void Start()
    {
        
    }

}