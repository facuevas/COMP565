using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();

        if (player.GetComponent<PlayerStats>().GetCurrentHealth() <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("Game over");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
