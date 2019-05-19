using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Questionnaire;

namespace UI
{
	public class UI_CustomQuestionArea : MonoBehaviour
	{
		public Text text;

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}

		void SetText(Question q) 
		{
			text.text = q.GetCustomContent();
		}
	}
}
