using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
public class HelmetUnlock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Check if is unlocked and if it is, change the color to this dans disable the trigger
        if (!GameManager.Instance.PlayerData.HasUnlockedHelmet(SceneManager.GetActiveScene().name)) return;
        MarkAsUnlocked();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameManager.Instance.PlayerData.AddUnlockableHelmet(SceneManager.GetActiveScene().name);
        MarkAsUnlocked();
    }

    private void MarkAsUnlocked()
    {
        var sr = gameObject.GetComponent<SpriteRenderer>();
        sr.color = new Color(0, 0, 0, 0.75f);
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }
}