using System.Collections;
using UnityEngine;

namespace LlamAcademy.Spring.Runtime
{
    public class SpringToScale : BaseSpringBehaviour, ISpringTo<Vector3>, INudgeable<Vector3>
    {
        private SpringVector3 Spring;

        private void Awake()
        {
            Spring = new SpringVector3()
            {
                StartValue = transform.localScale,
                EndValue = transform.localScale,
                Damping = Damping,
                Stiffness = Stiffness
            };
        }

        public void SpringTo(Vector3 TargetScale)
        {
            StopAllCoroutines();

            CheckInspectorChanges();

            StartCoroutine(DoSpringToTarget(TargetScale));
        }


        public void Nudge(Vector3 Amount)
        {
            CheckInspectorChanges();
            if (Mathf.Approximately(Spring.CurrentVelocity.sqrMagnitude, 0))
            {
                StartCoroutine(HandleNudge(Amount));
            }
            else
            {
                Spring.UpdateEndValue(Spring.EndValue, Spring.CurrentVelocity + Amount);
            }
        }

        private IEnumerator HandleNudge(Vector3 Amount)
{
            Spring.Reset();
            Spring.StartValue = transform.localScale;
            Spring.EndValue = transform.localScale;
            Spring.InitialVelocity = Amount;
            Vector3 targetScale = transform.localScale;
            transform.localScale = Spring.Evaluate(Time.deltaTime);

            while (!Mathf.Approximately(
               0,
               Vector3.SqrMagnitude(targetScale - transform.localScale)
           ))
            {
                transform.localScale = Spring.Evaluate(Time.deltaTime);

                yield return null;
            }

            Spring.Reset();
        }

        private IEnumerator DoSpringToTarget(Vector3 TargetScale)
        {
            if (Mathf.Approximately(Spring.CurrentVelocity.sqrMagnitude, 0))
            {
                Spring.Reset();
                Spring.StartValue = transform.localScale;
                Spring.EndValue = TargetScale;
            }
            else
            {
                Spring.UpdateEndValue(TargetScale, Spring.CurrentVelocity);
            }

            while (!Mathf.Approximately(Vector3.SqrMagnitude(transform.localScale - TargetScale), 0))
            {
                transform.localScale = Spring.Evaluate(Time.deltaTime);

                yield return null;
            }

            Spring.Reset();
        }

        private void CheckInspectorChanges()
        {
            Spring.Damping = Damping;
            Spring.Stiffness = Stiffness;
        }
    }
}