using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

namespace Managers
{
    public class PlayStateManager : MonoSingleton<PlayStateManager>
    {
        // Start is called before the first frame update
        public float TraverseTime = 10f;
        public float SongTime = 20f;

        private bool musicPlaying;

    

        public delegate void SceneChange();

        public static event SceneChange SceneChangingToTraverse;

        public static event SceneChange SceneChangingToMusic;

        private void Start()
        {
    

            DontDestroyOnLoad(this.gameObject);
            Starship.playerDied += PlayerDead;
            //StartCoroutine(SceneAutoTransition());
        
        }

        private IEnumerator SceneAutoTransition()
        {
            Debug.Log($"MusicPlaying: {musicPlaying}");


            yield return new WaitForSeconds((musicPlaying) ? SongTime : TraverseTime);



            if (musicPlaying)
            {
                SceneChangingToTraverse?.Invoke();
                SceneManager.LoadScene(0);
                SceneManager.UnloadSceneAsync(1);
                musicPlaying = false;
            }
            else
            {
                SceneChangingToMusic?.Invoke();
                yield return  new WaitForSeconds(2);
                SceneManager.LoadScene(1);
                SceneManager.UnloadSceneAsync(0);
                musicPlaying = true;
            }
            

            StartCoroutine(SceneAutoTransition());
        }

        public void StartGame()
        {
            SceneManager.LoadScene("OpenSpace");
            GameManager.Instance.SetPlayer();
            
        }


        private void PlayerDead()
        {
            StartCoroutine("WaitForDeath");
        }


        IEnumerator WaitForDeath()
        {
            yield return new WaitForSeconds(7f);
            SceneManager.LoadScene("StartMenu");
        }
    }
}

