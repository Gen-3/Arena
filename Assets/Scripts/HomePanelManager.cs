using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePanelManager : MonoBehaviour
{
    [SerializeField]GameObject changeEquipPanelAtHome = default;

    public void OpenPanel()
    {
        changeEquipPanelAtHome.SetActive(true);
    }

    public void ClosePanel()
    {
        changeEquipPanelAtHome.SetActive(false);
    }


}
