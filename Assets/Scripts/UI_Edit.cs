using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Chemix;

public class UI_Edit : MonoBehaviour
{
    public GameObject thisPanel;
    public InputField titleTextInput;
    public Transform BigContent;
    public Transform SmallContent;
    public GameObject stepPrefab;

    public UI_Step steps;

    public GameManager.ExperimentalSetup output;
    public GameObject titleText;
    public GameObject detailText;
    public GameObject objectList;


    // Use this for initialization
    void Start()
    {
        output = GM.GM_Core.instance.experimentalSetup;
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
        //Debug.Log(output.instrumentInfos.Count + " " + GM.GM_Core.instance.experimentalSetup.instrumentInfos.Count);
        //GM.GM_Core.instance.SwitchToScene("CustomLab");
		WWWForm form = new WWWForm();
		form.AddField("account", GM.GM_Core.instance.Account);
		form.AddField("password", GM.GM_Core.instance.Password);
		form.AddField("key", "scene");
		form.AddField("value", JsonUtility.ToJson(GM.GM_Core.instance.experimentalSetup));
		Chemix.Network.NetworkManager.Instance.Post(form, "scene/save", (success, reply) => 
		{
			string invite = reply.Detail;
			//ToDO: Hi, zr! Add some code here to make it go to another scene and show the invite key. Thx! ☆´∀｀☆

		}
		                                           );
    }

    public void Restore()
    {
        titleText.GetComponent<Renderer>().material.SetColor("_Color", output.title.color);
        titleText.transform.position = output.title.position;
        titleText.transform.localScale = output.title.size * titleText.GetComponent<Lab_Text>().srcScale;

        detailText.GetComponent<Renderer>().material.SetColor("_Color", output.title.color);
        detailText.transform.position = output.title.position;
        detailText.transform.localScale = output.title.size * titleText.GetComponent<Lab_Text>().srcScale;
        
        for (int i = 0; i < output.instrumentInfos.Count; i++)
        {
            GameObject obj = Instantiate(Resources.Load(output.instrumentInfos[i].type) as GameObject, objectList.transform);
            obj.name = output.instrumentInfos[i].type;
            obj.transform.rotation = output.instrumentInfos[i].quaternion;
            obj.transform.position = output.instrumentInfos[i].position;
            if (obj.GetComponent<Container>() != null)
            {
                Container c = obj.GetComponent<Container>();
                c.typeName = output.instrumentInfos[i].formula;
                c.quantity = output.instrumentInfos[i].mass;
            }
        }

        steps.title = output.taskFlow.title;
        titleTextInput.text = output.taskFlow.title;
        for (int i = 0; i < output.taskFlow.steps.Count; i++)
        {
            TaskFlow.Step tf = output.taskFlow.steps[i];
            GameObject newBigStep = Instantiate(stepPrefab, BigContent.transform);
            newBigStep.GetComponent<UI_StepContent>().stepTitle = tf.detail;
            steps.bigSteps.Add(newBigStep);

            List<GameObject> smallSteps = new List<GameObject>();
            for (int j = 0; j < tf.substeps.Count; j++)
            {
                TaskFlow.Substep ss = tf.substeps[i];
                GameObject newSmallStep = Instantiate(stepPrefab, SmallContent.transform);
                newSmallStep.GetComponent<UI_StepContent>().isBig = false;
                newSmallStep.GetComponent<UI_StepContent>().stepTitle = ss.detail;
                newSmallStep.GetComponent<UI_StepContent>().tName = (UI_StepContent.eventType)(int)ss.eventType;
                newSmallStep.GetComponent<UI_StepContent>().eName = steps.eventIdList[(int)ss.taskEvent];
                smallSteps.Add(newSmallStep);
            }
            steps.smallSteps.Add(smallSteps);
        }
    }
}
