using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
    private int levelNameIndex = 0;
    private string levelNameText = "Central Cavern ";
    private int lives = 3;
    private float liferSpacing = 7f;

    public void Awake() 
    {
        DontDestroyOnLoad(gameObject);
    }

    public void OnEnable()
    {
        CollectableBehaviour.collected += OnCollected;
        DeadlyBlockBehaviour.death += OnDeath;
        // find all the Eneemy objects in the scene and add their OnDeath method
        foreach (var enemy in FindObjectsByType<EnemyBahaviour>(FindObjectsSortMode.None))
        {
            EnemyBahaviour.death += OnDeath;
        }
    }

    public void OnDisable()
    {
        CollectableBehaviour.collected -= OnCollected;
        DeadlyBlockBehaviour.death -= OnDeath;
        // find all the Eneemy objects in the scene and add their OnDeath method
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
        Instantiate(minerWilly, new Vector3(-54, -10, 0), Quaternion.identity);
        for (int i = 0; i < lives; i++)
        {
            var lifeInstance = Instantiate(life, transform);
            lifeInstance.transform.localPosition = new Vector3(i * liferSpacing, 0, 0);
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

        // respawn miner willy
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Instantiate(minerWilly, new Vector3(-54, -10, 0), Quaternion.identity);
    }

}
