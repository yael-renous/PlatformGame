using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Dropdown _levelsDropdown;
    private List<TMPro.TMP_Dropdown.OptionData> dropdownOptions;
    void Start()
    {
        FillLevelNames();
    }

    private void FillLevelNames()
    {
        GameConfig gameConfig = GameManager.Instance.GetConfig();
        _levelsDropdown.ClearOptions();
        dropdownOptions = new List<TMPro.TMP_Dropdown.OptionData>();
        
        foreach (var level in gameConfig.levels)
        {
            TMPro.TMP_Dropdown.OptionData newOption = new TMPro.TMP_Dropdown.OptionData(level.levelName);
            dropdownOptions.Add(newOption);
        }
        _levelsDropdown.AddOptions(dropdownOptions);
    }

    public void OpenLevel()
    {
        int selectedIndex = _levelsDropdown.value;
        string levelName = dropdownOptions[selectedIndex].text;
        
        GameManager.Instance.OpenLevel(levelName);
    }
}
