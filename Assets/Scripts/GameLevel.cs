using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;

public class GameLevel : MonoBehaviour
{
    private int spawnedZombies;
    public GameObject[] spawnPoints;
    public GameObject[] zombies;
    public bool colidedAlready;




    public int numberOfZombies;
    void Start()
    {
        spawnedZombies = 0;

    }

    // Update is called once per frame
    void Update()
    {


    }

    IEnumerator SpawnZombies()
    {
        while (spawnedZombies < numberOfZombies)
        {
            yield return new WaitForSeconds(1f);

            Instantiate(zombies[0], spawnPoints[0].transform.position, Quaternion.identity);
            spawnedZombies++;

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player" && !colidedAlready)
        {
            StopPlayer();
            colidedAlready = true;
        }
    }


    public void StopPlayer()
    {
        FindObjectOfType<PlayerController>().GetComponent<PathFollower>().enabled = false;
        UIController ui = FindObjectOfType<UIController>();
        ui.infoText.text = "Kill all the zombies to continue!";
        StartCoroutine(ui.DisplayText(5f));
        StartCoroutine(SpawnZombies());
        FindObjectOfType<InputController>().GetComponent<HandleEnemyDead>().mobsToKill = numberOfZombies;
        //Destroy(gameObject, 6f);
    }


}
