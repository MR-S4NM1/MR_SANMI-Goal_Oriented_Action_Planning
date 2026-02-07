using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dante {
	public class ButtonsBehaviour : MonoBehaviour
	{
		#region PublicMethods
		
		public void ReloadCurrentLevel() {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

        public void LoadScene(int p_sceneId) {
            SceneManager.LoadScene(p_sceneId);
        }

        public void QuitGame() {
			Application.Quit();
		}
		
		#endregion
	}
}
