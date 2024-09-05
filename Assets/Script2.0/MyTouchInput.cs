using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class MyTouchInput : MonoBehaviour
{
    public TextMeshProUGUI DisplayText;
    private Touch TheTouch;
    private float TimeTouch;
    private float DisplayTime = 0.5f;
    


    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0) 
        {
            TheTouch = Input.GetTouch(0);
            if (TheTouch.phase == TouchPhase.Ended)
            {
                DisplayText.text = TheTouch.phase.ToString();
                this.TimeTouch = Time.time;
            }
            else if (Time.time - TimeTouch > DisplayTime)
            {
                DisplayText.text = TheTouch.phase.ToString();
                TimeTouch = Time.time;
            }
            else if( Time.time - TimeTouch > DisplayTime) 
            {
                DisplayText.text = "";
            }
       
        }
    }
}
