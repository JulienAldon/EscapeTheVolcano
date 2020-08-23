using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ArchetypeInterface : MonoBehaviour
{
    public Sprite[] weaponSprites;
    public Sprite[] traitSprites;
    public Sprite[] archetypeSprites;

    public Text archetypeText;
    // public Text lifeText;
    public Image archetypeImage;
    public Image traitImage;
    public Image weaponImage;
    public Image selectImage;

    public string archetype;
    public string trait;
    public string weapon;
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
        if (weapon == "Gatling") {
            weaponImage.sprite = weaponSprites[0];
        } else if (weapon == "Missile") {
            weaponImage.sprite = weaponSprites[1];
        } else if (weapon == "EnergyGun") {
            weaponImage.sprite = weaponSprites[2];
        } else {
            weaponImage.sprite = weaponSprites[3];
        }
        if (trait == "Blind") {
            traitImage.sprite = traitSprites[0];
        } else if (trait == "ColorBlind") {
            traitImage.sprite = traitSprites[1];
        } else if (trait == "Paranoid") {
            traitImage.sprite = traitSprites[2];
        } else if (trait == "Astronaut") {
            traitImage.sprite = traitSprites[3];
        } else if (trait == "Pacifist") {
            traitImage.sprite = traitSprites[4];
        } else if (trait == "Partygoer") {
            traitImage.sprite = traitSprites[5];
        } else if (trait == "Coprolalia") {
            traitImage.sprite = traitSprites[6];
        } else if (trait == "I.B.S") {
            traitImage.sprite = traitSprites[7];
        } else if (trait == "Sissy") {
            traitImage.sprite = traitSprites[8];
        } else if (trait == "Fat") {
            traitImage.sprite = traitSprites[9];
        } else {
            traitImage.sprite = traitSprites[10];
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
