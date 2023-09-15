using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FAST_Pinball;

public class FPT_RuntimeTest: MonoBehaviour
{
 float ColourTime;
 Color[] PixelArray;
 
 public TextMesh HardwareText;
 public TextMesh ConsoleText;
 Queue<string>   ConsoleLines;
 
 // Start is called before the first frame update
 void Start()
 {
  if (HardwareText != null)
    HardwareText.gameObject.SetActive(false);
    
  PixelArray = new Color[128];
  
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
                                FAST_Pinball.FAST.eNodeBoards.FP_IO1616
                               }, 
                               
                            // Ordered list of expansion boards
                            new List<FAST_Pinball.FAST.eExpansionBoards>()
                               {
                                FAST_Pinball.FAST.eExpansionBoards.NEURON
                               });
 }

 // Update is called once per frame
 void Update()
 {
  // If the system hasn't started up yet, don't do anything
  if (!FAST_Pinball.FAST.IsInitialized())
    return;
    
  ColourTime += Time.deltaTime;
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
      
  if (ColourTime > 1.0f)
    ColourTime -= 1.0f;
  
  
  //FAST_Pinball.FAST.SetPixelColours(0, PixelArray);
  FAST_Pinball.FAST.SetAllPixelsToColour(new Color(ColourTime, 0, ColourTime), FAST_Pinball.FAST.eExpansionDestinations.EXPANSION);

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
}
