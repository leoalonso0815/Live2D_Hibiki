using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingWallpaper : MonoBehaviour
{
    private Slider loadingSlider;
    private float loadingTimeValue = 0;

    void Awake()
    {
        loadingSlider = GetComponent<Slider>();     
    }

    private void Start()
    {
        StartCoroutine(StartLoading("live2d Hibiki"));
    }

    private IEnumerator StartLoading(string str)
    {
        AsyncOperation acop = SceneManager.LoadSceneAsync(str);
        //acop.allowSceneActivation = false;
        //while (loadingTimeValue <= 100)
        //{
        //    loadingTimeValue++;
        //    //Debug.Log(loadingTimeValue);
        //    loadingSlider.value = loadingTimeValue / 100;
        //    yield return new WaitForEndOfFrame();
        //}     
        //acop.allowSceneActivation = true;
        yield return acop;
    }
}
