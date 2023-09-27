using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FAST_Pinball;

public class FPT_RuntimeTest: MonoBehaviour
{
 float ColourTime;
 
 readonly int MAX_MARQUEE_PIXELS    = 10;
 Color[] PixelArray;
 
 public TextMesh HardwareText;
 public TextMesh ConsoleText;
 Queue<string>   ConsoleLines;
  
 float LampsFlashTime;
 
 // Start is called before the first frame update
 void Start()
 {
  if (HardwareText != null)
    HardwareText.gameObject.SetActive(false);
    
  PixelArray = new Color[MAX_MARQUEE_PIXELS];
  
  // Console echo
  if (ConsoleText != null)
    ConsoleText.text = "";
  ConsoleLines = new Queue<string>();
  
  Application.logMessageReceived += EchoDebugMessage;
  
  Debug.Log(">>>>Runtime - Initializing Fast hardware...");
  FAST_Pinball.FAST.Startup(FastInitializationCompleteCallback, 
  
                            // Ordered list of Node boards
                            new List<FAST_Pinball.FAST.eNodeBoards>()      
                               {
                                FAST_Pinball.FAST.eNodeBoards.FP_CAB0001
                               }, 
                               
                            // Ordered list of expansion boards
                            new List<FAST_Pinball.FAST.eExpansionBoards>()
                               {
                                FAST_Pinball.FAST.eExpansionBoards.NEURON
                               },
                               
                            // Not production
                            false);
 }

 // Update is called once per frame
 void Update()
 {
  // If the system hasn't started up yet, don't do anything
  if (!FAST_Pinball.FAST.IsInitialized())
    return;

  // Colour timer
  ColourTime += Time.deltaTime;
  if (ColourTime > 1.0f)
    ColourTime -= 1.0f;
  
  // Broadway marquee-type flash
  if (ColourTime < 0.5f)
    {
     for (int i=0; i<PixelArray.Length; ++i)
       PixelArray[i] = ((i&0x01)!=0)?Color.white:Color.black;
    }
  else
    {
     for (int i=0; i<PixelArray.Length; ++i)
       PixelArray[i] = ((i&0x01)!=0)?Color.black:Color.white;
    }
  
  // Set the first 10
  FAST_Pinball.FAST.SetPixelColours(0, PixelArray);
  
  // Set pixel 21 (index 20) to a fading purple colour
  FAST_Pinball.FAST.SetPixelColour(20, new Color(ColourTime, 0, ColourTime), FAST_Pinball.FAST.eExpansionDestinations.NEURON);

  if (Input.GetKeyDown(KeyCode.Escape))
    {
#if UNITY_EDITOR
     UnityEditor.EditorApplication.isPlaying = false;
#else
     Application.Quit();
#endif    
    }
 }
 
 
 void FastInitializationCompleteCallback(FAST_Pinball.FAST.eResult Success)
 {
  Debug.Log(">>>>Runtime - Fast is ready, now setting up drivers...");
  
  // Hardware text
  if (HardwareText != null)
    {
     if (FAST_Pinball.FAST.IsMachine_RETRO_11())
       HardwareText.text = "Found Hardware <color=yellow>SYSTEM 11</color>";
     else if (FAST_Pinball.FAST.IsMachine_RETRO_89())
       HardwareText.text = "Found Hardware <color=yellow>WPC 89</color>";
     else if (FAST_Pinball.FAST.IsMachine_RETRO_95())
       HardwareText.text = "Found Hardware <color=yellow>WPC 95</color>";
     else if (FAST_Pinball.FAST.IsMachine_MODERN())
       HardwareText.text = "Found Hardware <color=yellow>NEURON</color>";
     else 
       HardwareText.text = "Unknown Hardware";
     
     HardwareText.gameObject.SetActive(true);
    }    
    
  // Done!
  Debug.Log(">>>>Runtime - Initialization Complete");

  // Assign a spoof key to switch 0
  FAST_Pinball.FAST.RegisterSpoofKey(0, KeyCode.A);
  
  // Assign an event callback for switch 0
  FAST_Pinball.FAST.RegisterSwitchCallback(0, SwitchTestCallback);
  
  // Set all pixels green
  FAST_Pinball.FAST.SetAllPixelsToColour(new Color(0.0f, 1.0f, 0.0f), FAST_Pinball.FAST.eExpansionDestinations.NEURON);
 }
 
 
 void EchoDebugMessage(string logString, string stackTrace, LogType type)
 {
  ConsoleLines.Enqueue(logString);
  if (ConsoleLines.Count > 28)
    ConsoleLines.Dequeue();
    
  Queue<string> NewQueue = new Queue<string>();
  ConsoleText.text = "";
  for (; ConsoleLines.Count>0;)
    {
     if (ConsoleText != null)
       ConsoleText.text += ConsoleLines.Peek() + "\n";
     NewQueue.Enqueue(ConsoleLines.Dequeue());
    }
  ConsoleLines = NewQueue;
 }
 
 
 void SwitchTestCallback(int SwitchIndex, FAST_Pinball.FAST.eSwitchEvent SwitchEvent)
 {
  Debug.Log("Switch " + SwitchIndex + " is now " + SwitchEvent.ToString());
 }
}
