using System.Collections;
using System.Collections.Generic;
// using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Sonar : MonoBehaviour
{
    private SimpleSonarShader_Object _shader;

    // Start is called before the first frame update
    void Start()
    {
        _shader = GetComponent<SimpleSonarShader_Object>();
        // InvokeRepeating("StartSonar", 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartSonar()
    {
        _shader.StartSonarRing(new Vector3(7368f, 2352f, 11.45f), 1f);
    }

}