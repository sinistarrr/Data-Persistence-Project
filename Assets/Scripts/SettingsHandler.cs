using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{

    [SerializeField] private Slider gameSpeedSlider;
    [SerializeField] private TextMeshProUGUI sliderText = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameSpeedSliderChange(){
        sliderText.text = Math.Round(gameSpeedSlider.value, 2).ToString();
    }
    public void ApplyChanges(){
        MainManager.Instance.SetGameSpeed(gameSpeedSlider.value);
    }
    public void BackToMenu(){
        SceneManager.LoadScene(0);
    }
}
