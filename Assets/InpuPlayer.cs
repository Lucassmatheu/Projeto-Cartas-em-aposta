using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Enhancend = UnityEngine.InputSystem.EnhancedTouch;

public class InpuPlayer : MonoBehaviour
{
    private PlayerInput playerInput;

    private InputAction touchPositionAction;
    private InputAction touchPressAction;

    [SerializeField]
    private Transform player;  // Referência ao Transform do Player

    [SerializeField]
    private Cartas cartasPrefab;  // Referência ao prefab da carta

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        touchPressAction = playerInput.actions["TouchPress"];
        touchPositionAction = playerInput.actions["TouchPosition"];
    }

    private void OnEnable()
    {
        touchPressAction.performed += OnPressPerformed;
        touchPressAction.canceled += OnPressCanceled;
    }

    private void OnDisable()
    {
        touchPressAction.performed -= OnPressPerformed;
        touchPressAction.canceled -= OnPressCanceled;
    }

    private void OnPressPerformed(InputAction.CallbackContext context)
    {
        MovePlayerToCurrentPointerPosition();
    }

    private void OnPressCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("Pressão cancelada");
    }

    private void MovePlayerToCurrentPointerPosition()
    {
        Vector2 screenPosition = touchPositionAction.ReadValue<Vector2>();

        // Log para verificar o valor da posição de tela
        Debug.Log("Posição da tela: " + screenPosition);

        // Verifique se a posição de tela é válida
        //if (float.IsInfinity(screenPosition.x) || float.IsInfinity(screenPosition.y) ||
        //    float.IsNaN(screenPosition.x) || float.IsNaN(screenPosition.y))
        //{
        //    Debug.LogError("Posição de entrada inválida: " + screenPosition);
        //    return;
        //}

        // Calcular a profundidade z a partir da câmera até o plano do jogador
        //float playerZ = player.position.z - cartasPrefab.transform.position.z;
        //Debug.Log("Profundidade Z: " + playerZ);

        //Vector3 worldPosition = cartasPrefab.transform.TransformPoint(new Vector3(screenPosition.x, screenPosition.y, playerZ));

        //// Log para verificar o valor da posição do mundo
        //Debug.Log("Posição do mundo: " + worldPosition);

        //// Verifique se a posição do mundo é válida
        ////if (float.IsInfinity(worldPosition.x) || float.IsInfinity(worldPosition.y) || float.IsInfinity(worldPosition.z) ||
        ////    float.IsNaN(worldPosition.x) || float.IsNaN(worldPosition.y) || float.IsNaN(worldPosition.z))
        ////{
        ////    Debug.LogError("Posição do mundo inválida: " + worldPosition);
        ////    return;
        ////}

        //player.position = worldPosition;  // Atualiza a posição do jogador
        //Debug.Log("Movendo para: " + worldPosition);
        //player.position = screenPosition;
    }

    private void Update()
    {
        if (touchPressAction.WasPressedThisFrame())
        {
            MovePlayerToCurrentPointerPosition();
        }
    }

}
