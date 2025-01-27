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
    
    private bool m_Started = false;
    private int m_Points;
    private bool m_GameOver = false;
    private string currentPlayer;
    private string playerName;
    private int playerHighScore;
    private Dictionary<string, int> playersInfo = new Dictionary<string, int>();

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
        Debug.Log("Game Over Text = " + gameOverText.text);
        scoreText = GameObject.FindWithTag("Score Text").GetComponent<Text>();
        highScoreText = GameObject.FindWithTag("High Score Text").GetComponent<Text>();
        highScoreText.text = "Best Score : " + playerName + " : " + playerHighScore;
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
        SavePlayerInfo();
    }

    [Serializable]
    class SaveData{
        public Dictionary<string, int> playersInfo = new Dictionary<string, int>();
    }


    public void LoadPlayerInfo()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if(File.Exists(path)){
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            if(data.playersInfo.Count() == 0){
                playerName = " ";
                playerHighScore = 0;
            }
            else if(data.playersInfo.Count() == 1){
                playerName = data.playersInfo.Keys.Single();
                playerHighScore = data.playersInfo.Values.Single();
            }
            else{
                KeyValuePair<string, int> keyValuePair = data.playersInfo.Aggregate((x, y) => x.Value > y.Value ? x : y);
                playerName = keyValuePair.Key;
                playerHighScore = keyValuePair.Value;
            }

            
        }
    }

    public void SavePlayerInfo(){
        SaveData data = new SaveData();

        Debug.Log("saved at : " + Application.persistentDataPath + "/savefile.json");
        Debug.Log("data.playersInfo[" + playerName + "] = " + playerHighScore);
        data.playersInfo[playerName] = playerHighScore;
        string json = JsonUtility.ToJson(data);
        //string json = JsonConvert.SerializeObject(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void SetPlayerName(string name){
        currentPlayer = name;
    }
    public string GetPlayerName(){
        return currentPlayer;
    }

    public void SetBestPlayerName(string name){
        currentPlayer = name;
    }
    public string GetBestPlayerName(){
        return currentPlayer;
    }

    public void SetBestPlayerHighScore(int score){
        playerHighScore = score;
    }
    public int GetBestPlayerHighScore(){
        return playerHighScore;
    }
}
