using System;
using IngameDebugConsole;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        DebugLogConsole.AddCommand("测试菜单/测试","报错", () => throw new Exception("测试异常"));
        
    }
}