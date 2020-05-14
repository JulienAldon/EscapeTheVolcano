using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    public GameObject colorBlindEffect;
    public GameObject BlindEffect;
    public GameObject ParanoidEffect;
    public GameObject Weapon;
    public GameObject blood; 
    public GameObject fetard;
    public Animator anim;
    public GameObject gfx;
    public Material matWhite;

    private Material matDefault;

	private Shake shake;
    public int maxHealth;
    public int currentHealth {get; private set;}

    public int currentChar = 0;

    public string currentAffliction;
    public Stat Speed;
    public Stat ClassType;
    public int nbFlags;
    public GameObject confettisLeft;
    public GameObject confettisRight;
    public GameObject normalLeft;
    public GameObject normalRight;
    public GameObject[] interfaceTeam;

    private bool damaged;
    public float damageRate;
    private float nextDamage = 0;
    public static int nbCrystals;
    /*
        1 - Runner
        2 - Climber
        3 - Hacker
        4 - Tracker
        5 - Tank
        6 - Grenadier
    */

    void UpdateStats()
    {
        maxHealth = Team.team[currentChar].life;
        currentHealth = Team.team[currentChar].currentHealth;
        nbFlags = Team.team[currentChar].nbFlags;
        Speed.SetValue(Team.team[currentChar].speed);
        if (Team.team[currentChar].archetype == "Runner")
            ClassType.SetValue(1);
        else if (Team.team[currentChar].archetype == "Climber")
            ClassType.SetValue(2);
        else if (Team.team[currentChar].archetype == "Hacker")
            ClassType.SetValue(3);
        else if (Team.team[currentChar].archetype == "Tracker")
            ClassType.SetValue(4);
        else if (Team.team[currentChar].archetype == "Tank")
            ClassType.SetValue(5);
        else if (Team.team[currentChar].archetype == "Grenadier")
            ClassType.SetValue(6);
        
        TraitUpdate();

        Text life = GameObject.Find("LifeText").GetComponent<Text>();
        life.text = currentHealth.ToString();
        StartCoroutine(ChangeColor());
    }

    IEnumerator ChangeColor()
    {
        yield return new WaitForSeconds(0.8f);
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Team.team[currentChar].color;
    }

    void Awake()
    {
		shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();
        UpdateStats();
        int i = 0;
        foreach (var member in interfaceTeam)
        {
            member.GetComponent<ArchetypeInterface>().archetype = Team.team[i].archetype;
            i+=1;
        }
        interfaceTeam[currentChar].GetComponent<ArchetypeInterface>().isSelected = true;        
        currentHealth = maxHealth;
        Text life = GameObject.Find("LifeText").GetComponent<Text>();
        life.text = currentHealth.ToString();
    }
    
    void Start()
    {
        matDefault = gfx.GetComponent<SpriteRenderer>().material;        
    }

    public void CharacterSwitch()
    {
        anim.SetTrigger("Switch");
        GetComponent<TestController>().Shield.SetActive(false);
        if (currentChar >= Team.team.Length)
            currentChar = 0;
        interfaceTeam[currentChar].GetComponent<ArchetypeInterface>().isSelected = false;
        currentChar += 1;
        if (currentChar >= Team.team.Length)
            currentChar = 0;
        interfaceTeam[currentChar].GetComponent<ArchetypeInterface>().isSelected = true;
        UpdateStats();

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyBindScript.keys["Switch"])) {
            CharacterSwitch();
        }
        if (damaged && Time.time > nextDamage)
        {
            nextDamage = Time.time + damageRate;
            damaged = false;
        }
    }

    public void TakeDamage(int damage)
    {
        if (damaged)
            return;
        gfx.GetComponent<SpriteRenderer>().material = matWhite;
        Invoke("ResetMaterial", .2f);
        shake.camShake();
        anim.SetTrigger("Hit");
        currentHealth -= damage;
        Team.team[currentChar].currentHealth -= 1;
        Text life = GameObject.Find("LifeText").GetComponent<Text>();
        life.text = currentHealth.ToString();
        damaged = true;
        if (currentHealth <= 0) {
            Die();
            return;
        }
    }
    
    void ResetMaterial()
    {
        gfx.GetComponent<SpriteRenderer>().material = matDefault;
    }
    
    public void Die() 
    {
        // switch and supress char from team
        // delete
        interfaceTeam[currentChar].GetComponent<ArchetypeInterface>().isDead = true;
        if (Team.team.Length <= 0)
        {
            LavaDie();
            return;
        }
//        CharacterSwitch();
        GetComponent<TestController>().Shield.SetActive(false);
        interfaceTeam[currentChar].GetComponent<ArchetypeInterface>().isSelected = false;
        var list = new List<Character>(Team.team);
        list.Remove(Team.team[currentChar]);
        Team.team = list.ToArray();
        //interfaceTeam[currentChar].GetComponent<ArchetypeInterface>().isSelected = false;
        var list2 = new List<GameObject>(interfaceTeam);
        list2.Remove(interfaceTeam[currentChar]);
        interfaceTeam = list2.ToArray();
       
        // switch
        anim.SetTrigger("Switch");        
        currentChar+=1;
        if (currentChar >= Team.team.Length)
            currentChar = 0;
        interfaceTeam[currentChar].GetComponent<ArchetypeInterface>().isSelected = true;

        StartCoroutine(Death());
        // Supress team member display
        // Make cool thing to say the player is dead
    }

    IEnumerator Death()
    {
        shake.camShake();
        GameObject a = Instantiate(blood, transform.position, Quaternion.identity);        
        var main = a.GetComponent<ParticleSystem>().main;
        if (Team.team.Length <= 0)
        {
            LavaDie();
        }
        main.startColor = Team.team[currentChar].color;
        Time.timeScale = 0.1f; 
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 1;
    }
    
    public void LavaDie()
    {
        // Game over
        GameObject.Find("LevelLoader").GetComponent<LoadingLevel>().LoadGameOverScene();
        print("player died no switch possible Game Over");
        // Trigger Gameover scene
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 23)
        {
            nbCrystals += 1;
            print(nbCrystals);
        }
    }

    void TraitUpdate()
    {
        if (Team.team[currentChar].trait == "ColorBlind") {
            Weapon.SetActive(true);
            ParanoidEffect.SetActive(false);
            BlindEffect.SetActive(false);
            colorBlindEffect.SetActive(true);
            GetComponent<Rigidbody2D>().gravityScale = 2.35f;
            GetComponent<TestController>().BulletLeft = normalLeft;
            GetComponent<TestController>().BulletRight = normalRight;
            fetard.SetActive(false);
            currentAffliction = "ColorBlind";
        } else if (Team.team[currentChar].trait == "Blind") {
            Weapon.SetActive(true);
            ParanoidEffect.SetActive(false);
            BlindEffect.SetActive(true);
            colorBlindEffect.SetActive(false);
            GetComponent<Rigidbody2D>().gravityScale = 2.35f;
            GetComponent<TestController>().BulletLeft = normalLeft;
            GetComponent<TestController>().BulletRight = normalRight;
            fetard.SetActive(false);
            currentAffliction = "Blind";            
        } else if (Team.team[currentChar].trait == "Paranoid") {
            Weapon.SetActive(true);            
            ParanoidEffect.SetActive(true);
            BlindEffect.SetActive(false);
            colorBlindEffect.SetActive(false);
            GetComponent<Rigidbody2D>().gravityScale = 2.35f;
            GetComponent<TestController>().BulletLeft = normalLeft;
            GetComponent<TestController>().BulletRight = normalRight;
            fetard.SetActive(false);
            currentAffliction = "Paranoid";            
        } else if (Team.team[currentChar].trait == "Normal") {
            Weapon.SetActive(true);
            ParanoidEffect.SetActive(false);
            BlindEffect.SetActive(false);
            colorBlindEffect.SetActive(false);
            GetComponent<Rigidbody2D>().gravityScale = 2.35f;
            GetComponent<TestController>().BulletLeft = normalLeft;
            GetComponent<TestController>().BulletRight = normalRight;
            fetard.SetActive(false);
            currentAffliction = "Normal";            
        } else if (Team.team[currentChar].trait == "Pacifist") {
            // no weapons
            ParanoidEffect.SetActive(false);
            BlindEffect.SetActive(false);
            colorBlindEffect.SetActive(false);
            Weapon.SetActive(false);
            GetComponent<Rigidbody2D>().gravityScale = 2.35f;
            GetComponent<TestController>().BulletLeft = normalLeft;
            GetComponent<TestController>().BulletRight = normalRight;
            fetard.SetActive(false);
            currentAffliction = "Pacifist";            
        } else if (Team.team[currentChar].trait == "Astronaut") {
            // no gravity / less gravity
            ParanoidEffect.SetActive(false);
            BlindEffect.SetActive(false);
            colorBlindEffect.SetActive(false);         
            Weapon.SetActive(true);
            GetComponent<TestController>().BulletLeft = normalLeft;
            GetComponent<TestController>().BulletRight = normalRight;
            fetard.SetActive(false);
            currentAffliction = "Astronaut";
            GetComponent<Rigidbody2D>().gravityScale = 1f;
        } else if (Team.team[currentChar].trait == "Partygoer") {
            // fetard
            ParanoidEffect.SetActive(false);
            BlindEffect.SetActive(false);
            colorBlindEffect.SetActive(false);
            Weapon.SetActive(true);
            GetComponent<Rigidbody2D>().gravityScale = 2.35f;
            GetComponent<TestController>().BulletLeft = confettisLeft;
            GetComponent<TestController>().BulletRight = confettisRight;
            fetard.SetActive(true);
            currentAffliction = "Partygoer";
        } else if (Team.team[currentChar].trait == "Coprolalia") {
            // insults on hit
            ParanoidEffect.SetActive(false);
            BlindEffect.SetActive(false);
            colorBlindEffect.SetActive(false);
            Weapon.SetActive(true);
            GetComponent<Rigidbody2D>().gravityScale = 2.35f;
            GetComponent<TestController>().BulletLeft = normalLeft;
            GetComponent<TestController>().BulletRight = normalRight;
            fetard.SetActive(false);
            currentAffliction = "Coprolalia";            
        } else if (Team.team[currentChar].trait == "I.B.S") {
            // Irritable bowel syndrome caca partout
            ParanoidEffect.SetActive(false);
            BlindEffect.SetActive(false);
            colorBlindEffect.SetActive(false);
            Weapon.SetActive(true);
            GetComponent<Rigidbody2D>().gravityScale = 2.35f;
            GetComponent<TestController>().BulletLeft = normalLeft;
            GetComponent<TestController>().BulletRight = normalRight;
            fetard.SetActive(false);
            currentAffliction = "I.B.S";            
        } else if (Team.team[currentChar].trait == "Fat") {
            // jump less
            ParanoidEffect.SetActive(false);
            BlindEffect.SetActive(false);
            colorBlindEffect.SetActive(false);
            Weapon.SetActive(true);
            GetComponent<Rigidbody2D>().gravityScale = 3.5f;
            GetComponent<TestController>().BulletLeft = normalLeft;
            GetComponent<TestController>().BulletRight = normalRight;
            fetard.SetActive(false);
            currentAffliction = "Fat";            
        } else if (Team.team[currentChar].trait == "Sissy") {
            // auto switch on hit
            ParanoidEffect.SetActive(false);
            BlindEffect.SetActive(false);
            colorBlindEffect.SetActive(false);
            Weapon.SetActive(true);
            GetComponent<Rigidbody2D>().gravityScale = 2.35f;
            GetComponent<TestController>().BulletLeft = normalLeft;
            GetComponent<TestController>().BulletRight = normalRight;
            fetard.SetActive(false);
            currentAffliction = "Sissy";
        }
    }
}
