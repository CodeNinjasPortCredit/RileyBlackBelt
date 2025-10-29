using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;

    [SerializeField]
    [Header("Wizard Sound Effects")]
    public AudioClip wizard_Q;
    public AudioClip wizard_E;
    public AudioClip wizard_R;
    public AudioClip wizard_Walk;
    public AudioClip wizard_Jump;
    public AudioClip wizard_Hurt;
    public AudioClip wizard_Dead;

    [SerializeField]
    [Header("Ranger Sound Effects")]
    public AudioClip ranger_Q;
    public AudioClip ranger_E;
    public AudioClip ranger_R;
    public AudioClip ranger_Walk;
    public AudioClip ranger_Jump;
    public AudioClip ranger_Hurt;
    public AudioClip ranger_Dead;

    [SerializeField]
    [Header("Fighter Sound Effects")]
    public AudioClip fighter_Q;
    public AudioClip fighter_E;
    public AudioClip fighter_R;
    public AudioClip fighter_Walk;
    public AudioClip fighter_Jump;
    public AudioClip fighter_Hurt;
    public AudioClip fighter_Dead;

    [SerializeField]
    [Header("Enemy Eye sound effects")]
    public AudioClip eye_Walk;
    public AudioClip eye_Attack;
    public AudioClip eye_Dead;

    [SerializeField]
    [Header("Enemy Skele sound effects")]
    public AudioClip skele_Walk;
    public AudioClip skele_Attack;
    public AudioClip skele_Dead;

    [SerializeField]
    [Header("Enemy Goblin sound effects")]
    public AudioClip goblin_Walk;
    public AudioClip goblin_Attack;
    public AudioClip goblin_Dead;
         
    [SerializeField]  
    [Header("Enemy Mush sound effects")]
    public AudioClip mush_Walk;
    public AudioClip mush_Attack;
    public AudioClip mush_Dead;
    public AudioClip allHurt;

    [SerializeField]
    [Header("All Boss Sound Effects")]
    public AudioClip ABoss_Attack;
    public AudioClip ABoss_Hurt;
    public AudioClip ABoss_Dead;
    public AudioClip ABoss_Idle;
    public AudioClip ABoss_Run;

    [SerializeField]
    [Header("Fighter Boss Sound Effects")]
    public AudioClip FBoss_Attack;
    public AudioClip FBoss_Hurt;
    public AudioClip FBoss_Dead;
    public AudioClip FBoss_Idle;
    public AudioClip FBoss_Run;

    [SerializeField]
    [Header("Ranger Boss Sound Effects")]
    public AudioClip RBoss_Attack;
    public AudioClip RBoss_Hurt;
    public AudioClip RBoss_Dead;
    public AudioClip RBoss_Idle;
    public AudioClip RBoss_Run;

    [SerializeField]
    [Header("Wizard Boss Sound Effects")]
    public AudioClip WBoss_Attack;
    public AudioClip WBoss_Hurt;
    public AudioClip WBoss_Dead;
    public AudioClip WBoss_Idle;
    public AudioClip WBoss_Run;
    
    [SerializeField]
    [Header("Portal sound effects")]
    public AudioClip portal;

    [SerializeField]
    [Header("Background music")]
    public AudioClip background_Music;

    public SoundManager soundManager; // Assign in Inspector or find in Start()

    // Start is called before the first frame update
    void Start()
    {
        if (soundManager == null)
            soundManager = FindObjectOfType<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayFighterQ() {
        audioSource.PlayOneShot(fighter_Q);
    }
    public void PlayFighterE() {
        audioSource.PlayOneShot(fighter_E);
    }
    public void PlayFighterR() {
        audioSource.PlayOneShot(fighter_R);
    }
    public void PlayFighterWalk() {
        audioSource.PlayOneShot(fighter_Walk);
    }
    public void PlayFighterJump() {
        audioSource.PlayOneShot(fighter_Jump);
    }
    public void PlayFighterHurt() {
        audioSource.PlayOneShot(fighter_Hurt);
    }
    public void PlayFighterDead() {
        audioSource.PlayOneShot(fighter_Dead);
    }
    public void PlayWizardQ() {
        audioSource.PlayOneShot(wizard_Q);
    }
    public void PlayWizardE() {
        audioSource.PlayOneShot(wizard_E);
    }
    public void PlayWizardR() {
        audioSource.PlayOneShot(wizard_R);
        
    }
    public void PlayWizardWalk() {
        audioSource.PlayOneShot(wizard_Walk);
    }
    public void PlayWizardJump() {
        audioSource.PlayOneShot(wizard_Jump);
    }
    public void PlayWizardHurt() {
        audioSource.PlayOneShot(wizard_Hurt);
    }
    public void PlayWizardDead() {
        audioSource.PlayOneShot(wizard_Dead);

    }
    public void PlayRangerQ() {
        audioSource.PlayOneShot(ranger_Q);
    }
    public void PlayRangerE() {
        audioSource.PlayOneShot(ranger_E);
    }
    public void PlayRangerR()
    {
        Invoke("PlayRangerROneTime", 0f);
        Invoke("PlayRangerROneTime", 0.1f);
        Invoke("PlayRangerROneTime", 0.2f);
    }

    public void PlayRangerROneTime()
    {
        audioSource.PlayOneShot(ranger_R);
    }

    public void PlayRangerWalk()
    {
        audioSource.PlayOneShot(ranger_Walk);
    }
    public void PlayRangerJump() {
        audioSource.PlayOneShot(ranger_Jump);
    }
    public void PlayRangerHurt() {
        audioSource.PlayOneShot(ranger_Hurt);
    }
    public void PlayRangerDead() {
        audioSource.PlayOneShot(ranger_Dead);
    }
}
