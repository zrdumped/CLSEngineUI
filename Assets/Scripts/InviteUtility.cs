using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix
{
	public class InviteUtility 
	{
		static List<string> baseStrs = new List<string>{ "abcdefghij", "klmnopqrst", "uvwzxy1234", "567890!@#$"};

		static public string InviteFrom(int n)
		{
			string invite = "";
			for (int i = 0; i < 4; i++)
			{
				invite = baseStrs[Random.Range(0, baseStrs.Count)][(n % 10)] + invite;
				n /= 10;
			}
			return invite;
		}

		static public int ParseInvite(string invite)
		{
			int res = 0;
			foreach (char c in invite)
			{
				int p = GetNumber(c);
				if (p < 0)
				{
					return p;
				}
				res *= 10;
				res += p;
			}
			return res;
		}

		static int GetNumber(char c)
		{
			for (int i = 0; i < baseStrs.Count; i++)
			{
				if (baseStrs[i].IndexOf(c) >= 0)
				{
					return baseStrs[i].IndexOf(c);
				}
			}
			return -1;
		}
	}
}