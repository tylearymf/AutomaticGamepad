# AutomaticGamepad

[中文文档](https://github.com/tylearymf/AutomaticGamepad/blob/main/README_CN.md)

# About

* Write automated scripts for console games

| Supported Devices:video_game:  |     Xbox One X     |   Xbox Series X    | PlayStation 4 |   PlayStation 5    |
| ------------------------------ | :----------------: | :----------------: | :-----------: | :----------------: |
| **Test Result**:arrow_forward: | :white_check_mark: | :white_check_mark: |  :question:   | :white_check_mark: |

## Prerequisites

1. You need a PC, an Xbox (or PS5, PS4)
2. The idea is to stream an Xbox (or PS5) on a PC over a local area network, and then use the software to send virtual gamepad signals to a streaming window on the PC  

# Usage

1. Install the virtual gamepad driver **ViGEmBusSetup.msi** in the Drivers directory   
2. Run Xbox.exe (or PS Remote Play.exe) to stream your Xbox (or PS5).  
3. Configure **config.ini** and set the **GamepadType** parameter
   1.  Xbox：GamepadType=0
   2.  PlayStation：GamepadType=1
4. Run AutomaticGamepad.exe
5. Select the script you want to run, confirm that the window binding is successful, set the number of times to run, and finally click Start

<img src=".\Images\xbox_en.png" alt="Xbox Virtual Controller" />

<img src=".\Images\playstation_en.png" alt="Playstation Virtual Controller" />

---



# Script functions

1. The scripting language is JavaScript
2. The script file suffix is .ag
3. Place the script file in the same directory as the current program, and click Refresh in the program to view it

## sleep

```c#
// Put the program to sleep for a while
void sleep(double milliseconds)
```

* milliseconds：sleep time

## setdelay

```c#
// The delay after a method is called. For example, call button(), dpad()
void setdelay(double delay)
```

* delay：delay time（milliseconds）

## button

```c#
// Press button
// For example, press LB button, or L1 button
void button(string name, double duration = 200)
```

* name：[Button Key Name](#button_key_name)
* duration：Duration of the press. The default value is 200 ms

## dpad

```c#
// Press the DPAD button
// For example, press the up button of the DPad
void dpad(string name, double duration = 200)
```

* name：[DPad Key Name](#dpad_key_name)
* duration：Duration of the press. The default value is 200 ms

## trigger

```c#
// Press the trigger
// For example, press LT or RT, L1 or R1
void trigger(string name, double val, double duration = 200)
```

* name：[Trigger Key Name](#trigger_key_name)
* val：Trigger press value, range is [0, 1], full press is 1
* duration：Duration of the press. The default value is 200 ms

## axis

```c#
// Push the single axis of the joystick
// For example, push the X-axis or Y-axis of the left joystick, or push the X-axis or Y-axis of the right joystick
void axis(string name, double val, double duration = 200)
```

* name：[Axis Key Name](#axis_key_name)
* val：The joystick push value, the range is [-1, 1], the lower left is -1, and the upper right is 1
* duration：Duration of the press. The default value is 200 ms

## axis2

```c#
// Push the two axes of the joystick
void axis2(string name1, string name2, double val1, double val2, double duration = 200)
```

* name1：[Axis Key Name](#axis_key_name)
* name2：[Axis Key Name](#axis_key_name)
* val1：The joystick push value, the range is [-1, 1], the lower left is -1, and the upper right is 1
* val2：The joystick push value, the range is [-1, 1], the lower left is -1, and the upper right is 1
* duration：Duration of the press. The default value is 200 ms

## buttonstate

```c#
// Press or release the button
void buttonstate(string name, bool state)
```

* name：[Button Key Name](#button_key_name)
* state:：press is true, release is false

## dpadstate

```c#
// Press or release the dpad button
void dpadstate(string name, bool state)
```

* name：[DPad Key Name](#dpad_key_name)
* state:：press is true, release is false

## triggerstate

```c#
// Press or release the trigger
void triggerstate(string name, double val)
```

* name：[Trigger Key Name](#trigger_key_name)
* val：Trigger press value, range is [0, 1], full press is 1

## axisstate

```c#
// Push the single axis of the joystick
void axisstate(string name, double val)
```

* name：[Axis Key Name](#axis_key_name)
* val：The joystick push value, the range is [-1, 1], the lower left is -1, and the upper right is 1

## axis2state

```c#
// Push the two axes of the joystick
void axis2state(string name1, string name2, double val1, double val2)
```

* name1：[Axis Key Name](#axis_key_name)
* name2：[Axis Key Name](#axis_key_name)
* val1：The joystick push value, the range is [-1, 1], the lower left is -1, and the upper right is 1
* val2：The joystick push value, the range is [-1, 1], the lower left is -1, and the upper right is 1

# Custom key name

* Xbox：[XboxGamepad.cs](https://github.com/tylearymf/AutomaticGamepad/blob/main/Xbox/XboxGamepad.cs)
* PlayStation：[PlaystationGamepad.cs](https://github.com/tylearymf/AutomaticGamepad/blob/main/PlayStation/PlaystationGamepad.cs)

# <b name='button_key_name'>Button Key Name</b>
<table>
    <tr>
        <th colspan="2" align="center">Xbox</th>
        <th colspan="2" align="center">PlayStation</th>
    </tr>
    <tr>
        <td align="center">A Button</td>
        <td align="center">"a"</td>
        <td align="center">△ Button</td>
        <td align="center">”b1“</td>
    </tr>
    <tr>
        <td align="center">B Button</td>
        <td align="center">"b"</td>
        <td align="center">○ Button</td>
        <td align="center">"b2"</td>
    </tr>
    <tr>
        <td align="center">X Button</td>
        <td align="center">"x"</td>
        <td align="center">X Button</td>
        <td align="center">"b3"</td>
    </tr>
    <tr>
        <td align="center">Y Button</td>
        <td align="center">"y"</td>
        <td align="center">□ Button</td>
        <td align="center">"b4"</td>
    </tr>
    <tr>
        <td align="center">Left Bumper</td>
        <td align="center">"lb"</td>
        <td align="center">L1 Button</td>
        <td align="center">"l1"</td>
    </tr>
    <tr>
        <td align="center">Right Bumper</td>
        <td align="center">"rb"</td>
        <td align="center">R1 Button</td>
        <td align="center">"r1"</td>
    </tr>
    <tr>
        <td align="center">Left Stick Button</td>
        <td align="center">"lsb"</td>
        <td align="center">Left Stick Button</td>
        <td align="center">"l3"</td>
    </tr>
    <tr>
        <td align="center">Right Stick Button</td>
        <td align="center">"rsb"</td>
        <td align="center">Right Stick Button</td>
        <td align="center">"r3"</td>
    </tr>
    <tr>
        <td align="center">Menu Button</td>
        <td align="center">"menu"</td>
        <td align="center">Share Button</td>
        <td align="center">"share"</td>
    </tr>
    <tr>
        <td align="center">View Button</td>
        <td align="center">"view"</td>
        <td align="center">Option Button</td>
        <td align="center">"option"</td>
    </tr>
    <tr>
        <td align="center">XBOX Button</td>
        <td align="center">"home"</td>
        <td align="center">PS Button</td>
        <td align="center">"home"</td>
    </tr>
    <tr>
        <td align="center"></td>
        <td align="center"></td>
        <td align="center">Touchpad Button</td>
        <td align="center">"touchpad"</td>
    </tr>
</table>
# <b name="dpad_key_name">DPad Key Name</b>
<table>
    <tr>
        <th colspan="2" align="center">Xbox & PlayStation</th>
    </tr>
    <tr>
        <td align="center">Up key</td>
        <td align="center">"up"</td>
    </tr>
    <tr>
        <td align="center">Down key</td>
        <td align="center">"down"</td>
    </tr>
    <tr>
        <td align="center">Left Key</td>
        <td align="center">"left"</td>
    </tr>
    <tr>
        <td align="center">Right Key</td>
        <td align="center">"right"</td>
    </tr>
</table>
# <b name="trigger_key_name">Trigger Key Name</b>
<table>
    <tr>
        <th colspan="2" align="center">Xbox</th>
        <th colspan="2" align="center">PlayStation</th>
    </tr>
    <tr>
        <td align="center">Left Trigger</td>
        <td align="center">"lt"</td>
        <td align="center">L1 Key</td>
        <td align="center">"l1"</td>
    </tr>
    <tr>
        <td align="center">Right Trigger</td>
        <td align="center">"rt"</td>
        <td align="center">R1 Key</td>
        <td align="center">"r1"</td>
    </tr>
</table>
# <b name="axis_key_name">Axis Key Name</b>
<table>
    <tr>
        <th colspan="2" align="center">Xbox & PlayStation</th>
    </tr>
    <tr>
        <td align="center">X-axis of left joystick</td>
        <td align="center">"lsx"</td>
    </tr>
    <tr>
        <td align="center">Y-axis of left joystick</td>
        <td align="center">"lsy"</td>
    </tr>
    <tr>
        <td align="center">X-axis of right joystick</td>
        <td align="center">"rsx"</td>
    </tr>
    <tr>
        <td align="center">Y-axis of left joystick</td>
        <td align="center">"rsy"</td>
    </tr>
</table>

# Example

```javascript
// For example (XBOX)
// 1. Go to the ’Palace Approach Ledge Road‘ Site of Grace
// 2. Please two-hand your equipped 'Sacred Sword Relic' weapon
// 3. Finally, please follow the ‘Usage’ steps

// Go to the Site of Grace
button("view")
button("y")
button("a")
button("a")

// Wait for the map to finish loading
sleep(5500)

// Move to the front left
axis2("lsx", "lsy", -0.31, 1, 5200)

// Cast weapons skill
trigger("lt", 1)

// wait for the monsters to die
sleep(5000)

```

<img src=".\Images\example.gif" alt="example.gif" />

# Reference 

> [ViGEmBus](https://github.com/ViGEm/ViGEmBus)
> [ViGEm.NET](https://github.com/tylearymf/ViGEm.NET)



