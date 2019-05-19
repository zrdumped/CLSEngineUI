using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UI_Main : MonoBehaviour
    {
        private GM.GM_Core gm;
		public Animator wrongAnimator;
		public GameObject KeyPanel;
		public InputField Key;
		bool ToEdit = true;

        // Use this for initialization
        void Start()
        {
			gm = GM.GM_Core.instance;
			//KeyPanel.SetActive(false);
        }

        public void CreateNewExperiment_OnClick()
        {
			Debug.Log(gm.IsGuest + " " + gm.Account + gm.Password);
			if (gm.IsGuest)
			{
				wrongAnimator.Play("Notification In");
				return;
			}
            gm.SwitchToScene("BuildExperiment");
        }

		public void EditExperiment_OnClick()
		{
			if (gm.IsGuest)
			{
				wrongAnimator.Play("Notification In");
				return;
			}
			ToEdit = true;
			KeyPanel.SetActive(true);
		}

        public void Test_OnClick()
        {
			ToEdit = false;
			KeyPanel.SetActive(true);
            
        }

		public void SendKey()
		{
			string key = Key.text;
			WWWForm form = new WWWForm();
			form.AddField("invite", key);
			Chemix.Network.NetworkManager.Instance.Post(form, "scene/invite",
														(success, reply) =>
			{
				if (success)
				{
					gm.Invite = key;
					gm.experimentalSetup = JsonUtility.FromJson<Chemix.GameManager.ExperimentalSetup>(reply.Detail);
					if (ToEdit)
					{
						gm.SwitchToScene("BuildExperiment");
					}
					else
					{ 
						gm.SwitchToScene("CustomLab");
					}
				}
				else
				{
					wrongAnimator.Play("Notification In");
				}
			}
													   );
		}

		public void Leave()
		{ 
			KeyPanel.SetActive(false);
		}
    }
}
