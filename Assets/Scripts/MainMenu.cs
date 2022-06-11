using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject singleplayerMenu;
    [SerializeField] int levelCount;

    // Quits the application/game
    public void Exit ()
    {
        Application.Quit();
    }

    void Start()
    {
        // Loops through all the levels to display the player's high score
        for (int i = 1; i <= levelCount; ++i)
        {
            // Check if the player has set a high score for each level
            if (PlayerPrefs.HasKey("highscore" + i))
            {
                // Sets the high score text in the main manu
                GameObject lvl = singleplayerMenu.transform.Find("Level " + i).gameObject;
                GameObject highscore = lvl.transform.Find("Highscore").gameObject;
                highscore.GetComponent<TextMeshProUGUI>().SetText("Highscore: {0:0}", PlayerPrefs.GetInt("highscore" + i));
            }
        }
    }
}
