using UnityEngine;

public class GameInputManager : MonoBehaviour
{
    public static GameInputManager instance = null;
    public PlayerInput PlayerInput { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            PlayerInput = new PlayerInput();
            PlayerInput.Enable();
        }
        else if (instance != this)
            Destroy(gameObject); 
    }
    private void OnDestroy()
    {
        PlayerInput?.Dispose();
    }

}
