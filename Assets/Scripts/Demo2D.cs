using LlamAcademy.Spring;
using LlamAcademy.Spring.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Demo2D : MonoBehaviour
{
    [SerializeField]
    private Transform Target;
    [SerializeField]
    private TMP_InputField RotationInputField;
    [SerializeField]
    private SpringToTarget2D SpringToTarget;
    [SerializeField]
    private SpringToRotation RotationSpring;
    [SerializeField]
    private SpringToScale ScaleSpring;
    [SerializeField]
    [Range(1, 10)]
    private float NudgeFrequency = 5;
    [SerializeField]
    private Vector2 PositionNudgeAmount = new Vector2(-300, 1000);
    [SerializeField]
    private Vector3 RotationNudgeAmount = new Vector3(0, 0, 1000);
    [SerializeField]
    private Vector3 ScaleNudgeAmount = new Vector3(-50, -50, -50);

    [SerializeField]
    private bool EnableClickToMove = true;
    [SerializeField]
    private bool IsNudgingEnabled = true;

    private float LastMovementNudge;
    private float LastRotationNudge;
    private float LastScaleNudge;

    private void Awake()
    {
        LastMovementNudge = 0;
        LastRotationNudge = 2;
        LastScaleNudge = 3.5f;
    }

    public void SetRotation()
    {
        Target.transform.rotation = Quaternion.Euler(0, 0, float.Parse(RotationInputField.text));
        RotationSpring.SpringTo(new Vector3(0, 0, float.Parse(RotationInputField.text)));
    }

    public void ToggleMovementSpring()
    {
        EnableClickToMove = !EnableClickToMove;
    }

    public void ToggleNudging()
    {
        IsNudgingEnabled = !IsNudgingEnabled;
    }

    private void Update()
    {
        if (Application.isFocused && EnableClickToMove
            && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Target.transform.position = Mouse.current.position.ReadValue();
            SpringToTarget.SpringTo(Target.transform.position);
        }

        if (IsNudgingEnabled && LastMovementNudge + NudgeFrequency < Time.time)
        {
            SpringToTarget.Nudge(PositionNudgeAmount);
            LastMovementNudge = Time.time;
        }
        if (IsNudgingEnabled && LastRotationNudge + NudgeFrequency < Time.time)
        {
            RotationSpring.Nudge(RotationNudgeAmount);
            LastRotationNudge = Time.time;
        }
        if (IsNudgingEnabled && LastScaleNudge + NudgeFrequency < Time.time)
        {
            ScaleSpring.Nudge(ScaleNudgeAmount);
            LastScaleNudge = Time.time;
        }
    }
}
