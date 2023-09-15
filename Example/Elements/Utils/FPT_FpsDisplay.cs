 using UnityEngine;
 using System.Collections;
 
 public class FPT_FpsDisplay: MonoBehaviour
 {
  public string Prefix = "FPS: ";
  public string Postfix = "";
  public TextMesh FpsText;
  float deltaTime = 0.0f;
 
   void Update ()
   {
    deltaTime += (Time.deltaTime - deltaTime)*0.1f;
    float fps = 1.0f / deltaTime;
    FpsText.text = Prefix + Mathf.Ceil(fps).ToString() + Postfix;
   }
 }