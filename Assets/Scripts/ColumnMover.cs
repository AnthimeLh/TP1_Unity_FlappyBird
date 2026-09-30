using UnityEngine;

// Ce script se met sur CHAQUE paire de colonnes (le prefab).
// Il fait deux choses : avancer vers la gauche, et se détruire
// automatiquement une fois sorti de l'écran (pour ne pas accumuler
// des centaines d'objets inutiles en mémoire).
public class ColumnMover : MonoBehaviour
{
    public float speed = 3f;

    // Position X à partir de laquelle on considère que l'objet est
    // sorti de l'écran à gauche. Ajuste cette valeur selon la taille
    // de ta caméra (par exemple -12 pour une caméra orthographique
    // de taille 5 en 16:9).
    public float destroyX = -12f;

    void Update()
    {
        // Translate déplace l'objet dans l'espace local à chaque frame.
        // Time.deltaTime rend le mouvement indépendant du framerate.
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }
}