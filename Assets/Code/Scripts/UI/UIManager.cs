using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Chemix.UI
{
    public class UIManager : Singleton<UIManager>
    {
        public GameObject canvas;

        [Header("Prefabs")]
        public GameObject debugButtonPrefab;
        public GameObject equationBoxPrefab;
        public GameObject formulaLabelPrefab;
        public GameObject focusPrefab;

        [SerializeField]
        private float m_EquationBoxLifetime = 3f; // TODO: shouldn't be here

        private GameObject focusInstance;
        private GameObject focusOwner;

        private Dictionary<Transform, GameObject> m_OwnerToEquationBox = new Dictionary<Transform, GameObject>();

        public void DisplayEquationBox(List<ChemixObject> cobjects, string equationInfo)
        {
            GameObject equationBox = null;

            foreach (var co in cobjects)
            {
                if (m_OwnerToEquationBox.TryGetValue(co.transform, out equationBox))
                {
                    break;
                }
            }

            if (equationBox == null)
            {
                Transform owner = cobjects[0].transform;
                equationBox = Instantiate(equationBoxPrefab, canvas.transform);
                m_OwnerToEquationBox[owner] = equationBox;

                var uicontroller = equationBox.GetComponent<UIController>();
                uicontroller.followTarget = owner;
                var animator = equationBox.GetComponent<Animator>();
                animator.SetBool("isOpen", true);

                StartCoroutine(EquationBox_WaitAndDestroy(owner));
            }

            equationBox.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "反应: " + equationInfo;
        }

        IEnumerator EquationBox_WaitAndDestroy(Transform owner)
        {
            var animator = m_OwnerToEquationBox[owner].GetComponent<Animator>();

            yield return new WaitForSeconds(m_EquationBoxLifetime);
            animator.SetBool("isOpen", false);
            m_OwnerToEquationBox.Remove(owner);

            yield return new WaitForSeconds(1);
            Destroy(animator.gameObject);
        }

        public void CreateFormulaLabel(ChemixObject cobject)
        {
            var go = Instantiate(formulaLabelPrefab, canvas.transform);
            go.name = "[label] " + cobject.name;
            var formulaLabel = go.GetComponent<FormulaLabel>();
            formulaLabel.followTarget = cobject.transform;
            formulaLabel.owner = cobject;
        }

        public void DisplayFocus(GameObject owner)
        {
            if (!focusInstance)
            {
                focusInstance = Instantiate(focusPrefab, canvas.transform);
            }
            focusInstance.SetActive(true);
            focusInstance.transform.position = ChemixEngine.Instance.mainCamera.WorldToScreenPoint(owner.transform.position);
            focusOwner = owner;
        }

        public void HideFocus(GameObject owner)
        {
            if (focusOwner == owner)
            {
                focusInstance.SetActive(false);
            }
        }
    }
}