using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class My : MonoBehaviour
{
    public TextMeshProUGUI DisplayText;
    private Touch TheTouch;
    private Vector2 TouchStarPosition,TouchEndedPosition;
    private string direction;  
    
    void Update()
    {
        if (Input.touchCount > 0) 
        {
            TheTouch = Input.GetTouch(0);
            if (TheTouch.phase == TouchPhase.Began)
            {
                TouchStarPosition = TheTouch.position;
            }
            else if (TheTouch.phase == TouchPhase.Moved || TheTouch.phase == TouchPhase.Ended) 
            {
                TouchStarPosition = TheTouch.position;
                float x = TouchEndedPosition.x - TouchStarPosition.x;
                float y = TouchEndedPosition.y - TouchStarPosition.y;

                if (Mathf.Abs(x) == 0 && Mathf.Abs(y) == 0)
                {
                    direction = "Tapped";
                }
                else if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    direction = x > 0 ? "Right" : "Left";
                    // aqui entra a logica
                }
                else { direction = y > 0 ? "Up" : "Dow"; }

            }
            
        }
        DisplayText.text = direction;
    }
}
