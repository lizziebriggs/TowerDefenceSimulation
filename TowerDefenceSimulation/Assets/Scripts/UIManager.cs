using System;
using Map;
using SDS;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private StochasticDiffusionSearch sds;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private TowerSpawner towerSpawner;

    [Header("UI Elements")]
    [SerializeField] private Text playButtonText;
    [SerializeField] private Text towerText;
    [SerializeField] private Text iterationText;

    [Header("UI Map Setting Elements")]
    [SerializeField] private GameObject mapSettings;
    [SerializeField] private Animator mapSettingsAnimator;
    [SerializeField] private InputField widthInput;
    [SerializeField] private InputField heightInput;
    [SerializeField] private InputField towerPopInput;
    [SerializeField] private Dropdown towerDispersion;
    [SerializeField] private InputField agentPopInput;

    [Header("UI SDS Setting Elements")]
    [SerializeField] private GameObject sdsSettings;
    [SerializeField] private Animator sdsSettingsAnimator;
    [SerializeField] private Dropdown recruitmentModes;
    [SerializeField] private Toggle destroyTowersToggle;
    [SerializeField] private Toggle infiniteToggle;
    [SerializeField] private GameObject maxIterationInputObject;
    [SerializeField] private InputField maxIterationInput;

    
    private void Start()
    {
        // Set SDS to not be infinite by default
        infiniteToggle.isOn = false;
        ToggleInfinite();
        
        // Set SDS to not destroy towers by defaults
        destroyTowersToggle.isOn = false;
        ToggleTowerDestruction();
    }


    private void Update()
    {
        towerText.text = "Towers left: " + mapGenerator.Towers;
        iterationText.text = "Iterations: " + sds.Itr;
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


    public void TogglePlay(bool setPlay)
    {
        if (setPlay)
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


    public void ChangeTowerDispersion()
    {
        towerSpawner.Dispersion = (TowerSpawner.DispersionType)towerDispersion.value;
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


    public void OpenMapSettings()
    {
        bool isOpen = mapSettingsAnimator.GetBool("open");
        mapSettingsAnimator.SetBool("open", !isOpen);
        sdsSettings.SetActive(!sdsSettings.activeSelf);
    }


    public void OpenSDSSettings()
    {
        bool isOpen = sdsSettingsAnimator.GetBool("open");
        sdsSettingsAnimator.SetBool("open", !isOpen);
        mapSettings.SetActive(!mapSettings.activeSelf);
    }

    
    public void GenerateMap()
    {
        if(sds.PlaySDS) TogglePlay();
        
        mapGenerator.MapWidth = Convert.ToInt32(widthInput.text);
        mapGenerator.MapHeight = Convert.ToInt32(heightInput.text);
        towerSpawner.TowerPopulation = Convert.ToInt32(towerPopInput.text);
        sds.PopulationSize = Convert.ToInt32(agentPopInput.text);
        
        mapGenerator.ClearMap();
        sds.ClearAgents();
        
        mapGenerator.GenerateMap();
        sds.InitialiseAgents();
    }
}
