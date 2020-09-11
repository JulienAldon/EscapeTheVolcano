using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Team
{
    public static Character[] team = new Character[4];
    public static int nbSelected = 0;
    public static List<Character> deadCharacters = new List<Character>();
    public static List<Character> stash = new List<Character>();
    public static List<Character> market = new List<Character>();
    public static Contract currentContract;
    public static int money;
    public static List<string> skills = new List<string>();
    public static int blobKilled;
    public static int batKilled;
    public static int golemKilled;
    public static int monsterNumber;
}

public class Contract 
{
    public int level;
    public int reward;
    public string typeCrystalContract; // less more between none
    public string typeTimeContract; // less more between none
    public List<int> crystals = new List<int>();
    public List<int> time = new List<int>();
    public List<string> modifiers = new List<string>();
    public Contract(int _level, string _typeCrystalContract, string _typeTimeContract, List<string> _modifiers, List<int> _crystals, List<int> _time, int _reward) {
        level = _level;
        typeCrystalContract = _typeCrystalContract;
        typeTimeContract = _typeTimeContract;
        for (int i = 0; i < _modifiers.Count; i++) {
            Debug.Log(_modifiers[i]);
            modifiers.Add(_modifiers[i]);
        }
        for (int i = 0; i < _crystals.Count; i++) {
            crystals.Add(_crystals[i]);
        }
        for (int i = 0; i < _time.Count; i++) {
            time.Add(_time[i]);
        }
        reward = _reward;
    }
}