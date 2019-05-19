using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Questionnaire;

namespace UI
{
	public class UI_AddQuestionButton : MonoBehaviour
	{
		public QuestionType TypeName;
		public Text text;
		// Use this for initialization
		void Start()
		{
			text.text = TypeName.ToString();
		}

		// Update is called once per frame
		void Update()
		{

		}


	
	}
}
