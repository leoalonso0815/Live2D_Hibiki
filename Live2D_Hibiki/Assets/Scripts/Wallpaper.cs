using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class Wallpaper : MonoBehaviour
{
    private Texture2D tex;
    private SpriteRenderer mySpriteRenderer;
    private byte[] rawWallpaper;
    //public Transform wallpaper;
    private List<Sprite> mySprite = new List<Sprite>();//sprite图集
    //public List<Texture2D> texs = new List<Texture2D>();
    private List<string> paths = new List<string>();//文件路径
    private int pathsCount;
    float width, height, whRatio, wallpaperZ;
    float wallpaperSizeOrigin = 16.0f / 9.0f;
    private float playTime; //计时器
    public float playRate;//每隔多长时间替换壁纸
    private int wallpaperID = 0;
    private bool isFirst = true;

    void Awake()
    {
        createXml();
        string filePath = Application.dataPath + @"/wallpaperPlayRate.xml";
        if (File.Exists(filePath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            XmlNodeList nodeList = xmlDoc.SelectSingleNode("wallpaperPlayRate").ChildNodes;
            string value = nodeList[0].InnerText;
            playRate = Convert.ToSingle(value);
        }
    }

    void Start()
    {
        GlobeMgr.gm.SetWallpaper(false);
        //创建wallpaper文件夹
        string dPath = Application.dataPath;
        int num = dPath.LastIndexOf("/");
        dPath = dPath.Substring(0, num);
        string directory = dPath + "/wallpaper";
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        //创建sprite图集List
        if (Directory.Exists(directory))
        {
            mySpriteRenderer = GetComponent<SpriteRenderer>();
            string imgtype = "*.BMP|*.JPG|*.GIF|*.PNG|*.JPEG";
            string[] ImageType = imgtype.Split('|');
            for (int i = 0; i < ImageType.Length; i++)
            {
                //获取wallpaper文件夹下所有的图片路径
                string[] dirs = Directory.GetFiles(directory, ImageType[i]);
                for (int j = 0; j < dirs.Length; j++)
                {
                    paths.Add(dirs[j]);
                }
            }
            if (paths.Count != 0)
            {
                GlobeMgr.gm.progressSlider.SetActive(true);
                pathsCount = paths.Count;
                StartCoroutine(LoadingSprite()); //协程，一张一张加载图片
            }
            else
            {
                GlobeMgr.gm.SetWallpaper(true);
            }
        }
        //if (mySprite.Count >= 1 && mySprite.Count == paths.Count)
        //{
        //    SetWallpaper();
        //}
    }

    void Update()
    {
        if (mySprite.Count >= 1 && mySprite.Count == pathsCount)
        {
            if (isFirst) //加载第一张壁纸，只执行一次
            {
                SetWallpaper(0);
                isFirst = false;
            }
            //壁纸幻灯片播放
            if (playTime < playRate) //计时器逻辑
            {
                playTime += Time.deltaTime;
            }
            if (playTime >= playRate)
            {
                wallpaperID += 1;
                wallpaperID = wallpaperID % mySprite.Count;
                SetWallpaper(wallpaperID);
                playTime -= playRate;
            }
            //if (playTime>=playRate*0.5f)
            //{
            //    wallpaper.GetComponent<UIPlayTween>().Play(true);
            //}
        }
        //Debug.Log(wallpaperID);
        //Debug.Log(cuWallpaperID);
    }

    //壁纸大小最适化逻辑
    void SetWallpaper(int id)
    {
        mySpriteRenderer.sprite = mySprite[id];
        height = mySpriteRenderer.sprite.textureRect.height;
        width = mySpriteRenderer.sprite.textureRect.width;
        whRatio = width / height;
        //Debug.Log(whRate.ToString());
        //Debug.Log(wallpaperSizeOrigin.ToString());
        if (whRatio > wallpaperSizeOrigin)//大于高就按高
        {
            wallpaperZ = height * 5.4f / 1080.0f;
            transform.position = new Vector3(0, 0, -(5.4f - wallpaperZ));
        }
        else //小于等于高就按宽
        {
            wallpaperZ = width * 5.4f / 1920.0f;
            transform.position = new Vector3(0, 0, -(5.4f - wallpaperZ));
        }
    }

    void createXml()
    {
        string filePath = Application.dataPath + @"/wallpaperPlayRate.xml";
        if (!File.Exists(filePath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement root = xmlDoc.CreateElement("wallpaperPlayRate");
            XmlElement elmNew = xmlDoc.CreateElement("setPlayRate");
            elmNew.SetAttribute("id", "0");
            elmNew.SetAttribute("name", "playRateValue");
            XmlElement value = xmlDoc.CreateElement("value");
            value.InnerText = "15";

            elmNew.AppendChild(value);
            root.AppendChild(elmNew);
            xmlDoc.AppendChild(root);
            xmlDoc.Save(filePath);
            Debug.Log("createXml OK!");
        }
    }

    //一张一张加载图片
    private IEnumerator LoadingSprite()
    {
        for (int i = 0; i < paths.Count; i++)
        {
            tex = new Texture2D(1, 1);
            rawWallpaper = File.ReadAllBytes(paths[i]);
            tex.LoadImage(rawWallpaper);
            mySprite.Add(Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f));
            GlobeMgr.gm.wallpaperNum = i;
            GlobeMgr.gm.wallpaperListNum = paths.Count;
            GlobeMgr.gm.wallpaperLoadingProgress = i / (paths.Count * 1.0f);
            //Debug.Log("paths.Count=====" + paths.Count);
            //Debug.Log("GlobeMgr.gm.wallpaperNum=====" + GlobeMgr.gm.wallpaperNum);
            //Debug.Log(GlobeMgr.gm.wallpaperLoadingProgress);
            yield return null;
        }
        yield return null;
    }
}