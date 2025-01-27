using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textFieldName;
    [SerializeField] private TMP_InputField inputFieldName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Debug.Log(textFieldName.text);
        // Debug.Log(MainManager.Instance.GetPlayerName());
        textFieldName.text = "Best Score : " + MainManager.Instance.GetBestPlayerName() + " : " + MainManager.Instance.GetBestPlayerHighScore(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNew(){
        MainManager.Instance.SetPlayerName(inputFieldName.text);
        Debug.Log("inputFieldName.text = " + inputFieldName.text);
        Debug.Log("MainManager.Instance.GetPlayerName() = " + MainManager.Instance.GetPlayerName());
        // SceneManager.LoadScene(1);
        var op = SceneManager.LoadSceneAsync(1);
        op.completed += (x) => {
            Debug.Log("Loaded");
            MainManager.Instance.VariablesInit();
        };
    }

    public void Exit(){
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit(); // original code to quit Unity Player
        #endif
        MainManager.Instance.SavePlayerInfo();
        
    }
}
