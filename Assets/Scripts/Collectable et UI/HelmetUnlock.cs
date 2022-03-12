using UnityEngine;
using UnityEngine.SceneManagement;

public class HelmetUnlock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(SceneManager.GetActiveScene().name);
    }
}
