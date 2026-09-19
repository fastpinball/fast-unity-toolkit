# Changelog
All notable changes to this package will be documented in this file. The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)


## [0.6.2] - 2026-9-19

### Added

-  `FAST.ConfigurePixelPortLength(bankIndex, port, startIndex, count, type, dest)` sends
   the EXP bus's `ER:` command, reassigning how many of a 128-pixel bank's LED indices are
   routed to one physical LED output port (and where in the bank that run starts) --
   overriding a board's default fixed split (e.g. 4 ports of 32). `bankIndex` is 0-based
   across every 128-pixel bank registered for `dest`, in the same order boards were listed
   in `ExpansionOrder` at startup (a two-bank board, e.g. FP-EXP-0081, counts as two
   consecutive indices, each with its own EXP address and its own local 0-F port
   numbering). Needed to turn e.g. a two-bank board's two halves into two full 128-LED
   runs (port 0 on each bank) instead of the default four 32-LED ports per bank.


## [0.6.1] - 2026-9-19

### Added

-  Colour-table programming is now firmware-version aware. Boards on EXP firmware 0.48 or
   earlier keep using the single-entry binary `RT:` command; boards on firmware newer than
   0.48 use the new count-prefixed `RU:` command instead, which is what that firmware
   replaces `RT:` with.
-  Each board's OWN firmware version is now used for that gate, not one shared bus-level
   guess. The EXP bus's "ID:" handshake is a broadcast -- every attached board (and the
   Neuron itself) replies with its own "ID:EXP <ProductID> <Firmware>" line, so boards can
   genuinely run different firmware from each other. `FAST.StartupProcess` now drains all
   of those replies instead of just the first, and matches each one to the board being
   constructed by its ProductID (`FP-EXP-0061/0071/0081/0091`, `FP-EXP-1313` for the
   Neuron) in reply order.
-  `FAST.SetPaletteEntries(indices, colours, dest)` programs several palette entries in
   one call. On firmware newer than 0.48 this batches them into as few `RU:` messages as
   possible (chunked to the protocol's 127-entry count-byte limit, same as `RD:`/`RC:`);
   on 0.48 and earlier -- where batching does not work in firmware -- it falls back
   automatically to one `RT:` message per entry, same as calling `SetPaletteEntry` in a
   loop.


## [0.6.0] - 2026-9-22

### Added

-  Optional pixel **palette mode**, off by default. `FAST.SetPixelPaletteMode(true)`
   switches `SetPixelColour`/`SetPixelColours` over to the binary `RC:` wire command
   (index + 1-byte palette entry, 2 bytes/pixel) instead of `RD:` (index + R,G,B, 4
   bytes/pixel). `FAST.SetPaletteEntry(index, colour, dest)` programs one of 256 shared
   table entries via the binary `RT:` command and registers it for colour-to-index
   matching. Matching is always nearest-match, not exact -- incoming colours are
   continuous (fades, blends) and will essentially never hit a table entry exactly.
-  `FAST.StandardPixelPalette` -- a built-in default 224-entry palette (Grey + 15 hues
   around the HSV wheel, each a linear brightness ramp, floor -> 224). 14 of the hues are
   evenly spaced every 360/14 degrees; the 15th is an extra "amber" anchor at the midpoint
   between orange and yellow, where the even spacing otherwise left a noticeably more
   abrupt step than the rest of the wheel -- hue matching doesn't require even spacing.
   `StandardPixelPalette.Setup(dest)` programs it and switches palette mode on in one
   call, installing an O(1) analytical hue-row/shade search as the default converter --
   the hue row is computed directly from a closed-form HSV hue angle (`GetHueAngle`) and
   mapped to a row with pure arithmetic (`GetHueRow`, a round + one small special-cased
   branch for the amber anchor), no table scan of any kind. `PixelPalette.GetNearestIndex`
   (a generic O(256) linear scan) remains available as a fallback for a genuinely custom,
   non-standard palette. `Setup` takes a `UseFastMath` parameter (default true) to select
   between the two.
-  `PendingWrite.Critical` (`FastHardwareSerialPort.cs`): discrete must-deliver commands
   (`RT:`, `RA@`, `RF@`, and switch/solenoid/servo/stepper commands via `SendString`)
   are exempt from the write queue's evict-oldest policy, so they can never be silently
   dropped under pixel-traffic queue pressure.
-  Per-port write-queue capacity is now set per device type once `FAST.StartupProcess`'s
   "ID:" handshake classifies the port: `PIXEL_WRITE_QUEUE_CAPACITY` (16) for the pixel/
   lighting Expansion bus, where staleness is safe to discard, and
   `COMMAND_WRITE_QUEUE_CAPACITY` (512) for switches/solenoids/servos/steppers/displays,
   where every message must be delivered. `FastSerialCommunicator.SetPortWriteCapacity
   (FastDevIndex, Capacity)` is the entry point that applies this.

### Fixed

-  `RT:` is a binary command (one raw index byte followed by raw R,G,B bytes), not ASCII
   text -- `QueueColorTableEntry` builds the correct binary payload and queues it
   (`PendingColorTableEntries`) for `SendSerialCommand` to send once a port index is
   available. Sent one entry per command; multi-entry batching does not currently work
   in firmware.
-  `ExpansionBase.QueueColorTableEntry` now programs every attached pixel breakout board
   -- each board has its own onboard colour-table memory, so the table has to be written
   to each one individually, not just the first.
-  `RC:`/`RD:` sends are chunked to <=127 LEDs per message, matching the protocol's
   count-byte limit (`0x00`-`0x7F`) -- a full 128-pixel update could otherwise produce an
   out-of-spec count byte.
-  `StandardPixelPalette.GetIndex`'s hue-row match now uses cosine similarity
   (`dot(C, RowHue)/|RowHue|`) instead of a direction normalised by `1/(maxC-minC)`,
   which was numerically unstable for dim, pastel, or fading-toward-black colours.
-  `StandardPixelPalette.GetIndex`'s shade/column stage is now derived directly from a
   colour's brightness (`maxC`) instead of a 3D distance search against the matched
   row's entries, which was contaminated by hue mismatch between the incoming colour and
   the row's anchor.
-  The Grey-or-colour classification now uses relative saturation (`(max-min)/max`)
   instead of an absolute channel spread, so a dim but fully-saturated colour no longer
   misclassifies as Grey.
-  The default palette's 14 hues are generated from the HSV hue-wheel formula (one hue
   every 360/14 degrees at S=1,V=1) instead of hand-picked approximations, giving true
   blue/magenta and even spacing.
-  `BreakoutModule_128_Pixel.SendSerialCommand` caps how much of `SpecialSerialMessages`
   it flushes per call (`MAX_SPECIAL_MESSAGE_FLUSH_BYTES`, 256), cutting only at a
   complete command boundary, so a large backlog (e.g. a full palette upload) drains
   over several frames instead of landing as one uninterrupted burst.
-  0.5.1's evict-oldest write queue used one capacity (16) for every port, including the
   I/O Loop port carrying switch/solenoid traffic, which needs a much larger burst
   allowance since every command there must eventually be delivered (see the per-device
   capacities and `Critical` flag above).


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
