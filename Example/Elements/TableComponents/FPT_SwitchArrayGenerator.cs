using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPT_SwitchArrayGenerator : MonoBehaviour
{
 public GameObject SwitchVisualizerPref;
 
 const int SWITCH_COLUMNS = 16;
 
 // Start is called before the first frame update
 void Start(/*void*/)
 {
  for (int i=0; i<112; ++i)
    {
     GameObject NewSwitchObj = GameObject.Instantiate(SwitchVisualizerPref);
     NewSwitchObj.transform.parent = transform;
     NewSwitchObj.transform.localPosition = new Vector3(i%SWITCH_COLUMNS, -i/SWITCH_COLUMNS, 0.0f);
     NewSwitchObj.transform.localScale    = new Vector3(0.9f, 0.9f, 0.9f);
     NewSwitchObj.GetComponent<FPT_SwitchVisualizer>().SwitchIndex = i;
    }       
 }
}
