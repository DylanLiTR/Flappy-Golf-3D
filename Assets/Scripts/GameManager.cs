using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Transforms")]
    [SerializeField] Transform camTransform;
    [SerializeField] Transform flagTransform;

    [Header("Scripts")]
    [SerializeField] Movement ball;
    [SerializeField] OrbitCamera camScript;

    [Header("UI")]
    [SerializeField] GameObject win;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject abovePlayerText;
    [SerializeField] GameObject highscore;
    [SerializeField] TextMeshProUGUI scoreMsg;
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

        int flaps = ball.flapCount;
        if (flaps == 1)
        {
            scoreMsg.SetText("Hole in One!");
        }
        else
        {
            scoreMsg.SetText("{0:0} Flaps!", flaps);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        ball.disabled = true;
        camScript.frozen = true;
        win.SetActive(true);
        StartCoroutine(camScript.LerpFromTo(camTransform.position, flagTransform.position + 25 * Vector3.up, 
            camTransform.rotation, Quaternion.Euler(new Vector3(90f, 0f, 0f)), 3f));

        if (!PlayerPrefs.HasKey("highscore" + level) || flaps < PlayerPrefs.GetInt("highscore" + level))
        {
            highscore.SetActive(true);
            PlayerPrefs.SetInt("highscore" + level, flaps);
        }
    }

    public void Death()
    {
        ball.disabled = true;
        respawnTimer.SetActive(true);
        Invoke("Restart", restartDelay);
        StartCoroutine(UpdateRespawnTimer());
    }

    public void Restart()
    {
        respawnTimer.SetActive(false);
        scored = false;
        Time.timeScale = 1;
        ball.disabled = false;
        ball.drowned = false;
        camScript.frozen = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoHome()
    {
        Time.timeScale = 1;
        camScript.frozen = false;
        Destroy(GameObject.Find("AudioManager"));
        SceneManager.LoadScene(0);
    }

    void Pause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        hazards.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        camScript.frozen = true;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        hazards.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        camScript.frozen = false;
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
