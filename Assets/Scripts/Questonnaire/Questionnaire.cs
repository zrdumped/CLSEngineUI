using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Questionnaire
{
	//As we use json utilizer, we can't enjoy the inheritance. So I only support value question here. 
	//The Question is totally changed to ValueQuestion.
	//Good luck for anyone who's gonna change this part. You may want to reconstruct it. щ(｀ω´щ) 
	[Serializable]
	public class Questionnaire
	{
		public List<Question> questions;

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
