using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamDisplay : MonoBehaviour
{
    public GameObject Character;

    public Transform Slot1;
    public Transform Slot2;
    public Transform Slot3;
    public Transform Slot4;

    public void AddCharacter(int selected) {
        if (selected == 0){
            var clone = Instantiate(Character, Slot1.position, Quaternion.identity);
            clone.transform.parent = transform;
        }
        else if (selected == 1){
            var clone = Instantiate(Character, Slot2.position, Quaternion.identity);
            clone.transform.parent = transform;
        }
        else if (selected == 2){
            var clone = Instantiate(Character, Slot3.position, Quaternion.identity);
            clone.transform.parent = transform;
        }
        else if (selected == 3){
            var clone = Instantiate(Character, Slot4.position, Quaternion.identity);
            clone.transform.parent = transform;
        }
    }
}
