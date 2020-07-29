using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ArchetypeInterface : MonoBehaviour
{
    public Sprite[] archetypeSprites;

    public Text archetypeText;
    // public Text lifeText;
    public Image archetypeImage;
    public Image selectImage;

    public string archetype;
    public int life;    
    public bool isSelected;
    public bool isDead;
    // Start is called before the first frame update
    void Start()
    {
        UpdateImage();
    }

    void UpdateImage()
    {
        // CharacterStats Character = GameObject.Find("Player").GetComponent<CharacterStats>();
        if (archetype == "Runner") {
            archetypeImage.sprite = archetypeSprites[0];
            archetypeText.text = "Runner";
        } else if (archetype == "Climber") {
            archetypeImage.sprite = archetypeSprites[1];
            archetypeText.text = "Climber";
        } else if (archetype == "Hacker") {
            archetypeImage.sprite = archetypeSprites[2];
            archetypeText.text = "Hacker";
        } else if (archetype == "Tracker") {
            archetypeImage.sprite = archetypeSprites[3];
            archetypeText.text = "Tracker";
        } else if (archetype == "Tank") {
            archetypeImage.sprite = archetypeSprites[4];
            archetypeText.text = "Tank";
        } else if (archetype == "Grenadier") {
            archetypeImage.sprite = archetypeSprites[5];
            archetypeText.text = "Grenadier";
        }
    }

    // void UpdateLife()
    // {
    //     lifeText.text = life.ToString();        
    // }

    // Update is called once per frame
    void Update()
    {
        if (isDead == true) {
            selectImage.color = new Color32(0, 0, 0, 255);
            return;
        }
        if (isSelected == true)
        {
            selectImage.color = new Color32(140, 129, 85, 255);
        } else if (isSelected == false && isDead == false) {
            selectImage.color = new Color32(0, 0, 0, 255);            
        }
        
        // UpdateLife();
    }
}
