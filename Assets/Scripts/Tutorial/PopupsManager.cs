using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupsManager : MonoBehaviour
{
    public TextMeshProUGUI Move;
    public TextMeshProUGUI Jump;
    public TextMeshProUGUI Fire;
    public TextMeshProUGUI Switch;
    public TextMeshProUGUI Action;    
    
    // Start is called before the first frame update
    void Start()
    {
        Move.text = "Use " + KeyBindScript.keys["Left"] + " or " + KeyBindScript.keys["Right"] + " to move";
        Jump.text = "Use " + KeyBindScript.keys["Jump"] + " to Jump";
        Fire.text = "Use " + KeyBindScript.keys["Fire"] + " to Fire";
        Switch.text = "Use " + KeyBindScript.keys["Switch"] + " to Switch between team members";
        Action.text = "Use " + KeyBindScript.keys["Action"] + " to trigger specific Action";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
