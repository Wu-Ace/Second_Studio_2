using UnityEngine;
using UnityEngine.InputSystem;



public class PhoneInfo : MonoBehaviour
{
    public        Vector3   AccelermeterValue;
    public static PhoneInfo instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateAccelerometer();
    }


    public void UpdateAccelerometer()
    {
        var accelerometer = GetRemoteDevice<Accelerometer>();
        if (accelerometer == null)
        {
            Debug.Log("No remote accelerometer found.");
            return;
        }

        AccelermeterValue = accelerometer.acceleration.ReadValue();
        // Debug.Log( $"Accelerometer: x={AccelermeterValue.x} y={AccelermeterValue.y} z={AccelermeterValue.z}");
        double max = 0;
        if (AccelermeterValue.y > max)
        {
            max = AccelermeterValue.y;
        }

    }


    private static TDevice GetRemoteDevice<TDevice>()
        where TDevice : InputDevice
    {
        foreach (var device in InputSystem.devices)
            if (device.remote && device is TDevice deviceOfType)
                return deviceOfType;
        return default;
    }
}