using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillNode : MonoBehaviour
{
    public string title;
    [TextArea]
    public string description;
    [SerializeField]
    Color32 unseenColor;
    [SerializeField]
    Color32 seenColor;
    [SerializeField]
    Color32 upgradedColor;

    public bool seen;
    public bool upgraded;
    public GameObject[] unlocks;
    private Button button;
    private Image image;


    void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }  

    void Update()
    {
        if (seen == false) {
            image.color = unseenColor;
            button.interactable = false;
        } else if (seen == true && upgraded == false) {
            image.color = seenColor;
            button.interactable = true;
        } else if (upgraded == true) {
            image.color = upgradedColor;
            button.interactable = true;
        }
    }
}
