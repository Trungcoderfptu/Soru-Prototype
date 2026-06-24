using System;
using UnityEngine;
using UnityEngine.UI;
using SoruPrototype.Core;

namespace SoruPrototype.Dialogue
{
    // Struct lưu cấu hình biểu cảm của nhân vật
    [Serializable]
    public struct CharacterEmotion
    {
        public string emotionID; // Ví dụ: "normal", "smile", "angry"
        public Sprite portraitSprite;
    }

    // Struct đại diện cho 1 nhân vật (chứa nhiều biểu cảm)
    [Serializable]
    public struct CharacterProfile
    {
        public string charID; // Ví dụ: "Soru", "Vuong", "Trung"
        public CharacterEmotion[] emotions;
    }

    public class VNPortraitController : MonoBehaviour
    {
        [Header("UI References (Vị trí đứng)")]
        [SerializeField] private Image leftPortrait;
        [SerializeField] private Image centerPortrait;
        [SerializeField] private Image rightPortrait;

        [Header("Character Library")]
        [SerializeField] private CharacterProfile[] characterLibrary;

        private void OnEnable()
        {
            EventManager.StartListening("UI_ShowCharacter", OnShowCharacterReceived);
            EventManager.StartListening("UI_HideCharacter", OnHideCharacterReceived);
            EventManager.StartListening("UI_HideAllCharacters", OnHideAllCharactersReceived);
        }

        private void OnDisable()
        {
            EventManager.StopListening("UI_ShowCharacter", OnShowCharacterReceived);
            EventManager.StopListening("UI_HideCharacter", OnHideCharacterReceived);
            EventManager.StopListening("UI_HideAllCharacters", OnHideAllCharactersReceived);
        }

        private void OnShowCharacterReceived(string payload)
        {
            // Tách cái chuỗi "Soru,normal,right" thành 1 mảng 3 phần tử
            string[] data = payload.Split(',');
            if (data.Length < 3) return;

            string charID = data[0];
            string emotionID = data[1];
            string position = data[2].ToLower();

            // 1. Tìm ảnh nhân vật trong thư viện
            Sprite foundSprite = GetPortraitSprite(charID, emotionID);
            if (foundSprite == null) return;

            // 2. Xác định xem nên gán ảnh vào vị trí nào trên màn hình
            Image targetImage = GetTargetImage(position);
            if (targetImage != null)
            {
                targetImage.sprite = foundSprite;
                targetImage.color = Color.white; // Hiển thị ảnh (tắt trong suốt)
            }
        }

        // [ADD] Hàm xử lý khi bị yêu cầu ẩn 1 vị trí
        private void OnHideCharacterReceived(string position)
        {
            Image targetImage = GetTargetImage(position);
            if (targetImage != null)
            {
                // Đổi màu thành trong suốt hoàn toàn (Alpha = 0)
                targetImage.color = new Color(1, 1, 1, 0); 
            }
        }

        // [ADD] Hàm xử lý khi bị yêu cầu dọn dẹp toàn bộ
        private void OnHideAllCharactersReceived()
        {
            if (leftPortrait != null) leftPortrait.color = new Color(1, 1, 1, 0);
            if (centerPortrait != null) centerPortrait.color = new Color(1, 1, 1, 0);
            if (rightPortrait != null) rightPortrait.color = new Color(1, 1, 1, 0);
        }

        // --- Các hàm hỗ trợ ẩn bớt logic cho gọn ---

        private Sprite GetPortraitSprite(string charID, string emotionID)
        {
            foreach (var character in characterLibrary)
            {
                if (character.charID == charID)
                {
                    foreach (var emotion in character.emotions)
                    {
                        if (emotion.emotionID == emotionID) return emotion.portraitSprite;
                    }
                }
            }
            Debug.LogWarning($"<color=red>[VNPortrait]</color> Không tìm thấy ảnh: {charID} - {emotionID}");
            return null;
        }

        private Image GetTargetImage(string position)
        {
            switch (position)
            {
                case "left": return leftPortrait;
                case "center": return centerPortrait;
                case "right": return rightPortrait;
                default:
                    Debug.LogWarning($"<color=red>[VNPortrait]</color> Vị trí không hợp lệ: {position}");
                    return null;
            }
        }
    }
}