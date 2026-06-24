using System;
using UnityEngine;

namespace SoruPrototype.Core
{
    /// <summary>
    /// Struct hỗ trợ cấu hình âm thanh trực tiếp trên Inspector.
    /// </summary>
    [Serializable]
    public struct SoundProfile
    {
        [Tooltip("Tên định danh (ID) để các hệ thống khác gọi tới. Ví dụ: 'bgm_mainmenu', 'sfx_click'")]
        public string soundID;
        public AudioClip clip;
        [Range(0f, 1f)] public float defaultVolume;
    }

    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        [Tooltip("Nguồn phát nhạc nền (Phát lặp lại, chỉ 1 bài tại 1 thời điểm)")]
        [SerializeField] private AudioSource bgmSource;
        [Tooltip("Nguồn phát hiệu ứng (Phát đè lên nhau, một lần rồi tắt)")]
        [SerializeField] private AudioSource sfxSource;

        [Header("Audio Library")]
        [SerializeField] private SoundProfile[] bgmLibrary;
        [SerializeField] private SoundProfile[] sfxLibrary;

        

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Phát nhạc nền dựa trên ID cấu hình. Tự động thay thế nhạc cũ nếu đang phát.
        /// </summary>
        /// <param name="soundID">Tên định danh của bản nhạc cần phát.</param>
        public void PlayBGM(string soundID)
        {
            SoundProfile profile = Array.Find(bgmLibrary, s => s.soundID == soundID);

            if (profile.clip != null)
            {
                // Nếu bài nhạc này đang phát rồi thì bỏ qua để tránh bị giật lại từ đầu
                if (bgmSource.clip == profile.clip && bgmSource.isPlaying) return;

                bgmSource.clip = profile.clip;
                bgmSource.volume = profile.defaultVolume;
                bgmSource.loop = true;
                bgmSource.Play();
                Debug.Log($"[AudioManager] Playing BGM: {soundID}");
            }
            else
            {
                Debug.LogWarning($"[AudioManager] BGM ID not found: {soundID}");
            }
        }

        /// <summary>
        /// Phát hiệu ứng âm thanh một lần dựa trên ID.
        /// </summary>
        /// <param name="soundID">Tên định danh của hiệu ứng.</param>
        public void PlaySFX(string soundID)
        {
            SoundProfile profile = Array.Find(sfxLibrary, s => s.soundID == soundID);

            if (profile.clip != null)
            {
                sfxSource.PlayOneShot(profile.clip, profile.defaultVolume);
            }
            else
            {
                Debug.LogWarning($"[AudioManager] SFX ID not found: {soundID}");
            }
        }

        /// <summary>
        /// Dừng phát nhạc nền hiện tại.
        /// </summary>
        public void StopBGM()
        {
            if (bgmSource.isPlaying)
            {
                bgmSource.Stop();
            }
        }
        private const string BGM_KEY = "BGM_Volume";
        private const string SFX_KEY = "SFX_Volume";
        private const string BGM_MUTE_KEY = "BGM_Mute";
        private const string SFX_MUTE_KEY = "SFX_Mute";

        private void Start()
        {
            // Tự động load âm lượng đã lưu khi game vừa bật
            LoadAudioSettings();
        }

        public void SetBGMVolume(float volume)
        {
            if (bgmSource != null)
            {
                bgmSource.volume = Mathf.Clamp01(volume);
                PlayerPrefs.SetFloat(BGM_KEY, bgmSource.volume); // Lưu tự động
            }
        }

        public void SetSFXVolume(float volume)
        {
            if (sfxSource != null)
            {
                sfxSource.volume = Mathf.Clamp01(volume);
                PlayerPrefs.SetFloat(SFX_KEY, sfxSource.volume);
            }
        }

        public float GetBGMVolume() => PlayerPrefs.GetFloat(BGM_KEY, 1f); // Mặc định là 1 (100%)
        public float GetSFXVolume() => PlayerPrefs.GetFloat(SFX_KEY, 1f);

        // [ADD] Hàm xử lý Mute AudioSource và lưu trữ trạng thái
        public void ToggleMuteBGM(bool isMuted)
        {
            if (bgmSource != null)
            {
                bgmSource.mute = isMuted;
                PlayerPrefs.SetInt(BGM_MUTE_KEY, isMuted ? 1 : 0); // Ép kiểu bool sang int (1 = true, 0 = false)
            }
        }

        public void ToggleMuteSFX(bool isMuted)
        {
            if (sfxSource != null)
            {
                sfxSource.mute = isMuted;
                PlayerPrefs.SetInt(SFX_MUTE_KEY, isMuted ? 1 : 0);
            }
        }

        // [ADD] Getter lấy trạng thái Mute hiện tại từ bộ nhớ
        public bool IsBGMMuted() => PlayerPrefs.GetInt(BGM_MUTE_KEY, 0) == 1;
        public bool IsSFXMuted() => PlayerPrefs.GetInt(SFX_MUTE_KEY, 0) == 1;

        private void LoadAudioSettings()
        {
            SetBGMVolume(GetBGMVolume());
            SetSFXVolume(GetSFXVolume());
            ToggleMuteBGM(IsBGMMuted());
            ToggleMuteSFX(IsSFXMuted());
        }
    }
}