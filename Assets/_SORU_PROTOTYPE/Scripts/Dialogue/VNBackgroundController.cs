using System;
using UnityEngine;
using UnityEngine.UI;
using SoruPrototype.Core;

namespace SoruPrototype.Dialogue
{
    // Struct giúp cấu hình danh sách ảnh trực tiếp trên Inspector (Giống cách làm của AudioManager)
    [Serializable]
    public struct BackgroundProfile
    {
        public string bgID;       // Tên ID gọi từ Yarn (Ví dụ: "ktx_room", "cafe_shop")
        public Sprite bgSprite;   // File ảnh thật tương ứng
    }

    public class VNBackgroundController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image backgroundImage; // Component Image dùng để hiển thị nền

        [Header("Background Library")]
        [SerializeField] private BackgroundProfile[] bgLibrary; // Thư viện chứa ảnh

        private void OnEnable()
        {
            // Bật bộ đàm, lắng nghe tín hiệu có chứa data (string)
            EventManager.StartListening("UI_SetBackground", ChangeBackground);
        }

        private void OnDisable()
        {
            // Tắt bộ đàm
            EventManager.StopListening("UI_SetBackground", ChangeBackground);
        }

        // Hàm này sẽ tự động chạy khi YarnAdapter gọi TriggerEvent
        private void ChangeBackground(string incomingBgID)
        {
            if (backgroundImage == null) return;

            // Dò tìm trong thư viện xem có ảnh nào khớp ID không
            foreach (var profile in bgLibrary)
            {
                if (profile.bgID == incomingBgID)
                {
                    // Lắp ảnh mới vào và hiển thị
                    backgroundImage.sprite = profile.bgSprite;
                    backgroundImage.color = Color.white; // Đảm bảo ảnh không bị trong suốt
                    
                    Debug.Log($"<color=green>[VNBackground]</color> Đã thay nền thành công: {incomingBgID}");
                    return;
                }
            }

            Debug.LogWarning($"<color=red>[VNBackground]</color> Không tìm thấy ảnh nền nào có ID là: {incomingBgID} trong Library!");
        }
    }
}