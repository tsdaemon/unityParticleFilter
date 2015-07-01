using Assets.Scripts.Models;
using UnityEngine;

public class DroidMoving : MonoBehaviour
{
    public bool isMoving;
    public float moveSpeed = 10f;
    public float moveDopusk = 0.1f;
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
            var shift = transform.forward*forward*moveSpeed*Time.deltaTime;
            var dopusk = moveDopusk*Time.deltaTime;
            var randomMove = Random.insideUnitCircle*dopusk;
            var randomShift = new Vector3(shift.x + randomMove.x, 0f, shift.z + randomMove.y);
            transform.position += randomShift;
            // shift map using move model when move is big enough
            moveAdd.shift += shift;
            moveAdd.dopusk += dopusk;
            if (moveAdd.shift.sqrMagnitude >= 0.25)
            {
                map.ShiftPaticles(moveAdd);
                moveAdd = new MoveModel();
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

