using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class UI_QuestionNumberListItem : MonoBehaviour
	{
		int index;
		public Text text;
		public int Index
		{
			get
			{
				return index;
			}

			set
			{
				index = value;
				text.text = index.ToString();
			}
		}

		// Use this for initialization
		void Start()
		{
			text.text = (index + 1).ToString();
		}

		// Update is called once per frame
		void Update()
		{

		}

		public void OnSelected()
		{
			text.color = new Color(243 / 255, 1, 1);
		}

		public void OnNotSelected()
		{
			text.color = new Color(1, 1, 1);
		}
	}
}