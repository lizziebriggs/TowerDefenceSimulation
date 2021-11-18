using SDS;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private StochasticDiffusionSearch sds;

    [Header("UI Elements")]
    [SerializeField] private Dropdown recruitmentModes;
    
    
    public void PlaySDS()
    {
        sds.PlaySDS = true;
    }

    public void PauseSDS()
    {
        sds.PlaySDS = false;
    }

    public void ChangeRecruitmentMode()
    {
        sds.Recruitment = (StochasticDiffusionSearch.RecruitmentModes)recruitmentModes.value;
    }
}
