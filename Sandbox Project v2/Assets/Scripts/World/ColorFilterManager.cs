using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Manager
{
    public class ColorFilterManager : MonoBehaviour
    {
        public PostProcessVolume volume;

        private ColorGrading colorGrading;
        private AnimationCurve cCurve;

        private void Start()
        {
            volume = FindObjectOfType<PostProcessVolume>();
            colorGrading = volume.profile.GetSetting<ColorGrading>();
            cCurve = colorGrading.hueVsSatCurve.value.curve;
        }

        public void UpdateColorFilter(AnimationCurve colorCurve)
        {
            print("CCurve lenght: " + cCurve.length);
            print("ColorCurve lenght: " + colorCurve.length);

            //Moves previous keys to new positions, making a fluid and editable color filter
            //CHEAT
            while (cCurve.length > 0)
            {
                for (int i = 0; i < cCurve.length; i++)
                {
                    print("int: " + i);
                    cCurve.RemoveKey(i);
                }
            }

            foreach(Keyframe k in colorCurve.keys)
            {
                cCurve.AddKey(k);
            }
        }
    }
}
