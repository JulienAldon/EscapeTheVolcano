using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Market : MonoBehaviour
{
    public string[] archetypes;
    public string[] legendaryTraits;
    public string[] rareTraits;
    public string[] traits;
     public string[] legendaryTraitsBin;
    public string[] rareTraitsBin;
    public string[] traitsBin;
    public Color32 runner;
   public Color32 climber;
   public Color32 hacker;
   public Color32 tank;
   public Color32 tracker;
   public Color32 grenadier;
    public CharacterDisplay[] marketList;
    // create set or get it from Team
    void Start() 
    {
        if (Team.market.Count < 4) // market is empty
        {
            for (int i = 0; i < 4; i++) {
                Team.market.Add(GenerateCharacter());
            }
        }
    }

    void Update() 
    {
        int i = 0;
        foreach (Character charac in Team.market)
        {
            marketList[i].changeText(charac);
            i++;
        }
    }

    public void SelectCharacter(int i)
    {
        if (Team.nbSelected > 3) {
            marketList[i].GetComponent<Animator>().SetTrigger("refused");
            return;
        }
        if (Team.money >= Team.market[i].price) {
            Team.team[Team.nbSelected] = Team.market[i];
            Team.nbSelected += 1;
            Team.money -= Team.market[i].price;
            Team.market.RemoveAt(i);
            Team.market.Insert(i, GenerateCharacter());
            marketList[i].GetComponent<Animator>().SetTrigger("select");
        } else {
            marketList[i].GetComponent<Animator>().SetTrigger("refused");
        }
    }

    public void selectBinCharacter() {
        Team.team[Team.nbSelected] = GenerateBinCharacter();
        Team.nbSelected += 1;
    }
    // SelectChar
        // Put the char in the Team.team
        // regenerate a char
    
    // UnselectChar
        // refund
        // display message be carefull : 
        // if you refund this robot it'll no longer be availlable
        // destroy the char
    

    Character GenerateCharacter()
    {
        //efficiency < 5 ? common : efficiency >= 5 && efficiency <= 9 ? Rare : Legendary;
        int efficiencyNum = Random.Range(1, 11);
        string efficiency;
        string arch = archetypes[Random.Range(0, archetypes.Length)];
        string trait;
        if (efficiencyNum < 5) { // Common
            efficiency = "Common";
            trait = traits[Random.Range(0, traits.Length)];
        } else if (efficiencyNum >= 5 && efficiencyNum <= 9) { // TODO skillTree Mod
            efficiency = "Rare";
            trait = rareTraits[Random.Range(0, rareTraits.Length)];
        } else {
            efficiency = "Legendary";
            trait = legendaryTraits[Random.Range(0, legendaryTraits.Length)];
        }
        string name = generateName(5);
        int speed = Random.Range(1, 3);
        int life = Random.Range(3, 5);
        string weaponType = "";
        int weaponChoice;
        if (trait == "Pacifist" || trait == "Partygoer") {
            weaponType = "No Weapon";
        } else {
            if (efficiency == "Legendary")
                weaponChoice = Random.Range(0, 5);
            else 
                weaponChoice = Random.Range(0, 3);            
            if (weaponChoice == 0) {
                weaponType = "Missile";
            } else if (weaponChoice == 1) {
                weaponType = "EnergyGun";
            } else if (weaponChoice >= 2) {
                weaponType = "Gatling";
            }
        }
        Color color;
        if (arch == "Runner") {
            color = runner;
        } else if (arch == "Climber") {
            color = climber;
        } else if (arch == "Hacker") {
            color = hacker;
        } else if (arch == "Tracker") {
            color = tracker;
        } else if (arch == "Tank") {
            color = tank;
        } else { // grenadier
            color = grenadier;
        }
        Character carac = new Character(name, arch, speed, life, color, trait, efficiency, weaponType, "market");
        return carac;
    }

Character GenerateBinCharacter()
    {
        //efficiency < 5 ? common : efficiency >= 5 && efficiency <= 9 ? Rare : Legendary;
        int efficiencyNum = Random.Range(1, 11);
        string efficiency;
        string arch = archetypes[Random.Range(0, archetypes.Length)];
        string trait;
        if (efficiencyNum < 8) { // Common
            efficiency = "Common";
            trait = traitsBin[Random.Range(0, traitsBin.Length)];
        } else if (efficiencyNum >= 8 && efficiencyNum <= 10) { // TODO skilltree mod
            efficiency = "Rare";
            trait = rareTraitsBin[Random.Range(0, rareTraitsBin.Length)];
        } else {
            efficiency = "Legendary";
            trait = legendaryTraitsBin[Random.Range(0, legendaryTraitsBin.Length)];
        }
        string name = generateName(5);
        int speed = Random.Range(1, 3);
        int life = Random.Range(3, 5);
        string weaponType = "";
        int weaponChoice;
        if (trait == "Pacifist" || trait == "Partygoer") {
            weaponType = "No Weapon";
        } else {
            if (efficiency == "Legendary")
                weaponChoice = Random.Range(0, 5);
            else 
                weaponChoice = Random.Range(0, 3);            
            if (weaponChoice == 0) {
                weaponType = "Missile";
            } else if (weaponChoice == 1) {
                weaponType = "EnergyGun";
            } else if (weaponChoice >= 2) {
                weaponType = "Gatling";
            }
        }
        Color color;
        if (arch == "Runner") {
            color = runner;
        } else if (arch == "Climber") {
            color = climber;
        } else if (arch == "Hacker") {
            color = hacker;
        } else if (arch == "Tracker") {
            color = tracker;
        } else if (arch == "Tank") {
            color = tank;
        } else { // grenadier
            color = grenadier;
        }
        Character carac = new Character(name, arch, speed, life, color, trait, efficiency, weaponType, "market");
        return carac;
    }

    public string generateName(int length)
    {
        Random r = new Random();
        string[] consonants = {"b","c","d","f","g","h","j","k","l","m","n","p","q","r","s","sh","z","zh",
        "t","v","w","x","y"};
        string[] vowels = { "a", "e", "i", "o", "u"};
        string stringName;
        string[] arrayName = new string[length];
        bool Cons;
        int a = Random.Range(0, 2);
        if (a == 1) 
            Cons = true; 
        else 
            Cons = false;
        Cons = true;
        int i = 0;
        Cons = false;
        while (i < length)
        {
            if(Cons)
            {
                arrayName[i] = consonants[Random.Range(0, consonants.Length)];
                Cons = false;
                i++;
            }
            else
            {
                arrayName[i] = vowels[Random.Range(0, vowels.Length)];
                Cons = true;
                i++;
            }
        }
        arrayName[0] = arrayName[0].ToUpper();
        stringName = string.Join("", arrayName);
        return stringName;
    }
}
