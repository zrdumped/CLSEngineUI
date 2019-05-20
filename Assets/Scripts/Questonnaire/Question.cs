using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Questionnaire
{
	public enum QuestionType {
		计分题 = 0
	}
	[Serializable]
	public class Question
	{
		static Dictionary<QuestionType, Type> name2type = new Dictionary<QuestionType, Type> { 
			{ QuestionType.计分题, Type.GetType("Questionnaire.ValueQuestion")}
		};

		public string QuestionContent
		{
			get
			{
				return questionContent;
			}

			set
			{
				questionContent = value;
			}
		}

		public QuestionType TypeName
		{
			get
			{
				return typeName;
			}

			set
			{
				typeName = value;
			}
		}
		public string questionContent;
		public QuestionType typeName;

		public Question()
		{
			questionContent = "";
			typeName = QuestionType.计分题;
		}

		public Question(string content, QuestionType t)
		{
			questionContent = content;
			typeName = t;
		}

		public static Question getQuestionByTypeName(QuestionType typeName) 
		{
			return Activator.CreateInstance(name2type[typeName]) as Question;
		}

		virtual public string GetCustomContent ()
		{
			return "";
		}

		//ToDO: only support value answer here
		virtual public string GetCustomData(List<ValueAnswer> a)
		{
			return "";
		}
	}

}


