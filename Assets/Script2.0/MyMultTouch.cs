using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MyMultTouch : MonoBehaviour
{
    public TextMeshProUGUI DisplayText;
    private Touch TheTouch;
    private int maxTapCount = 0;
    private string multTouch;
        
    // Update is called once per frame
    void Update()
    {
        multTouch = string.Format("Max tap count: {0}\n", maxTapCount);
        if (Input.touchCount > 0) 
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                TheTouch = Input.GetTouch(i);
            }
        }
    }
}
