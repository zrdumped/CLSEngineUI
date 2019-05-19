using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Questionnaire;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace UI
{
	public class UI_ResultInspector : MonoBehaviour
	{
		public Questionnaire.Questionnaire questionnaire;
		public UI_QuestionNumberList NumList;
		public Text CurrentNumber;
		public string NumberFormat = "第{0:D}题";
		public Text CurrentQuestionType;
		public string QuestionTypeFormat = "题型：{0}";
		public Text Content;
		public GameObject QuestionNumberButton;
		public List<AnswerSheet> answerSheets;
		List<string> datas;
		public Text PeopleCount;
		public string PeopleCountFormat = "人数：{0:D}人";
		public Text Data;

		int currentIdx = 0;

		// Use this for initialization
		void Start()
		{
			gameObject.SetActive(false);
			Init();
		}

		public void Init()
		{
			//ToDo: get http data first
			if (true) 
			{
				byte[] bytes = new byte[5000];
				BinaryFormatter bf = new BinaryFormatter();
				using (MemoryStream ms = new MemoryStream(bytes))
				{
					questionnaire = bf.Deserialize(ms) as Questionnaire.Questionnaire;
				}
			}
			else
				questionnaire = new Questionnaire.Questionnaire();
			if (true)
			{
				byte[] bytes = new byte[5000];
				BinaryFormatter bf = new BinaryFormatter();
				using (MemoryStream ms = new MemoryStream(bytes))
				{
					answerSheets = bf.Deserialize(ms) as List<AnswerSheet>;
				}
			}
			else 
				answerSheets = new List<AnswerSheet>();
			/*Questionnaire.Questionnaire q = new Questionnaire.Questionnaire();
			q.Add(new ValueQuestion("abc", new Vector2(1, 2)));
			q.Add(new ValueQuestion("edf", new Vector2(4, 5)));
			BinaryFormatter bf = new BinaryFormatter();
			using (MemoryStream ms = new MemoryStream())
			{
				bf.Serialize(ms, q);
				ms.Flush();
				ms.Position = 0;
				questionnaire = bf.Deserialize(ms) as Questionnaire.Questionnaire;
			}
			AnswerSheet p = new AnswerSheet();
			p.Add(new ValueAnswer(1));
			p.Add(new ValueAnswer(2));
			List<AnswerSheet> answer = new List<AnswerSheet>();
			answer.Add(p);
			using (MemoryStream ms = new MemoryStream())
			{
				bf.Serialize(ms, answer);
				ms.Flush();
				ms.Position = 0;
				answerSheets = bf.Deserialize(ms) as List<AnswerSheet>;
			}*/
			datas = new List<string>();
			for (int i = 0; i < questionnaire.Count(); i++)
			{
				AddQuestionUI(questionnaire[i], i);
				List<ValueAnswer> r = new List<ValueAnswer>();
				for (int j = 0; j < answerSheets.Count; j++) 
				{
					r.Add(answerSheets[j][i]);
				}
				datas.Add(questionnaire[i].GetCustomData(r));
			}	
			SelectNewQuestion(NumList[0]);
			PeopleCount.text = string.Format(PeopleCountFormat, answerSheets.Count);
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
		
		void SetQuestionContent(Question q)
		{

			Content.text = string.Format("{0}\n{1}", q.QuestionContent, q.GetCustomContent());
		}

		void SetData(int index)
		{
			Data.text = datas[index];
		}

		void SelectNewQuestion(GameObject item)
		{
			NumList[currentIdx].GetComponent<UI_QuestionNumberListItem>().OnNotSelected();
			currentIdx = item.GetComponent<UI_QuestionNumberListItem>().Index;
			item.GetComponent<UI_QuestionNumberListItem>().OnSelected();
			Question q = questionnaire[currentIdx];
			SetCurrentNumber(currentIdx);
			SetCurrentQuestionType(q.TypeName);
			SetQuestionContent(q);
			SetData(currentIdx);
		}
			
		void SelectQuestion(GameObject item)
		{
			SelectNewQuestion(item);
		}
			
		public void Leave()
		{
			gameObject.SetActive(false);
		}

		public void ShowMyself()
		{ 
			gameObject.SetActive(true);
		}
	}
}