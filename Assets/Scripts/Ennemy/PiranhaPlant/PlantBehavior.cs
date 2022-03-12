using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class PlantBehavior : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float shootDelay = 3f;
    [SerializeField] private float fireRate = 1.75f;
    [SerializeField] private int hp = 2;
    [SerializeField] private int pointDestruction = 5;
    private float _shootTimer;
    private SpriteRenderer _sr;
    private Animator _animator;
    private RaycastHit2D[] _raycasts;
    private float _tempsDebutInvulnerabilite;
    private bool _invulnerable;
    private const float DelaisInvulnerabilite = 1f;
    private bool _destructionEnCours;

    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int PlayerInRange = Animator.StringToHash("PlayerInRange");

    // Start is called before the first frame update
    void Start()
    {
        _raycasts = new RaycastHit2D[2];
        _sr = GetComponent<SpriteRenderer>();
        _animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (hp <= 0 && !_destructionEnCours)
        {
            GameManager.Instance.PlayerData.IncrScore(pointDestruction);
            GameObject o;
            (o = gameObject).GetComponent<BoxCollider2D>().enabled = false;
            Destroy(o);
            _destructionEnCours = true;
        }

        if (Time.fixedTime > _tempsDebutInvulnerabilite + DelaisInvulnerabilite)
            _invulnerable = false;
    }

    private void FixedUpdate()
    {
        var position = transform.position;
        _raycasts[0] = Physics2D.Raycast(position, Vector3.left, 5.5f);
        _raycasts[1] = Physics2D.Raycast(position, Vector3.right, 5.5f);
        if (_shootTimer > 0) _shootTimer -= Time.deltaTime * fireRate;


        foreach (var raycast in _raycasts)
        {
            if (raycast.collider == null || !raycast.collider.CompareTag("Player")) continue;
            if (!(_shootTimer <= 0)) continue;
            _animator.SetTrigger(PlayerInRange);
            ShootProjectile();
            _shootTimer = shootDelay;
        }
    }

    private void ShootProjectile()
    {
        _sr = GetComponent<SpriteRenderer>();
        var direction = (GameObject.Find("Player").transform.position - transform.position).normalized;
        if (direction.x > 0 && !_sr.flipX) _sr.flipX = true;
        else if (direction.x < 0 && _sr.flipX) _sr.flipX = false;
        Instantiate(projectile, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.tag.Equals("Player")) return;
        if (_invulnerable) return;
        hp--;
        _animator.SetTrigger(Hit);
        _tempsDebutInvulnerabilite = Time.fixedTime;
        _invulnerable = true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (_raycasts == null) return;
        foreach (var raycast in _raycasts)
        {
            Gizmos.DrawLine(transform.position, raycast.point);
        }
    }
#endif
}