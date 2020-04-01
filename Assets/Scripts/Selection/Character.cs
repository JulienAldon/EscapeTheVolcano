using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public new string name;
    public string archetype;
    public int speed;
    public int life;
    public int nbFlags;

    public int currentHealth;
    
    public Sprite artwork;

    public Character(string _name, string _archetype, int _speed, int _life)
    {
        name = _name;
        archetype = _archetype;
        speed = _speed;
        life = _life;
        nbFlags = 5;
        currentHealth = life;
    }
}
