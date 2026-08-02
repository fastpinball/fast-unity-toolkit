# Changelog
All notable changes to this package will be documented in this file. The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)


## [0.4.10] - 2026-8-2

### Fixed

-  FAST.Shutdown() implemented (was a TODO no-op): stops all core reader threads, then closes serial ports — fixes memory ballooning after exiting play mode and the stalled "reloading domain" in the editor (F10)
-  OnDestroy now routes through Shutdown() so play-stop performs the full teardown (previously closed ports without stopping threads)
-  Core reader threads no longer spin at full speed on a dead/idle port (2ms sleep on no-data); empty message segments are skipped without sleeping so bursts drain at full rate
-  Reader response backlog capped at 10,000 entries (oldest dropped) so an undrained queue can no longer grow without bound
-  FastSerialCommunicator.ReadDataAsString returns null for no-data (timeout/error/no port) vs a string for a received message; send/shutdown paths guarded so a mid-session port loss degrades silently instead of throwing every frame
-  Core.Update no longer allocates two queues per core per frame (reused drain queue); LoopCore watchdog message built once instead of per frame


## [0.4.9] - 2026-7-17

### Added

-  void PulseDriver(..) overload with shaped two-stage output (pwm1 time/power, pwm2 time/power, rest time) for fine sub-millisecond effective flip power

### Fixed

-  PulseDriver(..) now rejects out-of-range pulse times (> 255ms) instead of emitting a malformed DL: command


## [0.4.8] - 2025-8-20

### Adjusted

-  void ConfigureFlipper_DualEx now has a hold PWM (or defaults to 100%)


## [0.4.7] - 2024-9-23

### Added

-  void ConfigureFlipper_SingleEx for more finite control of single-driver flippers


## [0.4.6] - 2024-9-23

### Added

- Neuron now supports 120 switches

### FIXED

- Neuron Switch out of range exceptions


## [0.4.5] - 2024-2-25

### Added

- Servo callbacks
- Servo_SetEstimatedTravelTime(..) for approximate position querying and callback responses
- STUB loop/net core for proper spoof switch & solenoid timings

### Fixed

- Stepper_Initialize(..) no longer incorrectly returns false on success
- Spoof switches when no hardware is detected
- Cleaner serial thread disposing


## [0.4.4] - 2024-2-16

### Fixed
- Stepper_Initialize(..) expansion index calculation
- Stepper_RunMs(..) millisecond time queue while motor is running

### Adjusted
- Expansion module core refactor


## [0.4.3] - 2024-2-15

### Added

- Intellisense documentation for functions


## [0.4.2] - 2024-2-14

### Added Stepper Functions
- Stepper_Initialize
- Stepper_SetStepsPerSecond
- Stepper_SetSpeedPercent
- Stepper_EnableMotorRamping
- Stepper_RunMs
- Stepper_Stop

*NOTE* Run-timing callbacks are not used fully

### Fixed
- ConfigureAutoFireDriver(..) PulesMS is now used properly

### Adjusted
- Expansion module core refactor


## [0.3.7] - 2023-12-18

### Adjusted
- ConfigureFlipper_Dual(..) now allows you to specify the strong coil pulse MS


## [0.3.6] - 2023-10-26

### Added Servo Functions
- Servo_SetConfig
- Servo_SetHomeRotation
- Servo_SendHome
- Servo_RotateTo
- Servo_GetRotation


## [0.3.5] - 2023-10-25

### Adjusted
- ConfigureFlipper_Dual(..) now allows you to specify a normally open or normally closed EOS switch

### Fixed
- ConfigureFlipper_Dual(..) config and EOS now works as expected

## [0.3.3] - 2023-10-14

### Added
- Switch config commands

### Adjusted
- ConfigureFlipper_Dual(..) configs

## [0.3.2] - 2023-10-5

### Fixed
- Assembly naming issues with DLLs

## [0.3.1] - 2023-10-4

### Changed
- A prefab is no longer needed. The call to FAST_Pinball.FAST.Startup(...) will automatically create a game object and initialize it
- The example scene has been updated to reflect the prefab requirement change

### Fixed
- Multi platform exporting

### Added
- GI commands

## [0.3.0] - 2023-09-26
*First real release*

### Fixed
- Package manager layouts

### Changed
- Git repository format so it works with package manager and the UPM Git Extention

### Added
- More Documentation
- Example scene and code
