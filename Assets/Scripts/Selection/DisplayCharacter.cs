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
    public Text name;

    private Character currentCharacter;

    void Start()
    {
        currentCharacter = Team.team[Team.nbSelected - 1];

        life.text = currentCharacter.life.ToString();
        speed.text = currentCharacter.speed.ToString();
        name.text = currentCharacter.name;
        archetype.text = currentCharacter.archetype;
        trait.text = currentCharacter.trait;
    }
}
