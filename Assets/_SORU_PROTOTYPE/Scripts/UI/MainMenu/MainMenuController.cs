using UnityEngine;
using SoruPrototype.Core; // Gọi namespace Core để dùng GameManager và GameState

namespace SoruPrototype.UI.MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
      
        
        /// <summary>
        /// Hàm này sẽ được gán vào sự kiện OnClick() của nút "Bắt đầu"
        /// </summary>
        public void OnPlayButtonClicked()
        {
            Debug.Log("<color=yellow>[MainMenuUI]</color> Người chơi bấm Bắt đầu. Đang gọi GameManager...");

            // Gọi bộ não Core chuyển trạng thái sang Visual Novel
            GameManager.Instance.ChangeState(GameState.VisualNovel);
        }
        public void OnSettingButtonClicked()
        {
            Debug.Log("<color=yellow>[MainMenuUI]</color> Người chơi bấm Setting. EventManager...");


            EventManager.TriggerEvent("UI_OpenSetting");
        }

        /// <summary>
        /// Hàm này gán vào sự kiện OnClick() của nút "Thoát" (nếu bạn có làm)
        /// </summary>
        public void OnQuitButtonClicked()
        {
            Debug.Log("<color=red>[MainMenuUI]</color> Thoát Game!");
            Application.Quit(); // Lệnh này chỉ hoạt động khi đã Build ra file .exe
        }
    }
}