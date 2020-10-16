using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameObject enterPoint;
    private GameObject ExitPoint;

    private GameObject[] oldCars;
    

    public bool isStart;

    public bool isfail = true;

    public int carCount = 0;

    void Start()
    {
        isStart = false;
        enterPoint = GameObject.Find("Entrances");
        InvokeRepeating("UpdateCars", 0, 0.1f);
        
    }

    void Update()
    {
        
    }


    void UpdateCars()
    {
        oldCars = GameObject.FindGameObjectsWithTag(Tags.OLDCAR_TAG);
    }

    public void CreateCar()
    {    
        carCount++;
        Debug.Log(carCount);
        for (int i = 0; i <= carCount; i++)
        {
            enterPoint.transform.GetChild(i).gameObject.SetActive(true);
        }
        isStart = false;    
    }

    public void ResetPos()
    {
        Debug.Log("hey hey hey");
        for (int i = 0; i < oldCars.Length; i++)
        {
            oldCars[i].GetComponent<CarScript>().GoFirstPos();
            
        }
        isStart = false;
        isfail = true;
    }
}
