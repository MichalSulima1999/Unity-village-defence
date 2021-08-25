using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private GameObject bulletDecal;
    [SerializeField] protected float speed = 50f;
    [SerializeField] protected float timeToDestroy = 3f;
    [SerializeField] protected bool enemyBullet;

    [SerializeField] Rigidbody rbody;

    public int damage { get; set; }

    public Vector3 target { get; set; }
    public bool hit { get; set; }

    // Start is called before the first frame update
    private void Start() {
        transform.LookAt(target);
        rbody.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.layer == 6) {
            ContactPoint contact = other.GetContact(0);
            GameObject decal = GameObject.Instantiate(bulletDecal, contact.point + contact.normal * 0.001f, Quaternion.LookRotation(contact.normal));
            Destroy(decal, 10f);
        } else if(!enemyBullet && other.gameObject.CompareTag("Enemy")) {
            other.gameObject.GetComponent<EnemyStats>().takeDamage(damage);
        } else if(enemyBullet && other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<PlayerStats>().takeDamage(damage);
        } else if (enemyBullet && other.gameObject.CompareTag("Base")) {
            other.gameObject.GetComponent<PlayerBase>().TakeDamage(damage);
        } else if (enemyBullet && other.gameObject.CompareTag("Turret")) {
            other.gameObject.GetComponent<Tower>().TakeDamage(damage);
        }

        
        Destroy(gameObject);
    }
}
