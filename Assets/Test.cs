using System;
using IngameDebugConsole;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        DebugLogConsole.AddCommand<int,bool,string>("测试菜单/测试","报错", (name,age,sex) =>
        {
            print(name);
            print(age);
            print(sex);
            throw new Exception("测试异常");
        });
        
    }
}