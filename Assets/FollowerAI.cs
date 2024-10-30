using System.Collections;
using UnityEngine;

public class FollowerAI : MonoBehaviour
{
    public float minDistance = 2f;
    public float maxDistance = 10f;
    public float intermediateDistance = 5f; // Nuevo radio intermedio
    public float speed = 3f;
    public float chaseSpeed = 8f;

    private Transform playerTransform;
    private Transform targetGoodTag;
    private Animator animator;
    private Rigidbody rb;
    private bool isFollowing = false;
    private bool isChasing = false;
    private bool isGoingToGoodTag = false;
    public MentalStateManager mentalStateManager;

    private void Start()
    {
        UpdateClosestPlayer();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        UpdateClosestPlayer();

        if (isChasing)
        {
            ChasePlayer();
        }
        else if (isGoingToGoodTag && targetGoodTag != null)
        {
            MoveToGoodTag();
        }
        else
        {
            if (playerTransform == null) return;

            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // Verificar si hay objetos GoodTag en el radio intermedio
            Collider[] colliders = Physics.OverlapSphere(transform.position, intermediateDistance);
            foreach (Collider col in colliders)
            {
                if (col.CompareTag("GoodTag"))
                {
                    targetGoodTag = col.transform;
                    isGoingToGoodTag = true;
                    isFollowing = false;
                    animator.SetBool("isFollow", false);
                    return;
                }
            }

            // Continuar con la lógica habitual si no se detectan objetos GoodTag
            if (distanceToPlayer > maxDistance)
            {
                isFollowing = true;
                FollowPlayer();
            }
            else
            {
                isFollowing = false;
            }

            animator.SetBool("isFollow", isFollowing);
        }
    }

    private void FollowPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;

        if (Vector3.Distance(transform.position, playerTransform.position) > minDistance)
        {
            rb.MovePosition(transform.position + direction * speed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    private void ChasePlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * chaseSpeed * Time.deltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= minDistance)
        {
            isChasing = false;
            animator.SetBool("isFollow", false);
        }
    }

    private void MoveToGoodTag()
    {
        if (targetGoodTag == null) return;

        Vector3 direction = (targetGoodTag.position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * speed * Time.deltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        float distanceToGoodTag = Vector3.Distance(transform.position, targetGoodTag.position);

        animator.SetBool("isFollow", true);

        // Si el NPC toca el objeto GoodTag, el objeto desaparece y el NPC regresa a su conducta habitual
        if (distanceToGoodTag <= minDistance)
        {
            Destroy(targetGoodTag.gameObject);
            targetGoodTag = null;
            isGoingToGoodTag = false;
            animator.SetBool("isFollow", false);

        }
    }

    public void CallNPCToChase()
    {
        isChasing = true;
        animator.SetBool("isFollow", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GoodTag"))
        {
            mentalStateManager.Goodtime();
            StartCoroutine(mentalStateManager.DestroyAfterDelay(other.gameObject, 0.2f));
        }
        else if (other.CompareTag("BadTag"))
        {
            mentalStateManager.Badtime();
            StartCoroutine(mentalStateManager.DestroyAfterDelay(other.gameObject, 0.2f));
        }
    }

    private void UpdateClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = Mathf.Infinity;
        Transform closestPlayer = null;

        foreach (GameObject player in players)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer < closestDistance)
            {
                closestDistance = distanceToPlayer;
                closestPlayer = player.transform;
            }
        }

        playerTransform = closestPlayer;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, intermediateDistance); // Dibujar el radio intermedio
    }
}
