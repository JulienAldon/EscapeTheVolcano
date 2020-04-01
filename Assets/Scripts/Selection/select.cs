using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class select : MonoBehaviour
{
    public TextMesh life;
    public TextMesh archetype;
    public TextMesh speed;
    public TextMesh name;
    public Animator animator;
    public string[] archetypes;
    public Character currentCharacter {get; private set;}

    private bool CanSwitch = false;

    void Start()
    {
        currentCharacter = GenerateCharacter();
        life.text = currentCharacter.life.ToString();
        speed.text = currentCharacter.speed.ToString();
        name.text = currentCharacter.name;
        archetype.text = currentCharacter.archetype;
    }
    
    public bool getCanSwitch()
    {
        return CanSwitch;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && CanSwitch == false) {
            refuse();   
        } else if (Input.GetKeyDown(KeyCode.RightArrow) && CanSwitch == false) {
            accept();
        }
        if (Team.nbSelected == 4)
        {
            CanSwitch = true;
        }
        if (CanSwitch == true && Input.GetKeyDown(KeyCode.Space)) {
            FindObjectOfType<LoadingLevel>().LoadNextLevel();
        }

    }

    void refuse() {
        animator.SetTrigger("refuse");
    }

    void regenerateCard()
    {
        gameObject.transform.position = new Vector3(-2f, 2.5f, 0);
        gameObject.transform.rotation =  Quaternion.Euler(0, 0, 0);

        currentCharacter = GenerateCharacter(); 
        life.text = currentCharacter.life.ToString();
        speed.text = currentCharacter.speed.ToString();
        name.text = currentCharacter.name;
        archetype.text = currentCharacter.archetype;
        animator.SetTrigger("reset");
    }

    void accept() {
        Character a = currentCharacter;
        Team.team[Team.nbSelected] = a;
		TeamDisplay t = GameObject.Find("Team").GetComponent<TeamDisplay>();
        t.AddCharacter(Team.nbSelected);
        animator.SetTrigger("accept");
        Team.nbSelected += 1;
    }

    Character GenerateCharacter()
    {
        string arch = archetypes[Random.Range(0, archetypes.Length)];
        string name = generateName(5);
        int speed = Random.Range(1, 3);
        int life = Random.Range(3, 5);
        Character carac = new Character(name, arch, speed, life);
        return carac;
    }

    public string generateName(int length)
        {
            Random r = new Random();
            string[] consonants = {"b","c","d","f","g","h","j","k","l","m","n","p","q","r","s","sh","z","zh",
            "t","v","w","x","y"};
            string[] vowels = { "a", "e", "i", "o", "u"};
            string stringName;
            string[] arrayName = new string[length];
            bool Cons;
            int a = Random.Range(0, 2);
            if (a == 1) 
                Cons = true; 
            else 
                Cons = false;
            Cons = true;
            int i = 0;
            Cons = false;
            while (i < length)
            {
                if(Cons)
                {
                    arrayName[i] = consonants[Random.Range(0, consonants.Length)];
                    Cons = false;
                    i++;
                }
                else
                {
                    arrayName[i] = vowels[Random.Range(0, vowels.Length)];
                    Cons = true;
                    i++;
                }
            }
            arrayName[0] = arrayName[0].ToUpper();
            stringName = string.Join("", arrayName);
            return stringName;
        }
}

