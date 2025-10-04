using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLevelManager : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void ReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
