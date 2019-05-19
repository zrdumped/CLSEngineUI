using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace UI
{
	public class UI_QuestionNumberList : MonoBehaviour
	{
		public List<GameObject> items = new List<GameObject>();
		public VerticalLayoutGroup layout;
		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}

		public void Add(GameObject item) 
		{
			items.Add(item);
			item.transform.SetParent(layout.transform);

		}

		public void RemoveLast() 
		{
			GameObject last = items[items.Count - 1];
			items.Remove(last);
			Destroy(last);

		}

		public GameObject this[int index]
		{
			get { return items[index]; }
			set { items[index] = value; }
		}
	}
}