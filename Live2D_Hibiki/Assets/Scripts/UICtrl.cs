using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICtrl : MonoBehaviour
{
    private Slider progressSlider;
    private Text progressText;
    private int wallpaperNum, wallpaperListNum;

    // Use this for initialization
    void Awake()
    {
        progressSlider = GetComponent<Slider>();
        progressText = transform.FindChild("Text").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        progressSlider.value = GlobeMgr.gm.wallpaperLoadingProgress;
        progressText.text = GlobeMgr.gm.wallpaperNum.ToString();
        if (progressSlider.value >= 0.98)
        {
            progressSlider.value = 1.0f;
        }
        if (GlobeMgr.gm.wallpaperNum == GlobeMgr.gm.wallpaperListNum - 1)
        {
            gameObject.SetActive(false);
            GlobeMgr.gm.SetWallpaper(true);
        }
    }
}
