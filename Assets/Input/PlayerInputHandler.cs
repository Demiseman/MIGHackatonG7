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

    // Este método se ejecuta una vez por frame
    private void Update()
    {
        // Mueve el jugador en función de la entrada, el vector se convierte a Vector3
        transform.Translate(new Vector3(movementInput.x, 0, movementInput.y) * speed * Time.deltaTime);
    }

    // Método que se llama cuando el input de movimiento es detectado
    public void OnMove(InputAction.CallbackContext ctx)
    {
        // Lee los valores del input como un Vector2 (ej. de un joystick o teclas WASD)
        movementInput = ctx.ReadValue<Vector2>();
    }



}