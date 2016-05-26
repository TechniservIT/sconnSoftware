using System;
using System.Collections.Generic;
using System.Text;

namespace sconnJTAG.Service.BoundaryScan
{

    /*
        SVF Structure
    The SVF file is defined as an ASCII file that consists of a set of SVF statements.
    Statements are terminated by a semicolon (;) and may continue for more than one line. The
    maximum number of ASCII characters per line is 256. SVF is not case sensitive, and
    comments can be inserted into an SVF file after an exclamation point (!) or a pair of slashes
    (//).
    Each statement consists of a command and parameters associated with that specific
    command. Commands can be grouped into three types: state commands, offset commands,
    and parallel commands.
    */

/*
State commands
State commands are used to specify how the test sequences will traverse the IEEE
1149.1 TAP state machine. The following state commands are supported:
SDR Scan Data Register
SIR Scan Instruction Register
ENDDR Define end state of DR scan
ENDIR Define end state of IR scan
RUNTEST Enter Run-Test/Idle state
STATE Go to specified stable state
TRST Drive the TRST line to the designated level
SDR performs an IEEE 1149.1 Data Register scan. SIR performs an IEEE 1149.1
Instruction Register scan. ENDDR and ENDIR establish a default state for the bus following
any Data Register scan or Instruction Register scan, respectively. RUNTEST goes to Runtest/
Idle state for a specific number of TCKs. for each of the above commands, a default path
through the state machine is used. Each of these commands also terminates in a stable,
nonscannable state.
STATE places the bus in a designated IEEE 1149.1 stable state. TRST activates or
deactivates the optional test reset signal of the IEEE 1149.1 bus.
*/




/*
Offset Commands
Offset commands allow a series of SVF commands to be targeted towards a
contiguous series of points in the scan path. Examples would be a sequence for executing selftest
on a device, or a cluster test where al devices involved in the cluster test are grouped
together. The following offset commands are supported:
HDR Header data for data bits
HIR Header data for instruction bits
TDR Trailer data for data bits
TIR Trailer data for instruction bits
HDR specifies a particular pattern of data bits to be padded onto the front of every data
scan. HIR specifies the same for the front of every Instruction Register scan. These patterns
need only be specified once and are included on each scan unless changed by a subsequent
HDR, HIR, TDR or TIR command.
*/





        /*
Parallel Commands
Parallel commands are used to map and apply the following commands:
PIO Specifies a parallel test pattern
PIOMAP Designates the mapping of bits in the PIO command to logical
pin names
Parallel commands allow SVF to combine serial and parallel sequences. PIOMAP
commands are used by parallel I/O controllers to map data bits in the command into parallel
I/O channels using the ASCII logical pin name as a reference. The PIO command specifies the
execution of a parallel pattern application/sample. SVF does not specify any other properties
of parallel I/O such as drive, levels, or skew.
Default State Transitions
SVF uses names for the TAP states that are similar to the IEEE 1149.1 TAP state
names. Below is a list of SVF equivalent names for the TAP states.
Test-logic-Reset [RESET]
Run-Test/Idle [IDLE]
Select-DR-Scan [DRSELECT]
Capture-DR [DRCAPTURE]
Shift-DR [DRSHIFT]
Pause-DR [DRPAUSE]
Exit1-DR [DREXIT1]
Exit2-DR [DREXIT2]
Update-DR [DRUPDATE]
Select-IR-Scan [IRSELECT]
Capture-IR [IRCAPTURE]
Shift-IR [IRSHIFT]
Pause-IR [IRPAUSE]
Exit1-IR [IREXIT1]
Exit2-IR [IREXIT2]
Update-IR [IRUPDATE]
Below is a listing to identify sample default paths taken when transitioning from one
state to a specified new state. For example, if the current state is RESET and you select
DRPAUSE as the end state, the TAP moves from RESET through IDLE, DRSELECT,
DRCAPTURE, DREXIT1 to DRPAUSE. You only have to specify the current and end states
and not each intermediate step.
Current State End State State Path
RESET RESET RESET
RESET IDLE RESET
IDLE
RESET DRPAUSE RESET
IDLE
DRSELECT
DRCAPTURE
DREXIT1
DRPAUSE
RESET IRPAUSE RESET
IDLE
DRSELECT
IRSELECT
IRCAPTURE
IREXIT1
IRPAUSE
SVF Example
The following is an example SVF file:
! Begin Test Program
! Disable Test Reset line
TRST OFF;
! Initialize UUT
STATE RESET;
! End IR scans in DRPAUSE
ENDIR DRPAUSE;
! 24 bit IR header
HIR 24 TDI (FFFFFF);
! 3 bit DR header
HDR 3 TDI (7) TDO (7) MASK (0);
! 16 bit IR trailer
TIR 16 TDI (FFFF);
! 2 bit DR trailer
TDR 2 TDI (3);
! 8 bit IR scan, load BIST seed
SDR 16 TDI (ABCD);
! RUNBIST for 95 TCK Clocks
RUNTEST 95 TCK ENDSTATE IRPAUSE
! 16 bit DR scan, check BIST status
SDR 16 TDI (0000) TDO (1234) MASK (FFFF);
! Enter Test-Logic-Reset
STATE RESET;
! End Test Program
The test begins by deasserting TRST. The DRPAUSE state is established as the default
end state for instruction scans and data scans. Twenty four bits of header and sixteen bits of
trailer data are specified for Instruction Register scans. No status bits are checked. three bits of
header data and two bits of trailer data are specified for Data Register scans.
In the example above, a single device in the middle of the scan is targeted. Notice from
the 24-bit IR header (3x8-bit IR) and the 3-bit DR header (3x1-bit DR) that the targeted
device has three devices before it in the scan path. From the 16-bit IR trailer (2x8-bit IR) and
the 2-bit DR trailer (2x2-bit DR), the targeted device has 2 devices following it in the scan
path. After the header and trailer offsets are established all subsequent scans are the
concatenation of the header, scan data, and trailer bits. the targeted device supports BIST,
which is initialized by scanning a hex ABCD into the selected Data Register. the BIST in the
targeted device is then executed by entering the Run-Test/Idle state for 95 clock cycles. Next,
the BIST result is scanned out and the status bits compared against a deterministic value to
determine pass/fail.

    */

public class BoundaryScanSerialVectorFormatInterpreter
{
}
}
