using UnityEngine;

// Ce script gère TOUT ce qui concerne l'oiseau :
// - le saut quand on appuie sur ESPACE
// - la rotation (piqué vers le bas / redressement vers le haut)
// - le déclenchement de l'animation de vol
// - la détection de la mort (collision)
public class Bird : MonoBehaviour
{
    [Header("Paramètres de vol")]
    public float flapForce = 5f;       // Force du "battement d'aile"
    public float maxRotationUp = 30f;  // Angle max quand l'oiseau monte
    public float maxRotationDown = -90f; // Angle max quand l'oiseau plonge
    public float rotationSpeed = 8f;   // Vitesse de rotation (fluidité)

    private Rigidbody2D rb;
    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        // On récupère les composants une seule fois, au démarrage,
        // pour ne pas faire GetComponent() à chaque frame (coûteux).
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // On dit à l'Animator que l'oiseau vole dès le début de la partie.
        if (animator != null)
            animator.SetBool("IsFlying", true);
    }

    void Update()
    {
        // Si l'oiseau est mort, on ne veut plus qu'il réagisse aux touches.
        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Flap();
        }

        RotateBird();
    }

    void Flap()
    {
        // On REMPLACE la vélocité verticale plutôt que d'ajouter une force :
        // cela donne un saut net et répétable, comme dans le vrai Flappy Bird.
        // (Si tu préfères une sensation plus "physique", remplace cette ligne par :
        //  rb.AddForce(Vector2.up * flapForce, ForceMode2D.Impulse);)
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, flapForce);
    }

    void RotateBird()
    {
        float targetAngle;

        if (rb.linearVelocity.y > 0)
        {
            // L'oiseau monte : on le redresse vers le haut
            targetAngle = maxRotationUp;
        }
        else
        {
            // L'oiseau descend : on le fait piquer progressivement,
            // plus il descend vite, plus l'angle est négatif (proche de maxRotationDown)
            targetAngle = Mathf.Clamp(rb.linearVelocity.y * 10f, maxRotationDown, maxRotationUp);
        }

        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        // Slerp = interpolation douce entre la rotation actuelle et la cible
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // Appelé automatiquement par Unity quand l'oiseau touche un Collider2D
    // NON marqué "Is Trigger" (donc les colonnes et le sol, s'ils ont des
    // colliders "solides").
    void OnCollisionEnter2D(Collision2D collision)
    {
        Die();
    }

    // Appelé quand l'oiseau traverse une zone marquée "Is Trigger"
    // (utilisée ici comme zone invisible de comptage de score, voir Partie 4)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ScoreZone") && !isDead)
        {
            GameManager.instance.AddScore();
        }
    }

    void Die()
    {
        if (isDead) return; // évite d'appeler GameOver() plusieurs fois

        isDead = true;

        if (animator != null)
            animator.SetBool("IsFlying", false); // l'animation s'arrête

        GameManager.instance.GameOver();
    }
}