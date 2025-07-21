using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public int health = 5;
    public int damage = 1;
    public int reward = 10;

    private Transform[] waypoints; // Waypoints específicos para cada enemigo
    private int currentWaypointIndex = 0;
    private Animator animator;
    private bool isDead = false;

    public bool IsDead => isDead; // Propiedad pública para que las torres accedan

    public Transform[] assignedWaypoints; // Array que asignará los waypoints específicos para cada enemigo

    void Start()
    {
        animator = GetComponent<Animator>();

        // Asignar los waypoints específicos según el punto de spawn
        if (assignedWaypoints.Length > 0)
        {
            waypoints = assignedWaypoints; // Asignar waypoints específicos
        }
        else
        {
            // Si no se asignan, se buscan los waypoints por defecto
            GameObject wpHolder = GameObject.Find("Waypoints");
            int count = wpHolder.transform.childCount;
            waypoints = new Transform[count];
            for (int i = 0; i < count; i++)
            {
                waypoints[i] = wpHolder.transform.GetChild(i);
            }
        }
    }

    void Update()
    {
        if (isDead) return; // No hacer nada si está muerto

        if (currentWaypointIndex < waypoints.Length)
        {
            Transform target = waypoints[currentWaypointIndex];
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target.position) < 0.1f)
            {
                currentWaypointIndex++;
            }
        }
        else
        {
            // Llegó al final del camino
            GameManager.instance.LoseLife(damage);
            Destroy(gameObject); // El enemigo se destruye al final del camino
        }
    }

    public void TakeDamage(int dmg)
    {
        if (isDead) return; // Evita seguir dañando si ya está muerto

        health -= dmg;

        if (health <= 0)
        {
            Die(); // Llamar a la función de muerte
        }
    }

    void Die()
    {
        isDead = true;

        // Ganancia de dinero al matar al enemigo
        GameManager.instance.AddMoney(reward);

        // Activar animación de muerte
        animator.SetTrigger("Die");

        // Desactivar la colisión para evitar interacciones
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        // Detener física y movimiento
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero; // Detener cualquier movimiento residual
            rb.isKinematic = true; // Hacer que no interactúe con la física
        }

        // Destruir el objeto después de un tiempo para dar espacio a la animación
        Destroy(gameObject, 1.5f);
    }
}
