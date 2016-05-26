using System;
using System.Collections.Generic;
using System.Text;

namespace sconnJTAG.Service.BoundaryScan
{

    /*
        A BSDL description for a device consists of the following elements:
            • Entity descriptions
            • Generic parameter
            • Logical Port description
            • Use statements
            • Pin Mapping(s)
            • Scan Port identification
            • Instruction Register description
            • Register Access description
            • Boundary Register description
    */

    public class BoundaryScanDescriptionLanguageInterpreter
    {

        /*
            Entity Descriptions -- The entity statement names the entity, such as the device name (e.g.,
            SN74ABT8245). An entity description begins with an entity statement and terminates
            with an end statement.
            entity XYZ is
            {statements to describe the entity go here}
            end XYZ
        */



/*
    Generic Parameter -- A generic parameter is a parameter that may come from outside the
    entity, or it may be defaulted, such as a package type (e.g., "DW").
    generic (PHYSICAL_PIN_MAP : string := "DW");
*/


/*
    Logical Port Description -- The port description gives logical names to the I/O pins (system
    and TAP pins), and denotes their nature such as input, output, bidirectional, and so on.
    port (OE:in bit;
    Y:out bit_vector(1 to 3);
    A:in bit_vector(1 to 3);
    GND, VCC, NC:linkage bit;
    TDO:out bit;
    TMS, TDI, TCK:in bit);
*/


/*
Use Statements -- The use statement refers to external definitions found in packages and
package bodies.
use STD_1149_1_1994.all;
*/



/*
Pin Mapping(s) -- The pin mapping provides a mapping of logical signals onto the physical
pins of a particular device package.
attribute PIN_MAP of XYZ : entity is
PHYSICAL_PIN_MAP;
constant DW:PIN_MAP_STRING:=
"OE:1, Y:(2,3,4), A:(5,6,7), GND:8, VCC:9, "&
"TDO:10, TDI:11, TMS:12, TCK:13, NC:14";
*/

/*
Scan Port Identification -- The scan port identification statements define the device's TAP.
attribute TAP_SCAN_IN of TDI : signal is TRUE;
attribute TAP_SCAN_OUT of TDO : signal is TRUE;
attribute TAP_SCAN_MODE of TMS : signal is TRUE;
attribute TAP_SCAN_CLOCK of TCK : signal is (50.0e6,
BOTH);
*/



/*
Instruction Register Description -- The Instruction Register description identifies the devicedependent
characteristics of the Instruction Register.
attribute INSTRUCTION_LENGTH of XYZ : entity is 2;
attribute INSTRUCTION_OPCODE of XYZ : entity is
"BYPASS (11), "&
"EXTEST (00), "&
"SAMPLE (10) ";
attribute INSTRUCTION_CAPTURE of XYZ : entity is
"01";
*/


/*
Register Access Description -- The register access defines which register is placed between
TDI and TDO for each instruction.
attribute REGISTER_ACCESS of XYZ : entity is
"BOUNDARY (EXTEST, SAMPLE), "&
"BYPASS (BYPASS) ";
*/


/*
Boundary Register Description -- The Boundary Register description contains a list of
boundary-scan cells, along with information regarding the cell type and associated
control.
attribute BOUNDARY_LENGTH of XYZ : entity is 7;
attribute BOUNDARY_REGISTER of XYZ : entity is
"0 (BC_1, Y(1), output3, X, 6, 0, Z), "&
"1 (BC_1, Y(2), output3, X, 6, 0, Z), "&
"2 (BC_1, Y(3), output3, X, 6, 0, Z), "&
"3 (BC_1, A(1), input, X), "&
"4 (BC_1, A(2), input, X), "&
"5 (BC_1, A(3), input, X), "&
"6 (BC_1, OE, input, X), "&
"6 (BC_1, *, control, 0)";

    */

}
}
