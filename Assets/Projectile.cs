using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 5f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 direction, Vector3 initialVelocity)
    {
        rb.linearVelocity = direction.normalized * speed + initialVelocity;
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.isKinematic = true;
        transform.parent = collision.transform;
    }
}