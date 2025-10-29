using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public string Class { get; set; }
    public int Level { get; set; } = 1;
    public float Health { get; set; }
    public float Mana { get; set; }
    public float Stamina { get; set; }
    public int Q_dmg { get; set; }
    public int E_dmg { get; set; }
    public int R_dmg { get; set; }
    public float HealthRS { get; set; }
    public float ManaRS { get; set; }
    public float StaminaRS { get; set; }



    public CharacterStats(string characterClass, float health, float mana, float stamina, int qDmg, int eDmg, int rDmg, float healthRS, float manaRS, float staminaRS)
    {
        Class = characterClass;
        Health = health;
        Mana = mana;
        Stamina = stamina;
        Q_dmg = qDmg;
        E_dmg = eDmg;
        R_dmg = rDmg;
        HealthRS = healthRS;
        ManaRS = manaRS;
        StaminaRS = staminaRS;
    }

    // Level up logic
    public virtual void LevelUp()
    {
        Level++;
        Health += 10;
        Stamina += 5;
        Mana += 5;
        Q_dmg += 5;
        E_dmg += 5;
        R_dmg += 5;
        HealthRS += 1;
        ManaRS += 1;
        StaminaRS += 1;
    }

    public void DisplayStats()
    {
        Debug.Log($"Class: {Class}, Level: {Level}, Health: {Health}, Mana: {Mana}, Stamina: {Stamina}");
    }

    public class Fighter : CharacterStats
    {
        public Fighter() : base("Fighter", 100, 0, 100, 10, 10, 10, 2, 0, 1)
        {
            StaminaRS += 5f;
        }

        // public override void LevelUp()
        // {
        //     base.LevelUp();
        //     Health += 10;
        //     Stamina += 10;
        //     Mana += 0;
        //     Q_dmg += 10;
        //     E_dmg += 10;
        //     R_dmg += 10;
        //     HealthRS += 1;
        //     ManaRS += 0f;
        //     StaminaRS += 5f;
        // }
    }

// Wizard Class
    public class Wizard : CharacterStats
    {
        public Wizard() : base("Wizard", 100, 100, 50, 15, 25, 20, 1, 2, 0)
        {
            StaminaRS += 7.5f;
        }

        // public override void LevelUp()
        // {
        //     base.LevelUp();
        //     Health += 5;
        //     Stamina += 7;
        //     Mana += 10; // Wizards get extra mana
        //     Q_dmg += 10;
        //     E_dmg += 10;
        //     R_dmg += 10;
        //     HealthRS += 0.5f;
        //     ManaRS += 1f;
        //     StaminaRS += 0f;
        // }
    }

    // Ranger Class
    public class Ranger : CharacterStats
    {
        public Ranger() : base("Ranger", 100, 50, 50, 6, 10, 0, 1, 2, 2)
        {
            StaminaRS += 7.5f;
            ManaRS += 7.5f;
        }

        // public override void LevelUp()
        // {
        //     base.LevelUp();
        //     Health += 7;
        //     Stamina += 10;
        //     Mana += 5;
        //     Q_dmg += 10;
        //     E_dmg += 10;
        //     R_dmg += 10;
        //     HealthRS += 0.5f;
        //     ManaRS += 1f;
        //     StaminaRS += 500f;
        // }
    }

    // Skeleton Class
    public class Skeleton : CharacterStats
    {
        public Skeleton() : base("Skeleton", 35, 0, 0, 10, 0, 0, 2.5f, 0, 0)
        {
        }
    }
}
