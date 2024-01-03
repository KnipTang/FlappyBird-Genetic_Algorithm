using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _Score;
    [SerializeField]
    private TextMeshProUGUI _birdsAlifeLabel;
    // Start is called before the first frame update
    public int score = 0;

    PopulationController populationController;
    void Start()
    {
        if(_birdsAlifeLabel != null)
        {
            _birdsAlifeLabel.text =
                    "Birds Alife: ";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_birdsAlifeLabel != null && _Score != null)
        {
            GameObject populationControllerObj = GameObject.FindGameObjectWithTag("PopulationController");
            if (populationControllerObj == null)
                return;
            
            populationController = populationControllerObj.GetComponent<PopulationController>();
            if (populationController == null)
                return;

            _Score.text = "Top score: " + score.ToString();

            if (populationController.bestPopulation.Count > 3)
            {
                _birdsAlifeLabel.text = "Birds Alife: " + (populationController.populationSize - populationController.deadBirds).ToString() + '\n' + "Gen: " + populationController.gen.ToString()
                    + '\n' + "Best Bird  : " + populationController.bestPopulation[0].GetComponent<Score>().fitnessScore
                    + '\n' + "Second Bird: " + populationController.bestPopulation[1].GetComponent<Score>().fitnessScore
                    + '\n' + "Third Bird : " + populationController.bestPopulation[2].GetComponent<Score>().fitnessScore
                    + '\n' + "Fourth Bird: " + populationController.bestPopulation[3].GetComponent<Score>().fitnessScore;
            }
        }
    }
}
