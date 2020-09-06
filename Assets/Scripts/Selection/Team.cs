using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Team
{
    public static Character[] team = new Character[4];
    public static int nbSelected = 0;
    public static Character[] deadCharacters = new Character[4];
    public static List<Character> stash = new List<Character>();
    public static List<Character> market = new List<Character>();
    public static Contract currentContract;

}

public class Contract 
{
    public string typeCrystalContract; // less more between
    public string typeTimeContract; // less more between
    public List<int> crystals;
    public List<int> time;
    public List<string> modifiers;
}