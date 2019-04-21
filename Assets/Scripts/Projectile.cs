using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    #region Properties

    private Rigidbody2D rigidbodyReference;
    private CircleCollider2D colliderReference;

    [SerializeField]
    private float velocity;
    [SerializeField]
    private LayerMask whatIsDestructible;

    private RaycastHit2D hit;
    private Vector3 originalVelocity;

    #endregion

    #region Implementation

    public void ShootTo(Vector2 pDirection)
    {
        rigidbodyReference.velocity = pDirection * velocity;
        originalVelocity = rigidbodyReference.velocity;
    }

    #endregion

    #region Unity callbacks

    public void Awake()
    {
        rigidbodyReference = GetComponent<Rigidbody2D>();
        colliderReference = GetComponent<CircleCollider2D>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            hit = Physics2D.CircleCast(transform.position, colliderReference.radius, rigidbodyReference.velocity.normalized, 0.1f, whatIsDestructible);
            if (hit)
            {
                DestructibleTileMap damageable = hit.collider.gameObject.GetComponent<DestructibleTileMap>();
                if (!Equals(damageable, null))
                {
                    Vector2 positionInTile = (hit.point - (Vector2)transform.position).normalized;
                    damageable.Damage(this, hit.point + (positionInTile.normalized * 0.5f));
                }                
            }
            Destroy(this.gameObject);
        }
    }

    #endregion
}
