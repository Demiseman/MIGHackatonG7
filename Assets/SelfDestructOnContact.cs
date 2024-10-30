using System.Collections;
using UnityEngine;

public class SelfDestructOnContact : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Verificar si el objeto que entra en contacto tiene la etiqueta "NPC"
        if (collision.collider.CompareTag("NPC"))
        {
            // Iniciar la destrucción del objeto después de 0.1 segundos
            StartCoroutine(DestroyAfterDelay(0.2f));
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        // Esperar el tiempo especificado
        yield return new WaitForSeconds(delay);

        // Destruir el objeto
        Destroy(gameObject);
    }
}
