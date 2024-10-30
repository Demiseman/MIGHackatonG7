using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsActivatorByNPC : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        // Obtener el Rigidbody y asegurarse de que comience como kinemático
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Verificar si el objeto que entra en contacto tiene la etiqueta "NPC"
        if (collision.collider.CompareTag("NPC"))
        {
            // Cambiar el Rigidbody a no kinemático, para que responda a la física
            if (rb != null)
            {
                rb.isKinematic = false;
            }
        }
    }
}
