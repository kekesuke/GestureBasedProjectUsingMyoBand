using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBullets : MonoBehaviour
{
    public Rigidbody rb;

    public GameObject explosion;

    public LayerMask enemyLayer;
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;
    public int explosionDamage;
    public float explosionRange;
    public float explosionForce;

    public float headDamage, legDamage, bodyDamage;

    //lifetime
    public int maxCollisions;
    public float maxLifeTime;
    private float lifeTime;
    public bool explodeOnTouch = true;

    private GameObject expl;

    int collisions;
    PhysicMaterial physicMaterial;


    private void Start()
    {
        SetUp();
    }

    // Update is called once per frame
    private void Update()
    {
        //when to explode
        if (collisions == maxCollisions)
        {
            Explode();
            Debug.Log("test1");
        }

        //coundown lifetime left
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {

            Explode();
            lifeTime = maxLifeTime;
        }
    }

    private void Explode()
    {
        Debug.Log("test");
        collisions = 0;
        if (explosion != null)
        {
            Destroy(Instantiate(explosion, transform.position, Quaternion.identity), 3f);
            Destroy(gameObject, 0.05f);
        }
        //check for enemies;
        // Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, enemyLayer);
        // for (int i = 0; i < enemies.Length; i++)
        // {
        //     enemies[i].GetComponent<EnemeyController>().TakeDamage(explosionDamage);

        //     //Add explosion force (if enemy has a rigidbody)
        //     if (enemies[i].GetComponent<Rigidbody>())
        //         enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);
        // }

        //add delay so everything is synced and executed

    }

    private void OnCollisionEnter(Collision collision)
    {

        //count up collisions

        switch (collision.collider.gameObject.tag)
        {
            case ("EnemyHead"):
                Debug.Log("head");
                collisions++;
                Explode();
                collision.collider.transform.parent.transform.parent.GetComponent<Zombie>().TakeDamage(headDamage);
                break;
            case ("EnemyBody"):

                Debug.Log("body");
                Explode();
                collision.collider.transform.parent.transform.parent.GetComponent<Zombie>().TakeDamage(bodyDamage);
                collisions++;
                break;
            case ("EnemyLegs"):
                Debug.Log("legs");
                Explode();
                collision.collider.transform.parent.transform.parent.GetComponent<Zombie>().TakeDamage(legDamage);
                collisions++;
                break;
        }

        if (collision.collider.CompareTag("Ground") && explodeOnTouch)
        {
            Explode();
        }


    }

    private void SetUp()
    {
        physicMaterial = new PhysicMaterial();
        physicMaterial.bounciness = bounciness;
        physicMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        physicMaterial.bounceCombine = PhysicMaterialCombine.Maximum;
        lifeTime = maxLifeTime;
        //assign material to collider

        GetComponent<SphereCollider>().material = physicMaterial;
        rb.useGravity = useGravity;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
