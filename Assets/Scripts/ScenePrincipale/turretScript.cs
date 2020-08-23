using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretScript : MonoBehaviour {
    public float FireRate = 3f;
    public GameObject Bullet;
    public Animator animator;
    public Transform headPos;
    public Transform canonEnd;
    public GameObject progressionBar;
    public GameObject turretbase;
    public GameObject range;
    public GameObject cursor;
    public float hackDistance = 65f;
    public float hackTime = 3f;
    public float hackRechargeSpeed = 0.1f;
    public float turretRange = 90f;
    private float hackTimer;
    float nextFire = 0f;
    bool deactivated = false;
    private GameObject target;
    private bool firing = false;
    private GameObject currentLaser;
    private bool hackingInProgress;
    private Vector3 firstCanonEndPos;
    private float hackingResetTime = 1f;
    private float hackingResetTimer;
    private float hackingCursorTime = 0.1f;
    private float hackingCursorTimer;
    private float nextRefreshHacker = 0;

    void Start () {
        target = GameObject.Find ("Player");
        if (Team.team[0] != null)
            hackTime = Team.team[target.GetComponent<CharacterStats> ().currentChar].hacker_time;
        firstCanonEndPos = canonEnd.position;
        hackTimer = hackTime;
        hackingCursorTimer = hackingCursorTime;
        hackingResetTimer = hackingResetTime;
    }

    void Update () {
        if (Team.team[0] != null)
            hackTime = Team.team[target.GetComponent<CharacterStats> ().currentChar].hacker_time;
        if (!deactivated) {
            if (currentLaser)
                firing = currentLaser.GetComponent<LaserScript> ().getStoppedState ();
            else
                firing = false;
            if (!firing) {
                Vector3 currentPosition = target.transform.position;
                Vector3 directionToTarget = transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < turretRange) {
                    rotateTowardPlayer ();
                    int mask = 1 << LayerMask.NameToLayer ("Player") | 1 << LayerMask.NameToLayer ("Ground");
                    RaycastHit2D hit = Physics2D.Raycast (transform.position, directionToTarget, Mathf.Infinity, mask);
                    if (Time.time > nextFire && hit.collider.tag != "Ground") {
                        nextFire = Time.time + FireRate;
                        fire ();
                    } else {
                        //animator.SetBool("shooting", false);
                    }
                }
            }
        }
        if (getDeactivation () == true) {
            range.GetComponent<SpriteRenderer> ().enabled = false;

        }
        // flow to allow a turret to regenerate his progression Bar
        hackingCursorTimer -= Time.deltaTime;
        if (hackingInProgress) {
            hackingResetTimer -= Time.deltaTime;
        }
        if (hackingResetTimer <= 0.0f) {
            hackingResetTimer = hackingResetTime;
            hackingInProgress = false;
        }
        if (hackingCursorTimer <= 0.0f) {
            hackingCursorTimer = hackingCursorTime;
            cursor.GetComponent<SpriteRenderer> ().enabled = false;
        }
        if (!hackingInProgress) {
            if (getDeactivation () == false) {
                hackTimer += hackRechargeSpeed;
                if (hackTimer > hackTime) {
                    hackTimer = hackTime;
                }
                updateProgressionBar ();
            }
        }
    }

    public bool getStoppedState () {
        return firing;
    }

    /// This function will update progression bar with hacktime (float) attribute
    public void updateProgressionBar () {
        if (getDeactivation () == false) {
            if (hackTime != 0)
                progressionBar.GetComponent<ProgressBar> ().SetProgress (hackTimer / hackTime / 2);
        }
    }

    /// This public function reset the hack mecanic
    /// The function will reset the hackTimer variable, then reset the progression bar (UI)
    public void resetHack () {
        Team.team[target.GetComponent<CharacterStats> ().currentChar].hacker_state = 20;
        hackingInProgress = false;
    }

    /// This public function is in charge of the hacking mecanic
    /// Given the player position (Vector2),
    ///nce hacktime elapse, the turret will be deactivated, if player go outside of the hackDistance,
    /// the progression is reset
    public void hack (Vector2 position) {
        Vector3 currentPosition = position;
        Vector3 directionToTarget = transform.position - currentPosition;
        float dSqrToTarget = directionToTarget.sqrMagnitude;

        hackingInProgress = true;
        if (dSqrToTarget > hackDistance) {
            resetHack ();
        } else if (getDeactivation () == false) {
            if (Time.time > nextRefreshHacker) {
                nextRefreshHacker = Time.time + 0.1f;
                if (Team.team[target.GetComponent<CharacterStats> ().currentChar].hacker_state - Team.team[target.GetComponent<CharacterStats> ().currentChar].hacker_step < 0) {
                    Team.team[target.GetComponent<CharacterStats> ().currentChar].hacker_state = 0;
                } else {
                    Team.team[target.GetComponent<CharacterStats> ().currentChar].hacker_state -= Team.team[target.GetComponent<CharacterStats> ().currentChar].hacker_step;
                }
            }
            cursor.GetComponent<SpriteRenderer> ().enabled = true;
            hackTimer -= Time.deltaTime;
            if (hackTime != 0)
                progressionBar.GetComponent<ProgressBar> ().SetProgress (hackTimer / hackTime / 2);
            if (hackTimer <= 0.0f) {
                deactivate ();
                hackTimer = hackTime;
            }
        }

    }

    /// Function that calculate and return the angle (float) toward the player
    float rotateTowardPlayer () {
        Vector2 direction = target.transform.position - transform.position;
        float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < -90 || angle > 90) {
            transform.localScale = new Vector3 (1, -1, 1);
        } else {
            transform.localScale = new Vector3 (1, 1, 1);
        }
        transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
        if (turretbase.GetComponent<SpriteRenderer> ().flipX == true) {
            transform.position = new Vector3 (headPos.position.x + 0.35f, headPos.position.y, 0);
        } else if (turretbase.GetComponent<SpriteRenderer> ().flipX == false) {
            transform.position = new Vector3 (headPos.position.x, headPos.position.y, 0);
        }
        return angle;
    }

    /// Function that trigger the deactivation event
    /// The turret will be deactivated, unable to shoot and play an animation on deactivation
    public void deactivate () {
        if (deactivated == false) {
            animator.SetBool ("deactivated", true);
            deactivated = true;
        }
    }

    /// Getter for the deactivated (bool) attribute
    public bool getDeactivation () {
        return deactivated;
    }

    /// Function that trigger the "fire" event
    /// this event will instantiate a new bullet and save the gameObject in the variable currentLaser
    void fire () {
        GameObject bulletObj = Instantiate (Bullet, canonEnd.position, canonEnd.rotation);
        currentLaser = bulletObj;
        bulletObj.transform.parent = gameObject.transform;
        //animator.SetBool("shooting", true);
    }
}