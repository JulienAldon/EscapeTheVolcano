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
    public float damageRate;
    private float nextDamage = 0f;
    public static int nbCrystals;

    public int nbFlags;
    public float runner_CDR;
    public float climber_CDR;
    public float grenadier_bombs;
    public float tank_shield;
    public float hacker_time;

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
        matDefault = gfx.GetComponent<SpriteRenderer> ().material;
    }

    void UpdateStats () {
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

        StartCoroutine (ChangeColor ());
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
        transform.GetChild (0).GetComponent<SpriteRenderer> ().color = Team.team[currentChar].color;
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
        // if (damaged && Time.time > nextDamage) {
        //     nextDamage = Time.time + damageRate;
        //     damaged = false;
        // }
        UpdatePower ();
    }

    public void TakeDamage (int damage, Vector2 _direction) {
        print(damaged);
        if (damaged)
            return;
        StartCoroutine( Damaged ());
        gfx.GetComponent<SpriteRenderer> ().material = matWhite;
        Invoke ("ResetMaterial", 1f);
        if (_direction.x < 0) {
            _direction.x = -1;
        } else if (_direction.x > 0) {
            _direction.x = 1;
        }
        GetComponent<Rigidbody2D>().AddForce(new Vector2(_direction.x * 30, 20), ForceMode2D.Impulse);
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
        yield return new WaitForSeconds (1);
        damaged = false;
    }
    void ResetMaterial () {
        gfx.GetComponent<SpriteRenderer> ().material = matDefault;
    }

    public void Die () {
        // switch and supress char from team
        // delete
        interfaceTeam[currentChar].GetComponent<ArchetypeInterface> ().isDead = true;
        if (Team.team.Length <= 0) {
            LavaDie ();
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
        currentChar += 1;
        if (currentChar >= Team.team.Length)
            currentChar = 0;
        interfaceTeam[currentChar].GetComponent<ArchetypeInterface> ().isSelected = true;
        UpdateStats ();
        StartCoroutine (Death ());
        // Supress team member display
        // Make cool thing to say the player is dead
    }

    public GameObject splatPrefab;

    IEnumerator Death () {
        shake.camShake ();
        // SplatCastRay();
        Instantiate (splatPrefab, transform.position, Quaternion.identity);
        Time.timeScale = 0.7f;
        yield return new WaitForSeconds (0.2f);
        Time.timeScale = 1;
    }

    public void LavaDie () {
        // Game over
        GameObject.Find ("LevelLoader").GetComponent<LoadingLevel> ().LoadGameOverScene ();
        // Trigger Gameover scene
    }

    void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.layer == 23) {
            nbCrystals += 1;
        }
    }

    void TraitUpdate () {
        if (Team.team[currentChar].trait == "ColorBlind") {
            Weapon.SetActive (true);
            ParanoidEffect.SetActive (false);
            BlindEffect.SetActive (false);
            colorBlindEffect.SetActive (true);
            GetComponent<Rigidbody2D> ().gravityScale = 2.35f;
            GetComponent<TestController> ().BulletLeft = normalLeft;
            GetComponent<TestController> ().BulletRight = normalRight;
            fetard.SetActive (false);
            currentAffliction = "ColorBlind";
        } else if (Team.team[currentChar].trait == "Blind") {
            Weapon.SetActive (true);
            ParanoidEffect.SetActive (false);
            BlindEffect.SetActive (true);
            colorBlindEffect.SetActive (false);
            GetComponent<Rigidbody2D> ().gravityScale = 2.35f;
            GetComponent<TestController> ().BulletLeft = normalLeft;
            GetComponent<TestController> ().BulletRight = normalRight;
            fetard.SetActive (false);
            currentAffliction = "Blind";
        } else if (Team.team[currentChar].trait == "Paranoid") {
            Weapon.SetActive (true);
            ParanoidEffect.SetActive (true);
            BlindEffect.SetActive (false);
            colorBlindEffect.SetActive (false);
            GetComponent<Rigidbody2D> ().gravityScale = 2.35f;
            GetComponent<TestController> ().BulletLeft = normalLeft;
            GetComponent<TestController> ().BulletRight = normalRight;
            fetard.SetActive (false);
            currentAffliction = "Paranoid";
        } else if (Team.team[currentChar].trait == "Normal") {
            Weapon.SetActive (true);
            ParanoidEffect.SetActive (false);
            BlindEffect.SetActive (false);
            colorBlindEffect.SetActive (false);
            GetComponent<Rigidbody2D> ().gravityScale = 2.35f;
            GetComponent<TestController> ().BulletLeft = normalLeft;
            GetComponent<TestController> ().BulletRight = normalRight;
            fetard.SetActive (false);
            currentAffliction = "Normal";
        } else if (Team.team[currentChar].trait == "Pacifist") {
            // no weapons
            ParanoidEffect.SetActive (false);
            BlindEffect.SetActive (false);
            colorBlindEffect.SetActive (false);
            Weapon.SetActive (false);
            GetComponent<Rigidbody2D> ().gravityScale = 2.35f;
            GetComponent<TestController> ().BulletLeft = normalLeft;
            GetComponent<TestController> ().BulletRight = normalRight;
            fetard.SetActive (false);
            currentAffliction = "Pacifist";
        } else if (Team.team[currentChar].trait == "Astronaut") {
            // no gravity / less gravity
            ParanoidEffect.SetActive (false);
            BlindEffect.SetActive (false);
            colorBlindEffect.SetActive (false);
            Weapon.SetActive (true);
            GetComponent<TestController> ().BulletLeft = normalLeft;
            GetComponent<TestController> ().BulletRight = normalRight;
            fetard.SetActive (false);
            currentAffliction = "Astronaut";
            GetComponent<Rigidbody2D> ().gravityScale = 1.5f;
        } else if (Team.team[currentChar].trait == "Partygoer") {
            // fetard
            ParanoidEffect.SetActive (false);
            BlindEffect.SetActive (false);
            colorBlindEffect.SetActive (false);
            Weapon.SetActive (true);
            GetComponent<Rigidbody2D> ().gravityScale = 2.35f;
            GetComponent<TestController> ().BulletLeft = confettisLeft;
            GetComponent<TestController> ().BulletRight = confettisRight;
            fetard.SetActive (true);
            currentAffliction = "Partygoer";
        } else if (Team.team[currentChar].trait == "Coprolalia") {
            // insults on hit
            ParanoidEffect.SetActive (false);
            BlindEffect.SetActive (false);
            colorBlindEffect.SetActive (false);
            Weapon.SetActive (true);
            GetComponent<Rigidbody2D> ().gravityScale = 2.35f;
            GetComponent<TestController> ().BulletLeft = normalLeft;
            GetComponent<TestController> ().BulletRight = normalRight;
            fetard.SetActive (false);
            currentAffliction = "Coprolalia";
        } else if (Team.team[currentChar].trait == "I.B.S") {
            // Irritable bowel syndrome caca partout
            ParanoidEffect.SetActive (false);
            BlindEffect.SetActive (false);
            colorBlindEffect.SetActive (false);
            Weapon.SetActive (true);
            GetComponent<Rigidbody2D> ().gravityScale = 2.35f;
            GetComponent<TestController> ().BulletLeft = normalLeft;
            GetComponent<TestController> ().BulletRight = normalRight;
            fetard.SetActive (false);
            currentAffliction = "I.B.S";
        } else if (Team.team[currentChar].trait == "Fat") {
            // jump less
            ParanoidEffect.SetActive (false);
            BlindEffect.SetActive (false);
            colorBlindEffect.SetActive (false);
            Weapon.SetActive (true);
            GetComponent<Rigidbody2D> ().gravityScale = 3f;
            GetComponent<TestController> ().BulletLeft = normalLeft;
            GetComponent<TestController> ().BulletRight = normalRight;
            fetard.SetActive (false);
            currentAffliction = "Fat";
        } else if (Team.team[currentChar].trait == "Sissy") {
            // auto switch on hit
            ParanoidEffect.SetActive (false);
            BlindEffect.SetActive (false);
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