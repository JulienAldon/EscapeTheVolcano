using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContractValidation : MonoBehaviour
{
    public TextMeshProUGUI crystals;
    public TextMeshProUGUI rewardText;
    public TextMeshProUGUI totalRewardText;
    public TextMeshProUGUI timeText;
    private List<string> objectives = new List<string>();
    private List<bool> validation = new List<bool>();

    public TextMeshProUGUI[] objectivesText;
    public GameObject[] validationTicks;
    public GameObject[] objectivesObject;
    // Start is called before the first frame update
    void Start()
    {
        crystals.text = CharacterStats.nbCrystals.ToString() + " crystals";
        float t = Timer.endTime;
        string minutes = ((int) t / 60).ToString();
        string seconds = (t % 60).ToString("f2");
        timeText.text = minutes + ":" + seconds;
        if (Team.currentContract.typeCrystalContract == "less") {
            objectives.Add("Get less than " + Team.currentContract.crystals[0].ToString() + " crystals.");
            if (Team.currentContract.crystals[0] > CharacterStats.nbCrystals) { 
                validation.Add(true);
            } else {
                validation.Add(false);
            }
        } else if (Team.currentContract.typeCrystalContract == "more") {
            objectives.Add("Get more than " + Team.currentContract.crystals[0].ToString() + " crystals.");
            if (Team.currentContract.crystals[0] < CharacterStats.nbCrystals) {
                validation.Add(true);
            } else {
                validation.Add(false);
            }
        } else if (Team.currentContract.typeCrystalContract == "between") {
            objectives.Add("Get between " + Team.currentContract.crystals[0].ToString() + " and " + Team.currentContract.crystals[1] + " crystals.");
            if (Team.currentContract.crystals[0] > CharacterStats.nbCrystals 
                && Team.currentContract.crystals[1] < CharacterStats.nbCrystals) {
                validation.Add(true);
            } else {
                validation.Add(false);
            }
        }

        if (Team.currentContract.typeTimeContract == "less") {
            objectives.Add("Get back in less than " + Team.currentContract.time[0].ToString() + " minutes.");
            if (Team.currentContract.time[0] > CharacterStats.nbCrystals) {
                validation.Add(true);
            } else {
                validation.Add(false);
            }
        } else if (Team.currentContract.typeTimeContract == "more") {
            objectives.Add("Get back in more than " + Team.currentContract.time[0].ToString() + " minutes.");
            if (Team.currentContract.time[0] < CharacterStats.nbCrystals) {
                validation.Add(true);
                
            } else {
                validation.Add(false);

            }
        } else if (Team.currentContract.typeTimeContract == "between") {
            objectives.Add("Get back between " + Team.currentContract.time[0].ToString() + " and " + Team.currentContract.time[1].ToString() +  " minutes.");
            if (Team.currentContract.time[0] > CharacterStats.nbCrystals 
                && Team.currentContract.time[1] < CharacterStats.nbCrystals) {
                validation.Add(true);
            } else {
                validation.Add(false);
            }
        } else {
            // no time objective
        }
        foreach (var mod in Team.currentContract.modifiers) {
            if (mod == "No Kill") {
                // Add line
                
                objectives.Add("No Kill");
                if (Team.blobKilled + Team.batKilled + Team.golemKilled == 0) {
                    validation.Add(true);
                } else {
                    validation.Add(false);
                }
                // validate modifiers objectives
            } else if (mod == "Kill All Mobs") {
                // Add line
                objectives.Add("Kill All Mobs");
                if (Team.blobKilled + Team.batKilled + Team.golemKilled == Team.monsterNumber) {
                    validation.Add(true);
                } else {
                    validation.Add(false);
                }
                // validate modifiers objectives
            } else if (mod == "Kill Only Blobs") {
                // Add line
                objectives.Add("Kill Only Blobs");
                if (Team.batKilled + Team.golemKilled == 0) {
                    validation.Add(true);
                } else {
                    validation.Add(false);
                }
                // validate modifiers objectives
            } else if (mod == "Kill Only Golem") {
                // Add line
                objectives.Add("Kill Only Golem");
                if (Team.batKilled + Team.blobKilled == 0) {
                    validation.Add(true);
                } else {
                    validation.Add(false);
                }
                // validate modifiers objectives
            } else if (mod == "Kill Only Bat") {
                // Add line
                objectives.Add("Kill Only Bat");
                if (Team.golemKilled + Team.blobKilled == 0) {
                    validation.Add(true);
                } else {
                    validation.Add(false);
                }
                // validate modifiers objectives
            } else if (mod == "No Robot Lost") {
                // Add line
                objectives.Add("No Robot Lost");
                if (Team.deadCharacters.Count == 0) {
                    validation.Add(true);
                } else {
                    validation.Add(false);
                }
                // validate modifiers objectives
            }    
        }
        int i = 0;
        foreach (var obj in objectives) {
            objectivesText[i].text = obj;
            objectivesObject[i].SetActive(true);
            i ++;
        }
        i = 0;
        int validatedCount = 0;
        foreach (var valid in validation) {
            if (valid) {
                validatedCount++;
                validationTicks[i].SetActive(true);
            } else {
                validationTicks[i].SetActive(false);
            }
            i++;
        }
        rewardText.text = ((Team.currentContract.reward / objectives.Count) * validatedCount).ToString() + " $ / crystals"; // here perks that grant 20%anyway
        totalRewardText.text = "Total : " + (((Team.currentContract.reward / objectives.Count) * validatedCount) * CharacterStats.nbCrystals).ToString() + " $";
    }
}
