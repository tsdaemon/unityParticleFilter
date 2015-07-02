using Assets.Scripts.Models;
using UnityEngine;

public class DroidMoving : MonoBehaviour
{
    public bool isMoving;
    public float moveSpeed = 10f;
    public float moveTolerance = 0.1f;
    public float rotationSpeed = 10f;
    private MoveModel moveAdd = new MoveModel();

    // subsystems
    private DroidMap map;
    

    void Start()
    {
        map = GetComponent<DroidMap>();
    }

    void Update()
    {
        var forward = Input.GetAxis("Vertical");
        if (forward != 0f)
        {
            // create random move
            var randomShift = Random.insideUnitCircle * moveTolerance * Time.deltaTime;
            var shift = transform.forward*forward*moveSpeed*Time.deltaTime +
                        new Vector3(randomShift.x, 0f, randomShift.y);
            transform.position += shift;

            // shift map using move model when move is big enough
            moveAdd.shift += transform.forward * forward * moveSpeed * Time.deltaTime;
            moveAdd.tolerance += moveTolerance * Time.deltaTime;
            if (moveAdd.shift.sqrMagnitude >= 0.25)
            {
                map.OnMove(moveAdd);
                moveAdd = new MoveModel();
                GetComponent<DroidSound>().PlayRandom();
            }
        }
        var horizontal = Input.GetAxis("Horizontal");
        if (horizontal != 0f)
        {
            // rotate over y axis
            var yRotation = horizontal*Time.deltaTime * rotationSpeed;
            var euler = transform.rotation.eulerAngles;
            euler.y += yRotation;
            transform.rotation = Quaternion.Euler(euler);
        }
    }
}

