using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Questionnaire
{
	[Serializable]
	public class AnswerSheet 
	{
		//ToDo: as we don't have any time, here is only answer for value question o(TωT)o
		List<ValueAnswer> answers;
		public AnswerSheet()
		{
			answers = new List<ValueAnswer>();
		}

		public int Add(ValueAnswer a)
		{
			answers.Add(a);
			return answers.Count - 1;
		}

		public void Remove(int index)
		{
			answers.RemoveAt(index);
		}

		public void Edit(int index, ValueAnswer a)
		{
			answers[index] = a;
		}

		public ValueAnswer this[int index]
		{
			get { return answers[index]; }
			set { answers[index] = value; }
		}

		public int Count()
		{
			return answers.Count;
		}
	}
}
