using UnityEngine;
using Yarn.Unity;

namespace SoruPrototype.Testing
{
    public class Test_StartDialogue : MonoBehaviour
    {
        [Header("Tham chiếu Hệ thống")]
        [Tooltip("Kéo thả object Dialogue System vào đây để code không phải tự tìm mò.")]
        [SerializeField] private DialogueRunner dialogueRunner;

        [Header("Cấu hình Test")]
        [Tooltip("Tên chính xác của Node cần chạy (Phân biệt chữ hoa/thường)")]
        [SerializeField] private string nodeToStart = "chapter1_p1";

        private void Start()
        {
            // Back-up: Tự động tìm nếu bạn quên kéo thả trên Inspector
            if (dialogueRunner == null)
            {
                dialogueRunner = FindAnyObjectByType<DialogueRunner>();
                
                if (dialogueRunner == null)
                {
                    Debug.LogError("<color=red>[Test]</color> CHẾT DỞ: Hoàn toàn không tìm thấy bộ não DialogueRunner trong Scene!");
                }
            }
        }

        private void Update()
        {
            // Nút bấm: Kích hoạt thoại
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (dialogueRunner == null) return;

                if (dialogueRunner.IsDialogueRunning)
                {
                    Debug.LogWarning("<color=yellow>[Test]</color> Báo cáo: Kịch bản đang chạy ngầm rồi, không thể đè thêm lệnh mới!");
                    return;
                }

                Debug.Log($"<color=cyan>[Test]</color> Kích hoạt thành công! Đang ra lệnh cho Yarn chạy Node: <b>{nodeToStart}</b>");
                dialogueRunner.StartDialogue(nodeToStart);
            }

            // Nút bấm: Ép dừng (Khắc phục lỗi kẹt thoại ngầm)
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (dialogueRunner != null && dialogueRunner.IsDialogueRunning)
                {
                    dialogueRunner.Stop();
                    Debug.Log("<color=orange>[Test]</color> Đã dọn dẹp hệ thống: Ép Yarn Spinner dừng mọi hoạt động.");
                }
            }
        }
    }
}