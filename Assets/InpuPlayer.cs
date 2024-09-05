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
    private Transform player;  // Refer�ncia ao Transform do Player

    [SerializeField]
    private Cartas cartasPrefab;  // Refer�ncia ao prefab da carta

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
        Debug.Log("Press�o cancelada");
    }

    private void MovePlayerToCurrentPointerPosition()
    {
        Vector2 screenPosition = touchPositionAction.ReadValue<Vector2>();

        // Log para verificar o valor da posi��o de tela
        Debug.Log("Posi��o da tela: " + screenPosition);

        // Verifique se a posi��o de tela � v�lida
        //if (float.IsInfinity(screenPosition.x) || float.IsInfinity(screenPosition.y) ||
        //    float.IsNaN(screenPosition.x) || float.IsNaN(screenPosition.y))
        //{
        //    Debug.LogError("Posi��o de entrada inv�lida: " + screenPosition);
        //    return;
        //}

        // Calcular a profundidade z a partir da c�mera at� o plano do jogador
        //float playerZ = player.position.z - cartasPrefab.transform.position.z;
        //Debug.Log("Profundidade Z: " + playerZ);

        //Vector3 worldPosition = cartasPrefab.transform.TransformPoint(new Vector3(screenPosition.x, screenPosition.y, playerZ));

        //// Log para verificar o valor da posi��o do mundo
        //Debug.Log("Posi��o do mundo: " + worldPosition);

        //// Verifique se a posi��o do mundo � v�lida
        ////if (float.IsInfinity(worldPosition.x) || float.IsInfinity(worldPosition.y) || float.IsInfinity(worldPosition.z) ||
        ////    float.IsNaN(worldPosition.x) || float.IsNaN(worldPosition.y) || float.IsNaN(worldPosition.z))
        ////{
        ////    Debug.LogError("Posi��o do mundo inv�lida: " + worldPosition);
        ////    return;
        ////}

        //player.position = worldPosition;  // Atualiza a posi��o do jogador
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
