using UnityEngine;
using System;
using System.Xml;
using System.IO;
using System.Collections;
using live2d;
using live2d.framework;

[ExecuteInEditMode]
public class SimpleModelMotionKC : MonoBehaviour
{
    private Live2DModelUnity live2DModel;
    private Live2DMotion motionBattle, motionPort, motionSecret, motionВерныйOne, motionВерныйNewYear, motionChrismas;
    private MotionQueueManager motionMgr;
    private L2DPhysics physics;
    private L2DTargetPoint dragMgr = new L2DTargetPoint();
    private EyeBlinkMotion eyeBlink = new EyeBlinkMotion();

    private Matrix4x4 live2DCanvasPos;

    public TextAsset mocFile;
    public TextAsset physicsFile;
    public AudioSource ВерныйAS;
    public UISlider volumeChangeSli;
    //public AudioSource audioPlayerAS;
    public Texture2D[] textureFiles;

    public enum FestivalState
    {
        normalday,
        hatsuharu,
        tsuyu,
        manatsu,
        aki
    }
    public enum FestivalStateTwo
    {
        normalday_two,
        valentine,
        white_valentine,
        xmas,
        tsuyu2016,
        sanma
    }
    public enum FestivalStateThree
    {
        normalday_three,
        setsubun
    }
    public FestivalState festivalState;
    public FestivalStateTwo festivalStateTwo;
    public FestivalStateThree festivalStateThree;

    public TextAsset[] idleMotionFile;//idle声音列表
    public TextAsset[] idleMotionFileTemp;//储存需要替换的文件，用于节日报时（动画）
    public AudioClip[] ВерныйAudioClipRandomIdle;//idle动画列表
    public AudioClip[] ВерныйAudioClipRandomIdleTemp;//储存需要替换的文件，用于节日报时（声音）

    //节日文件列表
    public TextAsset[] festivalMotionFile;
    public AudioClip[] festivalAudioClip;

    //报时文件列表
    public TextAsset[] timerMotionFile;
    public AudioClip[] ВерныйAudioClipTimer;

    int audioClipID, cuAudioClipID;//用于随机播放动画
    float playTime; //计时器
    public float playRate;//每隔多长时间点击左键可以播放动画

    int hourID;//当前小时的ID，用于播放报时语音
    //bool isPlayed = false;
    //public float myTime = 0;
    //public float myWaitTime = 10;

    void Start()
    {
        Live2D.init();
        load();
        playTime = playRate;
    }

    void load()
    {
        live2DModel = Live2DModelUnity.loadModel(mocFile.bytes);

        for (int i = 0; i < textureFiles.Length; i++)
        {
            live2DModel.setTexture(i, textureFiles[i]);
        }

        float modelWidth = live2DModel.getCanvasWidth();
        live2DCanvasPos = Matrix4x4.Ortho(0, modelWidth, modelWidth, 0, -50.0f, 50.0f);

        if (physicsFile != null) physics = L2DPhysics.load(physicsFile.bytes);

        motionMgr = new MotionQueueManager();
        motionBattle = Live2DMotion.loadMotion(idleMotionFile[0].bytes);
        motionPort = Live2DMotion.loadMotion(idleMotionFile[1].bytes);
        motionSecret = Live2DMotion.loadMotion(idleMotionFile[2].bytes);
        //motionLoop = Live2DMotion.loadMotion(motionFile[3].bytes);
        //motionВерныйOne = Live2DMotion.loadMotion(motionFile[4].bytes);
        //motionВерныйNewYear = Live2DMotion.loadMotion(motionFile[5].bytes);
        //motionTwoHour = Live2DMotion.loadMotion(motionFile[6].bytes);
    }


