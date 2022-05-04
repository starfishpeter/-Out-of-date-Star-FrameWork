using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 输入控制模块
/// </summary>
public class InputManager : BaseManager<InputManager>
{
    private bool Open = false;

    /// <summary>
    /// 构造函数 添加 update监听
    /// </summary>
    public InputManager()
    {
        MonoManager.GetInstance().AddUpdateListener(InputUpdate);
    }

    //开启或关闭检测
    public void CheckInput(bool isOpen)
    {
        Open = isOpen;
    }

    /// <summary>
    /// 输入模块的update
    /// </summary>
    private void InputUpdate()
    {
        if (!Open)
        {
            return;
        }
        CheckKeyCode(KeyCode.Escape);
        CheckMouseButton(0);
        CheckMouseButton(1);
        CheckMouseButton(2);

        InputHorizontal();
        InputVertical();

        MouseX();
        MouseY();
        MouseScroll();
    }

    /// <summary>
    /// 检测按键抬起按下 分发事件
    /// </summary>
    /// <param name="key">按键</param>
    private void CheckKeyCode(KeyCode key)
    {
        //按下
        if (Input.GetKeyDown(key))
        {
            EventCenter.GetInstance().EventTrigger("KeyDown", key);
        }
        //抬起
        if (Input.GetKeyUp(key))
        {
            EventCenter.GetInstance().EventTrigger("KeyUp", key);
        }
    }

    /// <summary>
    /// 键盘水平轴输入
    /// </summary>
    private void InputHorizontal()
    {
        EventCenter.GetInstance().EventTrigger("Horizontal", Input.GetAxis("Horizontal"));
        
    }

    /// <summary>
    /// 键盘竖直轴输入
    /// </summary>
    private void InputVertical()
    {
        EventCenter.GetInstance().EventTrigger("Vertical", Input.GetAxis("Vertical"));
    }

    /// <summary>
    /// 检测鼠标 抬起 按下 长按 分发事件
    /// </summary>
    /// <param name="button">鼠标键位</param>
    private void CheckMouseButton(int button)
    {
        if (Input.GetMouseButtonDown(button))
        {
            EventCenter.GetInstance().EventTrigger("MouseDown", button);
        }

        if (Input.GetMouseButtonUp(button))
        {
            EventCenter.GetInstance().EventTrigger("MouseUp", button);
        }

        if(Input.GetMouseButton(button))
        {
            EventCenter.GetInstance().EventTrigger("MouseOn", button);
        }
    }

    /// <summary>
    /// 鼠标水平输入
    /// </summary>
    private void MouseX()
    {
        EventCenter.GetInstance().EventTrigger("Mouse X", Input.GetAxis("Mouse X"));
    }

    /// <summary>
    /// 鼠标垂直输入
    /// </summary>
    private void MouseY()
    {
        EventCenter.GetInstance().EventTrigger("Mouse Y", Input.GetAxis("Mouse Y"));
    }

    /// <summary>
    /// 鼠标滚轮移动
    /// </summary>
    private void MouseScroll()
    {
        EventCenter.GetInstance().EventTrigger("MouseScroll", Input.mouseScrollDelta);
    }

}
