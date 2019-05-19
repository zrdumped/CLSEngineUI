using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Questionnaire
{
	[Serializable]
	public class ValueAnswer 
	{
		//ToDo: as we don't have any time, here is only answer for value question o(TωT)o
		public float result = 0;

		public float Result
		{
			get
			{
				return result;
			}

			set
			{
				result = value;
			}
		}


		public ValueAnswer() { }
		
		public ValueAnswer(float f) {
			Result = f;
		}

	}
}