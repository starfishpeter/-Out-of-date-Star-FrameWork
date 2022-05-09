using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInputMove : MonoBehaviour
{
    void Start()
    {
        InputManager.GetInstance().CheckInput(true);
        EventCenter.GetInstance().AddEventListener<KeyCode>("KeyDown", CheckInputDown);
        EventCenter.GetInstance().AddEventListener<KeyCode>("KeyUp", CheckInputUp);
        EventCenter.GetInstance().AddEventListener<float>("Horizontal", HorizontalMove);
        EventCenter.GetInstance().AddEventListener<float>("Vertical", VerticalMove);
        EventCenter.GetInstance().AddEventListener<int>("MouseDown", CheckMouseDown);
    }

    private void CheckInputDown(KeyCode key)
    {
        Debug.Log("按下");
    }

    private void CheckInputUp(KeyCode key)
    {
        Debug.Log("抬起");
    }

    private void CheckMouseDown(int button)
    {
        switch (button)
        {
            case 0:
                Debug.Log("左键按下");
                break;
            case 1:
                Debug.Log("右键按下");
                break;
        }
    }

    private void HorizontalMove(float direction)
    {
        float speed = direction * 10;
        this.transform.Translate(speed * Time.deltaTime, 0, 0);
    }

    private void VerticalMove(float direction)
    {
        float speed = direction * 10;
        this.transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
