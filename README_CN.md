# AutomaticGamepad（自动化虚拟手柄）



# 程序用途

* 为主机游戏编写自动化脚本

| 支持设备:video_game:    |     Xbox One X     |   Xbox Series X    | PlayStation 4 |   PlayStation 5    |
| ----------------------- | :----------------: | :----------------: | :-----------: | :----------------: |
| 测试情况:arrow_forward: | :white_check_mark: | :white_check_mark: |  :question:   | :white_check_mark: |

# 前置条件

1. 需要一台PC，一台Xbox（或者PS5、PS4）
2. 原理是通过本地局域网内用PC串流Xbox（或者PS5），然后通过该软件发送模拟手柄的信号到PC上的串流窗口

# 使用说明

1. 安装Drivers目录中对应版本的模拟手柄驱动程序 ViGEmBusSetup.msi
2. 运行Xbox.exe（或者PS Remote Play.exe），串流你的Xbox（或者PS5）
3. 配置 config.ini，设置相应的GamepadType
   1.  Xbox的设置为0，PlayStation的设置为1
4. 运行AutomaticGamepad.exe
5. 选择你要运行的脚本，点击绑定窗口，绑定成功后，设置运行次数，最后点击启动即可

<img src=".\Images\xbox_cn.png" alt="Xbox的虚拟控制器" />

<img src=".\Images\playstation_cn.png" alt="Playstation的虚拟控制器" />

---



# 脚本函数

1. 脚本语言是JavaScript
2. 脚本文件后缀为 .ag
3. 将脚本文件放置在与该软件同级的目录下，程序中点击刷新即可看到

## sleep

```c#
// 让程序休眠一段时间
void sleep(double milliseconds)
```

* milliseconds: 休眠时长（毫秒）

## setdelay

```c#
// 设置调用方法后的延迟时间，调用方法有：button()、dpad()等等
void setdelay(double delay)
```

* delay：延迟时间（毫秒）

## button

```c#
// 按下按钮
// 例如按下LB按钮，或者按下L1按钮
void button(string name, double duration = 200)
```

