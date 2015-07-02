using Assets.Scripts.Helpers;
using Assets.Scripts.Models;
using UnityEngine;

public class DroidLaser : MonoBehaviour
{
    public GameObject laser;
    public float scanningDuration = 1f;
    private float scanningAnimationTimer = 0f;
    public bool isScanning;

    public void Start()
    {
        laser.SetActive(false);
    }

    public void Update()
    {
        if (isScanning)
        {
            var angle = Time.deltaTime / scanningDuration * 360;
            laser.transform.Rotate(laser.transform.up, angle);
            scanningAnimationTimer += Time.deltaTime;
            if (scanningAnimationTimer >= scanningDuration)
            {
                StopScan();
                scanningAnimationTimer = 0;
            }
        }
    }

    private Quaternion rotation;
    public LaserData Scan()
    {
        if (!isScanning)
        {
            laser.SetActive(true);
            isScanning = true;
            rotation = laser.transform.rotation;
            laser.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
        }

        var point = new Vector3(laser.transform.position.x, 1f, laser.transform.position.z);
        return LaserHelper.ScanPoint(point); //(int)transform.rotation.eulerAngles.y
    }

    public void StopScan()
    {
        laser.SetActive(false);
        isScanning = false;
        laser.transform.rotation = rotation;
    }
}

