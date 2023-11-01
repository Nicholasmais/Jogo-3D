using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Results : MonoBehaviour {
    public Text timerText;
    public Text nomeText;
    public Text pontoText;


    private float timer;
    private int score;
    
    // Use this for initialization
    void Start () {
        timer = PlayerPrefs.GetFloat("Timer");
        timerText.text = timer.ToString("0.0") + "s";

        nomeText.text = PlayerPrefs.GetString("PlayerName");

        score = PlayerPrefs.GetInt("Score");
        pontoText.text = score.ToString();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
