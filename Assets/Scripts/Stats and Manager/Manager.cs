using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public string class_name;
    public CharacterStats characterstats_script;

    [Header ("Wizard Attacks")]
    public GameObject Fireball;
    public GameObject Icespear;
    public GameObject Lightning;

    [Header("Ranger Attacks")]
    public GameObject RangerArrow;

    [Header("Stats")]
    public CharacterStats.Wizard wizard_stats;
    public CharacterStats.Ranger ranger_stats;
    public CharacterStats.Fighter fighter_stats;

    // Same thing for the other attacks for the wizard + Ranger + Fighter

    // Start is called before the first frame update
    void Start()
    {
        characterstats_script = gameObject.GetComponent<CharacterStats>();

        CharacterStats.Wizard Wizard = new CharacterStats.Wizard();
        Wizard.DisplayStats();

        //TODO: We will come back to this when working on character selection
        wizard_stats = Wizard;


        CharacterStats.Skeleton Skeleton = new CharacterStats.Skeleton();
        Skeleton.DisplayStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
