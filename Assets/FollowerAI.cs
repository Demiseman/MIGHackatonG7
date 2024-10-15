using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerAI : MonoBehaviour
{
    // Distancia mínima a la que se acercará el asistente al jugador
    public float minDistance = 2f;

    // Distancia máxima que define cuando el asistente comienza a seguir al jugador
    public float maxDistance = 10f;

    // Velocidad de movimiento del asistente
    public float speed = 3f;

    // Referencia al transform del jugador
    private Transform playerTransform;

    // Referencia al Animator para controlar las animaciones
    private Animator animator;

    // Estado de seguimiento
    private bool isFollowing = false;

    private void Start()
    {
        // Buscar al jugador en la escena por su etiqueta "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Si se encuentra un jugador, obtener su transform
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // Obtener el componente Animator del asistente
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Verificar que el jugador exista antes de realizar acciones
        if (playerTransform == null) return;

        // Calcular la distancia actual al jugador
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Si la distancia es mayor a maxDistance, el asistente comienza a seguir al jugador
        if (distanceToPlayer > maxDistance)
        {
            isFollowing = true;
            FollowPlayer();
        }
        // Si el jugador está dentro del rango, el asistente se detiene
        else
        {
            isFollowing = false;
        }

        // Actualizar el parámetro "isFollow" del Animator para controlar la animación
        animator.SetBool("isFollow", isFollowing);
    }

    private void FollowPlayer()
    {
        // Calcular la dirección hacia el jugador
        Vector3 direction = (playerTransform.position - transform.position).normalized;

        // Mover al asistente solo si la distancia es mayor a minDistance
        if (Vector3.Distance(transform.position, playerTransform.position) > minDistance)
        {
            // Mover al asistente en la dirección del jugador
            transform.Translate(direction * speed * Time.deltaTime, Space.World);

            // Rotar al asistente para que mire hacia el jugador
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    // Dibujar los gizmos para visualizar las distancias mínima y máxima
    private void OnDrawGizmos()
    {
        // Dibujar la distancia mínima en verde
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minDistance);

        // Dibujar la distancia máxima en rojo
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}
