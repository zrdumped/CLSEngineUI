using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix
{
    /// <summary>
    /// TipBoard shows tip on what to do next.
    /// </summary>
    public class TipBoard : Singleton<TipBoard>
    {
        #region variables
        public TMPro.TextMeshPro titleMesh, detailMesh, warningMesh; //reactionMesh;

        TaskFlow taskFlow;
        int stepIndex = 0, substepIndex = 0;
        string stepName, taskDetail;
        TaskFlow.Substep lastSubstep;

        // text animation
        int frameCnt = 0, animStepIndex = 0;
        bool pendingUpdateText = false;
        int doneTextLen = 0, doneTextPtr = 0, todoTextLen = 0, todoTextPtr = 0;
        int warningLifetime = 0;

        const string k_TextHighlightColor = "<color=#00be72>";
        #endregion

        public void Warning(string warning)
        {
            warningMesh.gameObject.SetActive(true);
            warningMesh.text = warning;
            warningLifetime = 100;
        }

        void Progress()
        {
            // update step index
            if (taskFlow.steps[stepIndex].substeps.Count == substepIndex + 1)
            {
                stepIndex++;
                substepIndex = 0;
            }
            else
            {
                substepIndex++;
            }

            pendingUpdateText = true;

            // listen to events
            if (lastSubstep != null)
            {
                ChemixEventManager.Instance.Off(lastSubstep, Progress);
            }
            if (stepIndex < taskFlow.steps.Count)
            {
                lastSubstep = taskFlow.steps[stepIndex].substeps[substepIndex];
                ChemixEventManager.Instance.On(lastSubstep, Progress);
            }
        }

        void Start()
        {
            taskFlow = Chemix.taskFlow;
            animStepIndex = stepIndex = Chemix.Config.tipInitialStepIndex;
            if (taskFlow.steps.Count == 0)
            {
                this.enabled = false;
                titleMesh.text = "";
                detailMesh.text = "";
                warningMesh.text = "";
                return;
            }
            stepName = (animStepIndex + 1).ToString() + ") " + taskFlow.steps[animStepIndex].detail + "\n    ";
            titleMesh.text = taskFlow.title;
            warningMesh.text = "";

            // listen to events
            lastSubstep = taskFlow.steps[stepIndex].substeps[substepIndex];
            ChemixEventManager.Instance.On(lastSubstep, Progress);

            // setup in custom mode
            if (Chemix.CustomMode)
            {
                var setup = GameManager.Instance.GetExperimentalSetup();
                titleMesh.transform.position = setup.title.position;
                titleMesh.color = setup.title.color;
                titleMesh.fontSize *= setup.title.size;

                detailMesh.transform.position = setup.detail.position;
                detailMesh.color = setup.detail.color;
                detailMesh.fontSize *= setup.detail.size;
            }
        }

        void PrepareForAnimation()
        {
            taskDetail = "";

            if (animStepIndex == stepIndex)
            {
                for (int i = 0; i < substepIndex; i++)
                {
                    taskDetail += taskFlow.steps[animStepIndex].substeps[i].detail + "，";
                }
                doneTextLen = taskDetail.Length;
                taskDetail += taskFlow.steps[animStepIndex].substeps[substepIndex].detail;
                todoTextLen = taskDetail.Length;
            }
            else
            {
                int imax = taskFlow.steps[animStepIndex].substeps.Count - 1;
                for (int i = 0; i < imax; i++)
                {
                    taskDetail += taskFlow.steps[animStepIndex].substeps[i].detail + "，";
                }
                taskDetail += taskFlow.steps[animStepIndex].substeps[imax].detail;
                doneTextLen = todoTextLen = taskDetail.Length;
            }
        }

        void FixedUpdate()
        {
            frameCnt++;

            // update TextMesh every `animCycle` frames
            if (frameCnt > Chemix.Config.tipAnimCycle)
            {
                frameCnt = 0;

                if (pendingUpdateText)
                {
                    PrepareForAnimation();
                    pendingUpdateText = false;
                }

                // if this step complete, enter next step
                if (doneTextPtr == todoTextLen && animStepIndex < taskFlow.steps.Count)
                {
                    animStepIndex = stepIndex;
                    doneTextPtr = todoTextPtr = 0;

                    if (stepIndex >= taskFlow.steps.Count)
                    {
                        stepName = "";
                        taskDetail = taskFlow.completeMessage;
                        todoTextLen = doneTextLen = taskDetail.Length;
                    }
                    else
                    {
                        stepName = (animStepIndex + 1).ToString() + ") " + taskFlow.steps[animStepIndex].detail + "\n    ";
                        PrepareForAnimation();
                    }
                }

                // update text animation
                bool shouldUpdateDone = doneTextPtr < doneTextLen,
                    shouldUpdateTodo = todoTextPtr < todoTextLen;
                if (shouldUpdateDone || shouldUpdateTodo)
                {
                    if (shouldUpdateDone)
                        doneTextPtr++;
                    if (shouldUpdateTodo)
                        todoTextPtr++;

                    // update text mesh
                    //if (Chemix.CustomMode)
                    //{
                    //    detailMesh.text = stepName + taskDetail.Substring(0, todoTextPtr);
                    //}
                    //else
                    //{
                        detailMesh.text = stepName + k_TextHighlightColor;
                        detailMesh.text += taskDetail.Substring(0, doneTextPtr) + "</color>";
                        detailMesh.text += taskDetail.Substring(doneTextPtr, todoTextPtr - doneTextPtr);
                    //}
                }

                // update warning text
                if (warningLifetime > 0)
                {
                    warningLifetime--;
                    if (warningLifetime == 0)
                        warningMesh.gameObject.SetActive(false);
                }
            }
        }
    }
}