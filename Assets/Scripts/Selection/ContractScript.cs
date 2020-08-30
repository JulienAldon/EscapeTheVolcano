using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContractScript : MonoBehaviour
{
    string need = "";
    int crystalNeed;
    int timeIn;
    int reward;
    int timeInExtra = 0;
    int crystalNeedExtra = 0;
    string time = "";
    string mod = "";
    public string[] modifiers;

    public TextMeshProUGUI displayNeed;
    public TextMeshProUGUI displayIn;
    public TextMeshProUGUI displayMod;
    public TextMeshProUGUI displayReward;
    public int contractLevel;
    // Start is called before the first frame update
    void Start()
    {
        createContract(contractLevel);
        Debug.Log("\t" + need + ((crystalNeedExtra == 0) ? (crystalNeed.ToString()) : (crystalNeed.ToString() + " and " + crystalNeedExtra.ToString())) + " crystals\n\t" + time + ((timeInExtra == 0) ? (timeIn.ToString()) : (timeIn.ToString() + " and " + timeInExtra.ToString())) + " minutes." + mod + ".");
        displayNeed.text = need + ((crystalNeedExtra == 0) ? (crystalNeed.ToString()) : (crystalNeed.ToString() + " and " + crystalNeedExtra.ToString())) + " crystals."; 
        if (time != "")
            displayIn.text = time + ((timeInExtra == 0) ? (timeIn.ToString()) : (timeIn.ToString() + " and " + timeInExtra.ToString())) + " minutes.";
        else 
            displayIn.text = "";
        if (time != "")
           displayMod.text = mod + ".";
        else
           displayMod.text = "";        
        displayReward.text = "1 crystal\n" + (150 * reward).ToString() + "$";
    }
    
    void createContract(int level)
    {
        
        int choiceNeed = Random.Range(0, 3);
        need = choiceNeed == 0 ? "Less than " : choiceNeed == 1 ? "More than " : "Between ";
        crystalNeed = choiceNeed == 0 ? Random.Range(5, 10) : choiceNeed == 1 ? Random.Range(7, 15) : 0;
        if (crystalNeed == 0) {
            crystalNeed = Random.Range(5, 10);
            crystalNeedExtra = Random.Range(10, 15);
        }
        if (level == 1) {
            reward = choiceNeed == 0 ? 3 : choiceNeed == 1 ? 2 : 2;
            return;
        }
        int choiceIn = Random.Range(0, 3);
        time = choiceIn == 0 ? "Less than " : choiceIn == 1 ? "More than " : "Between ";
        timeIn = choiceIn == 0 ? Random.Range(3, 4) : choiceIn == 1 ? Random.Range(2, 4) : 0;
        if (timeIn == 0) {
            timeIn = Random.Range(2, 3);
            timeInExtra = Random.Range(3, 4);
        }
        if (level == 2) {
            reward = choiceNeed == 0 ? 4 : choiceNeed == 1 ? 2 : 3;
            return;
        }
        mod = modifiers[Random.Range(0, modifiers.Length)];
        reward = choiceNeed == 0 ? 5 : choiceNeed == 1 ? 3 : 4;

    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
