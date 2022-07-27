using UnityEngine;

namespace LlamAcademy.Spring.Runtime
{
    public class Vector3Nudger : MonoBehaviour
    {
        public BaseSpringBehaviour Nudgeable;
        public Vector3 NudgeAmount = new Vector3(0, 500, 0);
        public Vector2 NudgeFrequency = new Vector2(2, 10);
        public bool AutoNudge = true;

        private float LastNudgeTime;
        private float NextNudgeFrequency;

        private void Awake()
        {
            NextNudgeFrequency = Random.Range(NudgeFrequency.x, NudgeFrequency.y);
        }

        private void Update()
        {
            if (AutoNudge && LastNudgeTime + NextNudgeFrequency < Time.time)
            {
                Nudge();
            }
        }

        public void Nudge()
        {
            (Nudgeable as INudgeable<Vector3>).Nudge(NudgeAmount);
            LastNudgeTime = Time.time;
            NextNudgeFrequency = Random.Range(NudgeFrequency.x, NudgeFrequency.y);
        }
    }
}
