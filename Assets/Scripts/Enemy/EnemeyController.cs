using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeyController : MonoBehaviour
{
    bool isDead;
    public int health;
    private GameObject gameManger;

    // Start is called before the first frame update
    void Start()
    {
        gameManger = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) HandleDead();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            isDead = true;
        }
    }

    private void HandleDead()
    {
        
        Destroy(gameObject, 1f);
    }
}
