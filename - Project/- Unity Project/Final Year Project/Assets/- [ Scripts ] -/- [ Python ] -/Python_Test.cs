using System;
using System.Collections;
using System.Collections.Generic;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using UnityEngine;

public class Python_Test : MonoBehaviour
{
    private readonly ScriptEngine _pythonEngine = UnityPython.CreateEngine();
    
    [SerializeField] [TextArea(2, 20)] private string _pythonCode;

    
    
    void Update()
    {
        ScriptScope scope = _pythonEngine.CreateScope();
        ScriptSource source = _pythonEngine.CreateScriptSourceFromString(_pythonCode);

        try
        {
            source.Execute(scope);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
