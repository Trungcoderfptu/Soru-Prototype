using UnityEngine;
namespace SoruPrototype.Core
{
    public class GameManager : MonoBehaviour
    {
        // Singleton Instance để các script khác dễ dàng gọi: GameManager.Instance
        public static GameManager Instance { get; private set; }

        [Header("System Settings")]
        [SerializeField] private GameState currentState = GameState.MainMenu;
        
        // Thuộc tính để các hệ thống khác chỉ đọc (Read-only) trạng thái hiện tại
        public GameState CurrentState => currentState;

        private void Awake()
        {
            // Đảm bảo chỉ có một GameManager duy nhất, không bị trùng lặp khi đổi Scene
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            transform.SetParent(null);
            // Giữ GameManager sống sót xuyên suốt các Scene của game
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            // Khi vừa mở game, đặt trạng thái mặc định là MainMenu
            ChangeState(GameState.MainMenu);
            EventManager.TriggerEvent("OnGameStateChanged");
        }

        /// <summary>
        /// Hàm cốt lõi để chuyển đổi trạng thái game
        /// </summary>
        public void ChangeState(GameState newState)
        {
            // Nếu trạng thái mới trùng với trạng thái cũ thì bỏ qua
            if (currentState == newState) return;

            currentState = newState;
            Debug.Log($"<color=cyan>[GameManager]</color> Trạng thái game đổi thành: <b>{currentState}</b>");

            // Kích hoạt EventManager của bạn để phát tin cho toàn bộ Game nghe thấy
            EventManager.TriggerEvent("OnGameStateChanged");
        }
    }
}