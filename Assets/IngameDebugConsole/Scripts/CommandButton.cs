using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace IngameDebugConsole
{
    public class CommandButton : MonoBehaviour
    {
        [SerializeField]
        private Button Btn;

        [SerializeField]
        private InputField TemplateInput;

        [SerializeField]
        private Text NameTxt;

        [SerializeField] 
        private Text DescTxt;

        private Image btnImg;
        private string oldName;
        private List<InputField> inputs = new List<InputField>();

        private void Start()
        {
            btnImg = Btn.GetComponent<Image>();
        }

        public void Init(ConsoleMethodInfo consoleMethodInfo)
        {
            oldName = $">{consoleMethodInfo.command}({string.Join("",consoleMethodInfo.parameters)})";
            NameTxt.text = oldName;
            DescTxt.text = consoleMethodInfo.description;
            Btn.interactable = true;
            Btn.onClick.RemoveAllListeners();
            Btn.onClick.AddListener(() =>
            {
                try
                {
                    DebugLogConsole.ExecuteCommand(consoleMethodInfo.command, inputs.Select(r=> r.text));
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
            for (var i = 0; i < consoleMethodInfo.parameters.Length; i++)
            {
                var desc = consoleMethodInfo.parameters[i];
                var input = Instantiate(TemplateInput, TemplateInput.transform.parent);
                input.gameObject.SetActive(true);
                input.placeholder.GetComponent<Text>().text = desc;
                inputs.Add(input);
            }
        }

        IEnumerator Delay(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            action();
        }
    }
}