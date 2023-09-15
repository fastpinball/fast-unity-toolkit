using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FAST_Pinball
{
//------------------------------


public partial class FAST: MonoBehaviour
{
 public enum eResult
 {
    SUCCESS=0,
    
    // Connection
    NO_HARDWARE_FOUND,
    INCORRECT_CONFIGURATION,
    
    // Motors/Servos
    LIMIT_NOT_FOUND,
    AT_MAX_LIMIT,
    AT_MIN_LIMIT,
 }
 
 public enum eExpansionDestinations
   {
    NEURON=0,
    EXPANSION,
   }
   
 public enum eNodeBoards
   {
    FP_CAB0001=0,
    FP_IO0804,
    FP_IO1616,
    FP_IO3208,    
   }
   
 public enum eExpansionBoards
   {
    // 128 LEDs
    NEURON=0,
    
    // 128 LEDs, 4 Servos
    FP_EXP0071,
    
    // 256 LEDs
    FP_EXP0081,
    
    // 128 LEDs, 2 Breakouts
    FP_EXP0091,
   }
   
   
 public enum eDriverPWM
   {
    PWM_0   = 0x00,
    PWM_12  = 0x01,
    PWM_25  = 0x88,
    PWM_37  = 0x92,
    PWM_50  = 0xAA,
    PWM_62  = 0xBA,
    PWM_75  = 0xEE,
    PWM_87  = 0xFE,
    PWM_100 = 0xFF,
   }
}

//-----------------------------
// namespace FAST_Pinball
}
