using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public string name;
    public string archetype;
    public int speed;
    public int life;
    public Color color;
    public string trait;
    public string efficiency;
    public int currentHealth;
    
    public int nbFlags;
    public float runner_CDR;
    public float climber_CDR;
    public float grenadier_bombs;
    public float tank_shield;
    public float hacker_time;
    public float used_tank_shield;
    public int tank_state;
    public int runner_state;
    public int climber_state;
    public int runner_step;
    public int climber_step;
    public int hacker_state;
    public int hacker_step;
    public Sprite artwork;
    public string weaponType;
    public int currentFire;
    public float fireRate;
    public int price;
    public int headForm;
    public string from;

    public Character(string _name, string _archetype, int _speed, int _life, Color _color, string _trait, string _efficiency, string _weaponType, string _from)
    {
        from = _from;
        int priceMod = 1;
        name = _name;
        archetype = _archetype;
        speed = _speed;
        life = _life;
        color = _color;
        currentHealth = life;
        trait = _trait;
        efficiency = _efficiency;
        nbFlags = efficiency == "Common" ? 5 : efficiency == "Rare" ? 8 : 10; // 5 8 10
        runner_CDR = efficiency == "Common" ? 0.4f : efficiency == "Rare" ? 0.3f : 0; // .4 .3 0
        climber_CDR = efficiency == "Common" ? 0.8f : efficiency == "Rare" ? 0.6f : 0.2f; // .6 .4 .2
        grenadier_bombs = efficiency == "Common" ? 5 : efficiency == "Rare" ? 10 : 15; // 5 10 15
        tank_shield = efficiency == "Common" ? 10 : efficiency == "Rare" ? 15 : 20; // 10 15 20
        used_tank_shield = 0;
        hacker_time = efficiency == "Common" ? 2 : efficiency == "Rare" ? 1.5f : 1; // 2 1.5 1
        weaponType = _weaponType;
        currentFire = 0;
        // runner_state = 20;
        // climber_step = (int)20 % (climber_CDR * 10);
        // climber_state =  
        tank_state = (int)tank_shield;
        runner_state = 0;
        if (runner_CDR != 0) {
            runner_step = (int)(20 / (runner_CDR / 0.1f));
        } else {
            runner_step = 0;
        }
        climber_state = 0;
        climber_step = (int)(20 / (climber_CDR / 0.1));
        hacker_state = 20;
        hacker_step = (int)(20 / (hacker_time / 0.1));
        if (weaponType == "Missile") {
            priceMod += 1;
			fireRate = 0.3f;
        }
			// fireRate = 0.3f;
        else if (weaponType == "EnergyGun") {
			fireRate = 0.4f;
			// fireRate = 0.2f;
        }
        else if (weaponType == "Gatling") {
            priceMod += 2;
            fireRate = 0.06f;
            // fireRate = 0.03f;
        }
        else if (trait == "Partygoer")
            fireRate = 0.6f;
        price = efficiency == "Common" ? priceMod * 150 : efficiency == "Rare" ? priceMod * 500 : priceMod * 1000;
        headForm = Random.Range(0, 6);
        
    }
}
