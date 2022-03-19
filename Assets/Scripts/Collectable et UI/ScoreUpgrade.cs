using UnityEngine;

public class ScoreUpgrade : MonoBehaviour
{
    /// <summary>
    /// Valeur de l'énergie regagner au contact
    /// </summary>
    [SerializeField] private int _gainPoint = 3;

    [SerializeField] private AudioClip _clip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.tag.Equals("Player")) return;
        GameManager.Instance.AudioManager.PlayClipAtPoint(_clip, transform.position);
        GameManager.Instance.PlayerData.IncrScore(_gainPoint);
        Destroy(gameObject);
    }
}