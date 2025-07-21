using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int numEnemiesAlives;
    public int round;
    public GameObject[] spawnerPoints;
    public GameObject enemyPrefab, gameOverPanel;
    public TextMeshProUGUI roundText, numZombiesAlivesText,roundsSurvivedText;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        numZombiesAlivesText.text = $"Zombies: {numEnemiesAlives}";
        if (numEnemiesAlives == 0)
        {
            round++;
            roundText.text = $"Ronda: {round}";
            NextWave(round);
        }
    }

    public void NextWave(int round)
    {
        for (int i = 0; i < round; i++)
        {
            SpawnEnemy();
            numEnemiesAlives++;
        }
    }

    private void SpawnEnemy()
    {
        // crear una variable que eligue aleatoriamente  uno de los puntos de spawn que tenga en el esecario
        int randomPos = Random.Range(0, spawnerPoints.Length);
        // crea una gameobject que almacena el punto de spawn aleatorio
        GameObject spawnPoint = spawnerPoints[randomPos];
        // crea un enemigo en el punto de spawn aleatoria 
        Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity);
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        roundsSurvivedText.text = round.ToString();

        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestarGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void backToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
