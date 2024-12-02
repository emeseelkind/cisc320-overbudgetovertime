using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetBudget : MonoBehaviour
{
    public GameObject gameManager;

    private TextMeshProUGUI textMeshPro;
    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        float budget = gameManager.GetComponent<Build>().budget;

        textMeshPro.text = budget.ToString("F2");
    }
}
