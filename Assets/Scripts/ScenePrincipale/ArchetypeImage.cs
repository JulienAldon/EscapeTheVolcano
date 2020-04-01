using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ArchetypeImage : MonoBehaviour
{
    public Sprite[] archetypeSprites;
    // Start is called before the first frame update
    void Start()
    {
        UpdateImage();
    }

    void UpdateImage()
    {
        CharacterStats Character = GameObject.Find("Player").GetComponent<CharacterStats>();
        if (Character.ClassType.GetValue() == 1)
            gameObject.GetComponent<Image>().sprite = archetypeSprites[0];
        else if (Character.ClassType.GetValue() == 2)
            gameObject.GetComponent<Image>().sprite = archetypeSprites[1];
        else if (Character.ClassType.GetValue() == 3)
            gameObject.GetComponent<Image>().sprite = archetypeSprites[2];
        else if (Character.ClassType.GetValue() == 4)
            gameObject.GetComponent<Image>().sprite = archetypeSprites[3];
        else if (Character.ClassType.GetValue() == 5)
            gameObject.GetComponent<Image>().sprite = archetypeSprites[4];
        else if (Character.ClassType.GetValue() == 6)
            gameObject.GetComponent<Image>().sprite = archetypeSprites[5];
    }

    // Update is called once per frame
    void Update()
    {
        UpdateImage();
    }
}
