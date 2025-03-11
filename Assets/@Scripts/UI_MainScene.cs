using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainScene : MonoBehaviour
{
    

    public void OnCharacterButton()
    {
        SceneManager.LoadScene("Select");
    }

    
}
