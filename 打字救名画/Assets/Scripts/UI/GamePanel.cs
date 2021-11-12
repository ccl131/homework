using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GamePanel : BasePanel
{
    private int levelIndex=1;
    private int score=0;
    private int time=60;
    private float timer=0;
    private Text scoreText;
    private Text levelText;
    private Text timeText;

    public override void OnInit()
    {
        layer = PanelManager.Layer.bottom;
    }

    public override void OnShow(params object[] args)
    {
        MusicManager.Instance.PlayMusic("Audio/游戏背景音乐");
        levelIndex = int.Parse((string)args[0]);
        SceneManager.LoadScene("level_"+levelIndex.ToString());
        scoreText =Find<Text>("scoreText");
        levelText = Find<Text>("levelText");
        timeText = Find<Text>("timeText");

        levelText.text = levelIndex.ToString();
        Eventcenter.AddEventListener("失败",Fail);
        Eventcenter.AddEventListener("加分", AddScore);
        StartCoroutine(StartGame());
    }
    IEnumerator StartGame()
    {
        for (int i = 0; i < 3; i++)
        {
            MusicManager.Instance.PlaySound("Audio/Sound 11");
            yield return new WaitForSeconds(1f);
        }
        MusicManager.Instance.PlaySound("Audio/Sound 10");
        Eventcenter.EventTrigger("开始");
    }
    private void AddScore()
    {
        score += 10;
        scoreText.text = score.ToString();
    }

    private void Fail()
    {
        Eventcenter.Clear();
        SceneManager.LoadScene("Game");
        PanelManager.Open<GameOverPanel>(levelIndex,score);
        Close();
    }

    private void Update()
    {
        Timer();
    }

    private void Timer()
    {
        timer += Time.deltaTime;
        if(timer>=1f)
        {
            time--;
            timeText.text = time.ToString();
            if(time==0)
            {
                Eventcenter.Clear();
                SceneManager.LoadScene("Game");
                PanelManager.Open<WinPanel>(levelIndex, score);
                Player.score += score;
                DBManager.UpdatePlayerData();
                Close();
            }
            timer = 0;
        }
    }
}
