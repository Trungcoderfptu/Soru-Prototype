using UnityEngine;
using UnityEngine.UI;
using SoruPrototype.Core;

namespace SoruPrototype.UI.Global
{
    public class GlobalSettingController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject settingPanel; // Gắn chính bản thân cái Panel chứa UI vào đây
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Slider sfxSlider;
        
        [Header("Buttons")]
        [SerializeField] private Button closeButton;
        [SerializeField] private Button backToTitleButton; // Nút Về màn hình chính

        private void OnEnable()
        {
            // Khi Core System bật lên, lập tức lắng nghe sự kiện
            EventManager.StartListening("UI_OpenSetting", ShowSettingPanel);
        }

        private void OnDisable()
        {
            EventManager.StopListening("UI_OpenSetting", ShowSettingPanel);
        }

        private void Start()
        {
            // Đăng ký các sự kiện cho UI
            if (bgmSlider != null) bgmSlider.onValueChanged.AddListener(OnBgmChanged);
            if (sfxSlider != null) sfxSlider.onValueChanged.AddListener(OnSfxChanged);
            if (closeButton != null) closeButton.onClick.AddListener(HideSettingPanel);
            if (backToTitleButton != null) backToTitleButton.onClick.AddListener(OnBackToTitleClicked);
            
            // Mặc định ẩn UI Setting khi mới vào game
            if (settingPanel != null) settingPanel.SetActive(false);
        }

        private void ShowSettingPanel()
        {
            if (settingPanel == null) return;
            
            // TRƯỚC KHI HIỂN THỊ: Cập nhật lại thanh Slider cho đúng với Data thực tế từ AudioManager
            if (AudioManager.Instance != null)
            {
                if (bgmSlider != null) bgmSlider.value = AudioManager.Instance.GetBGMVolume();
                if (sfxSlider != null) sfxSlider.value = AudioManager.Instance.GetSFXVolume();
            }
            
            // Bật Panel lên
            settingPanel.SetActive(true);
        }

        private void HideSettingPanel()
        {
            // Nút Close gọi hàm này, chỉ đơn giản là ẩn UI đi, GameState vẫn giữ nguyên
            if (settingPanel != null) settingPanel.SetActive(false);
        }

        private void OnBgmChanged(float value)
        {
            if (AudioManager.Instance != null) AudioManager.Instance.SetBGMVolume(value);
        }

        private void OnSfxChanged(float value)
        {
            if (AudioManager.Instance != null) AudioManager.Instance.SetSFXVolume(value);
        }

        private void OnBackToTitleClicked()
        {
            Debug.Log("<color=yellow>[GlobalSetting]</color> Quay về Title...");
            
            // 1. Tắt bảng Setting đi
            HideSettingPanel();
            
            // 2. Yêu cầu GameManager chuyển State về MainMenu
            // Dù đang ở VisualNovel hay cảnh nào, đều sẽ bị dọn dẹp để về MainMenu
            GameManager.Instance.ChangeState(GameState.MainMenu);
        }
    }
}