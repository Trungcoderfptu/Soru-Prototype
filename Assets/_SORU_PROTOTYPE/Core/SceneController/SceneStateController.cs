using System;
using UnityEngine;

namespace SoruPrototype.Core
{
    /// <summary>
    /// Struct hỗ trợ ánh xạ trạng thái game với tên Scene tương ứng trong Inspector.
    /// </summary>
    [Serializable]
    public struct StateSceneMapping
    {
        public GameState state;
        public string sceneName;
    }

    public class SceneStateController : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Tham chiếu đến SceneController để gọi lệnh load/unload.")]
        [SerializeField] private SceneController sceneController;

        [Header("Configuration")]
        [Tooltip("Danh sách cấu hình ánh xạ: Trạng thái nào -> Load Scene nào.")]
        [SerializeField] private StateSceneMapping[] sceneMappings;

        // Lưu lại tên Scene đang được bật để biết đường gỡ nó ra khi đổi State
        private string currentActiveScene = string.Empty;

        private void OnEnable()
        {
            // Bật tai nghe: Lắng nghe sự kiện đổi state từ GameManager
            EventManager.StartListening("OnGameStateChanged", SyncSceneWithState);
        }

        private void OnDisable()
        {
            // Tắt tai nghe: Hủy lắng nghe để tránh rò rỉ bộ nhớ
            EventManager.StopListening("OnGameStateChanged", SyncSceneWithState);
        }

        /// <summary>
        /// Hàm này tự động chạy mỗi khi GameManager hét lên "OnGameStateChanged".
        /// </summary>
        private void SyncSceneWithState()
        {
            // Lấy State mới nhất từ GameManager
            GameState newState = GameManager.Instance.CurrentState;
            string sceneToLoad = string.Empty;

            // Tra cứu xem State này tương ứng với Scene nào
            foreach (var mapping in sceneMappings)
            {
                if (mapping.state == newState)
                {
                    sceneToLoad = mapping.sceneName;
                    break;
                }
            }

            // Nếu State mới không liên kết với Scene nào, hoặc trùng Scene cũ thì bỏ qua
            if (string.IsNullOrEmpty(sceneToLoad) || sceneToLoad == currentActiveScene) return;

            // 1. Dọn dẹp Scene cũ
            if (!string.IsNullOrEmpty(currentActiveScene))
            {
                sceneController.UnloadSceneAdditive(currentActiveScene);
            }

            // 2. Nạp Scene mới và cập nhật bản ghi
            sceneController.LoadSceneAdditive(sceneToLoad);
            currentActiveScene = sceneToLoad;
        }
    }
}