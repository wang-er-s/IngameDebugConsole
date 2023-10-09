using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

namespace IngameDebugConsole
{
    public class DebugLogCommand : MonoBehaviour
    {
        [SerializeField]
        private CommandButton commandBtnTemplate;

        [SerializeField]
        private Transform commandBtnContent;
        
        [SerializeField]
        private Transform menuContent;

        [SerializeField]
        private Toggle menuTogTemplate;

        private Dictionary<ConsoleMethodInfo, CommandButton> methodInfo2Go =
            new Dictionary<ConsoleMethodInfo, CommandButton>();
        
        private Dictionary<string, Toggle> menu2Tog = new();

        private Dictionary<string, List<ConsoleMethodInfo>> menu2Info = new();
        private Queue<CommandButton> cacheCommandBtns = new();
        private string currentMenu;

        public void RefreshMethodButtons(NotifyCollectionChangedAction action, ConsoleMethodInfo method)
        {
            switch (action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (menu2Info.TryGetValue(method.menu, out var list))
                    {
                        if (list.Exists((info => info.command == method.command)))
                        {
                            Debug.LogWarning($"重复添加method-{method.method.Name}");
                            return;
                        }
                    }
                    else
                    {
                        list = new List<ConsoleMethodInfo>();
                        menu2Info.Add(method.menu, list);
                        var menuBtn = Instantiate(menuTogTemplate, menuContent, false);
                        menuBtn.GetComponentInChildren<Text>().text = method.menu;
                        menuBtn.gameObject.SetActive(true);
                        menu2Tog.Add(method.menu, menuBtn);
                        string menu = method.menu;
                        menuBtn.onValueChanged.AddListener(b=>
                        {
                            if (b)
                                OnMenuSelected(menu);
                        });

                        if (menu2Tog.Count == 1)
                        {
                            menuBtn.isOn = true;
                        }
                    }
                    list.Add(method);

                    if (currentMenu == method.menu)
                    {
                        AddCommandBtn(method);
                    }
                    
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (methodInfo2Go.ContainsKey(method))
                    {
                        FreeCommandBtn(method);
                        methodInfo2Go.Remove(method);
                    }

                    if (menu2Info.TryGetValue(method.menu, out var list1))
                    {
                        list1.Remove(method);
                    }

                    break;
            }
        }

        private void OnMenuSelected(string menu)
        {
            currentMenu = menu;
            foreach (ConsoleMethodInfo methodInfo in methodInfo2Go.Keys)
            {
                FreeCommandBtn(methodInfo);
            }
            
            methodInfo2Go.Clear();

            foreach (ConsoleMethodInfo methodInfo in menu2Info[menu])
            {
                AddCommandBtn(methodInfo);
            }
        }

        private void AddCommandBtn(ConsoleMethodInfo info)
        {
            CommandButton btn = null;
            if (cacheCommandBtns.Count > 0)
            {
                btn = cacheCommandBtns.Dequeue();
            }
            else
            {
                btn = Instantiate(commandBtnTemplate);
            }
            btn.Init(info);
            btn.transform.SetParent(commandBtnContent);
            btn.gameObject.SetActive(true);
            methodInfo2Go.Add(info, btn);
        }

        private void FreeCommandBtn(ConsoleMethodInfo info)
        {
            if (methodInfo2Go.TryGetValue(info, out var btn))
            {
                btn.gameObject.SetActive(false);
                cacheCommandBtns.Enqueue(btn);
            }
        }
    }
}