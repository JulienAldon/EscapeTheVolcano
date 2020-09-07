using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillTreeManager : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public GameObject currentSelection;

    public void SkillClicked(GameObject skill)
    {
        SkillNode newSkill = skill.GetComponent<SkillNode>();
        title.text = newSkill.title;
        description.text = newSkill.description;
        currentSelection = skill;
    }

    public void SkillUpgrade()
    {
        if (currentSelection) {
            SkillNode newSkill = currentSelection.GetComponent<SkillNode>();
            newSkill.upgraded = true;
            foreach (var n in newSkill.unlocks) {
                n.GetComponent<SkillNode>().seen = true;
            }
        }
    }
}
