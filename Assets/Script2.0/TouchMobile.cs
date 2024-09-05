using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TouchMobile : MonoBehaviour
{
    public Transform slot1; // Referência ao slot fixo na mesa
    public LayerMask playerLayerMask; // Máscara de camada para detectar cartas do jogador
    public string playerLayerName = "Player1Card"; // Nome da layer do jogador

    private Touch theTouch;
    private Vector2 touchStartPosition, touchEndPosition;
    private bool isDragging;
    private GameObject currentCard;
    private string direction;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);

            switch (theTouch.phase)
            {
                case TouchPhase.Began:
                    touchStartPosition = theTouch.position;
                    Debug.Log("Toque iniciado em: " + touchStartPosition);
                    Ray ray = Camera.main.ScreenPointToRay(touchStartPosition);
                    RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector2.zero, Mathf.Infinity, playerLayerMask);

                    if (hit.collider != null)
                    {
                        isDragging = false;
                        currentCard = hit.collider.gameObject;
                        Debug.Log("Carta selecionada: " + currentCard.name);
                    }
                    break;

                case TouchPhase.Moved:
                    if (currentCard != null)
                    {
                        touchEndPosition = theTouch.position;
                        if (!isDragging)
                        {
                            isDragging = true;
                            Debug.Log("Arrastando iniciado em: " + touchEndPosition);
                        }
                        Debug.Log("Arrastando em: " + touchEndPosition);
                        Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchEndPosition.x, touchEndPosition.y, Camera.main.nearClipPlane));
                        currentCard.transform.position = new Vector3(newPosition.x, newPosition.y, 0);
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (currentCard != null)
                    {
                        touchEndPosition = theTouch.position;
                        Debug.Log("Toque terminado em: " + touchEndPosition);
                        if (isDragging)
                        {
                            ColocarCartaNoSlot1();
                        }
                        isDragging = false;
                        currentCard = null; // Resetar a carta atual
                    }
                    break;
            }

            // Atualizar a direção no log
            float x = touchEndPosition.x - touchStartPosition.x;
            float y = touchEndPosition.y - touchStartPosition.y;

            if (Mathf.Abs(x) == 0 && Mathf.Abs(y) == 0)
            {
                direction = "Tapped";
            }
            else if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                direction = x > 0 ? "Right" : "Left";
            }
            else
            {
                direction = y > 0 ? "Up" : "Down";
            }

            Debug.Log("Direção: " + direction);
        }
    }

    private void ColocarCartaNoSlot1()
    {
        if (currentCard != null)
        {
            // Mover a carta para o slot1 da mesa
            currentCard.transform.position = slot1.position;
            currentCard.transform.SetParent(slot1);
            Debug.Log("Carta posicionada no slot1 da mesa.");

            // Alterar a layer da carta para a layer do jogador
            int playerLayer = LayerMask.NameToLayer(playerLayerName);
            SetLayerRecursively(currentCard, playerLayer);
        }
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (child == null)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
