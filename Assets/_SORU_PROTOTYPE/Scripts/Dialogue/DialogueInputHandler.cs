using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // Thêm thư viện này để nhận diện Button (Selectable)
using Yarn.Unity;
using SoruPrototype.Core;

namespace SoruPrototype.Dialogue 
{
    public class DialogueInputHandler : MonoBehaviour
    {
        [Header("Dialogue References")]
        [SerializeField] private LineAdvancer lineAdvancer; 
        [SerializeField] private DialogueRunner dialogueRunner;

        private void Update()
        {
            if (GameManager.Instance.CurrentState != GameState.VisualNovel) 
                return;

            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                if (!dialogueRunner.IsDialogueRunning) return;

                // Sử dụng hàm kiểm tra thông minh thay vì hàm chặn mặc định
                if (IsPointerOverClickableUI())
                {
                    Debug.Log("<color=red>[Input]</color> Bị chặn: Bạn vừa bấm trúng một phím chức năng (Button)!");
                    return;
                }

                Debug.Log("<color=green>[Input]</color> Thành công: Đã bấm xuyên qua UI nền để Next thoại!");
                lineAdvancer.OnInputNextContent(); 
            }
        }

        /// <summary>
        /// Hàm bắn tia X-quang quét qua tất cả UI dưới con trỏ chuột
        /// Trả về True nếu chạm trúng Nút chức năng, False nếu chỉ là Background/Text thường
        /// </summary>
        private bool IsPointerOverClickableUI()
        {
            if (EventSystem.current == null) return false;

            // Tạo một mũi kim (Pointer) ảo tại vị trí chuột hiện tại
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = new Vector2(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y)
            };

            // Lấy danh sách tất cả các UI bị mũi kim này đâm xuyên qua
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            // Kiểm tra từng UI trong danh sách
            foreach (RaycastResult result in results)
            {
                // Nếu UI bị xuyên qua có chứa component Selectable (nghĩa là nó là Button, Toggle, Slider...)
                // GetComponentInParent giúp bắt được cả trường hợp bạn click trúng cái Text nằm bên trong cái Button
                if (result.gameObject.GetComponentInParent<Selectable>() != null)
                {
                    return true; // Phanh lại! Đây là phím chức năng.
                }
            }
            
            return false; // Không có phím chức năng nào, cho phép bấm xuyên qua!
        }
    }
}