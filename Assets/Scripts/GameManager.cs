using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    [SerializeField]
    private List<GameObject> levels = new List<GameObject>();
    [SerializeField]
    TMPro.TextMeshProUGUI levelName;
    [SerializeField]
    TMPro.TextMeshProUGUI scoreText;
    [SerializeField]
    TMPro.TextMeshProUGUI highScoreText;
    [SerializeField]
    private GameObject minerWilly;
    [SerializeField]
    private GameObject life;

    private int score = 0;
    private int highScore = 0;
    private int levelIndex = 0;
    private string levelNameText = "Central Cavern ";
    private int lives = 3;
    private float liferSpacing = 7f;
    private GameObject currentLevel;


    public void Awake() 
    {
        DontDestroyOnLoad(gameObject);
    }

    public void OnEnable()
    {
        CollectableBehaviour.collected += OnCollected;
        DeadlyBlockBehaviour.death += OnDeath;
    }

    public void OnDisable()
    {
        CollectableBehaviour.collected -= OnCollected;
        DeadlyBlockBehaviour.death -= OnDeath;
        // find all the Enemy objects in the scene and add their OnDeath method
        foreach (var enemy in FindObjectsByType<EnemyBahaviour>(FindObjectsSortMode.None))
        {
            EnemyBahaviour.death -= OnDeath;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText.text = $"Score {score.ToString("D6")}";
        highScoreText.text = $"High score {highScore.ToString("D6")}";
        levelName.text = levelNameText;

        // Intantiate the first level
        currentLevel = Instantiate(levels[levelIndex]);

        // Instantiate the miner willy
        Instantiate(minerWilly, new Vector3(-54, -10, 0), Quaternion.identity);

        // Instantiate the lives
        for (int i = 0; i < lives; i++)
        {
            var lifeInstance = Instantiate(life, transform);
            lifeInstance.transform.localPosition = new Vector3(i * liferSpacing, 0, 0);
        }

        // find all the Enemy objects in the scene and add their OnDeath method
        foreach (var enemy in FindObjectsByType<EnemyBahaviour>(FindObjectsSortMode.None))
        {
            EnemyBahaviour.death += OnDeath;
        }
    }

    private void OnCollected() 
    {
        score += 100;
        scoreText.text = $"Score {score.ToString("D6")}";
        if (score > highScore)
        {
            highScore = score;
            highScoreText.text = $"High score {highScore.ToString("D6")}";
        }
    }

    private void OnDeath()
    {
        // decrement lives
        lives--;

        // remove one of the life gameObjects
        if (lives < 0)
        {
            Debug.Log("Game Over");
        }
        else
        {
            Destroy(transform.GetChild(lives).gameObject);
        }

        // destroy the existing level (using the "Level" tag) and respan a new one
        if(currentLevel != null)
        {
            Destroy(currentLevel);
            currentLevel = Instantiate(levels[levelIndex]);
        }

        // respawn miner willy
        Instantiate(minerWilly, new Vector3(-54, -10, 0), Quaternion.identity);
    }

}