using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DisplayCharacter : MonoBehaviour
{
    public Text life;
    public Text archetype;
    public Text trait;
    public Text speed;
    public new Text name;
    public Text weapon;

    private Character currentCharacter;

    void Start()
    {
        currentCharacter = Team.team[Team.nbSelected - 1];

        life.text = currentCharacter.life.ToString();
        speed.text = currentCharacter.speed.ToString();
        name.text = currentCharacter.name;
        archetype.text = currentCharacter.archetype;
        trait.text = currentCharacter.trait;
        weapon.text = currentCharacter.weaponType;
        GetComponent<SpriteRenderer>().color =  currentCharacter.efficiency < 5 ? new Color32(65, 65, 65, 150) : currentCharacter.efficiency >= 5 && currentCharacter.efficiency <= 9 ? new Color32(0, 64, 255, 150) : new Color32(250, 230, 70, 150);;
    }
}
