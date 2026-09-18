# Changelog
All notable changes to this package will be documented in this file. The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)


## [0.5.2] - 2026-9-21

### Fixed

-  0.5.1's evict-oldest write queue used one capacity (16) for every port, including the
   I/O Loop port carrying switch/solenoid traffic. That traffic is discrete, must-deliver
   commands (a pulse, a startup/shutdown/watchdog configuration sweep), not coalescable
   state -- a legitimate burst of a few dozen such commands could have overflowed a
   16-deep queue and silently evicted a real command, which would have been a worse
   regression than the one 0.5.1 fixed.
-  Per-port write-queue capacity is now set per device type once `FAST.StartupProcess`'s
   "ID:" handshake classifies the port: `PIXEL_WRITE_QUEUE_CAPACITY` (16) for the pixel/
   lighting Expansion bus, where staleness is safe to discard, and the new
   `COMMAND_WRITE_QUEUE_CAPACITY` (512) for switches/solenoids/servos/steppers/displays,
   where every message must eventually be delivered. `FastSerialCommunicator.
   SetPortWriteCapacity(FastDevIndex, Capacity)` is the new entry point that applies this
   (public, in case a project needs to tune it further for its own device mix).


## [0.5.1] - 2026-9-21

### Fixed

-  The per-port write queue introduced in 0.5.0 rejected new sends once full (dropping the
   NEWEST payload) and only evicted a `WRITE_QUEUE_CAPACITY` of 128 stale entries slowly,
   so a burst of full-state updates (e.g. a lightshow driving many pixels every frame)
   could starve out the most recent state for a noticeable number of frames, and its
   backlog of already-superseded frames could delay -- or under sustained load, cause a
   dropped -- final "reset to default" update after the burst ended, leaving some lamps
   visibly stuck mid-show.
-  `_FastHardwareSend` now evicts the OLDEST queued payload to make room for a new one
   instead of rejecting the new one, via `EnqueueWithEviction`. Since sends represent the
   current full state of a target (not independent one-off events), the newest payload
   should always win under backpressure rather than being starved behind stale history.
-  `WRITE_QUEUE_CAPACITY` reduced from 128 to 16 per port, tightening the worst-case
   display lag bound under sustained bandwidth overload from ~2.3s to a small fraction of
   a second.


## [0.5.0] - 2026-9-18

### Changed

-  All outgoing serial writes (pixels, solenoids, switches, servos, steppers -- everything
   that funnels through `FastSerialCommunicator._FastHardwareSend`) are now handed off to a
   dedicated background writer thread per physical port instead of blocking the calling
   thread with a direct `SerialPort.Write(...)`. Previously, sending a large burst of
   changes in one frame (e.g. many pixels changing at once) could stall the main/game
   thread for several milliseconds while the OS serial buffer drained.
-  Outgoing write buffers are now pooled per-port instead of allocated fresh on every send,
   removing steady-state per-frame GC allocation on the write path. Pools are strictly
   per-port (never shared across devices/cores), and every queued payload carries its own
   explicit length alongside its buffer so a pooled buffer's stale trailing bytes from a
   previous, larger send can never be read or transmitted.

### Added

-  `SerialPort.WriteTimeout` (250ms) is now set when a port is opened, bounding how long
   the background writer thread's `Write()` can block if a port stalls or is unplugged,
   so a dead port can no longer hang that thread indefinitely.


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
