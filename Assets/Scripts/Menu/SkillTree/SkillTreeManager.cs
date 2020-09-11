using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillTreeManager : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI price;
    public TextMeshProUGUI description;
    public GameObject currentSelection;
    public GameObject notEnoughMoney;

    public void SkillClicked(GameObject skill)
    {
        SkillNode newSkill = skill.GetComponent<SkillNode>();
        title.text = newSkill.title;
        description.text = newSkill.description;
        price.text = newSkill.price + "$";
        currentSelection = skill;
    }

    public void SkillUpgrade()
    {
        if (currentSelection) {
            SkillNode newSkill = currentSelection.GetComponent<SkillNode>();
            if (Team.money >= newSkill.price) {
                if (!Team.skills.Contains(newSkill.title)) {
                    newSkill.upgraded = true;
                    Team.skills.Add(newSkill.title);
                    Team.money -= newSkill.price;
                    foreach (var n in newSkill.unlocks) {
                        n.GetComponent<SkillNode>().seen = true;
                    }
                    
                }
            } else {
                notEnoughMoney.GetComponent<Animator>().SetTrigger("open");
            }
        }
    }
}
