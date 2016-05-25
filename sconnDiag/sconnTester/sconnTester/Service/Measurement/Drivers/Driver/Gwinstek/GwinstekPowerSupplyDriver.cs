using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnTester.Model.Test;

namespace sconnTester.Service.Measurement.Drivers.Driver.Gwinstek
{
    public  class GwinstekPowerSupplyDriver : ISerialInterfaceDriver
    {
        public bool Connect()
        {
            throw new NotImplementedException();
        }

        public bool Connected { get; set; }
        public int Channels { get; set; }
        public double GetValue(int channel, ElectricMeasurementType type)
        {
            throw new NotImplementedException();
        }

        public void SetValue(int channel, double value, ElectricMeasurementType type)
        {
            throw new NotImplementedException();
        }


        /*
         * 
         * 
         * RS232 message terminator
The power supply has 25 commands available. Every command is end
up with <cr> (ASCII 0Dh or ACSCII 0D 0A acceptable). The return
message <cr>of the power supply is CR/LF (ASCII 0D 0A).
*L
Function:
To obtain all the status values of the power supply.
Syntax:
L<cr> HEX = 4C 0D
Explain:
When the message L<cr>is sent to the power supply from computer,
the power supply will return the message as follows immediately:
Vvv.vvAa.aaaWwww.wUuuIi.iiPpppFffffff<cr> 37 characters
totally
The contents consist of the uppercase V,A,W,U,I,P,F, the numeral
from 0 to 9 and decimal. Further details is described as follows:
vv.vv = The present output voltage, the unit: V.
a.aaa = The present output current, the unit: A.
www.w = The present output load, the unit: W.
uu = The maximum voltage limit at present, the unit: V.
i.ii = The maximum current limit at present, the unit: A.
ppp = The maximum load limit at present, the unit: W.
PSP SERIES
USER MANUAL
24
ffffff = The status of power supply at present.
1st f = the relay status 0: OFF 1:ON
2nd f = the temperature status 0: Normal 1: Overheat
3rd f = the wheel knob status 0: Normal 1: Fine
4th f = the wheel knob status 0: Lock 1: Unlock
5th f = the remote status 0: Normal 1: Remote(*)
6th f = the lock status 0: Unlock 1: Lock
*Note: The setting is workable through computer only when the
remote is at 1.
All the data above is in the range from 0 to 9.
When the uppercase U becoming the lowercase u means that the status
is in the setting of the voltage limit mode.
When the uppercase I becoming the lowercase i means that the status
is in the setting of the current limit mode.
When the uppercase P becoming the lowercase p means that the status
is in the setting of the load limit mode.
Example:
The return message from power supply is:
V20.00A2.500W050.0U40I5.00P200F101000<cr>
V20.00 means that the present output voltage is at 20.00V.
A2.500 means that the present output current is at 2.500A.
W050.0 means that the present output load is at 050.0W.
U40 means that the present voltage limit is at 40V.
15.00 means that the present current limit is at 5.00A.
P200 means that the present load limit is at 200W.
PSP SERIES
USER MANUAL
25
F101000
Not yet getting into Lock mode.
Not yet getting into Remote mode.
Wheel knob setting acceptable (this signal can
be ignored)
Wheel knob is in the Fine mode.
The temperature isn’t overheat.
Relay on.
*V
Function:
The present output voltage, the unit is V.
Syntax:
V<cr> HEX = 56 0D
Explain:
When the message of V<cr>is sent to the power supply from computer,
the power supply will return the following message immediately:
Vvv.vv<cr> 6 characters totally + CR/LF
The contents consist of the uppercase V, the numeral from 0 to 9 and
decimal. Further details is described as follows:
vv.vv = The present output voltage, the unit: V
PSP SERIES
USER MANUAL
26
*A
Function:
The present output current, the unit is A.
Syntax:
A<cr> HEX = 41 0D
Explain:
When the message of A<cr>is sent to the power supply from computer,
the power supply will return the following message immediately:
Aa.aaa<cr> 6 characters totally + CR/LF
The contents consist of the uppercase A, the numeral from 0 to 9 and
decimal. Further details is described as follows:
a.aaa = The present output current, the unit: A
*W
Function:
The present output load, the unit is W.
Syntax:
W<cr> HEX = 57 0D
Explain:
When the message of W<cr>is sent to the power supply from
computer, the power supply will return the following message
immediately:
Wwww.w<cr> 6 characters totally + CR/LF
The contents consist of the uppercase W, the numeral from 0 to 9 and
decimal. Further details is described as follows:
www.w = The present output load, the unit: W
PSP SERIES
USER MANUAL
27
*U
Function:
The maximum voltage limit at present, the unit is V.
Syntax:
U<cr> HEX = 55 0D
Explain:
When the message of U<cr> is sent to the power supply from
computer, the power supply will return the following message
immediately:
Uuu<cr> 3 characters totally + CR/LF
The contents consist of the uppercase U and the numeral from 0 to 9.
Further details is described as follows:
uu = The maximum voltage limit at present, the unit: V
When the uppercase U becoming the lowercase u means that the
power supply is in the setting status of voltage limit mode.
*I
Function:
The maximum current limit at present, the unit is A.
Syntax:
I<cr> HEX = 49 0D
Explain:
When the message of I<cr>is sent to the power supply from computer,
the power supply will return the following message immediately:
Ii.iii<cr> 5 characters totally + CR/LF
PSP SERIES
USER MANUAL
28
The contents consist of the uppercase I, the numeral from 0 to 9 and
decimal. Further details is described as follows:
i.ii = The maximum current limit at present, the unit: A
When the uppercase U becoming the lowercase u means that the
power supply is in the setting status of current limit mode.
*P
Function:
The maximum output load limit at present, the unit is W.
Syntax:
L<cr> HEX = 50 0D
Explain:
When the message of L<cr>is sent to the power supply from computer,
the power supply will return the following message immediately:
Pppp<cr> 4 characters totally + CR/LF
The contents consist of the uppercase P and the numeral from 0 to 9.
Further details is described as follows:
ppp = The maximum load limit at present, the unit: W
When the uppercase P becoming the lowercase p means that the power
supply is in the setting status of output load limit mode.
*F
Function:
The present status of the power supply.
PSP SERIES
USER MANUAL
29
Syntax:
F<cr> HEX = 46 0D
Explain:
When the message of F<cr> is sent to the power supply from computer,
the power supply will return the following message immediately:
Fffffff<cr> 7 characters totally + CR/LF
∣∣∣∣∣∣
 123446
The contents consist of the uppercase F and the numeral from 0 to 9.
Further details is described as follows:
1st f = the relay status 0:OFF 1:ON
2nd f = the temperature status 0: Normal 1: Overheat
3rd f = the wheel knob status 0: Normal 1: Fine
4th f = the wheel knob status 0: Lock 1: Unlock
5th f = the remote status 0: Normal 1: Remote(*)
6th f = the lock status 0: Unlock 1: Lock
*Note: The setting is workable through computer only when the
remote is at 1.
*SV+
Function:
Add one unit to the present voltage setting.
Syntax:
SV+<cr> HEX = 53 56 2B 0D
Explain:
When the message of SV+<cr> is sent to the power supply from
computer, the power supply will add one unit to the present voltage
setting immediately.
PSP SERIES
USER MANUAL
30
Example:
The present output voltage is at 20.00V, and the wheel knob status is
at normal, the SV+<cr> message is sent to the power supply, the
voltage of which will become 21.00V.
*SVFunction:
Subtract one unit from the present voltage setting.
Syntax:
SV-<cr> HEX = 53 56 2D 0D
Explain:
When the message of SV-<cr> is sent to the power supply from
computer, the power supply will subtract one unit from the present
voltage setting immediately.
Example:
The present output voltage is at 20.00V, and the wheel knob status is
at normal, the SV-<cr> message is sent to the power supply, the
voltage of which will become 19.00V.
*SU+
Function:
Add one unit to the present voltage limit setting.
Syntax:
SU+<cr> HEX = 53 55 2B 0D
Explain:
When the message of SU+<cr> is sent to the power supply from
computer, the power supply will add one unit to the present voltage
limit setting immediately.
PSP SERIES
USER MANUAL
31
Example:
The present voltage limit is at 30V, and the wheel knob status is at
normal, the SV+<cr> message is sent to the power supply, the voltage
limit of which will become 31V.
*SUFunction:
Subtract one unit from the present voltage limit setting.
Syntax:
SU-<cr> HEX = 53 55 2D 0D
Explain:
When the message of SU-<cr> is sent to the power supply from
computer, the power supply will subtract one unit from the present
voltage limit setting immediately.
PSP SERIES
USER MANUAL
32
Example:
The present voltage limit is at 30V, and the wheel knob status is at
normal, the SU-<cr> message is sent to the power supply, the voltage
limit of which will become 29V.
*SI+
Function:
Add one unit to the present current limit setting.
Syntax:
SI+<cr> HEX = 53 49 2B 0D
Explain:
When the message of SI+<cr> is sent to the power supply from
computer, the power supply will add one unit to the present current
limit setting immediately.
Example:
The present current limit is at 3.00A, and the wheel knob status is at
normal, the SI+<cr> message is sent to the power supply, the current
limit of which will become 3.10A.
*SIFunction:
Subtract one unit from the present current limit setting.
Syntax:
SI-<cr> HEX = 53 49 2D 0D
Explain:
When the message of SI-<cr> is sent to the power supply from
computer, the power supply will subtract one unit from the present
current limit setting immediately.
PSP SERIES
USER MANUAL
33
Example:
The present current limit is at 3.00A, and the wheel knob status is at
normal, the SI-<cr> message is sent to the power supply, the current
limit of which will become 2.90A.
*SP+
Function:
Add one unit to the present load limit setting.
Syntax:
SP+<cr> HEX = 53 50 2B 0D
Explain:
When the message of SP+<cr> is sent to the power supply from
computer, the power supply will add one unit to the present load limit
setting immediately.
Example:
The present load limit is at 100W, and the wheel knob status is at
normal, the SP+<cr> message is sent to the power supply, the load
limit of which will become 101W.
*SPFunction:
Subtract one unit from the present load limit setting.
Syntax:
SP-<cr> HEX = 53 50 2D 0D
Explain:
When the message of SP-<cr> is sent to the power supply from
computer, the power supply will subtract one unit from the present
load limit setting immediately.
PSP SERIES
USER MANUAL
34
Example:
The present load limit is at 100W, and the wheel knob status is at
normal, the SP-<cr> message is sent to the power supply, the load
limit of which will become 099W.
*SUM
Function:
Set the maximum voltage limit value.
Syntax:
SUM<cr> HEX = 53 55 4D 0D
Explain:
When the message of SUM<cr> is sent to the power supply from
computer, the power supply will set the voltage limit to the maximum
immediately.
Example:
The present voltage limit is at 20V, the SUM<cr> message is sent to
the power supply, the voltage limit of which will become 40V.
*SIM
Function:
Set the maximum current limit value.
Syntax:
SIM<cr> HEX = 53 49 4D 0D
Explain:
When the message of SIM<cr> is sent to the power supply from
computer, the power supply will set the current limit to the maximum
immediately.
PSP SERIES
USER MANUAL
35
Example:
The present current limit is at 2.50A, the SIM<cr> message is sent to
the power supply, the current limit of which will become 5.00A.
*SPM
Function:
Set the maximum load limit value.
Syntax:
SPM<cr> HEX = 53 50 4D 0D
Explain:
When the message of SPM<cr> is sent to the power supply from
computer, the power supply will set the load limit to the maximum
immediately.
Example:
The present load limit is at 100W, the SPM<cr> message is sent to the
power supply, the load limit of which will become 200W.
*KF
Function:
Set the wheel knob to Fine status.
Syntax:
KF<cr> HEX = 4B 46 0D
Explain:
When the message of KF<cr> is sent to the power supply from
computer, the power supply will set the wheel knob to Fine status
immediately.
Example:
The present wheel knob status is at Normal, the KF<cr> message is
PSP SERIES
USER MANUAL
36
sent to the power supply, the wheel knob status will become Fine.
*KN
Function:
Set the wheel knob to Normal status.
Syntax:
KF<cr> HEX = 4B 4E 0D
Explain:
When the message of KN<cr> is sent to the power supply from
computer, the power supply will set the wheel knob to Normal status
immediately.
Example:
The present wheel knob status is at Fine, the KN<cr> message is sent
to the power supply, the wheel knob status will become Normal.
*KO
Function:
Set the Relay status to Invert.
Syntax:
KO<cr> HEX = 4B 4F 0D
Explain:
When the message of KO<cr> is sent to the power supply from
computer, the power supply will invert the relay status immediately.
Example:
The present relay status is at OFF, the KO<cr> message is sent to the
power supply, the relay status will become ON, send the message
again will become OFF.
PSP SERIES
USER MANUAL
37
*KOE
Function:
Set the Relay status to ON.
Syntax:
KOE<cr> HEX = 4B 4F 45 0D
Explain:
When the message of KOE<cr> is sent to the power supply from
computer, whatever the relay status is, the relay of power supply will
be set to ON immediately.
*KOD
Function:
Set the Relay status to OFF.
Syntax:
KOD<cr> HEX = 4B 4F 44 0D
Explain:
When the message of KOD<cr> is sent to the power supply from
computer, whatever the relay status is, the relay of power supply will
be set to OFF immediately.
*EEP
Function:
Save the present status to the EEPROM.
Syntax:
EEP<cr> HEX = 45 45 50 0D
Explain:
When the message of EEP<cr> is sent to the power supply from
computer, the power supply will be save the present setting value to
EEPROM immediately.
PSP SERIES
USER MANUAL
38
*B
Function:
To obtain +% value.
Syntax:
B<cr> HEX = 42 0D
Explain:
When the message of B<cr> is sent to the power supply from
computer, the power supply will return the following message
immediately
Bbbb<cr> 4 characters totally +CR/LF
The contents consist of the uppercase B, and the numeral from 0 to 9.
Further details is described as follows:
bbb = The present +% value, the unit: %
When the uppercase B becoming the lowercase b means that the status
is in the setting of the +% mode.
*D
Function:
To obtain -% value.
Syntax:
D<cr> HEX = 44 0D
PSP SERIES
USER MANUAL
39
Explain:
When the message of D<cr> is sent to the power supply from
computer, the power supply will return the following message
immediately
Dddd<cr> 4 characters totally +CR/LF
The contents consist of the uppercase D, and the numeral from 0 to 9.
Further details is described as follows:
ddd = The present -% value, the unit: %
When the uppercase D becoming the lowercase d means that the status
is in the setting of the -% mode.
*Q
Function:
Display the present value at +% or -% mode.
Syntax:
Q<cr> HEX = 51 0D
Explain:
When the message of Q<cr> is sent to the power supply from
computer, the power supply will return the following message
immediately
Qqqqqqq<cr> 7 characters totally +CR/LF
PSP SERIES
USER MANUAL
40
The contents consist of the uppercase B, and the numeral 0 or 1.
Further details is described as follows:
Whether the first q is at % mode? 0: No 1:Yes
Whether the second q is at +% mode? 0: No 1: Yes
*SB+
Function:
To add one unit to the present setting of +%.
Syntax:
SB+<cr> HEX = 53 42 2B 0D
Explain:
When the message of SB+<cr> is sent to the power supply from
computer, the power supply will add one unit to the present setting of
+% immediately
Example:
The present +% value is at 105, after the command is sent from
computer, the +% value is at 106.
*SBFunction:
To decrease one unit from the present setting of +%.
Syntax:
SB-<cr> HEX = 53 42 2D 0D
PSP SERIES
USER MANUAL
41
Explain:
When the message of SD-<cr> is sent to the power supply from
computer, the power supply will decrease one unit from the present
setting of +% immediately
Example:
The present +% value is at 105, after the command is sent from
computer, the +% value is at 104.
*SD+
Function:
To add one unit to the present setting of -%.
Syntax:
SD+<cr> HEX = 53 44 2B 0D
Explain:
When the message of SD+<cr> is sent to the power supply from
computer, the power supply will add one unit to the present setting of -
% immediately
Example:
The present -% value is at 90, after the command is sent from
computer, the -% value is at 91.
PSP SERIES
USER MANUAL
42
*SDFunction:
To decrease one unit from the present setting of -%.
Syntax:
SD-<cr> HEX = 53 44 2D 0D
Explain:
When the message of SD-<cr> is sent to the power supply from
computer, the power supply will decrease one unit from the present
setting of -% immediately
Example:
The present -% value is at 90, after the command is sent from
computer, the -% value is at 89.
*SV
Function:
Set the output voltage value.
Syntax:
SV xx.xx<cr>
x is a number between 0 and 9.
Explain:
The power supply will set the desired value of output voltage when the
command is received.
PSP SERIES
USER MANUAL
43
Example:
SV 12.34
Set output voltage to 12.34V
*SU
Function:
Set voltage limit.
Syntax:
SU xx<cr>
x is a number between 0 and 9.
Explain:
The power supply will set desired up-limit value of the voltage when
the command is received.
Example:
SU 20
Set voltage limit to 20V
*SI
Function:
Set current limit.
Syntax:
SI x.xx<cr>
x is a number between 0 and 9.
PSP SERIES
USER MANUAL
44
Explain:
The power supply will set desired up-limit value of the current when
the command is received.
Example:
SU 1.25
Set current limit to 1.25A
*SP
Function:
Set power limit.
Syntax:
SP xxx<cr>
x is a number between 0 and 9.
Explain:
The power supply will set desired up-limit value of the power when
the command is received.
Example:
SP 100
Set power limit to 100W
**The power setting changes the c
* 
* 
         * */
    }
}
