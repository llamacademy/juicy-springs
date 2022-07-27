using System.Collections;
using UnityEngine;

namespace LlamAcademy.Spring.Runtime
{
    public class SpringToRotation : BaseSpringBehaviour, ISpringTo<Vector3>, ISpringTo<Quaternion>, INudgeable<Vector3>, INudgeable<Quaternion>
    {
        private SpringVector3 Spring;

        private void Awake()
        {
            Spring = new SpringVector3()
            {
                StartValue = transform.rotation.eulerAngles,
                EndValue = transform.rotation.eulerAngles,
                Damping = Damping,
                Stiffness = Stiffness
            };
        }

        public void SpringTo(Vector3 TargetRotation)
        {
            SpringTo(Quaternion.Euler(TargetRotation));
        }


        public void SpringTo(Quaternion TargetRotation)
        {
            StopAllCoroutines();

            CheckInspectorChanges();

            StartCoroutine(DoSpringToTarget(TargetRotation));
        }

        private IEnumerator DoSpringToTarget(Quaternion TargetRotation)
        {
            if (Mathf.Approximately(Spring.CurrentVelocity.sqrMagnitude, 0))
            {
                Spring.Reset();
                Spring.StartValue = transform.eulerAngles;
                Spring.EndValue = TargetRotation.eulerAngles;
            }
            else
            {
                Spring.UpdateEndValue(TargetRotation.eulerAngles, Spring.CurrentVelocity);
            }

            while (!Mathf.Approximately(0, 1 - Quaternion.Dot(transform.rotation, TargetRotation)))
            {
                transform.rotation = Quaternion.Euler(Spring.Evaluate(Time.deltaTime));

                yield return null;
            }

            Spring.Reset();
        }

        private void CheckInspectorChanges()
        {
            Spring.Damping = Damping;
            Spring.Stiffness = Stiffness;
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
            Spring.StartValue = transform.rotation.eulerAngles;
            Spring.EndValue = transform.rotation.eulerAngles;
            Spring.InitialVelocity = Amount;
            Quaternion targetRotation = transform.rotation;
            transform.rotation = Quaternion.Euler(Spring.Evaluate(Time.deltaTime));

            while (!Mathf.Approximately(
                0,
                1 - Quaternion.Dot(targetRotation, transform.rotation)
            ))
            {
                transform.rotation = Quaternion.Euler(Spring.Evaluate(Time.deltaTime));

                yield return null;
            }

            Spring.Reset();
        }

        public void Nudge(Quaternion Amount)
        {
            Nudge(Amount.eulerAngles);
        }
    }
}