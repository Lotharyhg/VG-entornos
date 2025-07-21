using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{

    public GameObject player;
    public Animator zombieAnimator;

    public float zombieDamage = 20f;
    public float zombieHealth = 100f;

    GameManager gameManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.FindObjectOfType<GameManager>();
        //Asignar variable para ubicar en el animator que está en root
        zombieAnimator = this.transform.GetChild(1).gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        // Mover el agente a través del NavMesh y alcanzar al player
        GetComponent<NavMeshAgent>().destination = player.transform.position;

        if (GetComponent<NavMeshAgent>().velocity.magnitude > 1 )
        {
            zombieAnimator.SetBool("isRunning", true);
        }
        else
        {
            zombieAnimator.SetBool("isRunning", false);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("Estoy Atacando");
            player.GetComponent<PlayerManager>().HitZombie(zombieDamage);
        }
    }
    public void HitWeapon(float weaponDamage) // pasamos el daño del arma
    {
        zombieHealth -= weaponDamage; // restamos la vida del zombie con el daño del arma

        if (zombieHealth <= 0)
        {
            // destruye al zombie
            Destroy(this.gameObject);
            // Disminuye el contador de zombies vivos
            gameManager.numEnemiesAlives--;
        }
    }
}
