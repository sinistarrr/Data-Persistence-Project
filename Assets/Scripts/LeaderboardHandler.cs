using System.Text;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class LeaderboardHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TextMeshProUGUI textMesh = GameObject.FindGameObjectWithTag("Leaderboard Content").GetComponent<TextMeshProUGUI>();
        StringBuilder myStringBuilder = new StringBuilder();
        MainManager.Instance.GetData().playersInfo.ForEach(playerInfo => myStringBuilder.Append(playerInfo.playerName + " : " + playerInfo.highScore + "\n"));
        textMesh.SetText(myStringBuilder.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
