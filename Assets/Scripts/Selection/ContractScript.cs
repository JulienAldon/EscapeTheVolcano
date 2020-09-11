using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContractScript : MonoBehaviour
{
    bool crystalRolled = false;
    bool timeRolled = false;
    public int reward;
    public string[] modifiers;
    public List<string> objectives;
    public TextMeshProUGUI[] displayObjectives;
    public TextMeshProUGUI displayReward;
    public int contractLevel;
    public string typeCrystalContract;
    public string typeTimeContract; // less more between none
    public List<int> currentCrystals;
    public List<int> currentTime;
    public List<string> currentModifiers;

    // Start is called before the first frame update
    void Start()
    {
        contractLevel = Random.Range(1, 3); // Add 1 with the skill tree talent
        // contractLevel = 3;
        createContract(contractLevel);
        int i = 0;
        foreach(var a in objectives) {
            displayObjectives[i].text = a;
            i++;
        }
        reward = reward * 150;    
        displayReward.text = "1 crystal\n" + reward.ToString() + "$";
    }
    
    void createContract(int level)
    {
        int choiceModifier;
        int choiceNeed;
        string need;
        int crystalNeed;
        int crystalNeedExtra = 0;
        int choiceIn;
        string time;
        int timeIn;
        int timeInExtra = 0;
        int choiceRandomModifier;
        for (int i = 0; i < level; i++) {
            choiceModifier = Random.Range(0, 3);
            if (crystalRolled) 
                choiceModifier = Random.Range(1, 3);
            if (timeRolled)
                choiceModifier = Random.Range(2, 3);
            if (choiceModifier == 0) {
                choiceNeed = Random.Range(0, 3);
                need = choiceNeed == 0 ? "Less than " : choiceNeed == 1 ? "More than " : "Between ";
                typeCrystalContract = choiceNeed == 0 ? "less" : choiceNeed == 1 ? "more" : "between";
                crystalNeed = choiceNeed == 0 ? Random.Range(5, 10) : choiceNeed == 1 ? Random.Range(7, 15) : 0;
                if (crystalNeed == 0) {
                    crystalNeed = Random.Range(5, 10);
                    crystalNeedExtra = Random.Range(10, 15);
                    currentCrystals.Add(crystalNeed);
                    currentCrystals.Add(crystalNeedExtra);
                }
                currentCrystals.Add(crystalNeed);
                objectives.Add(need + ((crystalNeedExtra == 0) ? (crystalNeed.ToString()) : (crystalNeed.ToString() + " and " + crystalNeedExtra.ToString())) + " crystals.");
                crystalRolled = true;
            } else if (choiceModifier == 1) {
                choiceIn = Random.Range(0, 3);
                time = choiceIn == 0 ? "Less than " : choiceIn == 1 ? "More than " : "Between ";
                typeTimeContract = choiceIn == 0 ? "less" : choiceIn == 1 ? "more" : "between";
                timeIn = choiceIn == 0 ? Random.Range(3, 4) : choiceIn == 1 ? Random.Range(2, 4) : 0;
                if (timeIn == 0) {
                    timeIn = Random.Range(2, 3);
                    timeInExtra = Random.Range(3, 4);
                    currentTime.Add(timeIn);
                    currentTime.Add(timeInExtra);
                }
                currentTime.Add(timeIn);
                objectives.Add(time + ((timeInExtra == 0) ? (timeIn.ToString()) : (timeIn.ToString() + " and " + timeInExtra.ToString())) + " minutes.");
                timeRolled = true;
            } else {
                choiceRandomModifier = Random.Range(0, modifiers.Length);
                if (objectives.Contains("No Kill")) {
                    choiceRandomModifier = Random.Range(5, modifiers.Length);
                }
                if (objectives.Contains("Kill All Mobs")) {
                    choiceRandomModifier = Random.Range(5, modifiers.Length);
                }
                if (objectives.Contains("Kill Only Blobs")) {
                    choiceRandomModifier = Random.Range(3, modifiers.Length);
                }
                if (objectives.Contains("Kill Only Golem")) {
                    choiceRandomModifier = Random.Range(4, modifiers.Length);
                }
                if (objectives.Contains("Kill Only Bat")) {
                    choiceRandomModifier = Random.Range(5, modifiers.Length);   
                }

                if (!objectives.Contains(modifiers[choiceRandomModifier])) {
                    objectives.Add(modifiers[choiceRandomModifier]);
                    currentModifiers.Add(modifiers[choiceRandomModifier]);
                }
            }
        }
        if (!crystalRolled) {
            typeCrystalContract = "none";
        }
        if (!timeRolled) {
            typeTimeContract = "none";
        }
        
        if (level == 1) {
            reward = Random.Range(1, 3);
        } else if (level == 2) {
            reward = Random.Range(2, 4);
        } else if (level == 3) {
            reward = Random.Range(3, 5);
        }

    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
