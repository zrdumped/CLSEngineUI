using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Questionnaire;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace UI
{
	public class UI_QuestionnaireArea : MonoBehaviour
	{
		public Questionnaire.Questionnaire questionnaire;
		public UI_QuestionNumberList NumList;
		public Text CurrentNumber;
		public string NumberFormat = "第{0:D}题";
		public Text CurrentQuestionType;
		public string QuestionTypeFormat = "题型：{0}";
		public Text Content;
		public InputField Result;
		public GameObject QuestionNumberButton;
		public UI_CustomQuestionArea CustomArea;
		public AnswerSheet answerSheet;

		int currentIdx = 0;

		// Use this for initialization
		void Start()
		{
			questionnaire = null;
			gameObject.SetActive(false);
			//Init();
		}

		public void Init()
		{
			if (questionnaire != null)
				return;
			questionnaire = GM.GM_Core.instance.experimentalSetup.questionnaire;
			answerSheet = new AnswerSheet();
			for (int i = 0; i < questionnaire.Count(); i++) 
			{
				answerSheet.Add(new ValueAnswer());
				AddQuestionUI(questionnaire[i], i);
			}
			SelectNewQuestion(NumList[0]);
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

		void SetCurrentNumber(int index)
		{
			CurrentNumber.text = string.Format(NumberFormat, index + 1);
		}

		void SetCurrentQuestionType(QuestionType t)
		{
			CurrentQuestionType.text = string.Format(QuestionTypeFormat, t);
		}

		void SetGeneralQuestionContent(string s)
		{
			Content.text = s;
		}

		void SetCustomQuestionContent(Question q)
		{
			CustomArea.text.text = q.GetCustomContent();
		}

		void SetAnswerArea(int index)
		{
			//ToDo: only support value question︿(￣︶￣)︿, fix it if you are free
			Result.text = answerSheet[index].Result.ToString();
		}

		void SaveOldResult(int index)
		{
			//ToDo: only support value question︿(￣︶￣)︿, fix it if you are free
			answerSheet[index].Result = float.Parse(Result.text);
		}

		void SelectNewQuestion(GameObject item)
		{
			NumList[currentIdx].GetComponent<UI_QuestionNumberListItem>().OnNotSelected();
			currentIdx = item.GetComponent<UI_QuestionNumberListItem>().Index;
			item.GetComponent<UI_QuestionNumberListItem>().OnSelected();
			Question q = questionnaire[currentIdx];
			SetCurrentNumber(currentIdx);
			SetCurrentQuestionType(q.TypeName);
			SetGeneralQuestionContent(q.QuestionContent);
			SetCustomQuestionContent(q);
			SetAnswerArea(currentIdx);
		}

		void SelectQuestion(GameObject item)
		{
			SaveOldResult(currentIdx);
			SelectNewQuestion(item);
		}

		public void Commit()
		{
			WWWForm form = new WWWForm();
			form.AddField("invite", GM.GM_Core.instance.Invite);
			form.AddField("value", JsonUtility.ToJson(answerSheet));
			Chemix.Network.NetworkManager.Instance.Post(form, "scene/submit", null);
			Leave();
		}

		public void Leave()
		{
			gameObject.SetActive(false);
		}

		public void Showmyself()
		{
			Init();
			gameObject.SetActive(true);
		}
	}
}