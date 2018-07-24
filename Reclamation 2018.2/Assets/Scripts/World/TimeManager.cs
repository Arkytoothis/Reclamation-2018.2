using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class TimeManager : Singleton<TimeManager>
{
    public enum WorldSpeed { Slowest = 1, Slow = 6, Normal = 12, Fast = 24, Fastest = 60 }

    int lastWeek = 0;
    public TimeData CurrentTime;
    public static TimeData GameStartTime;

    bool paused = false;
    public WorldSpeed CurrentSpeed = WorldSpeed.Normal;
    public WorldSpeed LastSpeed = WorldSpeed.Normal;

    public float speedMultiplier = 0.1f;
    public TMP_Text CurrentTimeLabel;

    public bool IsPaused { get { return paused; } }
    public bool displayShort = true;

    void Awake()
    {
        Reload();
        CurrentTime = new TimeData(1000, 1, 1, 1, 7, 30, 0);
        GameStartTime = new TimeData(CurrentTime);
    }

    void Update()
    {
        if (paused == false)
        {
            CurrentTime.Second += (int)CurrentSpeed;

            if (CurrentTime.Second >= 60)
            {
                CurrentTime.Second = 0;
                CurrentTime.Minute += 1;
                NewMinute();
            }

            if (CurrentTime.Minute >= 60)
            {
                CurrentTime.Minute = 0;
                CurrentTime.Hour++;
                NewHour();
            }

            if (CurrentTime.Hour >= 24)
            {
                CurrentTime.Hour = 0;
                CurrentTime.Day++;
                NewDay();
            }

            CurrentTime.Week = (CurrentTime.Day / 7) + 1;

            if (CurrentTime.Week == 5) CurrentTime.Week = 1;

            if (CurrentTime.Week != lastWeek)
                NewWeek();

            if (CurrentTime.Day > 28)
            {
                CurrentTime.Day = 1;
                CurrentTime.Month++;
                NewMonth();
            }

            if (CurrentTime.Month > 12)
            {
                CurrentTime.Month = 1;
                CurrentTime.Year++;
                NewYear();
            }

            lastWeek = CurrentTime.Week;
        }

        if(displayShort == true)
            CurrentTimeLabel.text = CurrentTime.Month + "/" + CurrentTime.Day + "/" + CurrentTime.Year + ", " + CurrentTime.Hour + ":" + CurrentTime.Minute;
        else
            CurrentTimeLabel.text = "Year " + CurrentTime.Year + "<pos=20%>Month " + CurrentTime.Month + "<pos=40%>Day " + CurrentTime.Day + "<pos=60%>Hour " + CurrentTime.Hour;
    }

    void NewSecond()
    {
        //DayNightManager.instance.UpdateTime();
    }

    void NewMinute()
    {
    }

    void NewHour()
    {
        //StrongholdManager.Instance.UpdateTime();
        //ResearchManager.Instance.UpdateTime();
        //BuildQueueManager.Instance.UpdateTime();
        //EventManager.Instance.UpdateEvents();
        //WorldMapManager.Instance.UpdateSites();
    }

    void NewDay()
    {
        //StockpileManager.Instance.TickDay();
        //EventManager.Instance.CheckForEventSpawn();
    }

    void NewWeek()
    {
        //StockpileManager.Instance.TickWeek();
        //PlayerManager.Instance.RefreshNewPcPool();
    }

    void NewMonth()
    {
    }

    void NewYear()
    {
    }

    public void SetSpeed(WorldSpeed speed)
    {
        //AudioManager.Instance.PlayChangeSpeed();
        CurrentSpeed = speed;

        switch (CurrentSpeed)
        {
            case WorldSpeed.Slowest:
                SetSlowest();
                break;
            case WorldSpeed.Slow:
                SetSlow();
                break;
            case WorldSpeed.Normal:
                SetNormal();
                break;
            case WorldSpeed.Fast:
                SetFast();
                break;
            case WorldSpeed.Fastest:
                SetFastest();
                break;
            default:
                break;
        }
    }

    public void TogglePause()
    {
        //AudioManager.Instance.PlayChangeSpeed();
        if (paused == true)
            Unpause();
        else
            Pause();
    }

    public void Pause()
    {
        paused = true;
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        paused = false;
        SetSpeed(LastSpeed);
    }

    public void SetSlowest()
    {
        //AudioManager.Instance.PlayDefaultClick();
        paused = false;
        CurrentSpeed = WorldSpeed.Slowest;
        LastSpeed = CurrentSpeed;
        Time.timeScale = 0.2f;
    }

    public void SetSlow()
    {
        //AudioManager.Instance.PlayDefaultClick();
        paused = false;
        CurrentSpeed = WorldSpeed.Slow;
        LastSpeed = CurrentSpeed;
        Time.timeScale = 0.5f;
    }

    public void SetNormal()
    {
        //AudioManager.Instance.PlayDefaultClick();
        paused = false;
        CurrentSpeed = WorldSpeed.Normal;
        LastSpeed = CurrentSpeed;
        Time.timeScale = 1f;
    }

    public void SetFast()
    {
        //AudioManager.Instance.PlayDefaultClick();
        paused = false;
        CurrentSpeed = WorldSpeed.Fast;
        LastSpeed = CurrentSpeed;
        Time.timeScale = 2f;
    }

    public void SetFastest()
    {
        //AudioManager.Instance.PlayDefaultClick();
        paused = false;
        CurrentSpeed = WorldSpeed.Fastest;
        LastSpeed = CurrentSpeed;
        Time.timeScale = 4f;
    }
}