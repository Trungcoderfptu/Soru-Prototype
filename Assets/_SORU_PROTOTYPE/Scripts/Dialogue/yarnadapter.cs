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
    }
}