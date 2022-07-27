using System.Collections;
using UnityEngine;

namespace LlamAcademy.Spring.Runtime
{
    public class SpringToTarget2D : BaseSpringBehaviour, ISpringTo<Vector2>, INudgeable<Vector2>
    {
        private SpringVector2 Spring;

        private void Awake()
        {
            Spring = new SpringVector2()
            {
                StartValue = transform.position,
                EndValue = transform.position,
                Damping = Damping,
                Stiffness = Stiffness
            };
        }

        private IEnumerator DoSpringToTarget(Vector2 TargetPosition)
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

            while (!Mathf.Approximately(Vector2.SqrMagnitude(
                    new Vector2(transform.position.x,
                        transform.position.y
                        ) - TargetPosition), 0))
            {
                transform.position = Spring.Evaluate(Time.deltaTime);

                yield return null;
            }

            Spring.Reset();
        }

        public void SpringTo(Vector2 TargetPosition)
        {
            StopAllCoroutines();

            CheckInspectorChanges();

            StartCoroutine(DoSpringToTarget(TargetPosition));
        }

        private void CheckInspectorChanges()
        {
            Spring.Damping = Damping;
            Spring.Stiffness = Stiffness;
        }

        public void Nudge(Vector2 Amount)
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

        private IEnumerator HandleNudge(Vector2 Amount)
        {
            Spring.Reset();
            Spring.StartValue = transform.position;
            Spring.EndValue = transform.position;
            Spring.InitialVelocity = Amount;
            Vector3 targetPosition = transform.position;
            transform.position = Spring.Evaluate(Time.deltaTime);

            while (!Mathf.Approximately(
                0,
                Vector2.SqrMagnitude(targetPosition - transform.position)
            ))
            {
                transform.position = Spring.Evaluate(Time.deltaTime);

                yield return null;
            }

            Spring.Reset();
        }
    }
}