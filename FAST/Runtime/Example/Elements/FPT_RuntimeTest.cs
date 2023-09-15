using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FAST_Pinball;

public class FPT_RuntimeTest: MonoBehaviour
{
    const float MAX_BRIGHTNESS = 0.9f;
    float ColourTime;
 
    // Start is called before the first frame update
    void Start()
    {  
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
        //-------------------------------------------------------
        // If the system hasn't started up yet, don't do anything
        if (!FAST_Pinball.FAST.IsInitialized())
            return;

        //-------------------------------------------------------
        // Basic colour animation
        ColourTime += Time.deltaTime;
        if (ColourTime > 1.0f)
            ColourTime -= 1.0f;
  
        // Fade from black to white
        FAST_Pinball.FAST.SetAllPixelsToColour(new Color(ColourTime*MAX_BRIGHTNESS, ColourTime*MAX_BRIGHTNESS, ColourTime*MAX_BRIGHTNESS), FAST_Pinball.FAST.eExpansionDestinations.EXPANSION);

        //-------------------------------------------------------
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
        if (FAST_Pinball.FAST.IsMachine_RETRO_11())
            Debug.Log("Found Hardware <color=yellow>SYSTEM 11</color>");
        else if (FAST_Pinball.FAST.IsMachine_RETRO_89())
            Debug.Log("Found Hardware <color=yellow>WPC 89</color>");
        else if (FAST_Pinball.FAST.IsMachine_RETRO_95())
            Debug.Log("Found Hardware <color=yellow>WPC 95</color>");
        else if (FAST_Pinball.FAST.IsMachine_MODERN())
            Debug.Log("Found Hardware <color=yellow>NEURON</color>");
        else 
            Debug.Log("Unknown Hardware");
    
        // Done!
        Debug.Log(">>>>Runtime - Initialization Complete");
    }
}
