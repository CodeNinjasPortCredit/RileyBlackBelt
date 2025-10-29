using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMana : MonoBehaviour
{
    public Slider slider;
    public int Mana;

    public void SetMaxMana(int mana)
    {
        slider.maxValue = mana;
        slider.value = mana;
    }

    public void SetMana(int mana)
    {
        slider.value = mana;
        Mana = mana;
    }
    private IEnumerator coroutine;
    private void Start()
    {
        SetMaxMana(100);
        SetMana(100);

        StartCoroutine("ManaRegenerator", 10);
    }
    
/*    IEnumerator Start()
    {
        StartCoroutine("ManaRegenerator", 10);
        yield return new WaitForSeconds(1.0f);
    }*/

    IEnumerator ManaRegenerator(int number_mana)
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            if (Mana < 100)
            {
                Mana = Mathf.Min(Mana, Mana + number_mana);
                Mana += (number_mana);
            }
            SetMana(Mana);
        }
    }
}