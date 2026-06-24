using UnityEngine;
using Yarn.Unity;
using SoruPrototype.Core;

namespace SoruPrototype.Dialogue
{
    public class YarnAdapter : MonoBehaviour
    {
        // Thẻ [YarnCommand] giúp Yarn Spinner nhận diện hàm này để gọi từ kịch bản
        [YarnCommand("trigger_event")]
        public static void TriggerGameEvent(string eventName)
        {
            Debug.Log($"<color=yellow>[YarnAdapter]</color> Kịch bản Yarn yêu cầu gọi sự kiện: <b>{eventName}</b>");

            //Gọi EventManager hàm triggerevent tại trạm phát sóng eventManager
            EventManager.TriggerEvent(eventName);


        }

        [YarnCommand("change_state")]
        public static void ChangeGameState(string stateName)
        {
            // Cỗ máy quét mã vạch (Enum.TryParse) hoạt động tại đây
            if (System.Enum.TryParse(stateName, out GameState newState))
            {
                Debug.Log($"<color=yellow>[YarnAdapter]</color> Kịch bản Yarn yêu cầu Đạo diễn chuyển cảnh sang: <b>{newState}</b>");

                // Cổng quét thẻ hợp lệ -> Gọi thẳng GameManager
                GameManager.Instance.ChangeState(newState);
            }
            else
            {
                // debug log
                Debug.LogError($"<color=red>[YarnAdapter] LỖI CÚ PHÁP:</color> Trạng thái <b>{stateName}</b> không tồn tại trong hệ thống GameState! Hãy kiểm tra lại file kịch bản.");
            }
        }

        //Phat Nhạc
        [YarnCommand("play_bgm")]
        public static void PlayBackgroundMusic(string bgmID)
        {
            if (AudioManager.Instance != null)
            {
                Debug.Log($"<color=magenta>[YarnAdapter]</color> Kịch bản yêu cầu phát nhạc BGM: <b>{bgmID}</b>");
                AudioManager.Instance.PlayBGM(bgmID);
            }
            else
            {
                Debug.LogWarning("<color=red>[YarnAdapter]</color> Không tìm thấy AudioManager trong Scene!");
            }
        }
        
        [YarnCommand("stop_bgm")]
        public static void StopBackgroundMusic()
        {
            if (AudioManager.Instance != null)
            {
                Debug.Log("<color=magenta>[YarnAdapter]</color> Kịch bản yêu cầu TẮT nhạc BGM");
                AudioManager.Instance.StopBGM();
            }
        }
        // [ADD] Lệnh đổi ảnh nền từ file Yarn
        // Cú pháp trong file .yarn: <<set_bg my_background_id>>
        [YarnCommand("set_bg")]
        public static void SetBackground(string bgID)
        {
            Debug.Log($"<color=cyan>[YarnAdapter]</color> Kịch bản yêu cầu đổi Background thành: <b>{bgID}</b>");
            
            // Gọi Trạm phát sóng, phát tín hiệu "UI_SetBackground" và đính kèm gói hàng là chuỗi bgID
            EventManager.TriggerEvent("UI_SetBackground", bgID);
        }
        //[ADD] Lệnh đổi avata nhân vật
        // Cú pháp trong file .yarn: <<show_char Soru normal right>>
        [YarnCommand("show_char")]
        public static void ShowCharacter(string charName, string emotion, string position)
        {
            // Ghép 3 thông tin thành 1 chuỗi, phân cách bằng dấu phẩy. Ví dụ: "Soru,normal,right"
            string payload = $"{charName},{emotion},{position}";
            
            Debug.Log($"<color=cyan>[YarnAdapter]</color> Kịch bản yêu cầu hiện nhân vật: <b>{charName}</b> (Mặt: {emotion}) ở vị trí: <b>{position}</b>");
            
            // Phát sóng cho UI
            EventManager.TriggerEvent("UI_ShowCharacter", payload);
        }
        // [ADD] Lệnh ẩn 1 nhân vật ở vị trí cụ thể (Cú pháp: <<hide_char left>>)
        [YarnCommand("hide_char")]
        public static void HideCharacter(string position)
        {
            Debug.Log($"<color=cyan>[YarnAdapter]</color> Kịch bản yêu cầu ẨN nhân vật ở vị trí: <b>{position}</b>");
            
            // Dùng kênh truyền string để báo cho UI biết cần xóa ảnh ở vị trí nào
            EventManager.TriggerEvent("UI_HideCharacter", position.ToLower());
        }

        // [ADD] Lệnh dọn dẹp toàn bộ nhân vật (Cú pháp: <<hide_all_chars>>)
        [YarnCommand("hide_all_chars")]
        public static void HideAllCharacters()
        {
            Debug.Log("<color=cyan>[YarnAdapter]</color> Kịch bản yêu cầu ẨN TOÀN BỘ nhân vật");
            
            // Dùng kênh Event không tham số vì chỉ cần hét lên "Xóa hết đi!"
            EventManager.TriggerEvent("UI_HideAllCharacters");
        }
    }
}