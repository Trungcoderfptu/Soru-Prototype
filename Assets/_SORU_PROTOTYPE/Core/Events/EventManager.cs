using System;
using System.Collections.Generic;
using UnityEngine;
namespace SoruPrototype.Core
{




    public static class EventManager
    {
        // Kho chứa danh sách các sự kiện (Tên sự kiện - Hành động đi kèm)
        private static readonly Dictionary<string, Action> EventDictionary = new Dictionary<string, Action>();

        // [ADD] Kho chứa sự kiện có đính kèm 1 chuỗi dữ liệu (Data Payload) - Dùng để truyền ID ảnh, tên nhân vật...
        private static readonly Dictionary<string, Action<string>> StringEventDictionary = new Dictionary<string, Action<string>>();

        // 1. Hàm đăng ký lắng nghe sự kiện (Bật máy thu sóng)
        public static void StartListening(string eventName, Action listener)
        {
            if (EventDictionary.TryGetValue(eventName, out Action thisEvent))
            {
                thisEvent += listener;
                EventDictionary[eventName] = thisEvent;
            }
            else
            {
                thisEvent += listener;
                EventDictionary.Add(eventName, thisEvent);
            }
        }

        // 2. Hàm hủy đăng ký sự kiện (Tắt máy thu khi không dùng để tránh rác bộ nhớ)
        public static void StopListening(string eventName, Action listener)
        {
            if (EventDictionary.TryGetValue(eventName, out Action thisEvent))
            {
                thisEvent -= listener;
                EventDictionary[eventName] = thisEvent;
            }
        }

        // 3. Hàm phát sóng sự kiện (Hét lên cho cả game cùng nghe)
        public static void TriggerEvent(string eventName)
        {
            if (EventDictionary.TryGetValue(eventName, out Action thisEvent))
            {
                thisEvent?.Invoke(); // Kích hoạt tất cả các hàm đang hóng sự kiện này cùng lúc
            }
        }
        
        // [ADD] BỘ 3 HÀM DÀNH CHO SỰ KIỆN CÓ CHỨA THAM SỐ STRING (Ví dụ: Truyền ID)
        
        public static void StartListening(string eventName, Action<string> listener)
        {
            if (StringEventDictionary.TryGetValue(eventName, out Action<string> thisEvent))
            {
                thisEvent += listener;
                StringEventDictionary[eventName] = thisEvent;
            }
            else
            {
                thisEvent += listener;
                StringEventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, Action<string> listener)
        {
            if (StringEventDictionary.TryGetValue(eventName, out Action<string> thisEvent))
            {
                thisEvent -= listener;
                StringEventDictionary[eventName] = thisEvent;
            }
        }

        public static void TriggerEvent(string eventName, string data)
        {
            if (StringEventDictionary.TryGetValue(eventName, out Action<string> thisEvent))
            {
                thisEvent?.Invoke(data); // Phát sóng sự kiện KÈM theo dữ liệu ID
            }
        }
    }
}