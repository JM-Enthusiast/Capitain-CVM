using UnityEngine;
using UnityEngine.SceneManagement;

public class FinDeNiveau : MonoBehaviour
{
    [SerializeField] private string sceneName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        Debug.Log("Félicitation, le niveau est terminé.");
        GameManager.Instance.PlayerData.CompleteLevel(SceneManager.GetActiveScene().name);
        GameManager.Instance.SaveData();
        GameManager.Instance.ChangerScene(sceneName);
    }
}
