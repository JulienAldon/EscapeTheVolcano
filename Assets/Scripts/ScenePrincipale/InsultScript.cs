using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System.Linq;

public class InsultScript : MonoBehaviour
{
    private List<string> insult;
    public TextMeshProUGUI currentInsult;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void changeInsult() 
    {
        insult = new List<string>();
        readTextFile("Assets/Scripts/Ressources/Haddock.txt");
        currentInsult.text = insult.ElementAt(Random.Range(0, insult.Count));
    }

    void readTextFile(string file_path)
    {
        StreamReader inp_stm = new StreamReader(file_path);
        while(!inp_stm.EndOfStream)
        {
            string inp_ln = inp_stm.ReadLine();
            insult.Add(inp_ln);
        }

        inp_stm.Close();
    }

    void onCollisionEnter2D(Collision2D colision)
    {
        Destroy(gameObject, .2f);
    }
}
