using System;
using SDS;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private StochasticDiffusionSearch sds;
    [SerializeField] private MapGenerator mapGenerator;

    [Header("UI SDS Setting Elements")]
    [SerializeField] private Text playButtonText;
    [SerializeField] private Dropdown recruitmentModes;
    [SerializeField] private Toggle destroyTowersToggle;
    [SerializeField] private Toggle infiniteToggle;
    [SerializeField] private GameObject maxIterationInputObject;
    [SerializeField] private InputField maxIterationInput;

    [Header("UI Map Setting Elements")]
    [SerializeField] private Animator settingsAnimator;
    [SerializeField] private InputField widthInput;
    [SerializeField] private InputField heightInput;
    [SerializeField] private InputField towerPopInput;
    [SerializeField] private InputField agentPopInput;

    
    private void Start()
    {
        // Set SDS to not be infinite by default
        infiniteToggle.isOn = false;
        ToggleInfinite();
        
        // Set SDS to not destroy towers by defaults
        destroyTowersToggle.isOn = false;
        ToggleTowerDestruction();
    }


    public void TogglePlay()
    {
        if (!sds.PlaySDS)
        {
            playButtonText.text = "Pause";
            sds.PlaySDS = true;
        }
        else
        {
            playButtonText.text = "Play";
            sds.PlaySDS = false;
        }
    }


    public void ToggleTowerDestruction()
    {
        if (destroyTowersToggle.isOn)
            sds.DestroyTowers = true;
        else if (!destroyTowersToggle.isOn)
            sds.DestroyTowers = false;
    }


    public void ToggleInfinite()
    {
        if (infiniteToggle.isOn)
        {
            maxIterationInputObject.SetActive(false);
            sds.Infinite = true;
        }
        else
        {
            maxIterationInputObject.SetActive(true);
            sds.Infinite = false;
        }
    }


    public void SetMaxIterations()
    {
        sds.MaxIterations = Convert.ToInt32(maxIterationInput.text);
    }

    
    public void ChangeRecruitmentMode()
    {
        sds.Recruitment = (StochasticDiffusionSearch.RecruitmentModes)recruitmentModes.value;
    }


    public void OpenSettings()
    {
        bool isOpen = settingsAnimator.GetBool("open");
        settingsAnimator.SetBool("open", !isOpen);
    }

    
    public void GenerateMap()
    {
        if(sds.PlaySDS) TogglePlay();
        
        mapGenerator.MapWidth = Convert.ToInt32(widthInput.text);
        mapGenerator.MapHeight = Convert.ToInt32(heightInput.text);
        mapGenerator.TowerPopulation = Convert.ToInt32(towerPopInput.text);
        sds.PopulationSize = Convert.ToInt32(agentPopInput.text);
        
        mapGenerator.ClearMap();
        sds.ClearAgents();
        
        mapGenerator.GenerateMap();
        sds.InitialiseAgents();
    }
}
