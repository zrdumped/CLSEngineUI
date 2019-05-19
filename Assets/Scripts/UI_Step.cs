using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Step : MonoBehaviour {

    public GameObject scrollContentBig;
    public GameObject scrollContentSmall;
    public GameObject stepPrefab;

    public List<GameObject> bigSteps;

    private List<List<GameObject>> smallSteps;

    public int curBigStepID = 0;
    public int curSmallStepID = 0;

    public GameObject bigTitleInputText;
    public GameObject bigTitleHintText;
    public GameObject smallTitleInputText;
    public GameObject smallTitleHintText;
    public GameObject addSmallStepButton;
    public GameObject eventNameDropdown;
    public GameObject eventTypeDropdown;

    public Lab.Lab_Controller labController;
    public GameObject thisPanel;

    List<Dropdown.OptionData> normalOptions;
    List<Dropdown.OptionData> conditionalOptions;

    public static Dictionary<UI_StepContent.eventName, bool> eventDic = new Dictionary<UI_StepContent.eventName, bool> {
        { UI_StepContent.eventName.NONE, true },
        { UI_StepContent.eventName.TEST1, true },
        { UI_StepContent.eventName.TEST2, false }
    };

    // Use this for initialization
    void Start () {
        bigSteps = new List<GameObject>();
        smallSteps = new List<List<GameObject>>();

        Dropdown.OptionData option;

        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        normalOptions = new List<Dropdown.OptionData>();
        conditionalOptions = new List<Dropdown.OptionData>();
        foreach (UI_StepContent.eventName t in System.Enum.GetValues(typeof(Container.substanceType)))
        {
            //Debug.Log(t.ToString());
            option = new Dropdown.OptionData();
            option.text = t.ToString();
            options.Add(option);
        }
        eventNameDropdown.GetComponent<Dropdown>().options = options;

        option = new Dropdown.OptionData();
        option.text = "普通";
        normalOptions.Add(option);

        option = new Dropdown.OptionData();
        option.text = "开";
        conditionalOptions.Add(option);
        option = new Dropdown.OptionData();
        option.text = "关";
        conditionalOptions.Add(option);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddBigStep()
    {
        GameObject newStep = Instantiate(stepPrefab, scrollContentBig.transform);
        bigSteps.Add(newStep);
        smallSteps.Add(new List<GameObject>());
        //newStep.AddComponent<UI_BigStep>();
        //newStep.GetComponent<UI_StepContent>().enabled = true;
        newStep.GetComponent<UI_StepContent>().stepID = bigSteps.Count;
        //newStep.GetComponent<UI_BigStep>().steps = new List<GameObject>();
        newStep.GetComponent<UI_StepContent>().controller = gameObject;
        newStep.GetComponentInChildren<Text>().text = "流程" + bigSteps.Count;
        //newStep.transform.SetParent(scrollContentBig.transform);
    }

    public void AddSmallStep()
    {
        GameObject newStep = Instantiate(stepPrefab, scrollContentSmall.transform);
        smallSteps[curBigStepID - 1].Add(newStep);
        //newStep.AddComponent<UI_BigStep>();
        //newStep.GetComponent<UI_StepContent>().enabled = true;
        //newStep.GetComponent<UI_SmallStep>().fatherID = curStepID;
        newStep.GetComponent<UI_StepContent>().stepID = smallSteps[curBigStepID - 1].Count;
        newStep.GetComponent<UI_StepContent>().isBig = false;
        newStep.GetComponent<UI_StepContent>().controller = gameObject;
        newStep.GetComponentInChildren<Text>().text = "步骤" + smallSteps[curBigStepID - 1].Count;
        //newStep.transform.SetParent(scrollContentSmall.transform);
        //Debug.Log(curBigStepID + " " + curSmallStepID);
    }

    public void updateFromBigStep(int stepID)
    {
        Debug.Log(curBigStepID + " " + curSmallStepID);

        if (curBigStepID != 0)
        {
            foreach (GameObject smallStep in smallSteps[curBigStepID - 1])
            {
                smallStep.SetActive(false);
            }
        }

        //Debug.Log(bigSteps[stepID - 1].GetComponent<UI_BigStep>().stepTitle);
        bigTitleInputText.GetComponent<InputField>().text = bigSteps[stepID - 1].GetComponent<UI_StepContent>().stepTitle;
        curBigStepID = stepID;
        bigTitleHintText.GetComponent<Text>().text = "流程" + curBigStepID + "标题";
        bigTitleInputText.GetComponent<InputField>().interactable = true;
        //addSmallStepButton.GetComponent<Button>().enabled = true;
        addSmallStepButton.SetActive(true);
        smallTitleInputText.GetComponent<InputField>().interactable = false;

        foreach (GameObject smallStep in smallSteps[curBigStepID - 1])
        {
            smallStep.SetActive(true);
        }

        eventNameDropdown.GetComponent<Dropdown>().interactable = false;
        eventTypeDropdown.GetComponent<Dropdown>().interactable = false;
    }

    public void updateFromSmallStep(int stepID)
    {
        //Debug.Log(stepID + " " + bigSteps.Count);
        //Debug.Log(bigSteps[stepID - 1].GetComponent<UI_BigStep>().stepTitle);
        smallTitleInputText.GetComponent<InputField>().text = smallSteps[curBigStepID - 1][stepID - 1].GetComponent<UI_StepContent>().stepTitle;
        curSmallStepID = stepID;
        smallTitleHintText.GetComponent<Text>().text = "步骤" + curSmallStepID + "标题";
        smallTitleInputText.GetComponent<InputField>().interactable = true;

        eventNameDropdown.GetComponent<Dropdown>().interactable = true;
        eventTypeDropdown.GetComponent<Dropdown>().interactable = true;

        eventNameDropdown.GetComponent<Dropdown>().value = (int)smallSteps[curBigStepID - 1][stepID - 1].GetComponent<UI_StepContent>().eName;
        int curDropdownType = (int)smallSteps[curBigStepID - 1][stepID - 1].GetComponent<UI_StepContent>().tName;

        if (!eventDic[smallSteps[curBigStepID - 1][stepID - 1].GetComponent<UI_StepContent>().eName])
        {
            eventTypeDropdown.GetComponent<Dropdown>().options = conditionalOptions;
            eventTypeDropdown.GetComponent<Dropdown>().value = (int)curDropdownType - 1;
        }
        else
            eventTypeDropdown.GetComponent<Dropdown>().options = normalOptions;
    }

    public void updateToBigStep()
    {
        bigSteps[curBigStepID - 1].GetComponent<UI_StepContent>().stepTitle = bigTitleInputText.GetComponent<InputField>().text;
        //smallTitleInputText.GetComponent<InputField>().interactable = false;
    }

    public void updateToSmallStep()
    {
        smallSteps[curBigStepID - 1][curSmallStepID - 1].GetComponent<UI_StepContent>().stepTitle = smallTitleInputText.GetComponent<InputField>().text;
        //smallTitleInputText.GetComponent<InputField>().interactable = false;
    }

    public void updateDropdownName()
    {
        UI_StepContent.eventName tmpName = (UI_StepContent.eventName)eventNameDropdown.GetComponent<Dropdown>().value;
        smallSteps[curBigStepID - 1][curSmallStepID - 1].GetComponent<UI_StepContent>().eName = tmpName;

        if (!eventDic[tmpName])
        {
            eventTypeDropdown.GetComponent<Dropdown>().options = conditionalOptions;
            eventTypeDropdown.GetComponent<Dropdown>().value = (int)smallSteps[curBigStepID - 1][curSmallStepID - 1].GetComponent<UI_StepContent>().tName - 1;
        }
        else
            eventTypeDropdown.GetComponent<Dropdown>().options = normalOptions;

        eventTypeDropdown.GetComponent<Dropdown>().interactable = true;
    }

    public void updateDropdownType()
    {
        if(!eventDic[smallSteps[curBigStepID - 1][curSmallStepID - 1].GetComponent<UI_StepContent>().eName])
            smallSteps[curBigStepID - 1][curSmallStepID - 1].GetComponent<UI_StepContent>().tName = (UI_StepContent.eventType)eventTypeDropdown.GetComponent<Dropdown>().value + 1;
        else
            smallSteps[curBigStepID - 1][curSmallStepID - 1].GetComponent<UI_StepContent>().tName = (UI_StepContent.eventType)eventTypeDropdown.GetComponent<Dropdown>().value;
    }

    public void CloseScreen()
    {
        labController.enabled = true;
        thisPanel.SetActive(false);
    }

}
