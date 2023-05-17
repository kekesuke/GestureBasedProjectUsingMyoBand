using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
using TMPro;

public class HandleEnemyDead : MonoBehaviour
{
    public static HandleEnemyDead instance;
    public TextMeshProUGUI scoreText;

    private void Awake()
    {

    }
    public int enemyKilled;
    public int mobsToKill;
    public int score;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckIfallMobsDead()
    {
        scoreText.text = "Score: " + score.ToString();
        if (enemyKilled == mobsToKill)
        {

            FindObjectOfType<PlayerController>().GetComponent<PathFollower>().enabled = true;
            UIController ui = FindObjectOfType<UIController>();
            ui.infoText.text = "Great job continue!";
            FindObjectOfType<InputController>().GetComponent<GameLevel>().colidedAlready = false;
            FindObjectOfType<InputController>().GetComponent<HandleEnemyDead>().enemyKilled = 0;
            StartCoroutine(ui.DisplayText(2f));

        }


    }
}
