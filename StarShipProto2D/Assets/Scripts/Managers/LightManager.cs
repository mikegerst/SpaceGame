using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Managers
{
    public class LightManager : MonoBehaviour
    {

        public GameObject light2D;

        GameObject copyLight;


        int lastBeat = 1;

        private void Start()
        {
            copyLight = light2D;
        }
        // Update is called once per frame
        void Update()
        {
            if (lastBeat != FModManager.beat)
            {
                CreateNewLight();
                lastBeat = FModManager.beat;
            }
        }

        void CreateNewLight()
        {
            var light = Instantiate(light2D);
            light.GetComponent<Light2D>().color = Random.ColorHSV();

        }

    }
}
