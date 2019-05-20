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
		public float minV = 0, maxV = 0;
		public Vector2 Range
		{
			get
			{
				return new Vector2(minV, maxV);
			}

			set
			{
				minV = value.x;
				maxV = value.y;
			}
		}
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
			return string.Format("取值范围：最小值：{0:F}，最大值：{1:F}", Range.x, Range.y);
		}

		//ToDO: only support value answer here
		virtual public string GetCustomData(List<ValueAnswer> anss)
		{
			float avg = 0, max = float.MinValue, min = float.MaxValue;
			foreach (ValueAnswer a in anss)
			{
				max = Mathf.Max(max, a.Result);
				min = Mathf.Min(min, a.Result);
				avg += a.Result;
			}
			if (anss.Count > 0)
				avg /= anss.Count;
			return string.Format("平均值：{0:F}, 最小值：{1:F}，最大值：{2:F}", avg, min, max);
		}
	}

}


