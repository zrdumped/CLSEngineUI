using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chemix;

public class UI_Edit : MonoBehaviour
{
    public GameObject thisPanel;

    public UI_Step steps;

    public GameManager.ExperimentalSetup output;
    public GameObject titleText;
    public GameObject detailText;
    public GameObject objectList;


    // Use this for initialization
    void Start()
    {
        output = new GameManager.ExperimentalSetup();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Open()
    {
        thisPanel.SetActive(true);
        Camera.main.gameObject.GetComponent<Lab.Lab_Controller>().enabled = false;
    }

    public void Complete()
    {
        output.title = new GameManager.TextInfo();
        output.title.color = titleText.GetComponent<Renderer>().material.GetColor("_Color");
        output.title.position = titleText.transform.position;
        output.title.size = titleText.transform.localScale.x / titleText.GetComponent<Lab_Text>().srcScale.x;

        output.detail = new GameManager.TextInfo();
        output.detail.color = detailText.GetComponent<Renderer>().material.GetColor("_Color");
        output.detail.position = detailText.transform.position;
        output.detail.size = detailText.transform.localScale.x / detailText.GetComponent<Lab_Text>().srcScale.x;

        output.instrumentInfos = new List<GameManager.InstrumentInfo>();
        for (int i = 0; i < objectList.transform.childCount; i++)
        {
            GameObject obj = objectList.transform.GetChild(i).gameObject;
            GameManager.InstrumentInfo info = new GameManager.InstrumentInfo();
            info.type = obj.name;
            info.quaternion = obj.transform.rotation;
            info.position = obj.transform.position;
            if(obj.GetComponent<Container>() != null)
            {
                Container c = obj.GetComponent<Container>();
                info.formula = c.typeName;
                info.mass = c.quantity;
            }
            output.instrumentInfos.Add(info);
        }

        output.taskFlow = new TaskFlow();
        output.taskFlow.title = steps.title;
        output.taskFlow.completeMessage = "恭喜你完成了本次实验";
        output.taskFlow.steps = new List<TaskFlow.Step>();
        for (int i = 0; i < steps.bigSteps.Count; i++)
        {
            TaskFlow.Step tf = new TaskFlow.Step();
            tf.detail = steps.bigSteps[i].GetComponent<UI_StepContent>().stepTitle;
            tf.substeps = new List<TaskFlow.Substep>();
            for (int j = 0; j < steps.smallSteps[i].Count; j++)
            {
                TaskFlow.Substep ss = new TaskFlow.Substep();
                ss.detail = steps.smallSteps[i][j].GetComponent<UI_StepContent>().stepTitle;
                ss.eventType = (TaskFlow.EventType)(int)steps.smallSteps[i][j].GetComponent<UI_StepContent>().tName;
                ss.taskEvent = (TaskFlow.TaskEvent)steps.eventID[steps.smallSteps[i][j].GetComponent<UI_StepContent>().eName];
                tf.substeps.Add(ss);
            }
            output.taskFlow.steps.Add(tf);
        }
        GM.GM_Core.instance.experimentalSetup = output;
        Debug.Log(output.instrumentInfos.Count + " " + GM.GM_Core.instance.experimentalSetup.instrumentInfos.Count);
        //GM.GM_Core.instance.SwitchToScene("CustomLab");
    }
}
