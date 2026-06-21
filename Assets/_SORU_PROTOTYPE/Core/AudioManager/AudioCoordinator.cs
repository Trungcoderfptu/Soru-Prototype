using System;
using UnityEngine;
using SoruPrototype.Core;

namespace SoruPrototype.Audio
{
    [Serializable]
    public struct StateBGMMapping
    {
        public GameState state;
        public string bgmID;
    }

    public class AudioCoordinator : MonoBehaviour
    {
        [Header("BGM Điều phối theo Trạng thái")]
        [Tooltip("Khai báo trạng thái nào thì phát bài nhạc nào ở đây")]
        [SerializeField] private StateBGMMapping[] stateBGMMappings;

        private void OnEnable()
        {
            // 1. Lắng nghe ĐÚNG MỘT sự kiện cốt lõi duy nhất từ GameManager
            EventManager.StartListening("OnGameStateChanged", SyncBGMWithState);
            
            // 2. Lắng nghe UI
            EventManager.StartListening("UI_Click", PlayClickSound);
            EventManager.StartListening("UI_Hover", PlayHoverSound);
        }

        private void OnDisable()
        {
            EventManager.StopListening("OnGameStateChanged", SyncBGMWithState);
            EventManager.StopListening("UI_Click", PlayClickSound);
            EventManager.StopListening("UI_Hover", PlayHoverSound);
        }

        private void Start()
        {
            // TỰ ĐỘNG ĐỒNG BỘ LÚC MỞ GAME:
            
            SyncBGMWithState();
        }

        /// <summary>
        /// Hàm này tự động chạy mỗi khi GameManager báo có sự thay đổi.
        /// Cơ chế hoạt động giống hệt SceneStateController.
        /// </summary>
        private void SyncBGMWithState()
        {
            if (AudioManager.Instance == null || GameManager.Instance == null) return;

            // Chủ động lấy State hiện tại từ Core
            GameState currentState = GameManager.Instance.CurrentState;

            // Bỏ qua nếu là State UI đè lên (Setting)
            if (currentState == GameState.Setting) return;

            // Đối chiếu State với cấu hình trong Inspector để phát nhạc
            foreach (var mapping in stateBGMMappings)
            {
                if (mapping.state == currentState)
                {
                    // Trả quyền điều khiển nhạc cho hệ thống Visual Novel (Yarn Spinner)
                    if (currentState == GameState.VisualNovel)
                    {
                        AudioManager.Instance.StopBGM();
                        return;
                    }

                    AudioManager.Instance.PlayBGM(mapping.bgmID);
                    return;
                }
            }
        }

        // --- CÁC HÀM XỬ LÝ SFX ---
        private void PlayClickSound()
        {
            if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("sfx_ui_click");
        }

        private void PlayHoverSound()
        {
            if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("sfx_ui_hover");
        }
    }
}

/* ==============================================================================
 * [TODO] NÂNG CẤP HỆ THỐNG PLAYLIST (PHÁT NHIỀU BÀI NHẠC LIÊN TIẾP)
 * * 1. Đổi cấu trúc dữ liệu: 
 * - Chuyển 'string bgmID' hiện tại thành mảng hoặc danh sách 'List<string> bgmPlaylist'.
 * * 2. Logic chuyển bài (Nằm gọn trong AudioManager):
 * - Thêm một biến theo dõi tiến trình bài hát.
 * - Dùng Coroutine hoặc Update() để kiểm tra: Nếu (!bgmSource.isPlaying) 
 * -> Tự động lấy ID tiếp theo trong bgmPlaylist để phát.
 * * 3. Bảo vệ Kiến trúc (Quan trọng):
 * - Toàn bộ logic đếm thời gian và đổi bài chỉ nằm ở hệ thống Audio.
 * - KHÔNG được thêm bất kỳ code xử lý âm thanh nào vào UI hay GameState 
 * để đảm bảo tính Đóng gói (Encapsulation) và chống Hardcode.
 * ============================================================================== */