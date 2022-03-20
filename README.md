AutomaticGamepad

**1. 程序用途**

* 支持编写脚本控制手柄，用于实现一些重复功能

----



**2. 前置条件**

1. 需要一台PC、Xbox，原理是通过本地局域网内用PC串流Xbox，然后通过该软件发送模拟手柄的信号到PC上的串流窗口
2. 理论上PS4、PS5的也可以用，代码里面是有接口可以创建PS的手柄控制器的。

---



**3. 使用说明**

1. 安装Drivers目录中对应版本的模拟手柄驱动程序 ViGEmBusSetup.msi
2. 运行Xbox应用，串流你的Xbox
3. 运行AutomaticGamepad.exe
4. 选择你要运行的脚本，点击绑定窗口，绑定成功后，设置运行次数，最后点击启动即可

<img src=".\Images\image-20220320234149677.png" alt="image-20220320234149677" style="zoom: 33%;" />

---



**4. 脚本说明**

1. 脚本语言是JavaScript
2. 脚本文件后缀为 .ag
3. 将脚本文件放置在该软件
4. 下列所有方法都是在 gamepad 这个对象下的，所以需要 gamepad.xxx

| 方法名                                 | 说明                                                         | 参数1          | 参数2                     | 参数3                     |
| :------------------------------------- | ------------------------------------------------------------ | -------------- | ------------------------- | ------------------------- |
| gamepad.Button(name, duration)         | 按下按钮，然后松开                                           | 按键名字       | 按下时长，默认值为200毫秒 |                           |
| gamepad.Trigger(name, value, duration) | 按下触发器，然后松开                                         | 触发器名字     | 范围为 [0, 1]             | 按下时长，默认值为200毫秒 |
| gamepad.Axis(name, value, duration)    | 移动摇杆，然后松开，摇杆值以左下角(-1,-1)，右上角为(1,1)，中间为(0,0)，默认摇杆不推动的时候值为(0,0) | 摇杆轴名字     | 范围为 [-1, 1]            | 按下时长，默认值为200毫秒 |
| gamepad.Sleep(duration)                | 将当前线程休眠                                               | 休眠时长(毫秒) |                           |                           |

| 按键名字                     | 对应的字符串 | 举个栗子                                                     |
| ---------------------------- | ------------ | ------------------------------------------------------------ |
| A键                          | "a"          | gamepad.Button("a")，按下A键                                 |
| B键                          | “b"          | gamepad.Button("b")，按下B键                                 |
| X键                          | “x"          | gamepad.Button("x")，按下X键                                 |
| Y键                          | "y"          | gamepad.Button("y")，按下Y键                                 |
| LB键                         | "lb"         | gamepad.Button("lb", 500)，按下LB键，500毫秒后松开           |
| RB键                         | "rb"         | gamepad.Button("rb", 300)，按下RB键，300毫秒后松开           |
| 左摇杆按下键                 | "lsb"        | gamepad.Button("lsb")，左摇杆按下                            |
| 右摇杆按下键                 | "rsb"        | gamepad.Button("rsb")，右摇杆按下                            |
| Dpad的上键                   | "up"         | gamepad.Button("up")，按下Dpad的上键                         |
| Dpad的下键                   | "down"       | gamepad.Button("down")，按下Dpad的下键                       |
| Dpad的左键                   | "left"       | gamepad.Button("left")，按下Dpad的左键                       |
| Dpad的右键                   | "right"      | gamepad.Button("right")，按下Dpad的右键                      |
| XBOX键（中间亮白灯键那个）   | "home"       | gamepad.Button("home")                                       |
| 视图键（XBOX键左下方的那个） | "view"       | gamepad.Button("view")                                       |
| 菜单键（XBOX键右下方的那个） | "menu"       | gamepad.Button("menu")                                       |
| LT触发器                     | "lt"         | gamepad.Trigger("lt", 0.5, 500)，按压LT触发器到一半的值，按压500毫秒后松开 |
| RT触发器                     | "rt"         | gamepad.Trigger("rt", 1, 300)，按压RT触发器到最大值，按压300毫秒后松开 |
| 左摇杆X轴                    | "lsx"        | gamepad.Axis("lsx", -1, 1000)，推动左摇杆到正左边的尽头，1000毫秒后松开 |
| 左摇杆Y轴                    | "lsy"        | gamepad.Axis("lsy", 0.8, 500)，推动左摇杆到正上方的80%左右位置，500毫秒后松开 |
| 右摇杆X轴                    | "rsx"        | gamepad.Axis("rsx", 1, 3000)，推动右摇杆到正右方的尽头，3000毫秒后松开 |
| 右摇杆Y轴                    | "rsy"        | gamepad.Axis("rsy", -1, 1000)，推动右摇杆到正下方的尽头，1000毫秒后松开 |

---



**6. 栗子说明**

```javascript
// 下面举个栗子来说明下用法
// 比如要在艾尔登法环的”通往王朝的崖上道路“这个地图用武器“神躯化剑”刷魂
// 步骤是要先去到这个地图“通往王朝的崖上道路”，并双持“神躯化剑”，然后就按照上面的使用说明走一遍流程即可

// 回城重置
gamepad.Button("view")
gamepad.Button("y")
gamepad.Button("a")
gamepad.Button("a")

// 等待加载地图时间
gamepad.Sleep(5500)

// 往前走4s
gamepad.Axis("lsy", 1, 4000)

// 往左走1.5s
gamepad.Axis("lsx", -1, 1500)

// 向右旋转镜头
gamepad.Axis("rsx", 0.5, 1000)

// 向前走1.65s
gamepad.Axis("lsy", 1, 1650)

// 放战技
gamepad.Trigger("lt", 1)

// 等待怪被清的差不多
gamepad.Sleep(5000)

```




---

**引用项目**

> [ViGEmBus](https://github.com/ViGEm/ViGEmBus)
> [ViGEm.NET](https://github.com/tylearymf/ViGEm.NET)



