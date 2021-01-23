using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobeMgr : MonoBehaviour
{
    static public GlobeMgr gm;
    public int wallpaperNum;
    public int wallpaperListNum;
    public float wallpaperLoadingProgress;
    public GameObject live2dCharacter, nguiRoot, progressSlider;

    private void Awake()
    {
        gm = this;
        //live2dCharacter.SetActive(false);
        //nguiRoot.SetActive(false);
    }

    //void Start()
    //{    

    //}

    public void SetWallpaper(bool isLoading)
    {
        if (isLoading)
        {
            live2dCharacter.SetActive(true);
            nguiRoot.SetActive(true);
        }
        else
        {
            live2dCharacter.SetActive(false);
            nguiRoot.SetActive(false);
        }
    }
}
