using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeController : BulletController
{
    [Range(20.0f, 75.0f)] public float LaunchAngle;
    [SerializeField] private float timeToExplode = 2f;
    [SerializeField] private GameObject explodeParticles;
    [SerializeField] private GameObject explodeDamage;

    // cache
    private Rigidbody rigid;

    // Use this for initialization
    void Start() {
        rigid = GetComponent<Rigidbody>();
        Invoke("Explode", timeToDestroy);

        if(enemyBullet)
            Launch();
        else {
            transform.LookAt(target);
            rigid.AddForce(transform.forward * speed, ForceMode.Impulse);
        }

    }

    void Launch() {
        // think of it as top-down view of vectors: 
        //   we don't care about the y-component(height) of the initial and target position.
        Vector3 projectileXZPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
        Vector3 targetXZPos = new Vector3(target.x, 0.0f, target.z);

        // rotate the object to face the target
        transform.LookAt(targetXZPos);

        // shorthands for the formula
        float R = Vector3.Distance(projectileXZPos, targetXZPos);
        float G = Physics.gravity.y;
        float tanAlpha = Mathf.Tan(LaunchAngle * Mathf.Deg2Rad);
        float H = target.y - transform.position.y;

        // calculate the local space components of the velocity 
        // required to land the projectile on the target object 
        float Vz = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)));
        float Vy = tanAlpha * Vz;

        // create the velocity vector in local space and get it in global space
        Vector3 localVelocity = new Vector3(0f, Vy, Vz);
        Vector3 globalVelocity = transform.TransformDirection(localVelocity);

        // launch the object by setting its initial velocity and flipping its state
        rigid.velocity = globalVelocity;
    }

    void Explode() {
        GameObject particles = Instantiate(explodeParticles, transform.position, Quaternion.identity);
        Destroy(particles, 1f);

        GameObject explode = Instantiate(explodeDamage, transform.position, Quaternion.identity);
        Explosion explosion = explode.GetComponent<Explosion>();
        explosion.damage = damage;
        explosion.enemyBullet = enemyBullet;

        Destroy(explode, 0.5f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        Invoke("Explode", timeToExplode);
    }
}
