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
    public TextMesh efficiency;

    public string[] archetypes;
    public string[] traits;
    public GameObject player;
    public GameObject gameContinue;
    public UnityEngine.Experimental.Rendering.Universal.Light2D middleLight = null;

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
        efficiency.text = currentCharacter.efficiency < 5 ? "COMMON": currentCharacter.efficiency >= 5 && currentCharacter.efficiency <= 9 ? "RARE": "LEGENDARY";
        efficiency.color = currentCharacter.efficiency < 5 ? new Color32(65, 65, 65, 255) : currentCharacter.efficiency >= 5 && currentCharacter.efficiency <= 9 ? new Color32(0, 64, 255, 255) : new Color32(250, 230, 70, 255);     
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
            FindObjectOfType<LoadingLevel>().LoadGameScene();
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
        StartCoroutine(changeColor(new Color32(154,17,0,255)));
        // TODO: add noise
    
    }

    public void setCanPressAgain()
    {
        canPressAgain = true;
    }

    public void regenerateCard()
    {
        currentCharacter = GenerateCharacter(); 
        currentPlayer = Instantiate(player, new Vector3(12f, 0.7f, 0f), Quaternion.identity);
        currentPlayer.transform.parent = transform;
        currentPlayer.transform.localScale = new Vector3(3f, 3f, 0.1f);
        currentPlayer.GetComponent<SpriteRenderer>().color = currentCharacter.color;
        life.text = currentCharacter.life.ToString();
        speed.text = currentCharacter.speed.ToString();
        name.text = currentCharacter.name;
        efficiency.text = currentCharacter.efficiency < 5 ? "COMMON": currentCharacter.efficiency >= 5 && currentCharacter.efficiency <= 9 ? "RARE": "LEGENDARY";
        efficiency.color = currentCharacter.efficiency < 5 ? new Color32(65, 65, 65, 255) : currentCharacter.efficiency >= 5 && currentCharacter.efficiency <= 9 ? new Color32(0, 64, 255, 255) : new Color32(250, 230, 70, 255);        
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

        // change light for 1.5 sec
        StartCoroutine(changeColor(new Color32(50,150,30,255)));
        // TODO: add noise        
    }

    IEnumerator changeColor(Color32 color)
    {
        yield return new WaitForSeconds(.4f);        
        Color32 def = middleLight.color;
        middleLight.color = color;
        yield return new WaitForSeconds(.8f);
        middleLight.color = def;
    }

    Character GenerateCharacter()
    {
        string arch = archetypes[Random.Range(0, archetypes.Length)];
        string trait = traits[Random.Range(0, traits.Length)];
        string name = generateName(5);
        int speed = Random.Range(1, 3);
        int life = Random.Range(3, 5);
        int efficiency = Random.Range(1, 11);
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
        Character carac = new Character(name, arch, speed, life, color, trait, efficiency);
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