    void Update()
    {
        //Debug.Log(DateTime.Now);
        Festival();
        Timer();
        //base Behavior & Motion Ctrl
        if (live2DModel == null) load();
        live2DModel.setMatrix(transform.localToWorldMatrix * live2DCanvasPos);
        if (!Application.isPlaying)
        {
            live2DModel.update();
            //live2DModel.draw();
            return;
        }

        //if (ВерныйAS.isPlaying)
        //{
        //    audioPlayerAS.volume = 0.1f;
        //    //audioPlayerAS.mute = true;
        //}
        //else
        //{
        //    audioPlayerAS.volume = 1.0f;
        //    //audioPlayerAS.mute = false;
        //}

        //每隔一段时间随机播放动画
        //playTime += Time.deltaTime;
        //if (playTime >= playRate)
        //{
        //if (motionMgr.isFinished())
        //{
        //    motionMgr.startMotion(Live2DMotion.loadMotion(motionFile[UnityEngine.Random.Range(0, 4)].bytes));
        //    playTime = 0;
        //}
        //}


        var pos = Input.mousePosition;
        if (playTime < playRate) //计时器逻辑
        {
            playTime += Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(0))//点击随机播放动画和对应语音
        {
            if (playTime >= playRate)
            {
                if (motionMgr.isFinished())
                {
                    audioClipID = UnityEngine.Random.Range(0, ВерныйAudioClipRandomIdle.Length);
                    ВерныйAS.clip = ВерныйAudioClipRandomIdle[audioClipID];
                    ВерныйAS.Play();
                    cuAudioClipID = audioClipID;//获取当前随机到的声音ID
                    //Debug.Log(cuAudioClipID);
                    motionMgr.startMotion(Live2DMotion.loadMotion(idleMotionFile[cuAudioClipID].bytes));//播放对应的动画
                    playTime -= playRate;
                }
            }
        }
        //else if (Input.GetMouseButtonDown(1))
        //{
        //    if (motionMgr.isFinished())
        //    {
        //        motionMgr.startMotion(motionBattle);
        //    }
        //}
        else if (Input.GetMouseButton(0))
        {
            dragMgr.Set(pos.x / Screen.width * 2 - 1, pos.y / Screen.height * 2 - 1);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            dragMgr.Set(0, 0);
        }

        dragMgr.update();
        live2DModel.setParamFloat("PARAM_ANGLE_X", dragMgr.getX() * 20);
        live2DModel.setParamFloat("PARAM_ANGLE_Y", dragMgr.getY() * 20);

        live2DModel.setParamFloat("PARAM_BODY_ANGLE_X", dragMgr.getX() * 10);

        live2DModel.setParamFloat("PARAM_EYE_BALL_X", dragMgr.getX() * 20);
        live2DModel.setParamFloat("PARAM_EYE_BALL_Y", dragMgr.getY() * 20);

        live2DModel.setParamFloat("PARAM_ikari", dragMgr.getX());
        live2DModel.setParamFloat("PARAM_ikari", dragMgr.getY());

        live2DModel.setParamFloat("PARAM_HAIR_FRONT", dragMgr.getX());
        live2DModel.setParamFloat("PARAM_HAIR_SIDE", dragMgr.getX());
        live2DModel.setParamFloat("PARAM_HAIR_BACK", dragMgr.getX());

        double timeSec = UtSystem.getUserTimeMSec() / 1000.0;
        double t = timeSec * 2 * Math.PI;
        live2DModel.setParamFloat("PARAM_BREATH", (float)(0.5f + 0.5f * Math.Sin(t / 3.0)));

        eyeBlink.setParam(live2DModel);

        if (physics != null) physics.updateParam(live2DModel);

        live2DModel.update();

        if (live2DModel == null) return;
        live2DModel.setMatrix(transform.localToWorldMatrix * live2DCanvasPos);

        if (!Application.isPlaying)
        {
            live2DModel.update();
            return;
        }

        motionMgr.updateParam(live2DModel);

        live2DModel.update();
        OnRenderObject();
    }


    void OnRenderObject()
    {
        if (live2DModel == null) return;
        live2DModel.draw();
    }

    public void VolumeChange()
    {
        ВерныйAS.volume = volumeChangeSli.value;
    }

