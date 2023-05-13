using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : Photon.MonoBehaviour
{
    [SerializeField] Transform flagTransform;

    [Header("Multiplayer")]
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] GameObject readyButton;
    [SerializeField] GameObject sceneCamera;
    [SerializeField] GameObject feedGrid;
    [SerializeField] GameObject playerFeed;
    [SerializeField] PhotonView photonView;

    [Header("UI")]
    [SerializeField] GameObject win;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameplayMenu;
    [SerializeField] GameObject highscore;
    [SerializeField] CustomizationMenu customizationMenu;
    [SerializeField] LoadData loadData;
    [SerializeField] TextMeshProUGUI scoreMsg;
    [SerializeField] TextMeshProUGUI roomText;
    [SerializeField] int level;

    [Header("Hazards/Respawn")]
    [SerializeField] GameObject hazards;
    [SerializeField] GameObject respawnTimer;
    [SerializeField] GameObject flag;
    [SerializeField, Range(1f, 5f)] float restartDelay = 3f;
    public GameObject ball;

    Movement movement;
    OrbitCamera camScript;
    Transform camTransform;
    GameObject player;
    bool scored;
    int playerCount;

    string[] rank = {"1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th"};

    void Awake()
    {
        if (PhotonNetwork.inRoom)
        {
            gameCanvas.SetActive(true);
            playerCount = PhotonNetwork.room.PlayerCount;
            roomText.SetText(playerCount + " players in lobby");
            if (PhotonNetwork.isMasterClient)
            {
                Hashtable hash = new Hashtable();
                hash.Add("readyCount", 0);
                hash.Add("place", 0);
                PhotonNetwork.room.SetCustomProperties(hash);
            }
        }
        else
        {
            SpawnPlayer();
        }
    }

    void Update()
    {
        // Check if cancel key was pressed to pause game when the player has not scored yet
        if (!scored && Input.GetButtonDown("Cancel"))
        {
            Pause();
            Debug.Log("Paused");
        }
    }

    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        ++playerCount;
        roomText.SetText(playerCount + " players in lobby");

        GameObject msg = Instantiate(playerFeed, new Vector2(0, 0), Quaternion.identity);
        msg.transform.SetParent(feedGrid.transform, false);
        msg.GetComponent<TextMeshProUGUI>().SetText(newPlayer.NickName + " joined the game");
        msg.GetComponent<TextMeshProUGUI>().color = Color.green;
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer newPlayer)
    {
        GameObject msg = Instantiate(playerFeed, new Vector2(0, 0), Quaternion.identity);
        msg.transform.SetParent(feedGrid.transform, false);
        msg.GetComponent<TextMeshProUGUI>().SetText(newPlayer.NickName + " left the game");
        msg.GetComponent<TextMeshProUGUI>().color = Color.red;
    }

    public void Ready()
    {
        readyButton.SetActive(false);
        int readyCount = (int)PhotonNetwork.room.CustomProperties["readyCount"];
        ++readyCount;
        if (readyCount == PhotonNetwork.room.PlayerCount)
        {
            photonView.RPC(nameof(StartGame), PhotonTargets.AllBuffered);
            return;
        }
        Hashtable hash = new Hashtable();
        hash.Add("readyCount", readyCount);
        PhotonNetwork.room.SetCustomProperties(hash);
    }

    [PunRPC]
    void StartGame()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        if (PhotonNetwork.inRoom)
        {
            player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(this.transform.position.x, this.transform.position.y), Quaternion.identity, 0);
        }
        else
        {
            player = Instantiate(playerPrefab, new Vector3(this.transform.position.x, this.transform.position.y), Quaternion.identity);
        }
        GameObject playerCamera = player.transform.Find("OrbitCamera").gameObject;
        playerCamera.SetActive(true);
        camTransform = playerCamera.transform;
        camScript = playerCamera.GetComponent<OrbitCamera>();

        GameplayController gameplayController = gameplayMenu.GetComponent<GameplayController>();
        gameplayController.orbitCamera = playerCamera;

        ball = player.transform.Find("Ball").gameObject;
        customizationMenu.customizationScript = ball.GetComponent<Customization>();
        movement = ball.GetComponent<Movement>();
        loadData.applyCustomization();

        ClothSphereColliderPair[] sphereColliders = flag.GetComponent<Cloth>().sphereColliders;
        System.Array.Resize(ref sphereColliders, sphereColliders.Length + 1);
        sphereColliders[sphereColliders.Length - 1] = new ClothSphereColliderPair(ball.GetComponent<SphereCollider>());
        flag.GetComponent<Cloth>().sphereColliders = sphereColliders;

        gameCanvas.SetActive(false);
        sceneCamera.SetActive(false);
        Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
    }

    public void Scored()
    {
        scored = true;

        if (!PhotonNetwork.inRoom)
        {
            int flaps = movement.flapCount;
            if (flaps == 1)
            {
                scoreMsg.SetText("Hole in One!");
            }
            else
            {
                scoreMsg.SetText("{0:0} Flaps!", flaps);
            }

            if (!PlayerPrefs.HasKey("highscore" + level) || flaps < PlayerPrefs.GetInt("highscore" + level))
            {
                highscore.SetActive(true);
                PlayerPrefs.SetInt("highscore" + level, flaps);
            }
            win.transform.Find("Restart").gameObject.SetActive(true);
        }
        else
        {
            int place = (int)PhotonNetwork.room.CustomProperties["place"];
            scoreMsg.SetText("You placed " + rank[place]);
            ++place;
            Hashtable hash = new Hashtable();
            hash.Add("place", place);
            PhotonNetwork.room.SetCustomProperties(hash);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        movement.disabled = true;
        camScript.frozen = true;
        pauseMenu.SetActive(false);
        win.SetActive(true);
        StartCoroutine(camScript.LerpFromTo(camTransform.position, flagTransform.position + 25 * Vector3.up, 
            camTransform.rotation, Quaternion.Euler(new Vector3(90f, 0f, 0f)), 3f));
    }

    public void Death()
    {
        if (PhotonNetwork.inRoom)
        {
            PhotonNetwork.Destroy(player);
            SpawnPlayer();
            pauseMenu.SetActive(false);
        }
        else
        {
            movement.disabled = true;
            respawnTimer.SetActive(true);
            Invoke("Restart", restartDelay);
            StartCoroutine(UpdateRespawnTimer());
        }
    }

    public void Restart()
    {
        respawnTimer.SetActive(false);
        scored = false;
        Time.timeScale = 1;
        movement.disabled = false;
        camScript.disabled = false;
        movement.drowned = false;

        if (PhotonNetwork.inRoom)
        {
            Death();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void GoHome()
    {
        Time.timeScale = 1;
        camScript.frozen = false;
        camScript.disabled = false;
        Destroy(GameObject.Find("AudioManager"));
        if (PhotonNetwork.inRoom) PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
    }

    void Pause()
    {
        if (!PhotonNetwork.inRoom) Time.timeScale = 0;
        else movement.disabled = true;
        pauseMenu.SetActive(true);
        hazards.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        camScript.disabled = true;
    }

    public void Resume()
    {
        if (!PhotonNetwork.inRoom) Time.timeScale = 1;
        else movement.disabled = false;
        pauseMenu.SetActive(false);
        hazards.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        camScript.disabled = false;
    }

    float timeLeft;
    public IEnumerator UpdateRespawnTimer()
    {
        timeLeft = restartDelay;
        while (timeLeft > 0)
        {
            respawnTimer.GetComponent<TextMeshProUGUI>().SetText("Respawning in {0:0}", timeLeft);
            yield return new WaitForSeconds(1.0f);
            --timeLeft;
        }
    }
}
