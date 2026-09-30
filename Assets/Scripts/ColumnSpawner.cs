using UnityEngine;

// Ce script se met sur un GameObject vide nommé par exemple "Spawner",
// placé à droite de l'écran. Il instancie le prefab de colonnes à
// intervalles réguliers, avec une hauteur aléatoire à chaque fois.
public class ColumnSpawner : MonoBehaviour
{
    [Header("Références")]
    public GameObject columnPairPrefab; // le prefab contenant colonne du haut + du bas

    [Header("Cadence")]
    public float spawnInterval = 2f;    // temps (secondes) entre deux générations
    public float firstSpawnDelay = 0.5f; // délai avant la TOUTE PREMIÈRE colonne (indépendant de spawnInterval)

    [Header("Variation de hauteur")]
    public float minOffsetY = -2f;      // décalage vertical minimum
    public float maxOffsetY = 2f;       // décalage vertical maximum
    // Astuce : l'écart ENTRE la colonne du haut et celle du bas est fixé
    // une fois pour toutes DANS le prefab (distance fixe entre les deux
    // sprites). Ici, on ne fait que déplacer la PAIRE entière de haut en bas,
    // ce qui fait varier la position du "trou" sans jamais réduire sa taille
    // en dessous du passage nécessaire à l'oiseau.

    void Start()
    {
        // InvokeRepeating appelle SpawnColumn() une première fois après
        // firstSpawnDelay secondes (court, pour que le jeu démarre vite),
        // puis toutes les spawnInterval secondes ensuite.
        InvokeRepeating(nameof(SpawnColumn), firstSpawnDelay, spawnInterval);
    }

    void SpawnColumn()
    {
        float randomOffsetY = Random.Range(minOffsetY, maxOffsetY);

        Vector3 spawnPosition = new Vector3(
            transform.position.x,
            transform.position.y + randomOffsetY,
            0f
        );

        Instantiate(columnPairPrefab, spawnPosition, Quaternion.identity);
    }

    // BONUS Partie 5 : pour augmenter la difficulté au fil du temps,
    // tu peux appeler cette méthode depuis GameManager (par exemple
    // toutes les 10 secondes) pour réduire l'intervalle et donc
    // rapprocher les colonnes :
    public void IncreaseDifficulty(float reduction)
    {
        spawnInterval = Mathf.Max(0.8f, spawnInterval - reduction);
        CancelInvoke(nameof(SpawnColumn));
        InvokeRepeating(nameof(SpawnColumn), spawnInterval, spawnInterval);
    }
}