    //节日语音
    void Festival()
    {
        DateTime dateTimeNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //初春
        DateTime hatsuharuStartTime = new DateTime(DateTime.Now.Year, 2, 4);
        DateTime hatsuharuEndTime = new DateTime(DateTime.Now.Year, 3, 4);
        //节分
        DateTime setsubunStartTime = new DateTime(DateTime.Now.Year, 1, 25);
        DateTime setsubunEndTime = new DateTime(DateTime.Now.Year, 2, 20);
        //梅雨
        DateTime tsuyuStartTime = new DateTime(DateTime.Now.Year, 6, 1);
        DateTime tsuyuEndTime = new DateTime(DateTime.Now.Year, 7, 15);
        //盛夏
        DateTime manatsuStartTime = new DateTime(DateTime.Now.Year, 8, 1);
        DateTime manatsuEndTime = new DateTime(DateTime.Now.Year, 8, 31);
        //秋季
        DateTime akiStartTime = new DateTime(DateTime.Now.Year, 9, 1);
        DateTime akiEndTime = new DateTime(DateTime.Now.Year, 10, 24);
        //秋刀鱼
        DateTime sanmaStartTime = new DateTime(DateTime.Now.Year, 10, 15);
        DateTime sanmaEndTime = new DateTime(DateTime.Now.Year, 11, 5);
        //情人节
        DateTime valentineStartTime = new DateTime(DateTime.Now.Year, 2, 10);
        DateTime valentineEndTime = new DateTime(DateTime.Now.Year, 2, 14);
        //白色情人节
        DateTime whiteValentineStartTime = new DateTime(DateTime.Now.Year, 3, 10);
        DateTime whiteValentineEndTime = new DateTime(DateTime.Now.Year, 3, 14);
        //圣诞
        DateTime xmasStartTime = new DateTime(DateTime.Now.Year, 12, 24);
        DateTime xmasEndTime = new DateTime(DateTime.Now.Year, 12, 25);
        //Debug.Log("xmasStartTime" + xmasStartTime);
        //Debug.Log("xmasEndTime" + xmasEndTime);
        //Debug.Log("dateTimeNow" + dateTimeNow);

        switch (festivalState)
        {
            case FestivalState.normalday:
                idleMotionFile[1] = idleMotionFileTemp[0];
                ВерныйAudioClipRandomIdle[1] = ВерныйAudioClipRandomIdleTemp[0];
                if (dateTimeNow >= hatsuharuStartTime && dateTimeNow <= hatsuharuEndTime)
                {
                    festivalState = FestivalState.hatsuharu;
                }
                else if (dateTimeNow >= tsuyuStartTime && dateTimeNow <= tsuyuEndTime)
                {
                    festivalState = FestivalState.tsuyu;
                }
                else if (dateTimeNow >= manatsuStartTime && dateTimeNow <= manatsuEndTime)
                {
                    festivalState = FestivalState.manatsu;
                }
                else if (dateTimeNow >= akiStartTime && dateTimeNow <= akiEndTime)
                {
                    festivalState = FestivalState.aki;
                }
                break;
            case FestivalState.hatsuharu:
                //初春
                idleMotionFile[1] = festivalMotionFile[1];
                ВерныйAudioClipRandomIdle[1] = festivalAudioClip[1];
                if (dateTimeNow < hatsuharuStartTime || dateTimeNow > hatsuharuEndTime)
                {
                    festivalState = FestivalState.normalday;
                }
                break;
            case FestivalState.tsuyu:
                idleMotionFile[1] = festivalMotionFile[4];
                ВерныйAudioClipRandomIdle[1] = festivalAudioClip[4];
                if (dateTimeNow < tsuyuStartTime || dateTimeNow > tsuyuEndTime)
                {
                    festivalState = FestivalState.normalday;
                }
                break;
            case FestivalState.manatsu:
                idleMotionFile[1] = festivalMotionFile[6];
                ВерныйAudioClipRandomIdle[1] = festivalAudioClip[6];
                if (dateTimeNow < manatsuStartTime || dateTimeNow > manatsuEndTime)
                {
                    festivalState = FestivalState.normalday;
                }
                break;
            case FestivalState.aki:
                idleMotionFile[1] = festivalMotionFile[7];
                ВерныйAudioClipRandomIdle[1] = festivalAudioClip[7];
                if (dateTimeNow < akiStartTime || dateTimeNow > akiEndTime)
                {
                    festivalState = FestivalState.normalday;
                }
                break;
        }

        switch (festivalStateTwo)
        {
            case FestivalStateTwo.normalday_two:
                idleMotionFile[4] = idleMotionFileTemp[1];
                ВерныйAudioClipRandomIdle[4] = ВерныйAudioClipRandomIdleTemp[1];
                if (dateTimeNow >= valentineStartTime && dateTimeNow <= valentineEndTime)
                {
                    festivalStateTwo = FestivalStateTwo.valentine;
                }
                else if (dateTimeNow >= whiteValentineStartTime && dateTimeNow <= whiteValentineEndTime)
                {
                    festivalStateTwo = FestivalStateTwo.white_valentine;
                }
                else if (dateTimeNow >= xmasStartTime && dateTimeNow <= xmasEndTime)
                {
                    festivalStateTwo = FestivalStateTwo.xmas;
                }
                else if (dateTimeNow >= tsuyuStartTime && dateTimeNow <= tsuyuEndTime)
                {
                    festivalStateTwo = FestivalStateTwo.tsuyu2016;
                }
                else if (dateTimeNow >= sanmaStartTime && dateTimeNow <= sanmaEndTime)
                {
                    festivalStateTwo = FestivalStateTwo.sanma;
                }
                break;
            case FestivalStateTwo.valentine:
                idleMotionFile[4] = festivalMotionFile[2];
                ВерныйAudioClipRandomIdle[4] = festivalAudioClip[2];
                if (dateTimeNow < valentineStartTime || dateTimeNow > valentineEndTime)
                {
                    festivalStateTwo = FestivalStateTwo.normalday_two;
                }
                break;
            case FestivalStateTwo.white_valentine:
                idleMotionFile[4] = festivalMotionFile[3];
                ВерныйAudioClipRandomIdle[4] = festivalAudioClip[3];
                if (dateTimeNow < whiteValentineStartTime || dateTimeNow > whiteValentineEndTime)
                {
                    festivalStateTwo = FestivalStateTwo.normalday_two;
                }
                break;
            case FestivalStateTwo.xmas:
                //圣诞节
                //Debug.Log("圣诞");
                idleMotionFile[4] = festivalMotionFile[0];
                ВерныйAudioClipRandomIdle[4] = festivalAudioClip[0];
                if (dateTimeNow < xmasStartTime || dateTimeNow > xmasEndTime)
                {
                    festivalStateTwo = FestivalStateTwo.normalday_two;
                }
                break;
            case FestivalStateTwo.tsuyu2016:
                idleMotionFile[4] = festivalMotionFile[5];
                ВерныйAudioClipRandomIdle[4] = festivalAudioClip[5];
                if (dateTimeNow < tsuyuStartTime || dateTimeNow > tsuyuEndTime)
                {
                    festivalStateTwo = FestivalStateTwo.normalday_two;
                }
                break;
            case FestivalStateTwo.sanma:
                idleMotionFile[4] = festivalMotionFile[8];
                ВерныйAudioClipRandomIdle[4] = festivalAudioClip[8];
                if (dateTimeNow < sanmaStartTime || dateTimeNow > sanmaEndTime)
                {
                    festivalStateTwo = FestivalStateTwo.normalday_two;
                }
                break;
        }

        switch (festivalStateThree)
        {
            case FestivalStateThree.normalday_three:
                idleMotionFile[7] = idleMotionFileTemp[2];
                ВерныйAudioClipRandomIdle[7] = ВерныйAudioClipRandomIdleTemp[2];
                if (dateTimeNow >= setsubunStartTime && dateTimeNow <= setsubunEndTime)
                {
                    festivalStateThree = FestivalStateThree.setsubun;
                }
                break;
            case FestivalStateThree.setsubun:
                idleMotionFile[7] = festivalMotionFile[9];
                ВерныйAudioClipRandomIdle[7] = festivalAudioClip[9];
                if (dateTimeNow < setsubunStartTime || dateTimeNow > setsubunEndTime)
                {
                    festivalStateThree = FestivalStateThree.normalday_three;
                }
                break;
        }
    }

