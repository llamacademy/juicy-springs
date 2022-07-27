using System.Collections;
using UnityEngine;

namespace LlamAcademy.Spring.Runtime
{
    public class SpringToTarget3D : BaseSpringBehaviour, ISpringTo<Vector3>, INudgeable<Vector3>
    {
        private SpringVector3 Spring;

        private void Awake()
        {
            Spring = new SpringVector3()
            {
                StartValue = transform.position,
                EndValue = transform.position,
                Damping = Damping,
                Stiffness = Stiffness
            };
        }

        public void SpringTo(Vector3 TargetPosition)
        {
            StopAllCoroutines();

            CheckInspectorChanges();

            StartCoroutine(DoSpringToTarget(TargetPosition));
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
            Spring.StartValue = transform.position;
            Spring.EndValue = transform.position;
            Spring.InitialVelocity = Amount;
            Vector3 targetPosition = transform.position;
            transform.position = Spring.Evaluate(Time.deltaTime);

            while (!Mathf.Approximately(
                0,
                Vector3.SqrMagnitude(targetPosition - transform.position)
            ))
            {
                transform.position = Spring.Evaluate(Time.deltaTime);

                yield return null;
            }

            Spring.Reset();
        }

        private IEnumerator DoSpringToTarget(Vector3 TargetPosition)
        {
            if (Mathf.Approximately(Spring.CurrentVelocity.sqrMagnitude, 0))
            {
                Spring.Reset();
                Spring.StartValue = transform.position;
                Spring.EndValue = TargetPosition;
            }
            else
            {
                Spring.UpdateEndValue(TargetPosition, Spring.CurrentVelocity);
            }

            while (!Mathf.Approximately(Vector3.SqrMagnitude(transform.position - TargetPosition), 0))
            {
                transform.position = Spring.Evaluate(Time.deltaTime);

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