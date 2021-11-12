using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private int textIndex = 0;
    private bool isStartGame = false;
    public Text bgText;
    public Text inputText;
    private string myText;
    private string[] textArrr =
        {
        "position","rotation","gameobject","transform","random","range","animation",
        "animator","text","sprite","move","unity","event","queue","dictionary",
        "score","player","manager","panel","select"
    };

    private void Start()
    {
        ChangeText();
        Eventcenter.AddEventListener("开始",()=> { isStartGame = true; });
    }


    void Update()
    {
        if (!isStartGame) return;
        CheckText();
    }

    private void CheckText()
    {

        if (Input.anyKeyDown)
        {
            MusicManager.Instance.PlaySound("Audio/按键");
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    string input = myText[textIndex].ToString();
                    if (input == keyCode.ToString().ToLower())
                    {
                        inputText.text += input;
                        Check();
                    }
                }
            }
        }
    }
    private void Check()
    {
        if (textIndex < bgText.text.Length)
        {
            textIndex++;
        }

        if (inputText.text == myText)
        {
            MusicManager.Instance.PlaySound("Audio/完成单词");
            Eventcenter.EventTrigger("上升");
            Eventcenter.EventTrigger("加分");
            ChangeText();
        }
    }

    private void ChangeText()
    {
        int index = Random.Range(0, textArrr.Length);
        myText = textArrr[index];
        bgText.text = myText;
        inputText.text = "";
        textIndex = 0;
    }
}
