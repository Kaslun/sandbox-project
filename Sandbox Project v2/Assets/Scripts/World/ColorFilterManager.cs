using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Manager
{
    public class ColorFilterManager : MonoBehaviour
    {
        public VolumeProfile profile;
        private ColorCurves colorGrading;
        private ColorCurves tmp;
        public VolumeComponent vComp;
        public TextureCurve textCurve;

        private void Start()
        {
            if(profile.TryGet(out tmp))
            {
                colorGrading = tmp;
            }
        }

        public void UpdateColorFilter(AnimationCurve colorCurve)
        {
            TextureCurve cCurve = colorGrading.hueVsSat.value;
            TextureCurve newCurve = new TextureCurve(colorCurve, 0, false, Vector2.zero);
            colorGrading.hueVsSat.Interp(cCurve, newCurve, Time.deltaTime);
        }
    }
}
