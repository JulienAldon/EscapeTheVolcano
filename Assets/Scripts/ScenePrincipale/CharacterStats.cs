using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth {get; private set;}

    public int currentChar = 0;

    public Stat Speed;
    public Stat ClassType;
    public int nbFlags;
    /*
        1 - Runner
        2 - Climber
        3 - Hacker
        4 - Tracker
        5 - Tank
        6 - Grenadier
    */

    void UpdateStats()
    {
        maxHealth = Team.team[currentChar].life;
        currentHealth = Team.team[currentChar].currentHealth;
        nbFlags = Team.team[currentChar].nbFlags;
        Speed.SetValue(Team.team[currentChar].speed);
        if (Team.team[currentChar].archetype == "Runner")
            ClassType.SetValue(1);
        else if (Team.team[currentChar].archetype == "Climber")
            ClassType.SetValue(2);
        else if (Team.team[currentChar].archetype == "Hacker")
            ClassType.SetValue(3);
        else if (Team.team[currentChar].archetype == "Tracker")
            ClassType.SetValue(4);
        else if (Team.team[currentChar].archetype == "Tank")
            ClassType.SetValue(5);
        else if (Team.team[currentChar].archetype == "Grenadier")
            ClassType.SetValue(6);
        Text life = GameObject.Find("LifeText").GetComponent<Text>();
        Text archetype = GameObject.Find("ArchetypeText").GetComponent<Text>();
        archetype.text = Team.team[currentChar].archetype;
        life.text = currentHealth.ToString();
    }

    void Awake()
    {
        UpdateStats();
        currentHealth = maxHealth;
        Text life = GameObject.Find("LifeText").GetComponent<Text>();
        life.text = currentHealth.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) {
            currentChar += 1;
            if (currentChar >= 4)
                currentChar = 0;
            UpdateStats();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            Die();
        }
        Team.team[currentChar].currentHealth -= 1;
        Text life = GameObject.Find("LifeText").GetComponent<Text>();
        life.text = currentHealth.ToString();
    }

    public void Die() 
    {
        // switch and supress char from team
        // delete
        var list = new List<Character>(Team.team);
        list.Remove(Team.team[currentChar]);
        Team.team = list.ToArray();
        // switch
        currentChar += 1;
        UpdateStats();
        // Supress team member display
        // Make cool thing to say the player is dead
        print("you died");
    }

    public void LavaDie()
    {
        // Game over
        GameObject.Find("LevelLoader").GetComponent<LoadingLevel>().LoadGameOverScene();
        print("player died in lava not possible to respawn GAMEOVER");
        // Trigger Gameover scene
    }
}
