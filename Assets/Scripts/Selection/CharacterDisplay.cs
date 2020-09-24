using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CharacterDisplay : MonoBehaviour
{
    public Character character;
   public TextMeshProUGUI name;
   public TextMeshProUGUI efficiency;
   public TextMeshProUGUI archetype;
   public TextMeshProUGUI weapon;
   public TextMeshProUGUI trait;
   public TextMeshProUGUI life;
   public TextMeshProUGUI speed;
   public TextMeshProUGUI price;
   public Image icon;
   public Image Head;
   public Image Body;
   public Color32 legendary;
   public Color32 rare;
   public Color32 common;
   public Sprite[] heads;

   

   public void changeText(Character c) 
   {
       character = c;
       name.text = c.name;
       efficiency.text = c.efficiency;
       archetype.text = c.archetype;
       trait.text = c.trait;
       weapon.text = c.weaponType;
       life.text = c.life.ToString();
       speed.text = c.speed.ToString();
       icon.color = efficiency.text == "Legendary" ? legendary : efficiency.text == "Rare" ? rare : common ;
       if (c.from == "market")
           price.text = c.price.ToString() + " $";
        else {
            price.text = "";
        }
       Head.color = c.color;
       Body.color = c.color;
       Head.sprite = heads[c.headForm];
   }
}
