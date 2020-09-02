using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBindScript : MonoBehaviour
{
    public static Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
    public Text up, left, down, right, jump, fire, switchKey, action;
    private GameObject currentKey;

    private Color32 normal = new Color32(135,135,135,255);
    private Color32 selected = new Color32(240,115,36,255);    

    // Start is called before the first frame update
    void Awake()
    {
        GameObject player = GameObject.Find("Player");
        if (!player) {
            if (!keys.ContainsKey("Up"))
                keys.Add("Up", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "Z")));
            if (!keys.ContainsKey("Down"))
                keys.Add("Down", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S")));
            if (!keys.ContainsKey("Left"))
                keys.Add("Left", (KeyCode) System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Left", "Q")));
            if (!keys.ContainsKey("Right"))
                keys.Add("Right", (KeyCode) System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Right", "D")));
            if (!keys.ContainsKey("Jump"))
                keys.Add("Jump", (KeyCode) System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Jump", "Space")));
            if (!keys.ContainsKey("Fire"))
                keys.Add("Fire", (KeyCode) System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Fire", "E")));
            if (!keys.ContainsKey("Switch"))
                keys.Add("Switch", (KeyCode) System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Switch", "A")));
            if (!keys.ContainsKey("Action"))
                keys.Add("Action", (KeyCode) System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Action", "R")));
        }
        up.text = keys["Up"].ToString();
        down.text = keys["Down"].ToString();
        left.text = keys["Left"].ToString();
        right.text = keys["Right"].ToString();
        jump.text = keys["Jump"].ToString();
        fire.text = keys["Fire"].ToString();
        switchKey.text = keys["Switch"].ToString();
        action.text = keys["Action"].ToString();
    }

    void Start()
    {
    //     keys.Add("Up", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "W")));
    //     keys.Add("Down", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S")));
    //     keys.Add("Left", (KeyCode) System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Left", "Q")));
    //     keys.Add("Right", (KeyCode) System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Right", "D")));
    //     keys.Add("Jump", (KeyCode) System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Jump", "Space")));
    //     keys.Add("Fire", (KeyCode) System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Fire", "E")));
    //     keys.Add("Switch", (KeyCode) System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Switch", "A")));
    //     keys.Add("Action", (KeyCode) System.Enum.Parse(typeof(KeyCode),PlayerPrefs.GetString("Action", "R")));
    }

    void OnGUI()
    {
        if (currentKey != null) {
            Event e = Event.current;
            if (Event.current.type == EventType.MouseDown) {
               
                keys[currentKey.name] = (KeyCode)((int)KeyCode.Mouse0 + Event.current.button);;
                currentKey.transform.GetChild(0).GetComponent<Text>().text = ((int)KeyCode.Mouse0 + Event.current.button).ToString();
                currentKey.GetComponent<Image>().color = normal;
                currentKey = null;
            }
            else if (e.isKey) {
                keys[currentKey.name] = e.keyCode;
                currentKey.transform.GetChild(0).GetComponent<Text>().text = e.keyCode.ToString();
                currentKey.GetComponent<Image>().color = normal;
                currentKey = null;
            }
        }
    }

    public void ChangeKey(GameObject clicked)
    {
        if (currentKey != null) {
            currentKey.GetComponent<Image>().color = normal;
        }
        currentKey = clicked;
        currentKey.GetComponent<Image>().color = selected;        

    }

    public void SaveKeys()
    {
        foreach ( var key in keys)
        {
            PlayerPrefs.SetString(key.Key, key.Value.ToString());
        }
        PlayerPrefs.Save();
    }
    // Update is called once per frame
    void Update()
    {
        up.text = keys["Up"].ToString();
        down.text = keys["Down"].ToString();
        left.text = keys["Left"].ToString();
        right.text = keys["Right"].ToString();
        jump.text = keys["Jump"].ToString();
        fire.text = keys["Fire"].ToString();
        switchKey.text = keys["Switch"].ToString();
        action.text = keys["Action"].ToString();
        // if (Input.GetKeyDown[keys["Up"]]) // access the key
    }
}
