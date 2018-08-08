using UnityEngine;
using System.Collections;
using Reclamation.Misc;

namespace Reclamation.World
{
    public class DayNightManager : Singleton<DayNightManager>
    {
        public Light sun;
        [Range(0, 1)]
        public float currentTimeOfDay = 0;

        //float sunInitialIntensity;

        void Start()
        {
            //sunInitialIntensity = sun.intensity;
        }

        void Update()
        {
            currentTimeOfDay = ((TimeManager.instance.CurrentTime.Hour * 60) + TimeManager.instance.CurrentTime.Minute) / 1440f;

            if (currentTimeOfDay > 1)
            {
                currentTimeOfDay = 0;
            }

            UpdateSun();
        }

        void UpdateSun()
        {
            sun.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, 170, 0);

            //float intensityMultiplier = 1;
            if (currentTimeOfDay <= 0.23f || currentTimeOfDay >= 0.75f)
            {
                //intensityMultiplier = 0;
            }
            else if (currentTimeOfDay <= 0.25f)
            {
                //intensityMultiplier = Mathf.Clamp01((currentTimeOfDay - 0.23f) * (1 / 0.02f));
            }
            else if (currentTimeOfDay >= 0.73f)
            {
                //intensityMultiplier = Mathf.Clamp01(1 - ((currentTimeOfDay - 0.73f) * (1 / 0.02f)));
            }

            //sun.intensity = sunInitialIntensity * intensityMultiplier;

            //RenderSettings.ambientIntensity = (intensityMultiplier + 1) - 0.75f;
        }
    }
}