using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Python_Test : MonoBehaviour
{
    //private readonly ScriptEngine _pythonEngine = Python.CreateEngine();
    
    [SerializeField] [TextArea(1, 500)] private string _pythonCode;
    [Space(100)] public float hi;
    
    
    void Update()
    {
        //ScriptScope scope = _pythonEngine.CreateScope();
        //ScriptSource source = _pythonEngine.CreateScriptSourceFromString(_pythonCode);

        try
        {
            //source.Execute(scope);
            //string output = scope.GetVariable<string>("output");
            
            //Debug.Log(output);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
