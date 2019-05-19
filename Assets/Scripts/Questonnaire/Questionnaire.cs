using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Questionnaire
{
	[Serializable]
	public class Questionnaire
	{
		List<Question> questions;

		public Questionnaire()
		{
			questions = new List<Question>();
		}

		public int Add(Question q)
		{
			questions.Add(q);
			return questions.Count - 1;
		}

		public void Remove(int index)
		{
			questions.RemoveAt(index);
		}

		public void Edit(int index, Question q)
		{
			questions[index] = q;
		}

		public Question this[int index]
		{
			get { return questions[index];}
			set { questions[index] = value;}
		}

		public int Count()
		{
			return questions.Count;
		}
	}
}