    void Timer()
    {
        //报时逻辑
        if (DateTime.Now.Hour == 00 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
        {
            hourID = DateTime.Now.Hour;
            ВерныйAS.clip = ВерныйAudioClipTimer[hourID];
            ВерныйAS.Play();
            motionMgr.startMotion(Live2DMotion.loadMotion(timerMotionFile[hourID].bytes));
        }
        if (DateTime.Now.Hour == 01 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
        {
            hourID = DateTime.Now.Hour;
            ВерныйAS.clip = ВерныйAudioClipTimer[hourID];
            ВерныйAS.Play();
            motionMgr.startMotion(Live2DMotion.loadMotion(timerMotionFile[hourID].bytes));
        }
        if (DateTime.Now.Hour == 02 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
        {
            hourID = DateTime.Now.Hour;
            ВерныйAS.clip = ВерныйAudioClipTimer[hourID];
            ВерныйAS.Play();
            motionMgr.startMotion(Live2DMotion.loadMotion(timerMotionFile[hourID].bytes));
        }
        if (DateTime.Now.Hour == 03 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
        {
            motionMgr.startMotion(Live2DMotion.loadMotion(timerMotionFile[hourID].bytes));
            hourID = DateTime.Now.Hour;
            ВерныйAS.clip = ВерныйAudioClipTimer[hourID];
            ВерныйAS.Play();
        }
        if (DateTime.Now.Hour == 04 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
        {
            hourID = DateTime.Now.Hour;
            ВерныйAS.clip = ВерныйAudioClipTimer[hourID];
            ВерныйAS.Play();
            motionMgr.startMotion(Live2DMotion.loadMotion(timerMotionFile[hourID].bytes));
        }
        if (DateTime.Now.Hour == 05 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
        {
            hourID = DateTime.Now.Hour;
            ВерныйAS.clip = ВерныйAudioClipTimer[hourID];
            ВерныйAS.Play();
            motionMgr.startMotion(Live2DMotion.loadMotion(timerMotionFile[hourID].bytes));
        }
        if (DateTime.Now.Hour == 06 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
        {
            hourID = DateTime.Now.Hour;
            ВерныйAS.clip = ВерныйAudioClipTimer[hourID];
            ВерныйAS.Play();
            motionMgr.startMotion(Live2DMotion.loadMotion(timerMotionFile[hourID].bytes));
        }
        if (DateTime.Now.Hour == 07 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
        {
            hourID = DateTime.Now.Hour;
            ВерныйAS.clip = ВерныйAudioClipTimer[hourID];
            ВерныйAS.Play();
            motionMgr.startMotion(Live2DMotion.loadMotion(timerMotionFile[hourID].bytes));
        }
        if (DateTime.Now.Hour == 08 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
        {
            hourID = DateTime.Now.Hour;
            ВерныйAS.clip = ВерныйAudioClipTimer[hourID];
            ВерныйAS.Play();
            motionMgr.startMotion(Live2DMotion.loadMotion(timerMotionFile[hourID].bytes));
        }
        if (DateTime.Now.Hour == 09 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
        {
            hourID = DateTime.Now.Hour;
            ВерныйAS.clip = ВерныйAudioClipTimer[hourID];
            ВерныйAS.Play();
            motionMgr.startMotion(Live2DMotion.loadMotion(timerMotionFile[hourID].bytes));
        }
        if (DateTime.Now.Hour == 10 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
        {
            hourID = DateTime.Now.Hour;
            ВерныйAS.clip = ВерныйAudioClipTimer[hourID];
            ВерныйAS.Play();
            motionMgr.startMotion(Live2DMotion.loadMotion(timerMotionFile[hourID].bytes));
        }
        if (DateTime.Now.Hour == 11 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
        {
            hourID = DateTime.Now.Hour;
            ВерныйAS.clip = ВерныйAudioClipTimer[hourID];
            ВерныйAS.Play();
            motionMgr.startMotion(Live2DMotion.loadMotion(timerMotionFile[hourID].bytes));
        }
        if (DateTime.Now.Hour == 12 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
        {
            hourID = DateTime.Now.Hour;
            ВерныйAS.clip = ВерныйAudioClipTimer[hourID];
            ВерныйAS.Play();
            motionMgr.startMotion(Live2DMotion.loadMotion(timerMotionFile[hourID].bytes));
        }
        if (DateTime.Now.Hour == 13 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
        {
            hourID = DateTime.Now.Hour;
            ВерныйAS.clip = ВерныйAudioClipTimer[hourID];
            ВерныйAS.Play();
            motionMgr.startMotion(Live2DMotion.loadMotion(timerMotionFile[hourID].bytes));
        }
        if (DateTime.Now.Hour == 14 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
        {
            hourID = DateTime.Now.Hour;
            ВерныйAS.clip = ВерныйAudioClipTimer[hourID];
            ВерныйAS.Play();
            motionMgr.startMotion(Live2DMotion.loadMotion(timerMotionFile[hourID].bytes));
        }
        if (DateTime.Now.Hour == 15 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
        {
            hourID = DateTime.Now.Hour;
            ВерныйAS.clip = ВерныйAudioClipTimer[hourID];
            ВерныйAS.Play();
            motionMgr.startMotion(Live2DMotion.loadMotion(timerMotionFile[hourID].bytes));
        }
        if (DateTime.Now.Hour == 16 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
        {
            hourID = DateTime.Now.Hour;
            ВерныйAS.clip = ВерныйAudioClipTimer[hourID];
            ВерныйAS.Play();
            motionMgr.startMotion(Live2DMotion.loadMotion(timerMotionFile[hourID].bytes));
        }
        if (DateTime.Now.Hour == 17 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
        {
            hourID = DateTime.Now.Hour;
            ВерныйAS.clip = ВерныйAudioClipTimer[hourID];
            ВерныйAS.Play();
            motionMgr.startMotion(Live2DMotion.loadMotion(timerMotionFile[hourID].bytes));
        }
        if (DateTime.Now.Hour == 18 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
        {
            hourID = DateTime.Now.Hour;
            ВерныйAS.clip = ВерныйAudioClipTimer[hourID];
            ВерныйAS.Play();
            motionMgr.startMotion(Live2DMotion.loadMotion(timerMotionFile[hourID].bytes));
        }
        if (DateTime.Now.Hour == 19 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
        {
            hourID = DateTime.Now.Hour;
            ВерныйAS.clip = ВерныйAudioClipTimer[hourID];
            ВерныйAS.Play();
            motionMgr.startMotion(Live2DMotion.loadMotion(timerMotionFile[hourID].bytes));
        }
        if (DateTime.Now.Hour == 20 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
        {
            hourID = DateTime.Now.Hour;
            ВерныйAS.clip = ВерныйAudioClipTimer[hourID];
            ВерныйAS.Play();
            motionMgr.startMotion(Live2DMotion.loadMotion(timerMotionFile[hourID].bytes));
        }
        if (DateTime.Now.Hour == 21 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
        {
            hourID = DateTime.Now.Hour;
            ВерныйAS.clip = ВерныйAudioClipTimer[hourID];
            ВерныйAS.Play();
            motionMgr.startMotion(Live2DMotion.loadMotion(timerMotionFile[hourID].bytes));
        }
        if (DateTime.Now.Hour == 22 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
        {
            hourID = DateTime.Now.Hour;
            ВерныйAS.clip = ВерныйAudioClipTimer[hourID];
            ВерныйAS.Play();
            motionMgr.startMotion(Live2DMotion.loadMotion(timerMotionFile[hourID].bytes));
        }
        if (DateTime.Now.Hour == 23 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
        {
            hourID = DateTime.Now.Hour;
            ВерныйAS.clip = ВерныйAudioClipTimer[hourID];
            ВерныйAS.Play();
            motionMgr.startMotion(Live2DMotion.loadMotion(timerMotionFile[hourID].bytes));
        }
    }
}