using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStamina : MonoBehaviour
{
    public Slider slider;
    public int Stamina;

    public void SetMaxStamina(int stamina)
    {
        slider.maxValue = stamina;
        slider.value = stamina;
    }

    public void SetStamina(int stamina)
    {
        slider.value = stamina;
        Stamina = stamina;
    }
    private IEnumerator coroutine;
    private void Start()
    {
        // SetMaxStamina(100);
        // SetStamina(100);

    }

    /*    IEnumerator Start()
        {
            StartCoroutine("ManaRegenerator", 10);
            yield return new WaitForSeconds(1.0f);
        }*/

    // IEnumerator StaminaRegenerator(int number_mana)
    // {
    //     while (true)
    //     {
    //         yield return new WaitForSeconds(1.0f);
    //         if (Stamina < 100)
    //         {
    //             Stamina += (number_mana);
    //         }
    //         if (Stamina > 100)
    //         {
    //             Stamina = 100;
    //         }
    //         SetStamina(Stamina);
    //     }
    // }
}