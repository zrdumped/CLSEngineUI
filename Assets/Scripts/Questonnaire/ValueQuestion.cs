using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Questionnaire
{
	[Serializable]
	public class ValueQuestion : Question
	{
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

		public ValueQuestion() 
		{
			TypeName = QuestionType.计分题;
		}

		public ValueQuestion(string content, Vector2 r) {
			Range = r;
			QuestionContent = content;
			TypeName = QuestionType.计分题;
		}

		override public string GetCustomContent()
		{
			return string.Format("取值范围：最小值：{0:F}，最大值：{1:F}", Range.x, Range.y);
		}

		override public string GetCustomData(List<ValueAnswer> anss)
		{
			float avg = 0, max = float.MinValue, min = float.MaxValue;
			foreach (ValueAnswer a in anss) 
			{
				max = Mathf.Max(max, a.Result);
				min = Mathf.Min(min, a.Result);
				avg += a.Result;
			}
			if(anss.Count > 0)
				avg /= anss.Count;
			return string.Format("平均值：{0:F}, 最小值：{1:F}，最大值：{2:F}", avg, min, max);
		}
	}

}
