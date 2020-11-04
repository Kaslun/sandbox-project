using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class SceneLoader : MonoBehaviour
    {

        public void LoadScene(int index)
        {
            SceneManager.LoadScene(index);
        }

        public void LoadSceneAsync(int index)
        {     
            SceneManager.LoadSceneAsync(index);
        }

        public void LoadSceneAdditive(int index)
        {
            SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        }
    }
}
