using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // El objeto a seguir por la cámara
    public GameObject target;

    // Distancia entre la cámara y el objeto
    public Vector3 offset = new Vector3(0, 5, -10);

    // Velocidad de seguimiento
    public float followSpeed = 5f;

    private void LateUpdate()
    {
        // Verificar que el target no sea nulo antes de proceder
        if (target == null) return;

        // Calcular la posición deseada de la cámara
        Vector3 targetPosition = target.transform.position + offset;

        // Interpolar suavemente hacia la posición deseada
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Opcional: Hacer que la cámara mire siempre al objeto
        transform.LookAt(target.transform);
    }
}
