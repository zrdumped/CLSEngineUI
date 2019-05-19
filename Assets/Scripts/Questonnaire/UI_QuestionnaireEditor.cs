using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Questionnaire;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Chemix.Network;

namespace UI
{
	public class UI_QuestionnaireEditor : MonoBehaviour
	{
		public Questionnaire.Questionnaire questionnaire;
		public List<UI_AddQuestionButton> AddQuestionBtns;
		public UI_QuestionNumberList NumList;
		public Text CurrentNumber;
		public string NumberFormat = "第{0:D}题";
		public Text CurrentQuestionType;
		public string QuestionTypeFormat = "题型：{0}";
		public InputField QuestionContent;
		public Button Delete;
		public GameObject QuestionNumberButton;
		public GameObject CustomEditArea;

		int currentIdx = 0;

		// Use this for initialization
		void Start()
		{
			this.gameObject.SetActive(false);
			Init();
		}

		public void Init()
		{
			if (GM.GM_Core.instance.experimentalSetup == null || GM.GM_Core.instance.experimentalSetup.questionnaire == null) 
			{
				questionnaire = new Questionnaire.Questionnaire();
			}
			else 
				questionnaire = GM.GM_Core.instance.experimentalSetup.questionnaire;
			AddQuestionBtns = new List<UI_AddQuestionButton>(gameObject.GetComponentsInChildren<UI_AddQuestionButton>());
			foreach (UI_AddQuestionButton btn in AddQuestionBtns)
			{
				btn.GetComponent<Button>().onClick.AddListener(() => AddQuestion(btn.TypeName));
			}
			Delete.GetComponent<Button>().onClick.AddListener(DeleteQuestion);
			if (questionnaire.Count() == 0)
			{
				AddQuestion(QuestionType.计分题);
			}
			else
			{ 
				for (int i = 0; i < questionnaire.Count(); i++)
				{
					AddQuestionUI(questionnaire[i], i);
				}
				SelectNewQuestion(NumList[0]);
			}
		}

		// Update is called once per frame
		void Update()
		{

		}

		void AddQuestionUI(Question q, int index)
		{
			GameObject newNum = Instantiate(QuestionNumberButton);
			newNum.GetComponent<Button>().onClick.AddListener(() => SelectQuestion(newNum));
			newNum.GetComponent<UI_QuestionNumberListItem>().Index = index;
			NumList.Add(newNum);
		}

		void AddQuestion(QuestionType TypeName)
		{
			Question q = Question.getQuestionByTypeName(TypeName);
			questionnaire.Add(q);
			GameObject newNum = Instantiate(QuestionNumberButton);
			newNum.GetComponent<Button>().onClick.AddListener(() => SelectQuestion(newNum));
			newNum.GetComponent<UI_QuestionNumberListItem>().Index = questionnaire.Count() - 1;
			NumList.Add(newNum);
			SelectQuestion(newNum);
		}

		void DeleteQuestion()
		{
			if (questionnaire.Count() <= 1)
				return;
			NumList.RemoveLast();
			questionnaire.Remove(currentIdx);
			if (currentIdx == questionnaire.Count()) 
			{
				currentIdx--;
			}
			SelectNewQuestion(NumList[currentIdx]);
		}

		void SetCurrentNumber(int index)
		{
			CurrentNumber.text = string.Format(NumberFormat, index + 1);
		}

		void SetCurrentQuestionType(QuestionType t)
		{
			CurrentQuestionType.text = string.Format(QuestionTypeFormat, t);
		}

		void SaveOldResult(int index)
		{ 
			questionnaire[currentIdx].QuestionContent = QuestionContent.text;
			//ToDo: only support value question︿(￣︶￣)︿, fix it if you are free
			((ValueQuestion)questionnaire[currentIdx]).Range =
				new Vector2(float.Parse(CustomEditArea.GetComponent<UI_ValueQuestionEditArea>().Min.text),
							float.Parse(CustomEditArea.GetComponent<UI_ValueQuestionEditArea>().Max.text));
			                                                               
		}

		void SelectNewQuestion(GameObject item)
		{ 
			NumList[currentIdx].GetComponent<UI_QuestionNumberListItem>().OnNotSelected();
			currentIdx = item.GetComponent<UI_QuestionNumberListItem>().Index;
			item.GetComponent<UI_QuestionNumberListItem>().OnSelected();
			Question q = questionnaire[currentIdx];
			SetCurrentNumber(currentIdx);
			SetCurrentQuestionType(q.TypeName);
			QuestionContent.text = q.QuestionContent;
			//ToDo: only support value question︿(￣︶￣)︿, fix it if you are free
			CustomEditArea.GetComponent<UI_ValueQuestionEditArea>().Min.text = ((ValueQuestion)q).Range.x.ToString();
			CustomEditArea.GetComponent<UI_ValueQuestionEditArea>().Max.text = ((ValueQuestion)q).Range.y.ToString();
		}

		void SelectQuestion(GameObject item) 
		{
			SaveOldResult(currentIdx);
			SelectNewQuestion(item);
		}

		public void ShowMyself() 
		{
			Debug.Log("Hi");
			gameObject.SetActive(true);
		}


		public void Save()
		{
			SaveOldResult(currentIdx);
			GM.GM_Core.instance.experimentalSetup.questionnaire = questionnaire;
			Leave();
		}

		public void Leave()
		{
			gameObject.SetActive(false);
		}


	}
}
