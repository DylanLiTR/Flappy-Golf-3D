using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleplayer : MonoBehaviour
{
    public void StartLevel(int level)
    {
        SceneManager.LoadScene(level);
    }
}
