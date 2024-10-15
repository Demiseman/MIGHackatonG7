using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class PlayerInputHandler : MonoBehaviour
{
    // Velocidad de movimiento del jugador
    public float speed = 5f;
    
    // Almacena la dirección del movimiento que será proporcionada por el input
    private Vector2 movementInput;

    // Referencia a la cámara principal
    private Camera mainCamera;

    // Inicialización
    private void Start()
    {
        // Obtener la cámara principal automáticamente
        mainCamera = Camera.main;
    }

    // Este método se ejecuta una vez por frame
    private void Update()
    {
        // Llamar al método para mover al jugador en la dirección de la cámara
        MovePlayer();
    }

    private void MovePlayer()
    {
        // Convertir el input a un Vector3 (X y Z) para el movimiento
        Vector3 moveDirection = new Vector3(movementInput.x, 0, movementInput.y);

        // Tomar la dirección de la cámara (forward y right) para ajustar el movimiento relativo a la cámara
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        // Asegurarse de que el movimiento esté alineado con el plano horizontal (sin inclinación vertical)
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calcular la dirección final del movimiento
        Vector3 direction = cameraForward * moveDirection.z + cameraRight * moveDirection.x;

        // Mover al jugador en la dirección calculada
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // Rotar al jugador para que mire en la dirección del movimiento
        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    // Método que se llama cuando el input de movimiento es detectado
    public void OnMove(InputAction.CallbackContext ctx)
    {
        // Lee los valores del input como un Vector2 (ej. de un joystick o teclas WASD)
        movementInput = ctx.ReadValue<Vector2>();
    }
}
