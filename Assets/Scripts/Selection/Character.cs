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
    public Color color;
    public string trait;

    public int currentHealth;
    
    public Sprite artwork;

    public Character(string _name, string _archetype, int _speed, int _life, Color _color, string _trait)
    {
        name = _name;
        archetype = _archetype;
        speed = _speed;
        life = _life;
        nbFlags = 5;
        color = _color;
        currentHealth = life;
        trait = _trait;
    }
}
