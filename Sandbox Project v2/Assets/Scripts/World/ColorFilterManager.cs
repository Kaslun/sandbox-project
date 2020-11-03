using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Manager
{
    public class ColorFilterManager : MonoBehaviour
    {
        public PostProcessVolume blueVolume;
        public PostProcessVolume greenVolume;
        public PostProcessVolume volume;
        public ColorGrading colorGrading;

        public void Start()
        {
            UpdateColorFilter();
        }
        public void UpdateColorFilter(SplineParameter newSpline)
        {
            colorGrading = volume.profile.GetSetting<ColorGrading>();

            colorGrading.hueVsSatCurve.value = AddSplineCurves(colorGrading.hueVsSatCurve, newSpline);
        }

        public static SplineParameter AddSplineCurves(SplineParameter spline1, SplineParameter spline2)
        {
            SplineParameter newCurve;

            newCurve = spline1.value + spline2.value;

            return newCurve;
        }
    }
}
