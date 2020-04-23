//2020-04-23
//Matthew Demoe 
//Developed for Directed Studies in IT under Alvaro Joffre Uribe-Quevedo

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.Linq;


public class DLL_Test : MonoBehaviour
{
    private static DLL_Test _instance;

    public static DLL_Test Instance()
    {
        if (_instance == null)
        {
            _instance = new GameObject().AddComponent<DLL_Test>();
        }

        return _instance;
    }


    [DllImport("Glove_Plugin")]
    static extern void o(int portNum);

    [DllImport("Glove_Plugin")]
    static extern void b();

    [DllImport("Glove_Plugin")]
    static extern void e();

    [DllImport("Glove_Plugin")]
    static extern void c();

    [DllImport("Glove_Plugin")]
    static extern IntPtr rd();

    [SerializeField]
    int portNumber = 7;

    IntPtr gloveInput;

    int[] inputInts = new int[20];

    [SerializeField]
    List<int> valuesToCheck = new List<int>();
    List<int> gloveOutputs = new List<int>();

    string outputString = string.Empty;

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;

        for (int i = 0; i < valuesToCheck.Count; i++)
        {
            gloveOutputs.Add(0);
        }

        o(portNumber);
        b();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();

        PrintOutput();

        if (Input.GetKeyDown(KeyCode.X))
        {
            e();
            c();
            Debug.Log("Port Closed");
        }
    }

    public List<int> GetGloveOutputs()
    {
        return gloveOutputs;
    }

    void PrintOutput()
    {
        for (int i = 0; i < valuesToCheck.Count; i++)
        {
            outputString += inputInts[valuesToCheck[i]] + " : ";
        }

        //Debug.Log(outputString);

        outputString = string.Empty;
    }

    void GetInput()
    {
        try
        {
            gloveInput = rd();

            string inputString = Marshal.PtrToStringAnsi(gloveInput);
            //Debug.Log(inputString);

            string[] splitString = inputString.Split("    ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            //Debug.Log(splitString[0]);

            for (int i = 0; i < splitString.Length; i++)
            {
                try
                {
                    //Debug.Log(i + " : " + splitString[i]);
                    inputInts[i] = int.Parse(splitString[i]);
                }
                catch (FormatException e)
                {

                }
            }            
        }

        catch (NullReferenceException e)
        {
        }

        for (int i = 0; i < valuesToCheck.Count; i++)
        {
            gloveOutputs[i] = inputInts[valuesToCheck[i]];
        }
    }

    private void OnApplicationQuit()
    {
        e();
        c();
    }
}
