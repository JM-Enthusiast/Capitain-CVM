using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class ProjectileMovement : MonoBehaviour
{
    [SerializeField]
    private int speed = 10;

    private SpriteRenderer _sr;
    // Start is called before the first frame update
    void Start()
    {
        _sr = this.GetComponent<SpriteRenderer>();
        var direction = (GameObject.Find("Player").transform.position - transform.position).normalized;
        direction = new Vector3(direction.x, 0, 0);
        if (direction.x > 0) _sr.flipX = true;
        gameObject.GetComponent<Rigidbody2D>().AddForce(direction * speed);
    }
}
