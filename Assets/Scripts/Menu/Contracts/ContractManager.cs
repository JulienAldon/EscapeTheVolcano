using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContractManager : MonoBehaviour
{
   public TextMeshProUGUI reward;
    public TextMeshProUGUI description;
    public GameObject currentContract;
    public GameObject[] allContracts;
    public Color32 selectedColor;
    public Color32 unselectedColor;
    public void ContractClicked(GameObject contract) {
        ContractScript newContract = contract.GetComponent<ContractScript>();
        reward.text = newContract.reward.ToString() + " $";
        currentContract = contract;
        // description.text = 
        description.text = "";
        foreach (var a in newContract.objectives) {
            description.text += "\n" + a;       
        }
    }

    public void ContractSelected() {
        if (currentContract) {
            ContractScript newContract = currentContract.GetComponent<ContractScript>();
            Team.currentContract = new Contract(newContract.contractLevel, newContract.typeCrystalContract, newContract.typeTimeContract, newContract.currentModifiers, newContract.currentCrystals, newContract.currentTime, newContract.reward);
            foreach (var c in allContracts) {
                c.GetComponent<Image>().color = unselectedColor;
            }
            currentContract.GetComponent<Image>().color = selectedColor;
        }
    }

    public void ContractUnselect() {
        description.text = "";
    }
}
