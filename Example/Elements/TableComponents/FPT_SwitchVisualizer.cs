using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPT_SwitchVisualizer : MonoBehaviour
{
 public GameObject SwitchOnObj;
 public GameObject SwitchOffObj;
 public int        SwitchIndex;
 
 // Start is called before the first frame update
 void Start()
 {
  SwitchOnObj.SetActive(false);
  SwitchOffObj.SetActive(false);
 }


 // Update is called once per frame
 void Update()
 {
  bool SwitchState = FAST_Pinball.FAST.IsSwitchDown(SwitchIndex);
  SwitchOnObj.SetActive(SwitchState);
  SwitchOffObj.SetActive(!SwitchState);
 }
}
