﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public new string name;
    public string archetype;
    public int speed;
    public int life;
    public Color color;
    public string trait;
    public int efficiency;
    public int currentHealth;
    
    public int nbFlags;
    public float runner_CDR;
    public float climber_CDR;
    public float grenadier_bombs;
    public float tank_shield;
    public float hacker_time;
    public float used_tank_shield;

    
    public Sprite artwork;

    public Character(string _name, string _archetype, int _speed, int _life, Color _color, string _trait, int _efficiency)
    {
        name = _name;
        archetype = _archetype;
        speed = _speed;
        life = _life;
        color = _color;
        currentHealth = life;
        trait = _trait;
        efficiency = _efficiency;
        nbFlags = efficiency < 5 ? 5 : efficiency >= 5 && efficiency <= 9 ? 8 : 10; // 5 8 10
        runner_CDR = efficiency < 5 ? 0.4f : efficiency >= 5 && efficiency <= 9 ? 0.3f : 0; // .4 .3 0
        climber_CDR = efficiency < 5 ? 0.6f : efficiency >= 5 && efficiency <= 9 ? 0.4f : 0.2f; // .6 .4 .2
        grenadier_bombs = efficiency < 5 ? 5 : efficiency >= 5 && efficiency <= 9 ? 10 : 15; // 5 10 15
        tank_shield = efficiency < 5 ? 10 : efficiency >= 5 && efficiency <= 9 ? 15 : 20; // 10 15 20
        used_tank_shield = 0;
        hacker_time = efficiency < 5 ? 2 : efficiency >= 5 && efficiency <= 9 ? 1.5f : 1; // 2 1.5 1
    }
}
