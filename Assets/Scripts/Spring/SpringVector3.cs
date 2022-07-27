using UnityEngine;

namespace LlamAcademy.Spring
{
    public class SpringVector3 : BaseSpring<Vector3>
    {
        private FloatSpring XSpring = new();
        private FloatSpring YSpring = new();
        private FloatSpring ZSpring = new();

        public override float Damping
        {
            get { return base.Damping; }
            set
            {
                XSpring.Damping = value;
                YSpring.Damping = value;
                ZSpring.Damping = value;
                base.Damping = value;
            }
        }

        public override float Stiffness
        {
            get { return base.Stiffness; }
            set
            {
                XSpring.Stiffness = value;
                YSpring.Stiffness = value;
                ZSpring.Stiffness = value;
                base.Stiffness = value;
            }
        }

        public override Vector3 StartValue
        {
            get
            {
                return new Vector3(
                    XSpring.StartValue,
                    YSpring.StartValue,
                    ZSpring.StartValue
                );
            }
            set
            {
                XSpring.StartValue = value.x;
                YSpring.StartValue = value.y;
                ZSpring.StartValue = value.z;
            }
        }
        public override Vector3 EndValue
        {
            get
            {
                return new Vector3(
                    XSpring.EndValue, 
                    YSpring.EndValue, 
                    ZSpring.EndValue
                );
            }
            set
            {
                XSpring.EndValue = value.x;
                YSpring.EndValue = value.y;
                ZSpring.EndValue = value.z;
            }
        }

        public override Vector3 InitialVelocity
        {
            get
            {
                return new Vector3(
                    XSpring.InitialVelocity, 
                    YSpring.InitialVelocity, 
                    ZSpring.InitialVelocity
                );
            }
            set
            {
                XSpring.InitialVelocity = value.x;
                YSpring.InitialVelocity = value.y;
                ZSpring.InitialVelocity = value.z;
            }
        }

        public override Vector3 CurrentVelocity
        {
            get
            {
                return new Vector3(
                    XSpring.CurrentVelocity, 
                    YSpring.CurrentVelocity, 
                    ZSpring.CurrentVelocity
                );
            }
            set
            {
                XSpring.CurrentVelocity = value.x;
                YSpring.CurrentVelocity = value.y;
                ZSpring.CurrentVelocity = value.z;
            }
        }

        public override  Vector3 CurrentValue
        {
            get
            {
                return new Vector3(
                    XSpring.CurrentValue, 
                    YSpring.CurrentValue, 
                    ZSpring.CurrentValue
                );
            }
            set
            {
                XSpring.CurrentValue = value.x;
                YSpring.CurrentValue = value.y;
                ZSpring.CurrentValue = value.z;
            }
        }

        public override Vector3 Evaluate(float DeltaTime)
        {
            CurrentValue = new Vector3(
                XSpring.Evaluate(DeltaTime),
                YSpring.Evaluate(DeltaTime),
                ZSpring.Evaluate(DeltaTime)
            );
            CurrentVelocity = new Vector3(
                XSpring.CurrentVelocity, 
                YSpring.CurrentVelocity, 
                ZSpring.CurrentVelocity
            );
            return CurrentValue;
        }

        public override void Reset()
        {
            XSpring.Reset();
            YSpring.Reset();
            ZSpring.Reset();
        }

        public override void UpdateEndValue(Vector3 Value, Vector3 Velocity)
        {
            XSpring.UpdateEndValue(Value.x, Velocity.x);
            YSpring.UpdateEndValue(Value.y, Velocity.y);
            ZSpring.UpdateEndValue(Value.z, Velocity.z);
        }
    }
}
