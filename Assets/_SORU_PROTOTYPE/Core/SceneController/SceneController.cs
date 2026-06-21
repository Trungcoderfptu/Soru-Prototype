using UnityEngine;
using UnityEngine.SceneManagement;

namespace SoruPrototype.Core
{
    public class SceneController : MonoBehaviour
    {
        /// <summary>
        /// Nạp một Scene vào bộ nhớ theo chế độ Additive (không hủy các Scene đang chạy).
        /// </summary>
        /// <param name="sceneName">Tên Scene cần nạp.</param>
        public void LoadSceneAdditive(string sceneName)
        {
            Debug.Log($"<color=green>[SceneController]</color> Loading Additive Scene: {sceneName}");
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        /// <summary>
        /// Gỡ bỏ một Scene khỏi bộ nhớ để giải phóng tài nguyên.
        /// </summary>
        /// <param name="sceneName">Tên Scene cần gỡ.</param>
        public void UnloadSceneAdditive(string sceneName)
        {
            Debug.Log($"<color=orange>[SceneController]</color> Unloading Scene: {sceneName}");
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }
}