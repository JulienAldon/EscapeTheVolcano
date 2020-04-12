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
    public TextMesh trait;

    public string[] archetypes;
    public string[] traits;
    public GameObject player;
    public GameObject gameContinue;

    public Character currentCharacter {get; private set;}

    private GameObject currentPlayer;
    private bool CanSwitch = false;
    private bool canPressAgain = false;
    private bool once = true;

    void Start()
    {
        currentCharacter = GenerateCharacter();
        life.text = currentCharacter.life.ToString();
        speed.text = currentCharacter.speed.ToString();
        name.text = currentCharacter.name;
        archetype.text = currentCharacter.archetype;
        trait.text = currentCharacter.trait;
        currentPlayer = GameObject.Find("Player");
        
        currentPlayer.GetComponent<SpriteRenderer>().color = currentCharacter.color;        
    }
    
    public bool getCanSwitch()
    {
        return CanSwitch;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyBindScript.keys["Left"]) && CanSwitch == false && canPressAgain == true) {
            canPressAgain = false;
            refuse();   
        } else if (Input.GetKeyDown(KeyBindScript.keys["Right"]) && CanSwitch == false && canPressAgain == true) {
            canPressAgain = false;
            accept();
        } else if (CanSwitch == true && Input.anyKeyDown) {
            gameContinue.active = false;
            print(gameContinue.active);
            FindObjectOfType<LoadingLevel>().LoadNextLevel();
        }
        if (Team.nbSelected == 4 && once)
        {
            once = false;
            gameContinue.active = true;
            CanSwitch = true;
        }
       

    }

    void refuse() {
        currentPlayer.GetComponent<PlayerAnimation>().refuse();        
    }

    public void setCanPressAgain()
    {
        canPressAgain = true;
    }

    public void regenerateCard()
    {
        currentCharacter = GenerateCharacter(); 
        currentPlayer = Instantiate(player, new Vector3(12f, 0.7f, 0f), Quaternion.identity);
        currentPlayer.GetComponent<SpriteRenderer>().color = currentCharacter.color;
        life.text = currentCharacter.life.ToString();
        speed.text = currentCharacter.speed.ToString();
        name.text = currentCharacter.name;
        archetype.text = currentCharacter.archetype;
        trait.text = currentCharacter.trait;
    }

    void accept() {
        Character a = currentCharacter;
        Team.team[Team.nbSelected] = a;
		TeamDisplay t = GameObject.Find("Team").GetComponent<TeamDisplay>();
        t.AddCharacter(Team.nbSelected);
        Team.nbSelected += 1;
        currentPlayer.GetComponent<PlayerAnimation>().accept();
    }

    Character GenerateCharacter()
    {
        string arch = archetypes[Random.Range(0, archetypes.Length)];
        string trait = traits[Random.Range(0, traits.Length)];
        string name = generateName(5);
        int speed = Random.Range(1, 3);
        int life = Random.Range(3, 5);
        Color color;
        if (arch == "Runner") {
            color = new Color32(102,255,80, 255);
        } else if (arch == "Climber") {
            color = new Color32(95,25,255, 255);
        } else if (arch == "Hacker") {
            color = new Color32(255,150,0, 255);
        } else if (arch == "Tracker") {
            color = new Color32(255,255,255, 255);
        } else if (arch == "Tank") {
            color = new Color32(75,55,215, 255);
        } else { // grenadier
            color = new Color32(60,180,255, 255);
        }
        Character carac = new Character(name, arch, speed, life, color, trait);
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

