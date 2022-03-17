using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var helmets = GameManager.Instance.PlayerData.UnlockedHelmets;
        foreach (var helmet in helmets)
        {
            GameObject.Find(helmet).transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
