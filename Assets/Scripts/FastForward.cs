using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FastForward : MonoBehaviour
{
    public bool isFF = false;
    public Image btnImage;
    public TMP_Text btnText;
    
    public void GoGoSpeed() 
    {
      if (!isFF) 
      {
          btnImage.color = Color.green;
          Time.timeScale = 3;
          btnText.SetText("Speed: 3x");
          isFF = true;
      } 
      else 
      {
          btnImage.color = Color.white;
          Time.timeScale = 1;
          btnText.SetText("Speed: 1x");
          isFF = false;
    }
  }
}
