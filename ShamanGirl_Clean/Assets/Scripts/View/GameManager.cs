using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts.ListView;
using System;
using UnityEngine.SceneManagement;
using Assets.Scripts.View;

public class GameManager : DefaultManagerView
{

    public static GameManager instance;
    public int score = 0;

    public Text scoreLabel;
    public GameObject gameOverCanvas;
    public GameObject levelCompleteCanvas;

    private GameController controller;
    

    public int maxLevelScore;

    void Awake()
    {
        if (instance == null)
            instance = gameObject.GetComponent<GameManager>();
        if (controller == null)
            controller = new GameController();
    }
    // Use this for initialization
    void Start()
    {
        scoreLabel.text = score.ToString() + " / " + maxLevelScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateScore()
    {
        score++;
        scoreLabel.text = score.ToString() + " / " + maxLevelScore.ToString() ;

    }
    public void AddScore(int score)
    {
        this.controller.Score += score;
        this.score += score;
        if(scoreLabel!=null) scoreLabel.text = this.controller.Score.ToString();

    }
    public void OnLevelEnd()
    {
        if (score >= maxLevelScore)
        {
            if (levelCompleteCanvas != null) levelCompleteCanvas.SetActive(true);
        }
        else
        {
            if (gameOverCanvas != null) gameOverCanvas.SetActive(true);
        }
    }
    public void LoadNextLeve()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public override void OnBackButton()
    {
        SceneManager.LoadScene(MenuManager.SCENE_MENU);
    }
    public void OnReloadLevelButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnProperties()
    {
        //
    }
}