* name：[按钮键位名](#button_key_name)
* duration：按下的持续时长，默认值200毫秒（毫秒）

## dpad

```c#
// 按下 dpad 按钮
// 例如按下DPad的上按键
void dpad(string name, double duration = 200)
```

* name：[DPad键位名](#dpad_key_name)
* duration：按下的持续时长，默认值200毫秒（毫秒）

## trigger

```c#
// 按下 触发器
// 例如按下LT或者RT，按下L1或者R1
void trigger(string name, double val, double duration = 200)
```

* name：[触发器键位名](#trigger_key_name)
* val：触发器按压值，范围为[0, 1]，完全按压下去为1
* duration：按下的持续时长，默认值200毫秒（毫秒）

## axis

```c#
// 推动摇杆的一个轴
// 例如推动左摇杆的X轴或者Y轴，推动右摇杆的X轴或Y轴
void axis(string name, double val, double duration = 200)
```

* name：[摇杆键位名](#axis_key_name)
* val：摇杆推动值，范围为[-1, 1]，左下为-1，右上为1
* duration：按下的持续时长，默认值200毫秒（毫秒）

## axis2

```c#
// 推动摇杆的两个轴
void axis2(string name1, string name2, double val1, double val2, double duration = 200)
```

* name1：[摇杆键位名](#axis_key_name)
* name2：[摇杆键位名](#axis_key_name)
* val1：摇杆推动值，范围为[-1, 1]，左下为-1，右上为1
* val2：摇杆推动值，范围为[-1, 1]，左下为-1，右上为1
* duration：按下的持续时长，默认值200毫秒（毫秒）

## buttonstate

```c#
// 按下按钮
void buttonstate(string name, bool state)
```

* name：[按钮键位名](#button_key_name)
* state：按下是 true，松开是 false

## dpadstate

```c#
// 按下 dpad 按钮
void dpadstate(string name, bool state)
```

* name：[DPad键位名](#dpad_key_name)
* state：按下是 true，松开是 false

## triggerstate

```c#
// 按下 触发器
void triggerstate(string name, double val)
```

* name：[触发器键位名](#trigger_key_name)
* val：触发器按压值，范围为[0, 1]，完全按压下去为1

## axisstate

```c#
// 推动摇杆的一个轴
void axisstate(string name, double val)
```

* name：[摇杆键位名](#axis_key_name)
* val：摇杆推动值，范围为[-1, 1]，左下为-1，右上为1

## axis2state

```c#
// 推动摇杆的两个轴
void axis2state(string name1, string name2, double val1, double val2)
```

* name1：[摇杆键位名](#axis_key_name)
* name2：[摇杆键位名](#axis_key_name)
* val1：摇杆推动值，范围为[-1, 1]，左下为-1，右上为1
* val2：摇杆推动值，范围为[-1, 1]，左下为-1，右上为1

# 键位名自定义

* Xbox：[XboxGamepad.cs](https://github.com/tylearymf/AutomaticGamepad/blob/main/Xbox/XboxGamepad.cs)
* PlayStation：[PlaystationGamepad.cs](https://github.com/tylearymf/AutomaticGamepad/blob/main/PlayStation/PlaystationGamepad.cs)

# <b name='button_key_name'>按钮键位名</b>
<table>
    <tr>
        <th colspan="2" align="center">Xbox</th>
        <th colspan="2" align="center">PlayStation</th>
    </tr>
    <tr>
        <td align="center">A1键</td>
        <td align="center">"a"</td>
        <td align="center">△键</td>
        <td align="center">b1</td>
    </tr>
    <tr>
        <td align="center">B键</td>
        <td align="center">"b"</td>
        <td align="center">○键</td>
        <td align="center">"b2"</td>
    </tr>
    <tr>
        <td align="center">X键</td>
        <td align="center">"x"</td>
        <td align="center">X键</td>
        <td align="center">"b3"</td>
    </tr>
    <tr>
        <td align="center">Y键</td>
        <td align="center">"y"</td>
        <td align="center">□键</td>
        <td align="center">"b4"</td>
    </tr>
    <tr>
        <td align="center">LB键</td>
        <td align="center">"lb"</td>
        <td align="center">L1键</td>
        <td align="center">"l1"</td>
    </tr>
    <tr>
        <td align="center">RB键</td>
        <td align="center">"rb"</td>
        <td align="center">R1键</td>
        <td align="center">"r1"</td>
    </tr>
    <tr>
        <td align="center">左摇杆按下键</td>
        <td align="center">"lsb"</td>
        <td align="center">左摇杆按下键键</td>
        <td align="center">"l3"</td>
    </tr>
    <tr>
        <td align="center">右摇杆按下键</td>
        <td align="center">"rsb"</td>
        <td align="center">右摇杆按下键</td>
        <td align="center">"r3"</td>
    </tr>
    <tr>
        <td align="center">菜单键</td>
        <td align="center">"menu"</td>
        <td align="center">分享键</td>
        <td align="center">"share"</td>
    </tr>
    <tr>
        <td align="center">视图键</td>
        <td align="center">"view"</td>
        <td align="center">选项键</td>
        <td align="center">"option"</td>
    </tr>
    <tr>
        <td align="center">XBOX键</td>
        <td align="center">"home"</td>
        <td align="center">PS键</td>
        <td align="center">"home"</td>
    </tr>
    <tr>
        <td align="center"></td>
        <td align="center"></td>
        <td align="center">触摸板键</td>
        <td align="center">"touchpad"</td>
    </tr>
</table>

# <b name="dpad_key_name">DPad键位名</b>
<table>
    <tr>
        <th colspan="2" align="center">Xbox & PlayStation</th>
    </tr>
    <tr>
        <td align="center">方向上键</td>
        <td align="center">"up"</td>
    </tr>
    <tr>
        <td align="center">方向下键</td>
        <td align="center">"down"</td>
    </tr>
    <tr>
        <td align="center">方向左键</td>
        <td align="center">"left"</td>
    </tr>
    <tr>
        <td align="center">方向右键</td>
        <td align="center">"right"</td>
    </tr>
</table>

# <b name="trigger_key_name">触发器键位名</b>
<table>
    <tr>
        <th colspan="2" align="center">Xbox</th>
        <th colspan="2" align="center">PlayStation</th>
    </tr>
    <tr>
        <td align="center">LT键</td>
        <td align="center">"lt"</td>
        <td align="center">L1键</td>
        <td align="center">"l1"</td>
    </tr>
    <tr>
        <td align="center">RT键</td>
        <td align="center">"rt"</td>
        <td align="center">R1键</td>
        <td align="center">"r1"</td>
    </tr>
</table>

# <b name="axis_key_name">摇杆键位名</b>
<table>
    <tr>
        <th colspan="2" align="center">Xbox & PlayStation</th>
    </tr>
    <tr>
        <td align="center">左摇杆X轴</td>
        <td align="center">"lsx"</td>
    </tr>
    <tr>
        <td align="center">左摇杆Y轴</td>
        <td align="center">"lsy"</td>
    </tr>
    <tr>
        <td align="center">右摇杆X轴</td>
        <td align="center">"rsx"</td>
    </tr>
    <tr>
        <td align="center">右摇杆Y轴</td>
        <td align="center">"rsy"</td>
    </tr>
</table>

# 例子说明

```javascript
// 比如要在艾尔登法环的”通往王朝的崖上道路“这个地图用武器“神躯化剑”刷魂
// 步骤是要先去到这个地图“通往王朝的崖上道路”，并双持“神躯化剑”，然后就按照上面的使用说明走一遍流程即可

// 回城重置
button("view")
button("y")
button("a")
button("a")

// 等待加载地图时间
sleep(5500)

// 往左前走
axis2("lsx", "lsy", -0.31, 1, 5200)

// 放战技
trigger("lt", 1)

// 等待怪被清的差不多
sleep(5000)

```

<img src=".\Images\example.gif" alt="example.gif" />

# 引用项目

> [ViGEmBus](https://github.com/ViGEm/ViGEmBus)
> [ViGEm.NET](https://github.com/tylearymf/ViGEm.NET)



