using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject win;
    [SerializeField] GameObject ball;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject orbitCamera;
    [SerializeField] GameObject abovePlayerText;
    [SerializeField] GameObject frameCounter;
    [SerializeField] int level;

    [Header("Hazards/Respawn")]
    [SerializeField] GameObject hazards;
    [SerializeField] GameObject respawnTimer;
    [SerializeField, Range(1f, 5f)] float restartDelay = 3f;

    bool scored;

    void Update()
    {
        // Check if cancel key was pressed to pause game when the player has not scored yet
        if (!scored && Input.GetButtonDown("Cancel"))
        {
            Pause();
        }
    }

    public void Scored()
    {
        scored = true;
        abovePlayerText.SetActive(false);

        GameObject scoreMsg = win.transform.Find("ScoreMessage").gameObject;

        int flaps = ball.GetComponent<Movement>().flapCount;
        if (flaps == 1)
        {
            scoreMsg.GetComponent<TextMeshProUGUI>().SetText("Hole in One!");
        }
        else
        {
            scoreMsg.GetComponent<TextMeshProUGUI>().SetText("{0:0} Flaps!", flaps);
        }
        win.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        ball.GetComponent<Movement>().disabled = true;

        if (!PlayerPrefs.HasKey("highscore" + level) || flaps < PlayerPrefs.GetInt("highscore" + level))
        {
            PlayerPrefs.SetInt("highscore" + level, flaps);
        }
    }

    public void Death()
    {
        ball.GetComponent<Movement>().disabled = true;
        respawnTimer.SetActive(true);
        Invoke("Restart", restartDelay);
        StartCoroutine(UpdateRespawnTimer());
    }

    public void Restart()
    {
        respawnTimer.SetActive(false);
        scored = false;
        Time.timeScale = 1;
        ball.GetComponent<Movement>().disabled = false;
        ball.GetComponent<Movement>().drowned = false;
        orbitCamera.GetComponent<OrbitCamera>().frozen = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoHome()
    {
        Time.timeScale = 1;
        orbitCamera.GetComponent<OrbitCamera>().frozen = false;
        Destroy(GameObject.Find("AudioManager"));
        SceneManager.LoadScene(0);
    }

    void Pause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        hazards.SetActive(false);
        frameCounter.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        orbitCamera.GetComponent<OrbitCamera>().frozen = true;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        hazards.SetActive(true);
        frameCounter.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        orbitCamera.GetComponent<OrbitCamera>().frozen = false;
    }

    float timeLeft;
    public IEnumerator UpdateRespawnTimer()
    {
        timeLeft = restartDelay;
        while (timeLeft > 0)
        {
            respawnTimer.GetComponent<TextMeshProUGUI>().SetText("Respawning in {0:0}", timeLeft);
            yield return new WaitForSeconds(1.0f);
            timeLeft--;
        }
    }
}
