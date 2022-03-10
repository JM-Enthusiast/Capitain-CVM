using UnityEngine;

public class PlantBehavior : MonoBehaviour
{
    private RaycastHit2D _hit, _hitBehind;
    [SerializeField]
    private GameObject projectile;
    // Start is called before the first frame update
    void Start()
    {
        _hit = Physics2D.Raycast(transform.position, Vector3.left, 5.5f);
        _hitBehind = Physics2D.Raycast(transform.position, Vector3.right, 5.5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        _hit = Physics2D.Raycast(transform.position, Vector3.left, 5.5f);
        _hitBehind = Physics2D.Raycast(transform.position, Vector3.right, 5.5f);

        if (_hit.collider != null && _hit.collider.CompareTag("Player"))
        {
            ShootProjectile();
        }
        if (_hitBehind.collider != null && _hitBehind.collider.CompareTag("Player"))
        {
            ShootProjectile();
        }
    }

    private void ShootProjectile()
    {
        Instantiate(projectile, transform.position, Quaternion.identity);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, _hit.point);
        Gizmos.DrawLine(transform.position, _hitBehind.point);
    }
#endif
}
