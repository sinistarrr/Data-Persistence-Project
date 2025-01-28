using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;


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

    public void StartNew(int sceneNumber){
        switch(sceneNumber){
            case 1:
                var op = SceneManager.LoadSceneAsync(1);
                op.completed += (x) => {
                    MainManager.Instance.VariablesInit();
                    MainManager.Instance.SetPlayerName(inputFieldName.text);
                };
                break;
            case 2:
                SceneManager.LoadSceneAsync(2);
                break;
            case 3:
                SceneManager.LoadSceneAsync(3);
                break;
            default:
                Debug.Log("Error Couldn't load scene");
                break;
        }
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
