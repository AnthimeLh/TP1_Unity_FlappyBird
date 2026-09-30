using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// Le GameManager est le "chef d'orchestre" du jeu : il centralise
// le score et l'état de fin de partie. On utilise le patron de
// conception "Singleton" (instance statique) pour que n'importe
// quel script (comme Bird.cs) puisse l'appeler facilement via
// GameManager.instance, sans avoir à le chercher dans la scène.
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Interface (TextMeshPro)")]
    public TMP_Text scoreText;       // Texte UI affichant le score en jeu
    public GameObject gameOverPanel; // Panneau UI affiché à la fin
    public TMP_Text finalScoreText;  // (optionnel) score affiché sur l'écran de fin

    private int score = 0;
    private bool isGameOver = false;

    void Awake()
    {
        // On s'assure qu'il n'existe qu'un seul GameManager
        instance = this;
    }

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        UpdateScoreUI();
    }

    public void AddScore()
    {
        if (isGameOver) return;

        score++;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (finalScoreText != null)
            finalScoreText.text = "Score : " + score;

        // Time.timeScale = 0 met TOUT le jeu en pause (physique, Update
        // des scripts qui utilisent Time.deltaTime, animations...).
        // C'est une façon simple d'arrêter le jeu sans détruire les objets.
        Time.timeScale = 0f;
    }

    // À appeler depuis le bouton "Rejouer" de l'UI (OnClick dans l'inspecteur)
    public void Restart()
    {
        Time.timeScale = 1f; // très important : sinon la scène rechargée reste en pause !
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}