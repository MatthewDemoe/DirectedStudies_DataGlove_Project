//2020-04-23
//Matthew Demoe 
//Developed for Directed Studies in IT under Alvaro Joffre Uribe-Quevedo

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawController : MonoBehaviour
{
    List<Animator> animators = new List<Animator>();

    List<int> gloveOutputs;

    [SerializeField]
    float knuckleAverage = 0;

    [SerializeField]
    float middleAverage = 0;

    float wristFlexion = 0.0f;

    Transform palm;

    [SerializeField]
    float knuckleFloor = 500.0f;

    [SerializeField]
    float knuckleBaseCeiling = 775.0f;

    [SerializeField]
    float knuckleMiddleCeiling = 1000.0f;

    [SerializeField]
    float wristFloor = 10000.0f;

    [SerializeField]
    float wristCeiling = 16000.0f;

    [SerializeField]
    float clawTop = 4.25f;

    [SerializeField]
    float clawBottom = -0.25f;

    [SerializeField]
    float speed = 0.5f;

    Vector3 startingPos;
    Vector3 resetPos;

    float resetTimeMax = 5.0f;
    float resetTimer = 0.0f;

    bool isReset = true;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            animators.Add(transform.GetChild(0).GetChild(i).GetComponent<Animator>());
        }

        startingPos = transform.position;

        animators.Add(GetComponent<Animator>());
        palm = transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        //handClose = Mathf.Clamp(handClose, 0.0f, 1.0f);

        GetInput();
        //PrintValues();
        SendAnimValues();
        //LowerClaw();
    }

    void GetInput()
    {
        gloveOutputs = DLL_Test.Instance().GetGloveOutputs();

        for (int i = 0; i < gloveOutputs.Count - 1; i++)
        {
            if ((i % 2) == 0)
                middleAverage += gloveOutputs[i];

            else
                knuckleAverage += gloveOutputs[i];
        }

        wristFlexion = gloveOutputs[gloveOutputs.Count - 1];

        knuckleAverage /= (gloveOutputs.Count - 1) / 2;
        middleAverage /= (gloveOutputs.Count - 1) / 2;

        NormalizeValues();
    }

    void NormalizeValues()
    {
        knuckleAverage = Mathf.Clamp(UtilMath.Lmap(knuckleAverage, knuckleFloor, knuckleBaseCeiling, 0.0f, 1.0f), 0.0f, 1.0f);
        middleAverage = Mathf.Clamp(UtilMath.Lmap(middleAverage, knuckleFloor, knuckleMiddleCeiling, 0.0f, 1.0f), 0.0f, 1.0f);

        wristFlexion = Mathf.Clamp(UtilMath.Lmap(wristFlexion, wristFloor, wristCeiling, 0.0f, 1.0f), 0.0f, 1.0f);
    }

    void SendAnimValues()
    {
        foreach (Animator anim in animators)
        {
            anim.SetFloat("Knuckle_Base", knuckleAverage);
            anim.SetFloat("Knuckle_Middle", middleAverage);

        }
    }

    void LowerClaw()
    {
        transform.position = new Vector3(transform.position.x, UtilMath.Lmap(wristFlexion, 0.0f, 1.0f, clawBottom, clawTop), transform.position.z);
    }

    void DropCoins()
    {
        for (int i = 0; i < palm.childCount; i++)
        {
            palm.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
        }

        palm.DetachChildren();
    }

    public float GetBaseKnuckle()
    {
        return knuckleAverage;
    }

    public float GetMiddleKnuckle()
    {
        return middleAverage;
    }

    public float GetWrist()
    {
        return wristFlexion;
    }

    public bool HandIsClosed()
    {
        return (knuckleAverage >= 0.9f);
    }
    public bool HandIsOpen()
    {
        return (knuckleAverage <= 0.4f);
    }

    public bool FingersAreClenched()
    {
        return (middleAverage >= 0.9f);
    }

    public bool WristIsFlexed()
    {
        Debug.Log(wristFlexion);
        return (wristFlexion <= 0.1f);
    }

    public bool WristIsStraight()
    {
        return (wristFlexion >= 0.9f);
    }

    public void MoveHorizontal()
    {
        transform.Translate(0.0f, 0.0f, -speed * Time.deltaTime);
    }

    public void MoveVertical()
    {
        transform.Translate(speed * Time.deltaTime, 0.0f, 0.0f);
    }

    public void Lower()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y - speed * Time.deltaTime, clawBottom, clawTop), transform.position.z);
    }

    public void ResetClaw()
    {
        resetPos = transform.position;
        Debug.Log("Resetting");
        StartCoroutine("BeginReset");
    }

    IEnumerator BeginReset()
    {
        isReset = false;

        while (resetTimer <= resetTimeMax)
        {
            resetTimer += Time.deltaTime;

            float newX = UtilMath.EasingFunction.Linear(resetPos.x, startingPos.x, resetTimer / resetTimeMax);
            float newY = UtilMath.EasingFunction.Linear(resetPos.y, startingPos.y, resetTimer / resetTimeMax);
            float newZ = UtilMath.EasingFunction.Linear(resetPos.z, startingPos.z, resetTimer / resetTimeMax);

            transform.position = new Vector3(newX, newY, newZ);

            yield return new WaitForEndOfFrame();
        }

        resetTimer = 0.0f;
        isReset = true;
        CancelInvoke("BeginReset");
        GameLoop.Instance().AdvanceGameState();
    }

    public bool GetIsReset()
    {
        return isReset;
    }

    public void SetBaseCeiling()
    {
        gloveOutputs = DLL_Test.Instance().GetGloveOutputs();

        for (int i = 0; i < gloveOutputs.Count - 1; i++)
        {
            if ((i % 2) == 0)
                middleAverage += gloveOutputs[i];

            else
                knuckleAverage += gloveOutputs[i];
        }

        wristFlexion = gloveOutputs[gloveOutputs.Count - 1];

        knuckleAverage /= (gloveOutputs.Count - 1) / 2;
        middleAverage /= (gloveOutputs.Count - 1) / 2;

        knuckleBaseCeiling = knuckleAverage;
    }

    public void SetMiddleCeiling()
    {
        gloveOutputs = DLL_Test.Instance().GetGloveOutputs();

        for (int i = 0; i < gloveOutputs.Count - 1; i++)
        {
            if ((i % 2) == 0)
                middleAverage += gloveOutputs[i];

            else
                knuckleAverage += gloveOutputs[i];
        }

        wristFlexion = gloveOutputs[gloveOutputs.Count - 1];

        knuckleAverage /= (gloveOutputs.Count - 1) / 2;
        middleAverage /= (gloveOutputs.Count - 1) / 2;

        knuckleMiddleCeiling = middleAverage;
    }
}
