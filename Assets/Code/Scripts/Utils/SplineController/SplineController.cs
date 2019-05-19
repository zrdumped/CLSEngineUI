using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace Chemix.Utils
{
    public enum eOrientationMode { NODE = 0, TANGENT }

    // TODO: rewrite or refactor, it's horrible to put other logic in OnDrawGizmos()
    [AddComponentMenu("Splines/Spline Controller")]
    [RequireComponent(typeof(SplineInterpolator))]
    public class SplineController : BaseSlave
    {
        public GameObject SplineRoot;
        public float Duration = 10;
        public eOrientationMode OrientationMode = eOrientationMode.NODE;
        public eWrapMode WrapMode = eWrapMode.ONCE;
        public bool AutoStart = true;
        public bool AutoClose = true;
        public Color gizmoColor = Color.black;

        SplineInterpolator mSplineInterp;
        Transform[] mTransforms;
        bool bInitGizmo = false;

        public void Play()
        {
            if (!mSplineInterp.enabled)
                mSplineInterp.enabled = true;
            else
                FollowSpline();
        }

        public override void ReceiveCommand()
        {
            Play();
        }

        void OnDrawGizmos()
        {
            Transform[] trans = GetTransforms();
            if (trans.Length < 2)
                return;

            SplineInterpolator interp = GetComponent(typeof(SplineInterpolator)) as SplineInterpolator;

            if (!Application.isPlaying || !bInitGizmo)
            {
                SetupSplineInterpolator(interp, trans);
                interp.StartInterpolation(null, false, WrapMode);
                bInitGizmo = true;
            }

            Vector3 prevPos = trans[0].position;
            for (int c = 1; c <= 100; c++)
            {
                float currTime = c * Duration / 100;
                Vector3 currPos = interp.GetHermiteAtTime(currTime);
                //float mag = (currPos - prevPos).magnitude * 2;
                Gizmos.color = gizmoColor; //new Color(mag, 0, 0, 1);
                Gizmos.DrawLine(prevPos, currPos);
                prevPos = currPos;
            }
        }

        void Start()
        {
            mSplineInterp = GetComponent(typeof(SplineInterpolator)) as SplineInterpolator;

            mTransforms = GetTransforms();

            if (SplineRoot != null)
            {
                // Change to SetActive() may result in no rotation control
                //SplineRoot.SetActiveRecursively(false);

                //SplineRoot.SetActive(false);

                for (int i = 0; i < SplineRoot.transform.childCount; i++)
                {
                    SplineRoot.transform.GetChild(i).gameObject.SetActive(false);
                }
            }

            FollowSpline();

            if (!AutoStart)
            {
                mSplineInterp.enabled = false;
            }
        }

        void SetupSplineInterpolator(SplineInterpolator interp, Transform[] trans)
        {
            interp.Reset();

            float step = (AutoClose) ? Duration / trans.Length :
                Duration / (trans.Length - 1);

            int c;
            for (c = 0; c < trans.Length; c++)
            {
                if (OrientationMode == eOrientationMode.NODE)
                {
                    var splineEvent = trans[c].GetComponent<SplineEvent>();
                    UnityEvent onArrive = new UnityEvent();
                    if (splineEvent)
                    {
                        onArrive = splineEvent.onArrive;
                    }
                    interp.AddPoint(trans[c].position, trans[c].rotation, step * c, new Vector2(0, 1), onArrive);
                }
                else if (OrientationMode == eOrientationMode.TANGENT)
                {
                    Quaternion rot;
                    if (c != trans.Length - 1)
                        rot = Quaternion.LookRotation(trans[c + 1].position - trans[c].position, trans[c].up);
                    else if (AutoClose)
                        rot = Quaternion.LookRotation(trans[0].position - trans[c].position, trans[c].up);
                    else
                        rot = trans[c].rotation;

                    var splineEvent = trans[c].GetComponent<SplineEvent>();
                    UnityEvent onArrive = new UnityEvent();
                    if (splineEvent)
                    {
                        onArrive = splineEvent.onArrive;
                    }
                    interp.AddPoint(trans[c].position, rot, step * c, new Vector2(0, 1), onArrive);
                }
            }

            if (AutoClose)
                interp.SetAutoCloseMode(step * c);
        }

        /// <summary>
        /// Returns children transforms, sorted by name.
        /// </summary>
        Transform[] GetTransforms()
        {
            if (SplineRoot != null)
            {
                List<Component> components = new List<Component>(SplineRoot.GetComponentsInChildren(typeof(Transform)));
                List<Transform> transforms = components.ConvertAll(c => (Transform)c);

                transforms.Remove(SplineRoot.transform);
                transforms.Sort(delegate (Transform a, Transform b)
                {
                    return a.name.CompareTo(b.name);
                });

                if (transforms.Count > 0)
                {
                    transforms[0].position = transform.position;
                    transforms[0].rotation = transform.rotation;
                }

                return transforms.ToArray();
            }

            return null;
        }

        /// <summary>
        /// Starts the interpolation
        /// </summary>
        void FollowSpline()
        {
            if (mTransforms.Length > 0)
            {
                SetupSplineInterpolator(mSplineInterp, mTransforms);
                mSplineInterp.StartInterpolation(null, true, WrapMode);
            }
        }
    }
}