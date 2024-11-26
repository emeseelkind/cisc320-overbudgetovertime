using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    public delegate void startGame();
    public static event startGame addGravity;

    public delegate void deleteMode();
    public static event deleteMode toggleDelete;

    public delegate void changeMaterial(int type);
    public static event changeMaterial onMaterialChange;

    public delegate void pReset();
    public static event pReset resetMap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void TriggerGravity()
    {
        addGravity?.Invoke();
    }

    public static void TriggerDelete()
    {
        toggleDelete?.Invoke();
    }

    public static void TriggerMaterialChange(int type)
    {
        onMaterialChange?.Invoke(type);
    }

    public static void TriggerMapReset()
    {
        resetMap?.Invoke();
    }
}
