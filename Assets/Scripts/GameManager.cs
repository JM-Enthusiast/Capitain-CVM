using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Properties

    [SerializeField] private GameObject fondu;
    /// <summary>
    /// Référence au GameManager
    /// </summary>
    private static GameManager _instance;
    /// <summary>
    /// Permet d'accès à l'instance en cours du GameManager
    /// </summary>
    public static GameManager Instance { get { return _instance; } }
    /// <summary>
    /// Contient les données de jeu
    /// </summary>
    private PlayerData _playerData;
    public PlayerData PlayerData { get { return _playerData; } }

    private AudioManager _audioManager;
    public AudioManager AudioManager { get { return _audioManager; } }
    #endregion

    #region Methods
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            Destroy(fondu);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(fondu);

            // Initialisation des données de jeu
            LoadPlayerData();
            _audioManager = GetComponent<AudioManager>();
        }
    }

    private void Start()
    {
        SaveData();
        SceneManager.activeSceneChanged += ChangementScene;
        ChangementScene(new Scene(), SceneManager.GetActiveScene());
    }

    public void SaveData()
    {
        StartCoroutine(SaveData(PlayerData));
    }

    public IEnumerator SaveData(PlayerData data)
    {
        using (var stream = new StreamWriter(
            Path.Combine(Application.persistentDataPath, "savedata_encrypt.json"),
            false, Encoding.UTF8))
        {
            stream.Write(PlayerDataJson.WriteJson(data));
            Debug.Log(Path.Combine(Application.persistentDataPath, "savedata_encrypt.json"));
        }
        yield return new WaitForEndOfFrame();
    }

    private void LoadPlayerData()
    {
        string path = Path.Combine(Application.persistentDataPath, "savedata_encrypt.json");
        if (File.Exists(path))
        {
            using (StreamReader stream = new StreamReader(path,
            Encoding.UTF8))
            {
                _playerData = PlayerDataJson.ReadJson(stream.ReadToEnd());
            }
        }
        else
        {
            _playerData = new PlayerData(4);
            SaveData();
        }
    }

    public void RechargerNiveau()
    {
        PlayerData.UIPerteEnergie = null;
        PlayerData.UIPerteVie = null;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name,
            LoadSceneMode.Single);
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public void ChangerScene(string nomScene)
    {
        _audioManager.StopAudio(0.3f);
        if (!nomScene.Equals("MainMenu"))
            fondu.SetActive(true);
        SceneManager.LoadScene(nomScene);
    }

    public void ChangementScene(Scene current, Scene next)
    {
        fondu.SetActive(false);
    }

    #endregion
}
