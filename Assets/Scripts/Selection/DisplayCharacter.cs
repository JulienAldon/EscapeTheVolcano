using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DisplayCharacter : MonoBehaviour
{
    public TextMesh life;
    public TextMesh archetype;
    public TextMesh speed;
    public TextMesh name;

    private Character currentCharacter;

    void Start()
    {
        currentCharacter = Team.team[Team.nbSelected - 1];

        life.text = currentCharacter.life.ToString();
        speed.text = currentCharacter.speed.ToString();
        name.text = currentCharacter.name;
        archetype.text = currentCharacter.archetype;
    }
}
