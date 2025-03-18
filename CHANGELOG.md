# Changelog
All notable changes to this package will be documented in this file. The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)

## [0.4.6] - 2024-9-23

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
