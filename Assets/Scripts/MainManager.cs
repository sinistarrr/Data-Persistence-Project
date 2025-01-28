using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainManager : MonoBehaviour
{
    public Brick brickPrefab;
    public int lineCount = 6;

    private Rigidbody ball;
    private Text scoreText;
    private Text highScoreText;
    private Text gameOverText;
    private float gameSpeed = 1;
    private bool m_Started = false;
    private int m_Points;
    private bool m_GameOver; // = false;
    private string currentPlayer;
    private int currentPlayerHighScore = 0;
    private string playerName;
    private int playerHighScore;
    private SaveData data;

    public static MainManager Instance;

    private void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadPlayerInfo();
        }
        else{
            Destroy(gameObject);
        }

    }

    // Start is called before the first frame update


    void Start()
    {
        // ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody>();
        // gameOverText = GameObject.FindGameObjectWithTag("Game Over Text");
        // scoreText = GameObject.FindGameObjectWithTag("Score Text").GetComponent<Text>();
        // highScoreText = GameObject.FindGameObjectWithTag("High Score Text").GetComponent<Text>();
        // highScoreText.text = "Best Score : " + playerName + " : " + playerHighScore;
        // const float step = 0.6f;
        // int perLine = Mathf.FloorToInt(4.0f / step);
        
        // int[] pointCountArray = new [] {1,1,2,2,5,5};
        // for (int i = 0; i < lineCount; ++i)
        // {
        //     for (int x = 0; x < perLine; ++x)
        //     {
        //         Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
        //         var brick = Instantiate(brickPrefab, position, Quaternion.identity);
        //         brick.PointValue = pointCountArray[i];
        //         brick.onDestroyed.AddListener(AddPoint);
        //     }
        // }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                ball.transform.SetParent(null);
                ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var op = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
                op.completed += (x) => {
                    Debug.Log("Loaded");
                    VariablesInit();
                };
                // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void VariablesInit(){
        ball = GameObject.FindWithTag("Ball").GetComponent<Rigidbody>();
        gameOverText = GameObject.FindWithTag("Game Over Text").GetComponent<Text>();
        scoreText = GameObject.FindWithTag("Score Text").GetComponent<Text>();
        highScoreText = GameObject.FindWithTag("High Score Text").GetComponent<Text>();
        highScoreText.text = "Best Score : " + playerName + " : " + playerHighScore;
        m_GameOver = false;
        m_Started = false;
        m_Points = 0;
        Time.timeScale = gameSpeed;
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < lineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(brickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }
    void AddPoint(int point)
    {
        m_Points += point;
        scoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        gameOverText.enabled = true;
        if(m_Points > playerHighScore){
            playerHighScore = m_Points;
            playerName = currentPlayer;
            highScoreText.text = "Best Score : " + playerName + " : " + playerHighScore;
        }
        if(m_Points > currentPlayerHighScore){
            currentPlayerHighScore = m_Points;
        }
        SavePlayerInfo();
    }

    [Serializable]
    public class PlayerInfo{
        public string playerName;
        public int highScore;
    }

    [Serializable]
    public class SaveData {
        public List<PlayerInfo> playersInfo = new List<PlayerInfo>();
    }

    public void LoadPlayerInfo()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if(File.Exists(path)){
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<SaveData>(json);
            data.playersInfo.Sort((a, b) => b.highScore.CompareTo(a.highScore));
            if(data.playersInfo.Count() == 0){
                playerName = " ";
                playerHighScore = 0;
            }
            else{
                playerHighScore = data.playersInfo.Max(elem => elem.highScore);
                playerName = data.playersInfo.Find(elem => elem.highScore == playerHighScore).playerName;
            }
            
        }
    }

    public void SavePlayerInfo(){
        Debug.Log("saved at : " + Application.persistentDataPath + "/savefile.json");

        int saveIndex = data.playersInfo.FindIndex(elem => elem.playerName == currentPlayer);
        if(saveIndex != -1){
            if(currentPlayerHighScore > data.playersInfo[saveIndex].highScore){
                data.playersInfo[saveIndex].highScore = currentPlayerHighScore;
            }
        }
        else{
            PlayerInfo playerInfo = new PlayerInfo();
            playerInfo.highScore = currentPlayerHighScore;
            playerInfo.playerName = currentPlayer;
            data.playersInfo.Add(playerInfo);
        }
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void SetPlayerName(string name){
        currentPlayer = name;
    }
    public string GetPlayerName(){
        return currentPlayer;
    }
    public void SetPlayerHighScore(int score){
        currentPlayerHighScore = score;
    }
    public int GetPlayerHighScore(){
        return currentPlayerHighScore;
    }

    public void SetBestPlayerName(string name){
        playerName = name;
    }
    public string GetBestPlayerName(){
        return playerName;
    }

    public void SetBestPlayerHighScore(int score){
        playerHighScore = score;
    }
    public int GetBestPlayerHighScore(){
        return playerHighScore;
    }
    public SaveData GetData(){
        return data;
    }

    public void SetGameSpeed(float speed){
        gameSpeed = speed;
    }
}
