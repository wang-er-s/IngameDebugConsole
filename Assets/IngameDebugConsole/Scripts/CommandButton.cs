using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IngameDebugConsole
{
    public class CommandButton : MonoBehaviour
    {
        [SerializeField]
        private Button Btn;

        [SerializeField]
        private InputField Input;

        [SerializeField]
        private Text NameTxt;

        [SerializeField] 
        private Text DescTxt;

        private Image btnImg;
        private string oldName;

        private void Start()
        {
            btnImg = Btn.GetComponent<Image>();
        }

        public void Init(ConsoleMethodInfo consoleMethodInfo)
        {
            Input.placeholder.GetComponent<Text>().text = "请输入参数，类型在上面，每个参数用空格分隔";
            Input.gameObject.SetActive(consoleMethodInfo.parameterTypes.Length > 0);
            oldName = $">{consoleMethodInfo.command}({string.Join("",consoleMethodInfo.parameters)})";
            NameTxt.text = oldName;
            DescTxt.text = consoleMethodInfo.description;
            Btn.interactable = true;
            Btn.onClick.RemoveAllListeners();
            Btn.onClick.AddListener(() =>
            {
                try
                {
                    DebugLogConsole.ExecuteCommand($"{consoleMethodInfo.command} {Input.text}");
                }
                catch (Exception e)
                {
                    DebugLogTips.ShowTips("执行命令出错，查看log", true);
                    Debug.LogError(e);
                }

                btnImg.color = Color.green;
                Btn.interactable = false;
                StartCoroutine(Delay(0.5f, () =>
                {
                    btnImg.color = Color.white;
                    Btn.interactable = true;
                }));
            });
        }

        IEnumerator Delay(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            action();
        }
    }
}