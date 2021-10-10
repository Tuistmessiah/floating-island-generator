using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientLogging : MonoBehaviour
{
    [SerializeField] bool isOn = true;
    public static ClientLogging instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public void Debugger(string _message)
    {
        if (this.isOn)
        {
            Debug.Log($"-> {_message}");
        }
    }

    public void Debugger(string[] _messages, string _id = "")
    {
        if (this.isOn)
        {
            Debug.Log($"-> :{_id}: {string.Join(", ", _messages)}");
        }
    }

}
