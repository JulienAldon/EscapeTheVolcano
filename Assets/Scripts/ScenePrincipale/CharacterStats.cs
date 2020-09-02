using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour {
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
    public int currentHealth { get; private set; }

    public int currentChar = 0;

    public string currentAffliction;
    public Stat Speed;
    public Stat ClassType;
    public int efficiency;
    public GameObject confettisLeft;
    public GameObject confettisRight;
    public GameObject normalLeft;
    public GameObject normalRight;
    public GameObject[] interfaceTeam;
    public GameObject lifeLostParticles;

    private bool damaged;
    public bool damagedInertia;
    public float damageRate;
    private float nextDamage = 0f;
    public static int nbCrystals;

    public int nbFlags;
    public float runner_CDR;
    public float climber_CDR;
    public float grenadier_bombs;
    public float tank_shield;
    public float hacker_time;
	private AudioManager audioManager;
    float knockBackStartTime;
    public float knockBackDuration;
    public Vector2 knockBackSpeed;
    public bool knockBack;
    public float knockBackResistance = 1f;
    /*
        1 - Runner
        2 - Climber
        3 - Hacker
        4 - Tracker
        5 - Tank
        6 - Grenadier
    */
    void Awake () {
        shake = GameObject.FindGameObjectWithTag ("ScreenShake").GetComponent<Shake> ();
        UpdateStats ();
        int i = 0;
        foreach (var member in interfaceTeam) {
            member.GetComponent<ArchetypeInterface> ().archetype = Team.team[i].archetype;
            member.GetComponent<ArchetypeInterface> ().weapon = Team.team[i].weaponType;
            member.GetComponent<ArchetypeInterface> ().trait = Team.team[i].trait;
            i += 1;
        }
        interfaceTeam[currentChar].GetComponent<ArchetypeInterface> ().isSelected = true;
        currentHealth = maxHealth;
    }

    void Start () {
        knockBack = false;
		audioManager = FindObjectOfType<AudioManager> ();                
        matDefault = gfx.GetComponent<SpriteRenderer> ().material;
    }

    void UpdateStats () {
        StartCoroutine (ChangeColor ());
        nbFlags = Team.team[currentChar].nbFlags;
        runner_CDR = Team.team[currentChar].runner_CDR;
        climber_CDR = Team.team[currentChar].climber_CDR;
        grenadier_bombs = Team.team[currentChar].grenadier_bombs;
        tank_shield = Team.team[currentChar].tank_shield;
        hacker_time = Team.team[currentChar].hacker_time;

        maxHealth = Team.team[currentChar].life;
        currentHealth = Team.team[currentChar].currentHealth;
        Speed.SetValue (Team.team[currentChar].speed);
        efficiency = Team.team[currentChar].efficiency;
        if (Team.team[currentChar].archetype == "Runner")
            ClassType.SetValue (1);
        else if (Team.team[currentChar].archetype == "Climber")
            ClassType.SetValue (2);
        else if (Team.team[currentChar].archetype == "Hacker")
            ClassType.SetValue (3);
        else if (Team.team[currentChar].archetype == "Tracker")
            ClassType.SetValue (4);
        else if (Team.team[currentChar].archetype == "Tank")
            ClassType.SetValue (5);
        else if (Team.team[currentChar].archetype == "Grenadier")
            ClassType.SetValue (6);

        TraitUpdate ();

        UpdateLife ();
        UpdatePower ();
    }

    public void UpdatePower () {
        int a = 0;
        foreach (var power in interfaceTeam) {
            for (int i = 0; i < 20; i++) {
                if (Team.team[a].archetype == "Runner") {
                    if (Team.team[a].runner_state > i) {
                        power.transform.GetChild (0).GetChild (0).GetChild (i).gameObject.SetActive (true);
                    } else {
                        power.transform.GetChild (0).GetChild (0).GetChild (i).gameObject.SetActive (false);
                    }
                } else if (Team.team[a].archetype == "Climber") {
                    if (Team.team[a].climber_state > i) {
                        power.transform.GetChild (0).GetChild (0).GetChild (i).gameObject.SetActive (true);
                    } else {
                        power.transform.GetChild (0).GetChild (0).GetChild (i).gameObject.SetActive (false);
                    }
                } else if (Team.team[a].archetype == "Hacker") {
                    if (Team.team[a].hacker_state > i) {
                        power.transform.GetChild (0).GetChild (0).GetChild (i).gameObject.SetActive (true);
                    } else {
                        power.transform.GetChild (0).GetChild (0).GetChild (i).gameObject.SetActive (false);
                    }

                } else if (Team.team[a].archetype == "Tracker") {
                    if (Team.team[a].nbFlags > i) {
                        power.transform.GetChild (0).GetChild (0).GetChild (i).gameObject.SetActive (true);
                    } else {
                        power.transform.GetChild (0).GetChild (0).GetChild (i).gameObject.SetActive (false);
                    }

                } else if (Team.team[a].archetype == "Tank") {
                    if (Team.team[a].tank_state > i) {
                        power.transform.GetChild (0).GetChild (0).GetChild (i).gameObject.SetActive (true);
                    } else {
                        power.transform.GetChild (0).GetChild (0).GetChild (i).gameObject.SetActive (false);
                    }

                } else if (Team.team[a].archetype == "Grenadier") {
                    if (Team.team[a].grenadier_bombs > i) {
                        power.transform.GetChild (0).GetChild (0).GetChild (i).gameObject.SetActive (true);
                    } else {
                        power.transform.GetChild (0).GetChild (0).GetChild (i).gameObject.SetActive (false);
                    }

                }
            }
            a += 1;
        }
    }

    void UpdateLife () {
        int a = 0;
        foreach (var elem in interfaceTeam) {
            for (int i = 0; i < 5; i++) {
                if (Team.team[a].currentHealth > i) {
                    elem.transform.GetChild (2).GetChild (i).gameObject.SetActive (true);
                } else {
                    elem.transform.GetChild (2).GetChild (i).gameObject.SetActive (false);
                }
            }
            a += 1;
        }
    }

    IEnumerator ChangeColor () {
        yield return new WaitForSeconds (0.8f);
        print(Team.team[currentChar].color);
        gfx.GetComponent<SpriteRenderer> ().color = Team.team[currentChar].color;
    }

    public void CharacterSwitch () {
        anim.SetTrigger ("Switch");
        GetComponent<TestController> ().Shield.SetActive (false);
        GetComponent<TestController> ().joint.enabled = false;
        GetComponent<TestController> ().line.enabled = false;
        if (currentChar >= Team.team.Length)
            currentChar = 0;
        interfaceTeam[currentChar].GetComponent<ArchetypeInterface> ().isSelected = false;
        currentChar += 1;
        if (currentChar >= Team.team.Length)
            currentChar = 0;
        interfaceTeam[currentChar].GetComponent<ArchetypeInterface> ().isSelected = true;
        UpdateStats ();

    }
    void Update () {
        if (Input.GetKeyDown (KeyBindScript.keys["Switch"])) {
            CharacterSwitch ();
        }
        if (isAllTeamDead && Input.anyKeyDown) {
            LavaDie ();
        }
        // if (damaged && Time.time > nextDamage) {
        //     nextDamage = Time.time + damageRate;
        //     damaged = false;
        // }
        UpdatePower ();
        CheckKnockBack();
    }

    private void KnockBack(int direction)
    {
        knockBack = true;
        knockBackStartTime = Time.time;
        GetComponent<Rigidbody2D>().velocity = new Vector2(knockBackSpeed.x * knockBackResistance * direction, knockBackSpeed.y * knockBackResistance);
    }

    private void CheckKnockBack()
    {
        if (GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Static) {
            return ;
        }
        if (Time.time >= knockBackStartTime + knockBackDuration && knockBack) {
            knockBack = false;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f,  GetComponent<Rigidbody2D>().velocity.y);
        }
    }
    public void TakeDamage (int damage, int _direction) {
        if (damaged)
            return;
        StartCoroutine( Damaged ());
        gfx.GetComponent<SpriteRenderer> ().material = matWhite;
        Invoke ("ResetMaterial", 1f);
        KnockBack(_direction);
        shake.camShake ();
        anim.SetTrigger ("Hit");
        currentHealth -= damage;
        Team.team[currentChar].currentHealth -= 1;

        UpdateLife ();
        if (currentHealth <= 0) {
            Die ();
            return;
        }
    }

    IEnumerator Damaged() {
        damaged = true;
        gameObject.layer = 15; // change layer to shell to be untouchable
        yield return new WaitForSeconds (1);
        gameObject.layer = 10; 
        damaged = false;
    }
    void ResetMaterial () {
        gfx.GetComponent<SpriteRenderer> ().material = matDefault;
    }
    bool isAllTeamDead = false;
    public void Die () {
        // switch and supress char from team
        // delete
        Team.team[currentChar].currentHealth = 0;
        UpdateStats ();
        interfaceTeam[currentChar].GetComponent<ArchetypeInterface> ().isDead = true;
        if (Team.team.Length <= 0) {
            StartCoroutine (FinalDeath ());
            return;
        }
        GetComponent<TestController> ().Shield.SetActive (false);
        interfaceTeam[currentChar].GetComponent<ArchetypeInterface> ().isSelected = false;
        var list = new List<Character> (Team.team);
        list.Remove (Team.team[currentChar]);
        Team.team = list.ToArray ();
        var list2 = new List<GameObject> (interfaceTeam);
        list2.Remove (interfaceTeam[currentChar]);
        interfaceTeam = list2.ToArray ();
        // switch
        anim.SetTrigger ("Switch");

        if (currentChar >= Team.team.Length)
            currentChar = 0;
    
        if (Team.team.Length <= 0) {
            
            //deathVeil -> playerDeathAnim 
            //-> You Died -> Play audio -> press anykey to stop
            StartCoroutine (FinalDeath ());
            return;
        }
        interfaceTeam[currentChar].GetComponent<ArchetypeInterface> ().isSelected = true;
        
        UpdateStats ();
        StartCoroutine (Death ());       
    }
    public GameObject youDied;
    public GameObject splatPrefab;
    public Animator deathVeil;
    IEnumerator FinalDeath () {
        knockBack = true;																	        
        audioManager.Play ("Lost");
        deathVeil.SetTrigger("Death");
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds (1.3f);
        youDied.SetActive(true);
        Time.timeScale = 0.7f;
        gfx.SetActive(false);
        Instantiate (splatPrefab, transform.position, Quaternion.identity);
        audioManager.Play ("PlayerDeath");
        yield return new WaitForSeconds (0.2f);
        Time.timeScale = 1;
        isAllTeamDead = true;
    }
    IEnumerator Death () {
        shake.camShake ();
        // SplatCastRay();
        audioManager.Play ("PlayerDeath", UnityEngine.Random.Range (1f, 3f));																			
        Time.timeScale = 0.7f;
        Instantiate (splatPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds (0.2f);
        Time.timeScale = 1;
    }

    public void LavaDie () {
        // Game over
        GameObject.Find ("LevelLoader").GetComponent<LoadingLevel> ().LoadGameOverScene ();
        // Trigger Gameover scene
    }
    public void LavaDamage() {
        // check if perks double lava is here
            // add 1 to a variable in character set at 0
            // if var == 2
                // die
        // add force to rb
        GetComponent<Rigidbody2D> ().AddForce(new Vector2(0, 15), ForceMode2D.Impulse);
        Die();
    }

    void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.layer == 23) {
            nbCrystals += 1;
        }
    }

    public GameObject BlindLight;
    public GameObject NormalLight;
    void TraitUpdate () {
        if (Team.team[currentChar].trait == "ColorBlind") {
            knockBackResistance = 1f;
            Weapon.SetActive (true);
            ParanoidEffect.SetActive (false);
            BlindEffect.SetActive (false);
            BlindLight.SetActive (false);
            NormalLight.SetActive (true);
            colorBlindEffect.SetActive (true);
            GetComponent<Rigidbody2D> ().gravityScale = 2.35f;
            GetComponent<TestController> ().BulletLeft = normalLeft;
            GetComponent<TestController> ().BulletRight = normalRight;
            fetard.SetActive (false);
            currentAffliction = "ColorBlind";
        } else if (Team.team[currentChar].trait == "Blind") {
            knockBackResistance = 1f;
            Weapon.SetActive (true);
            ParanoidEffect.SetActive (false);
            BlindEffect.SetActive (true);
            BlindLight.SetActive (true);
            NormalLight.SetActive (false);

            colorBlindEffect.SetActive (false);
            GetComponent<Rigidbody2D> ().gravityScale = 2.35f;
            GetComponent<TestController> ().BulletLeft = normalLeft;
            GetComponent<TestController> ().BulletRight = normalRight;
            fetard.SetActive (false);
            currentAffliction = "Blind";
        } else if (Team.team[currentChar].trait == "Paranoid") {
            knockBackResistance = 1f;
            Weapon.SetActive (true);
            ParanoidEffect.SetActive (true);
            BlindEffect.SetActive (false);
            colorBlindEffect.SetActive (false);
            BlindLight.SetActive (false);
            NormalLight.SetActive (true);
            GetComponent<Rigidbody2D> ().gravityScale = 2.35f;
            GetComponent<TestController> ().BulletLeft = normalLeft;
            GetComponent<TestController> ().BulletRight = normalRight;
            fetard.SetActive (false);
            currentAffliction = "Paranoid";
        } else if (Team.team[currentChar].trait == "Normal") {
            knockBackResistance = 1f;
            Weapon.SetActive (true);
            ParanoidEffect.SetActive (false);
            BlindEffect.SetActive (false);
            BlindLight.SetActive (false);
            NormalLight.SetActive (true);
            colorBlindEffect.SetActive (false);
            GetComponent<Rigidbody2D> ().gravityScale = 2.35f;
            GetComponent<TestController> ().BulletLeft = normalLeft;
            GetComponent<TestController> ().BulletRight = normalRight;
            fetard.SetActive (false);
            currentAffliction = "Normal";
        } else if (Team.team[currentChar].trait == "Pacifist") {
            // no weapons
            knockBackResistance = 1f;
            ParanoidEffect.SetActive (false);
            BlindEffect.SetActive (false);
            BlindLight.SetActive (false);
            NormalLight.SetActive (true);
            colorBlindEffect.SetActive (false);
            Weapon.SetActive (false);
            GetComponent<Rigidbody2D> ().gravityScale = 2.35f;
            GetComponent<TestController> ().BulletLeft = normalLeft;
            GetComponent<TestController> ().BulletRight = normalRight;
            fetard.SetActive (false);
            currentAffliction = "Pacifist";
        } else if (Team.team[currentChar].trait == "Astronaut") {
            // no gravity / less gravity
            knockBackResistance = 1.5f;
            ParanoidEffect.SetActive (false);
            BlindEffect.SetActive (false);
            BlindLight.SetActive (false);
            NormalLight.SetActive (true);
            colorBlindEffect.SetActive (false);
            Weapon.SetActive (true);
            GetComponent<TestController> ().BulletLeft = normalLeft;
            GetComponent<TestController> ().BulletRight = normalRight;
            fetard.SetActive (false);
            currentAffliction = "Astronaut";
            GetComponent<Rigidbody2D> ().gravityScale = 1.75f;
        } else if (Team.team[currentChar].trait == "Partygoer") {
            // fetard
            knockBackResistance = 1f;
            ParanoidEffect.SetActive (false);
            BlindEffect.SetActive (false);
            BlindLight.SetActive (false);
            NormalLight.SetActive (true);
            colorBlindEffect.SetActive (false);
            Weapon.SetActive (true);
            GetComponent<Rigidbody2D> ().gravityScale = 2.35f;
            GetComponent<TestController> ().BulletLeft = confettisLeft;
            GetComponent<TestController> ().BulletRight = confettisRight;
            fetard.SetActive (true);
            currentAffliction = "Partygoer";
        } else if (Team.team[currentChar].trait == "Coprolalia") {
            // insults on hit
            knockBackResistance = 1f;
            ParanoidEffect.SetActive (false);
            BlindEffect.SetActive (false);
            BlindLight.SetActive (false);
            NormalLight.SetActive (true);
            colorBlindEffect.SetActive (false);
            Weapon.SetActive (true);
            GetComponent<Rigidbody2D> ().gravityScale = 2.35f;
            GetComponent<TestController> ().BulletLeft = normalLeft;
            GetComponent<TestController> ().BulletRight = normalRight;
            fetard.SetActive (false);
            currentAffliction = "Coprolalia";
        } else if (Team.team[currentChar].trait == "I.B.S") {
            // Irritable bowel syndrome caca partout
            knockBackResistance = 1f;
            ParanoidEffect.SetActive (false);
            BlindEffect.SetActive (false);
            BlindLight.SetActive (false);
            NormalLight.SetActive (true);
            colorBlindEffect.SetActive (false);
            Weapon.SetActive (true);
            GetComponent<Rigidbody2D> ().gravityScale = 2.35f;
            GetComponent<TestController> ().BulletLeft = normalLeft;
            GetComponent<TestController> ().BulletRight = normalRight;
            fetard.SetActive (false);
            currentAffliction = "I.B.S";
        } else if (Team.team[currentChar].trait == "Fat") {
            // jump less
            knockBackResistance = 0.5f;
            ParanoidEffect.SetActive (false);
            BlindEffect.SetActive (false);
            BlindLight.SetActive (false);
            NormalLight.SetActive (true);
            colorBlindEffect.SetActive (false);
            Weapon.SetActive (true);
            GetComponent<Rigidbody2D> ().gravityScale = 3f;
            GetComponent<TestController> ().BulletLeft = normalLeft;
            GetComponent<TestController> ().BulletRight = normalRight;
            fetard.SetActive (false);
            currentAffliction = "Fat";
            // transform.localScale = new Vector3(transform.localScale.x * 1.5f, transform.localScale.y, transform.localScale.z); // true fat
        } else if (Team.team[currentChar].trait == "Sissy") {
            // auto switch on hit
            knockBackResistance = 1f;
            ParanoidEffect.SetActive (false);
            BlindEffect.SetActive (false);
            BlindLight.SetActive (false);
            NormalLight.SetActive (true);
            colorBlindEffect.SetActive (false);
            Weapon.SetActive (true);
            GetComponent<Rigidbody2D> ().gravityScale = 2.35f;
            GetComponent<TestController> ().BulletLeft = normalLeft;
            GetComponent<TestController> ().BulletRight = normalRight;
            fetard.SetActive (false);
            currentAffliction = "Sissy";
        }
    }
}