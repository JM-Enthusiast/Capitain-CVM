using UnityEngine;

public class PlantBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private float shootDelay = 3f;
    [SerializeField]
    private float fireRate = 1.75f;
    [SerializeField]
    private int hp = 2;
    [SerializeField]
    private int pointDestruction = 5;
    private float _shootTimer = 0f;
    private SpriteRenderer _sr;
    private Animator _animator;
    private RaycastHit2D[] _raycasts;
    private float _tempsDebutInvulnerabilite;
    private bool _invulnerable = false;
    public const float DelaisInvulnerabilite = 1f;
    private bool _destructionEnCours = false;
    // Start is called before the first frame update
    void Start()
    {
        _raycasts = new RaycastHit2D[2];
        _sr = this.GetComponent<SpriteRenderer>();
        _animator = this.gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (this.hp <= 0 && !this._destructionEnCours)
        {
            GameManager.Instance.PlayerData.IncrScore(this.pointDestruction);
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            GameObject.Destroy(this.transform.parent.gameObject);
            this._destructionEnCours = true;
        }

        if (Time.fixedTime > _tempsDebutInvulnerabilite + DelaisInvulnerabilite)
            _invulnerable = false;
    }

    private void FixedUpdate()
    {
        _raycasts[0] = Physics2D.Raycast(transform.position, Vector3.left, 5.5f);
        _raycasts[1] = Physics2D.Raycast(transform.position, Vector3.right, 5.5f);
        if (_shootTimer > 0) _shootTimer -= Time.deltaTime * fireRate;


        foreach (var raycast in _raycasts)
        {
            if (raycast.collider != null && raycast.collider.CompareTag("Player"))
            {
                if (_shootTimer <= 0)
                {
                    _animator.SetTrigger("PlayerInRange");
                    ShootProjectile();
                    _shootTimer = shootDelay;
                }
            }
        }
    }

    private void ShootProjectile()
    {
        _sr = this.GetComponent<SpriteRenderer>();
        var direction = (GameObject.Find("Player").transform.position - transform.position).normalized;
        if (direction.x > 0 && !_sr.flipX) _sr.flipX = true;
        else if (direction.x < 0 && _sr.flipX) _sr.flipX = false;
        Instantiate(projectile, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if (!_invulnerable)
            {
                hp--;
                _animator.SetTrigger("Hit");
                _tempsDebutInvulnerabilite = Time.fixedTime;
                _invulnerable = true;
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (_raycasts != null)
        {
            foreach (var raycast in _raycasts)
            {
                Gizmos.DrawLine(transform.position, raycast.point);
            }
        }
    }
#endif
}
