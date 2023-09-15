using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FAST_Pinball
{
//------------------------------


public partial class FAST: MonoBehaviour
{
 public delegate void BasicResultCallback(eResult ResultCode);
 
 
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
   
   
   
   
   
   
 
 // Singleton stuff
 static FAST CurrentFASTInstance = null;
 public static FAST Instance(/*void*/) { return CurrentFASTInstance; }
 
 // Some flags
 bool HasStarted = false;
  
 
 
 
 
 
 
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /*----------------------------------------------------------------------------------------------------------*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                        Unity Interface                                                 -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*----------------------------------------------------------------------------------------------------------*/
 /************************************************************************************************************/
 /************************************************************************************************************/
#region Unity_Interface
 // Unity constructor
 void Awake(/*void*/)
 {
  // Instance already exists?
  if (CurrentFASTInstance != null)
    {
     Destroy(gameObject);
     return;     
    }
  
  // Store this instance
  CurrentFASTInstance = this;
  DontDestroyOnLoad(gameObject);
  
  // Not started yet
  HasStarted = false;
 }
 
 
 // Unity distructor
 void OnDestroy(/*void*/)
 {
  // Make sure this is actual the real runtime shutting down
  if (CurrentFASTInstance == this)
    {
     Log("Shutting down serial devices...");
     
     // Free the instance
     CurrentFASTInstance = null;
     
     Log("FAST Hardware is now UNLOADED");
    }
 }
 
 
 
 
 // Unity update function
 void Update()
 {
  _UpdateInternal();
 }
#endregion
 
 
 
 
 
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /*----------------------------------------------------------------------------------------------------------*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                        Engine Utilities                                                -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*----------------------------------------------------------------------------------------------------------*/
 /************************************************************************************************************/
 /************************************************************************************************************/
#region Engine_Utilities
 /// <summary>
 /// Required initialization for the FAST unity runtime
 /// </summary>
 public static void Startup(BasicResultCallback Callback, List<eNodeBoards> NodeOrder, List<eExpansionBoards> ExpansionOrder)
 {
  if (CurrentFASTInstance != null)
    CurrentFASTInstance.StartCoroutine(CurrentFASTInstance.StartupProcess(Callback, NodeOrder, ExpansionOrder));
 }
 
 
 
 
 
 /// <summary>
 /// A way to check to see if FAST hardware has started up
 /// </summary> 
 public static bool IsInitialized(/*void*/)
 {
  if (CurrentFASTInstance == null)
    return false;
    
  return CurrentFASTInstance.HasStarted;
 }
 
 
 
 
 
 /// <summary>
 /// Closes and cleans up the FAST control system
 /// </summary>
 public static void Shutdown(/*void*/)
 {
  if (CurrentFASTInstance == null)
    return;
    
  // TODO
 }
    

 /// <summary>
 /// Closes then restarts FAST control system with the same settings as previous
 /// </summary> 
 public static void Reset(/*void*/)
 {
  if (CurrentFASTInstance == null)
    return;
    
  // TODO
 }
     


 

 /// <summary>
 /// Tests Modern/Neuron board connection
 /// </summary> 
 public static bool IsMachine_MODERN(/*void*/)
 {    
  if (CurrentFASTInstance == null)
    return false;
    
  return CurrentFASTInstance.HasStarted;
 }
 
 /// <summary>
 /// Tests Retro-11 board connection
 /// </summary> 
 public static bool IsMachine_RETRO_11(/*void*/)   { return false;  }
 
 /// <summary>
 /// Tests Retro-89 board connection
 /// </summary> 
 public static bool IsMachine_RETRO_89(/*void*/)   { return false;  }
 
 /// <summary>
 /// Tests Retro-95 board connection
 /// </summary>
 public static bool IsMachine_RETRO_95(/*void*/)   { return false;  }
#endregion
 
 
 
 
 
 
 

 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /*----------------------------------------------------------------------------------------------------------*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                      Driver Configuration                                              -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*----------------------------------------------------------------------------------------------------------*/
 /************************************************************************************************************/
 /************************************************************************************************************/
#region Driver_Configuration
 /// <summary>
 /// Sets up a flipper that only uses one driver. This is typically an older style flipper where the EOS is connected directly to the strong winding of the coil.
 /// </summary> 
 /// <param name="DriverIndex">A global zero-based index of the driver to be used</param>
 /// <param name="SwitchIndex">A global zero-based index for the flipper switch</param>
 public static void ConfigureFlipper_Single(int DriverIndex, int SwitchIndex) {; }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Sets up a flipper with separately controlled windings. The EOS switch will be used to prevent the strong coil from firing or holding for longer then it should.
 /// </summary> 
 /// <param name="DriverHardIndex">A global zero-based index of the driver used for the flipper's hard/strong winding</param>
 /// <param name="DriverHoldIndex">A global zero-based index of the driver used for the hold winding</param>
 /// <param name="SwitchIndex">A global zero-based index for the flipper switch</param>
 /// <param name="LocalEosSwitch">A local zero-based index for the EOS switch</param>
 /// <remarks>Setting the EOS switch to -1 will effectively make it is unused</remarks>
 public static void ConfigureFlipper_Dual(int DriverHardIndex, int DriverHoldIndex, int SwitchIndex, int LocalEosSwitch=-1) {; }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Sets up a hardware-controlled driver. Typically things like kickers, slingshots, jet bumpers, etc.
 /// </summary>   
 /// <param name="DriverIndex">A global zero-based index of the driver to be used</param>
 /// <param name="SwitchIndex">A global zero-based index of the switch</param>
 /// <param name="PulseMs">The time, in milliseconds, for an autofire coil to remain engaged</param>
 /// <param name="SleepMs">Optional, defaults to 50ms. The time, in milliseconds, for an autofire coil ignore switch triggers</param>
 public static void ConfigureAutoFireDriver(int DriverIndex, int SwitchIndex, int PulseMs, int SleepMs=0x32) {; }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Alias to <value>ConfigureAutoFireDriver</value>
 /// </summary> 
 /// <remarks>This function will be removed in a future version. Please do not use it.</remarks>
 [Obsolete("ConfigureSlingshot is deprecated, please use ConfigureAutoFireDriver instead.")]
 public static void ConfigureSlingshot(int DriverIndex, int SwitchIndex, int PulseMs, int SleepMs=0x32) {; }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Sets up a hardware-controlled driver that has a millisecond delay before firing
 /// </summary> 
 /// <param name="DriverIndex">A global zero-based index of the driver to be used</param>
 /// <param name="SwitchIndex">A global zero-based index of the switch</param>
 /// <param name="PulseMs">The time, in milliseconds, for an autofire coil to remain engaged</param>
 /// <param name="DelayMs">Optional, defaults to 64ms. The time, in milliseconds, for the ball to settle before enabling the driver</param>
 /// <remarks>This is useful for trough entry kickers and auto-firing poppers/kickouts</remarks>
 public static void ConfigureDelayedDriver(int DriverIndex, int SwitchIndex, int PulseMs, int DelayMs=0x40) {; }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Sets up a driver, 100% on, which software can enable and disable at will
 /// </summary> 
 /// <param name="DriverIndex">A global zero-based index of the driver to be used</param>
 public static void ConfigureSoftControlledDriver(int DriverIndex) {; }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Sets up a driver, with a hold power, which software can enable and disable at will
 /// </summary> 
 /// <param name="DriverIndex">A global zero-based index of the driver to be used</param>
 /// <param name="HardPulseMS">The time, in milliseconds, the driver will remain engaged</param>
 /// <param name="HoldPower">The hold coil PWM strength</param>
 /// <remarks>This is useful for diverters, gates, disappering/appearing posts, etc.</remarks>
 public static void ConfigureSoftControlledHoldDriver(int DriverIndex, int HardPulseMS, eDriverPWM HoldPower) {; }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Resets a driver's settings to default uninitialized values
 /// </summary> 
 /// <param name="DriverIndex">A global zero-based index of the driver to be used</param>
 public static void ClearDriverSettings(int DriverIndex) {; }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Resets flipper driver settings to default uninitialized values
 /// </summary> 
 /// <param name="DriverHardIndex">A global zero-based index of the driver used for the flipper's hard/strong winding</param>
 /// <param name="DriverHoldIndex">A global zero-based index of the driver used for the hold winding</param>
 public static void ClearFlipperSettings(int DriverHardIndex, int DriverHoldIndex=-1) {; }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Resets all driver settings to default uninitialized values
 /// </summary>  
 public static void ClearAllDriverSettings(/*void*/) {; }
#endregion
 
 
 
 
 
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /*----------------------------------------------------------------------------------------------------------*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                        Driver Control                                                  -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*----------------------------------------------------------------------------------------------------------*/
 /************************************************************************************************************/
 /************************************************************************************************************/
#region Driver_Control
 /// <summary>
 /// Activates a driver indefinitely. Will not deactivate until another driver command is sent to this driver index.
 /// </summary> 
 /// <param name="DriverIndex">The global zero-based index of the driver to enable</param>
 /// <remarks>TL:driver,3</remarks>
 public static void EnableDriver(int DriverIndex) {; }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Deactivates the driver and disables any other related command previously enacted on the driver index.
 /// </summary> 
 /// <param name="DriverIndex">The global zero-based index of the driver to enable</param>
 /// <remarks>TL:driver,2</remarks>
 public static void DisableDriver(int DriverIndex) {; }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Sends a quick pulse to the driver. It will not affect other driver settings.
 /// </summary> 
 /// <param name="DriverIndex">The global zero-based index of the driver to enable</param>
 /// <remarks>TL:driver,1</remarks>
 public static void PokeDriver(int DriverIndex) {; }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Turns on internal FAST hardware control for the particular driver index. Typically this is for things such as timed control, jet bumpers, slingshots, etc.
 /// </summary> 
 /// <param name="DriverIndex">The global zero-based index of the driver to enable</param>
 /// <remarks>TL:driver,0</remarks>
 public static void EnableAutoDriver(int DriverIndex) {; }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Activates a driver for the specified Milliseconds. It will cancel any other driver control commands.
 /// </summary>   
 /// <remarks>This overrides any previous configuration for the driver. Please make sure you reset any extra configurations after the MS expires or the driver may not control as you expect</remarks>
 public static void PulseDriver(int DriverIndex, int PulseTimeMs) {; }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Allows user flipper control
 /// <value>HoldDriverIndex</value> is optional
 /// </summary> 
 /// <param name="StrongDriverIndex">The global zero-based index of the driver used for the flipper's hard/strong winding</param>
 /// <param name="HoldDriverIndex">The global zero-based index of the driver used for the hold winding</param>
 /// <remarks>This will not set flipper configurations, it is a convienience function to turn on hardware-controlled switches</remarks>
 public static void EnableFlippers(int StrongDriverIndex, int HoldDriverIndex=-1) {; }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Stops user flipper control
 /// <value>HoldDriverIndex</value> is optional
 /// </summary> 
 /// <param name="StrongDriverIndex">The global zero-based index of the driver used for the flipper's hard/strong winding</param>
 /// <param name="HoldDriverIndex">The global zero-based index of the driver used for the hold winding</param>
 /// <remarks>This will not clear flipper driver flags, it is a convienience function to turn off hardware-controlled switches</remarks>
 public static void DisableFlippers(int StrongDriverIndex, int HoldDriverIndex=-1) {; }
#endregion
 
 
 
 
  
  
  
  
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /*----------------------------------------------------------------------------------------------------------*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                         Switch Input                                                   -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*----------------------------------------------------------------------------------------------------------*/
 /************************************************************************************************************/
 /************************************************************************************************************/
#region Switches
 /// <summary>
 /// Returns state information for a switch input
 /// </summary> 
 /// <returns>
 /// <value>true</value> if the switch is closed (A ball or thing is on the switch)
 /// <value>false</value> if the switch is open (A ball or thing is not on the switch)
 /// </returns> 
 /// <param name="SwitchIndex">The global zero-based index to test</param>
 /// <param name="SpoofKey">Optional. A unity keycode for "spoofing" a key press/trigger</param>
 /// <remarks>This switch does not invert switches- Meaning, optos will be in reverse of what you expect!</remarks>
 public static bool IsSwitchDown(int SwitchIndex, KeyCode SpoofKey=KeyCode.None)
 {
  // Get the spoof key
  return Input.GetKey(SpoofKey);
 }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Returns state information for a switch input
 /// </summary> 
 /// <returns>
 /// <value>true</value> if the switch is open (A ball or thing is not on the switch)
 /// <value>false</value> if the switch is closed (A ball or thing is on the switch)
 /// </returns>   
 /// <param name="SwitchIndex">The global zero-based index to test</param>
 /// <param name="SpoofKey">Optional. A unity keycode for "spoofing" a key press/trigger</param>
 /// <remarks>This switch does not invert switches- Meaning, optos will be in reverse of what you expect!</remarks>
 public static bool IsSwitchUp(int SwitchIndex, KeyCode SpoofKey=KeyCode.None)
 {
  // Get the spoof key
  return Input.GetKey(SpoofKey) == false;
 }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Returns state trigger state for a switch input
 /// </summary> 
 /// <returns>
 /// <value>true</value> if the switch is triggered closed this frame
 /// <value>false</value> otherwise
 /// </returns>
 /// <param name="SwitchIndex">The global zero-based index to test</param>
 /// <param name="SpoofKey">Optional. A unity keycode for "spoofing" a key press/trigger</param>
 /// <remarks>This switch does not invert switches- Meaning, optos will be in reverse of what you expect!</remarks>
 public static bool IsSwitchDownTriggered(int SwitchIndex, KeyCode SpoofKey=KeyCode.None)
 {
  // Get the spoof key
  return Input.GetKeyDown(SpoofKey);
 }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Returns state trigger state for a switch input
 /// </summary> 
 /// <returns>
 /// <value>true</value> if the switch is triggered open this frame
 /// <value>false</value> otherwise
 /// </returns> 
 /// <param name="SwitchIndex">The global zero-based index to test</param>
 /// <param name="SpoofKey">Optional. A unity keycode for "spoofing" a key press/trigger</param>
 /// <remarks>This switch does not invert switches- Meaning, optos will be in reverse of what you expect!</remarks>
 public static bool IsSwitchUpTriggered(int SwitchIndex, KeyCode SpoofKey=KeyCode.None)
 {
  // Get the spoof key
  return Input.GetKeyUp(SpoofKey);
 } 
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Returns state trigger state for a switch input
 /// </summary> 
 /// <returns>
 /// <value>true</value> if the switch is triggered closed OR open this frame
 /// <value>false</value> otherwise
 /// </returns> 
 /// <param name="SwitchIndex">The global zero-based index to test</param>
 /// <param name="SpoofKey">Optional. A unity keycode for "spoofing" a key press/trigger</param>
 /// <remarks>This switch does not invert switches- Meaning, optos will be in reverse of what you expect!</remarks>
 public static bool IsSwitchTriggered(int SwitchIndex, KeyCode SpoofKey=KeyCode.None)
 {
  // Get the spoof key
  return (Input.GetKeyDown(SpoofKey) || Input.GetKeyUp(SpoofKey));
 }
#endregion
 
 
 
 
  
  
  
  
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /*----------------------------------------------------------------------------------------------------------*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                          Pixel Lamps                                                   -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*----------------------------------------------------------------------------------------------------------*/
 /************************************************************************************************************/
 /************************************************************************************************************/
#region Pixel_Lamps
 /// <summary>
 /// Sets all pixels for an expansion board to black
 /// </summary> 
 /// <param name="Dest">The expansion board to send the pixels command to</param>
 /// <remarks>The default expansion destination is the NEURON internal expansion address</remarks>
 public static void TurnOffPixels(eExpansionDestinations Dest=eExpansionDestinations.NEURON) {; }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Sets all pixels for an expansion board to a specified colour
 /// </summary> 
 /// <param name="NewColour">The colour to be set</param>
 /// <param name="Dest">The expansion board to send the pixels command to</param>
 /// <remarks>The default expansion destination is the NEURON internal expansion address</remarks>
 public static void SetAllPixelsToColour(Color NewColour, eExpansionDestinations Dest=eExpansionDestinations.NEURON) {; }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Sets a particular pixel to the specified colour
 /// </summary> 
 /// <param name="Index">The local pixel index</param>
 /// <param name="NewColour">The colour to be set</param>
 /// <param name="Dest">The local expansion board base to send the pixel command to</param>
 /// <remarks>The default expansion destination is the NEURON internal expansion address. The Index is zero-based by expansion board, meaning the NEURON has 0-127 pixels, and EXPANSION has 0-511 pixels</remarks>
 public static void SetPixelColour(int Index, Color NewColour, eExpansionDestinations Dest=eExpansionDestinations.NEURON) {; }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Sets a contiguous group of pixels to colours in a list
 /// </summary> 
 /// <param name="StartIndex">The local pixel index that corresponds to <value>NewColours[0]</value></param>
 /// <param name="NewColours">An array of colours for pixels to be set to</param>
 /// <param name="Dest">The local expansion board base to send the pixel command to</param>
 /// <remarks>The default expansion destination is the NEURON internal expansion address. The Index is zero-based by expansion board, meaning the NEURON has 0-127 pixels, and EXPANSION has 0-511 pixels</remarks>
 public static void SetPixelColours(int StartIndex, Color[] NewColours, eExpansionDestinations Dest=eExpansionDestinations.NEURON) {; }
#endregion
 
 
 
 
  
  
  
  
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /*----------------------------------------------------------------------------------------------------------*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                        Servo Functions                                                 -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*----------------------------------------------------------------------------------------------------------*/
 /************************************************************************************************************/
 /************************************************************************************************************/
#region Servos
 /// <summary>
 /// Sets an internal value for what a servo's default position
 /// </summary> 
 /// <param name="ServoIndex">The global servo index</param>
 /// <param name="RotationPercent">The servo rotation, between 0.0f to 1.0f</param>
 /// <param name="Dest">The local expansion board base to send the pixel command to</param>
 public static void Servo_SetHomeRotation(int ServoIndex, float RotationPercent, eExpansionDestinations Dest=eExpansionDestinations.NEURON)
 {
 }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Gets the expected rotation value for what a servo
 /// </summary> 
 /// <param name="ServoIndex">The global servo index</param>
 /// <param name="RotationPercent">The servo rotation, between 0.0f to 1.0f</param>
 /// <param name="Dest">The local expansion board base to send the pixel command to</param>
 /// <returns>
 /// <value>float</value> inclusive (0.0f to 1.0f) that represents the rotation position
 /// </returns>
 /// <remarks>This is not the current position of the servo hardware, it is only the current expected position of the servo.</remarks>
 public static float Servo_GetRotation(int ServoIndex, eExpansionDestinations Dest=eExpansionDestinations.NEURON)
 {
  // Default at position t=0 for now
  return 0.0f;
 }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Sends the servo to it's currently set "home" position, or 0.0f if none is set.
 /// </summary> 
 /// <param name="ServoIndex">The global servo index</param>
 /// <param name="Dest">The local expansion board base to send the pixel command to</param>
 public static void Servo_SendHome(int ServoIndex, BasicResultCallback Callback, eExpansionDestinations Dest=eExpansionDestinations.NEURON)
 {
  // Spoof that we are there
  if (Callback != null)
     Callback(eResult.SUCCESS);
 } 
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Sends the servo to a new rotation
 /// </summary> 
 /// <param name="ServoIndex">The global servo index</param>
 /// <param name="DestRotationPercent">The desired servo rotation, between 0.0f to 1.0f</param>
 /// <param name="Dest">The local expansion board base to send the pixel command to</param>
 public static void Servo_RotateTo(int ServoIndex, float DestRotationPercent, BasicResultCallback Callback, eExpansionDestinations Dest=eExpansionDestinations.NEURON)
 {
  // Spoof that we are there
  if (Callback != null)
     Callback(eResult.SUCCESS);
 }
#endregion 
 
 
 
 
  
  
  
  
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /*----------------------------------------------------------------------------------------------------------*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                    Stepper Motor Functions                                             -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*----------------------------------------------------------------------------------------------------------*/
 /************************************************************************************************************/
 /************************************************************************************************************/
#region Stepper_Motors
 /// <summary>
 /// Sets a list of limit switches for the stepper motor to use
 /// </summary> 
 /// <param name="StepperIndex">The global stepper index</param>
 /// <param name="GlobalLimitSwitches">A list of global FAST switch indecies that will treated as limit switches for the motor</param>
 /// <param name="CircularLimits">Set true if this motor can rotate freely, set false if the first and last limit switch are hard-stops</param>
 /// <param name="Dest">The local expansion board base to send the pixel command to</param>
 /// <remarks>A single limit switch is allowed, it can be treated as a step-trigger. 2 Or more limit switches will denote a min, inbetween, and max limit for the motor.</remarks>
 public static void Stepper_SetLimitSwitches(int StepperIndex, List<int> GlobalLimitSwitches, bool CircularLimits, eExpansionDestinations Dest=eExpansionDestinations.NEURON)
 {
 }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Sends the servo to a new rotation
 /// </summary> 
 /// <param name="StepperIndex">The global stepper index</param>
 /// <param name="Dest">The local expansion board base to send the pixel command to</param>
 /// <returns>
 /// <value>int</value> the limit switch the motor is currently on
 /// <value>-1</value> if no limit switches have been registered
 /// </returns>
 /// <remarks>The limit switch index is local-zero based to the registered list. IE: 0 is limit switch 0, not FAST switch 0.</remarks>
 public static int Stepper_GetCurrentLimitIndex(int StepperIndex, eExpansionDestinations Dest=eExpansionDestinations.NEURON)
 {
    // No limit switches present at this point
    return -1;
 }

 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Sets an internal value for what a servo's default position
 /// </summary> 
 /// <param name="StepperIndex">The global stepper index</param>
 /// <param name="HomeLocalLimitSwitch">A registed limit switch index to use as the default position</param>
 /// <param name="Dest">The local expansion board base to send the pixel command to</param>
 /// <returns>
 /// <value>true</value> If the limit switch was set
 /// <value>false</value> If the limit switch index is outside the bounds of the the registered list
 /// </returns>
 /// <remarks>The limit switch index is local-zero based to the registered list. IE: 0 is limit switch 0, not FAST switch 0.</remarks>
 public static bool Stepper_Rehome(int StepperIndex, int HomeLocalLimitSwitch, eExpansionDestinations Dest=eExpansionDestinations.NEURON)
 {
    return true;
 }

 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Rotate to a limit switch. If the motor is circular, then it will take the shortest distance (based on registered limit switches) to get to the desired switch. If only 2 switches are registered, it will ping-pong between both switches.
 /// </summary> 
 /// <param name="StepperIndex">The global stepper index</param>
 /// <param name="DestinationLocalLimitSwitch">A registed limit switch index</param>
 /// <param name="Callback">A callback to be called when the motor reaches it's limit switch</param>
 /// <param name="Dest">The local expansion board base to send the pixel command to</param>
 /// <returns>
 /// <value>eResult.SUCCESS</value> When at the destination limit switch index
 /// <value>eResult.LIMIT_NOT_FOUND</value> If the destination limit switch index is outside the bounds of the the registered list
 /// </returns>
 /// <remarks>The limit switch index is local-zero based to the registered list. IE: 0 is limit switch 0, not FAST switch 0.</remarks>
 public static void Stepper_RunToLimit(int StepperIndex, int DestinationLocalLimitSwitch, BasicResultCallback Callback, eExpansionDestinations Dest=eExpansionDestinations.NEURON)
 {
  // Spoof that we are there
  if (Callback != null)
    Callback(eResult.SUCCESS);
 }

 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Rotate to the next limit switch. If the motor is circular, then it will wrap around from the max limit to the min limit.
 /// </summary> 
 /// <param name="StepperIndex">The global stepper index</param>
 /// <param name="Callback">A callback to be called when the motor reaches it's limit switch</param>
 /// <param name="Dest">The local expansion board base to send the pixel command to</param>
 /// <returns>
 /// <value>eResult.SUCCESS</value> When at the destination limit switch index
 /// <value>eResult.LIMIT_NOT_FOUND</value> If no limit switches have been registered
 /// <value>eResult.AT_MAX_LIMIT</value> If the motor is already at it's maximum limit and cannot wrap
 /// </returns>
 /// <remarks>If a single limit switch was registerd, the motor will run until that limit switch is triggered again effectively creating a device increment (for stairs or score reels)</remarks>
 public static void Stepper_ToNext(int StepperIndex, BasicResultCallback Callback, eExpansionDestinations Dest=eExpansionDestinations.NEURON)
 {
  // Spoof that we are there
  if (Callback != null)
    Callback(eResult.SUCCESS);
 }

 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Rotate to the previous limit switch. If the motor is circular, then it will wrap around from the min limit to the max limit.
 /// </summary> 
 /// <param name="StepperIndex">The global stepper index</param>
 /// <param name="Callback">A callback to be called when the motor reaches it's limit switch</param>
 /// <param name="Dest">The local expansion board base to send the pixel command to</param>
 /// <returns>
 /// <value>eResult.SUCCESS</value> When at the destination limit switch index
 /// <value>eResult.LIMIT_NOT_FOUND</value> If no limit switches have been registered
 /// <value>eResult.AT_MIN_LIMIT</value> If the motor is already at it's minumum limit and cannot wrap
 /// </returns>
 /// <remarks>If a single limit switch was registerd, the motor will run until that limit switch is triggered again effectively creating a device decrement (for stairs or score reels)</remarks>
 public static void Stepper_ToPrev(int StepperIndex, BasicResultCallback Callback, eExpansionDestinations Dest=eExpansionDestinations.NEURON)
 {
  // Spoof that we are there
  if (Callback != null)
    Callback(eResult.SUCCESS);
 } 
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /// <summary>
 /// Rotate for a period of time specified in milliseconds
 /// </summary> 
 /// <param name="StepperIndex">The global stepper index</param>
 /// <param name="RotationTimeMs">The time, in milliseconds, to continuously run the motor</param>
 /// <param name="Callback">A callback to be called when the motor rotation ends</param>
 /// <param name="Dest">The local expansion board base to send the pixel command to</param>
 /// <returns>
 /// <value>eResult.SUCCESS</value> When at the destination limit switch index
 /// </returns>
 public static void Stepper_RunMs(int StepperIndex, int RotationTimeMs, BasicResultCallback Callback, eExpansionDestinations Dest=eExpansionDestinations.NEURON)
 {
  // Spoof that we are there
  if (Callback != null)
    Callback(eResult.SUCCESS);
 } 
#endregion
 
 
 
 
 
 
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /*----------------------------------------------------------------------------------------------------------*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                          Retro Lamps                                                   -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*----------------------------------------------------------------------------------------------------------*/
 /************************************************************************************************************/
 /************************************************************************************************************/
#region Retro_Lamps
 public static void RETRO_SetLampState(int LampIndex, bool IsOn) {; }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 public static bool RETRO_IsLampEnabled(int LampIndex) { return false; }
#endregion
 
 
 
 
 
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /*----------------------------------------------------------------------------------------------------------*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                       Retro 11 Functions                                               -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*----------------------------------------------------------------------------------------------------------*/
 /************************************************************************************************************/
 /************************************************************************************************************/
#region Retro_11
 public static bool RETRO_11_ConfigureFlipperDriver(int DriverNumber) { return false; }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 public static void RETRO_11_SetAuxRelayState(bool Enabled=false) {; }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 public static void RETRO_11_SetFlipperState(bool Enabled=false) {; }
#endregion
 
 
 
 
 
 
 
#region InternalFunctions
 /************************************************************************************************************/
 /************************************************************************************************************/
 /*----------------------------------------------------------------------------------------------------------*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                       Internal Functions                                               -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*----------------------------------------------------------------------------------------------------------*/
 /************************************************************************************************************/
 /************************************************************************************************************/
 IEnumerator StartupProcess(BasicResultCallback Callback, List<eNodeBoards> NodeOrder, List<eExpansionBoards> ExpansionOrder)
 {
  // Already setup? don't do anything
  if (HasStarted)
    {
     LogWarning("System has already started!");
     Callback(eResult.SUCCESS);
     yield break;
    }
  
  // Done! ... spoof this for now!
  HasStarted = true;
  Callback(eResult.SUCCESS);
 }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 void _UpdateInternal(/*void*/)
 {
 }
 
 
 
 
 
 
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 /*----------------------------------------------------------------------------------------------------------*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                    Internal Logging Functions                                            -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*-                                                                                                        -*/
 /*----------------------------------------------------------------------------------------------------------*/
 /************************************************************************************************************/
 /************************************************************************************************************/
 protected static void LogInfo(string LogData)      
 {
  Debug.Log("FastProtocol Info> " + LogData);
 }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 protected static void Log(string LogData)      
 {
#if UNITY_EDITOR 
  Debug.Log("FastProtocol> " + LogData);
#endif
 }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
protected static void LogWarning(string LogData) 
 {
#if UNITY_EDITOR  
  Debug.LogWarning("FastProtocol> " + LogData); 
#endif
 }
 
 
 
 
 /************************************************************************************************************/
 /************************************************************************************************************/
 protected static void LogError(string LogData) 
 {
#if UNITY_EDITOR  
  Debug.LogError("FastProtocol> " + LogData); 
#endif
 }
#endregion
}
 
//-----------------------------
// namespace FAST_Pinball
}
