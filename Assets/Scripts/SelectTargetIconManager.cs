using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTargetIconManager : MonoBehaviour
{
    public GameObject MaskImageOnTheselectedIcon1 = default;
    public GameObject MaskImageOnTheselectedIcon2 = default;
    public GameObject MaskImageOnTheselectedIcon3 = default;
    public GameObject MaskImageOnTheselectedIcon4 = default;

    public void Reset()
    {
        MaskImageOnTheselectedIcon1.SetActive(false);
        MaskImageOnTheselectedIcon2.SetActive(false);
        MaskImageOnTheselectedIcon3.SetActive(false);
        MaskImageOnTheselectedIcon4.SetActive(false);
    }

    public void SelectToArena()
    {
        Reset();
        MaskImageOnTheselectedIcon1.SetActive(true);
    }
    public void SelectToCatsle()
    {
        Reset();
        MaskImageOnTheselectedIcon2.SetActive(true);
    }
    public void SelectToTown()
    {
        Reset();
        MaskImageOnTheselectedIcon3.SetActive(true);
    }
    public void SelectToGuild()
    {
        Reset();
        MaskImageOnTheselectedIcon4.SetActive(true);
    }


}
