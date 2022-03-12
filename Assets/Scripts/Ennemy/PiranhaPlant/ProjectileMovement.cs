using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] private int speed = 10;

    private SpriteRenderer _sr;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Lesgo");
        _sr = GetComponent<SpriteRenderer>();
        var direction = (GameObject.Find("Player").transform.position - transform.position).normalized;
        direction = new Vector3(direction.x, 0, 0);
        if (direction.x > 0) _sr.flipX = true;
        gameObject.GetComponent<Rigidbody2D>().AddForce(direction * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var pb = collision.gameObject.GetComponent<PlayerBehaviour>();
            if (pb != null)
                pb.CallEnnemyCollision();
        }

        if (!collision.gameObject.CompareTag("Ennemy"))
        {
            Destroy(gameObject);
        }
    }
}