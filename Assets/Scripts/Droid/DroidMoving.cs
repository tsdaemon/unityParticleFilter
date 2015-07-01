using UnityEngine;

public class DroidMoving : MonoBehaviour
{
    public bool isMoving;
    public float animationTime = 1.0f;

    private Vector3 to;
    private float rotationFrom;
    private float rotationTo;

    private bool isRotating;
    private bool isForward;

    void Update()
    {
        if (isForward)
        {
            if ((to - transform.position).sqrMagnitude > 0.01f)
            {
                var add = (to - from)*Time.deltaTime/animationTime;
                transform.localPosition += add;
            }
            else 
            {
                isMoving = false;
                isForward = false;
                transform.position = to;
            }
        }
        if (isRotating)
        {
            var rotationToAngles = rotationTo > 0 ? rotationTo : rotationTo + 360;
            if (Mathf.Abs(rotationToAngles - transform.localRotation.eulerAngles.y) > 3f)
            {
                var yAdd = (rotationTo - rotationFrom)*Time.deltaTime/animationTime;
                var euler = transform.rotation.eulerAngles;
                euler.y += yAdd;
                transform.rotation = Quaternion.Euler(euler);
            }
            else
            {
                isMoving = false;
                isRotating = false;
                var euler = transform.rotation.eulerAngles;
                euler.y = rotationTo;
                transform.rotation = Quaternion.Euler(euler);
            }
        }
    }

    public void MoveForward()
    {
        if (isMoving)
        {
            return;
        }
        isMoving = true;

        isForward = true;
        from = transform.localPosition;
        to = transform.forward + transform.localPosition;
    }

    public void RotateCCW()
    {
        RotateOn(-90);
    }

    private void RotateOn(float angle)
    {
        if (isMoving)
        {
            return;
        }
        isMoving = true;

        isRotating = true;
        rotationFrom = transform.rotation.eulerAngles.y;
        rotationTo = rotationFrom + angle;
    }

    public void RotateCW()
    {
        RotateOn(90);
    }

    public Vector3 from { get; set; }
}

