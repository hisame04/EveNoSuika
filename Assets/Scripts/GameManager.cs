using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public ProduceItemManager produceItemManager;
    public GameOverLineController gameOverLineController;
    public UIController uiController;
    public bool onGame;
    public int point;

    void Awake()
    {
        // シーンにインスタンスがまだ存在しない場合、自身をインスタンスとして設定する
        if (Instance == null)
        {
            Instance = this;
            // シーンをまたいでも破棄しない場合は以下の行を追加
            // DontDestroyOnLoad(gameObject); 
        }
        else
        {
            // すでに他のインスタンスが存在する場合は、自身を破棄する
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        onGame = true;
        point = 0;
    }

    // Update is called once per frame
    void Update()
    {
        onGame = !gameOverLineController.gameOver;
        if (onGame)
        {
            produceItemManager.ProduceItemUpdate();
            uiController.SetNextItem();
        }
        else
        {
            uiController.GameOver();
        }

        // if (Input.GetKeyDown(KeyCode.Space))
        // {
            
        // }
        
    }
    public void AddPoint(int scoreValue)
    {
        if (!onGame) return; // ゲームオーバー中はスコアを加算しない
        point += scoreValue;
        Debug.Log("score+"+scoreValue);
        uiController.UpdatePoint(point); // スコアが変更されたらUIを更新
    }
}
