namespace Curiologix
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class TimerForSpin : MonoBehaviour
    {
        public Text timerText;
        private static TimeSpan nextFreeTurn = new TimeSpan(0, 0, 30);
        TimeSpan remainingTime;
        public const string TIMER_KEY = "TIMER_KEY";
        private DateTime timeStamp;
        public GameObject Spinbttn, WheelLights,WheelGrey;

        private void OnEnable()
        {
            DateTime.TryParse(PlayerPrefs.GetString(TIMER_KEY, DateTime.Now.ToString()), out timeStamp);
        }
        void Update()
        {
            TimeSpan t = DateTime.Now - timeStamp;
            try
            {
                remainingTime = nextFreeTurn - t;
                if (remainingTime.Seconds < 1)
                {
                    timerText.text = "SPIN";
                    ActivateFreeSpin();
                }
                else
                {
                    timerText.text = string.Format("{0:D1}:{1:D2}:{2:D2}",
                        remainingTime.Hours, remainingTime.Minutes, remainingTime.Seconds);
                }
                if (remainingTime.TotalMinutes <= 0)
                {
                  //  ActivateFreeSpin();
                }
            }
            catch (Exception e)
            {
                ActivateFreeSpin();
                print(e.StackTrace);
            }
        }

        public void ActivateFreeSpin()
        {
          //  timeStamp = DateTime.Now;
            Spinbttn.SetActive(true);
            WheelLights.SetActive(true);
            WheelGrey.SetActive(false);
           
        }
        public void Spinpress()
        {
            gameObject.GetComponent<Text>().enabled = false;
        }
        public void DefaultSettings()
        {
            timeStamp = DateTime.Now;
            Spinbttn.SetActive(false);
            WheelLights.SetActive(false);
            WheelGrey.SetActive(true);
            gameObject.GetComponent<Text>().enabled = true;
        }
        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                PlayerPrefs.SetString(TIMER_KEY, timeStamp.ToString());
                PlayerPrefs.Save();
            }
        }
    }
}