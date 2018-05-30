using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    private int score;

    public static ScoreManager instance;
    public Text scoreText;
    public Text hightScoreText;

    public int Score {
        get { return score; }
        set {
            score = value;
            scoreText.text = score.ToString();

            if (PlayerPrefs.GetInt("HightScore") < score) {
                PlayerPrefs.SetInt("HightScore", score);
                hightScoreText.text = score.ToString();
            }
            PlayerPrefs.Save();
        }
    }

    private void Awake() {
      //  PlayerPrefs.DeleteAll ();
        instance = this;
        if (!PlayerPrefs.HasKey("HightScore")) {
            PlayerPrefs.SetInt("HightScore", 0);
        }
        scoreText.text = "0";
        hightScoreText.text = PlayerPrefs.GetInt("HightScore").ToString();
        //Debug.Log(PlayerPrefs.GetInt("HightScore").ToString());
    }
}
