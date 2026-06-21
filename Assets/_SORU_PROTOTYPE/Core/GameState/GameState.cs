namespace SoruPrototype.Core
{
    public enum GameState
    {
        MainMenu,      // Màn hình chính
        VisualNovel,   // Đang đọc kịch bản/thoại
        MiniGame,      // Đang chơi minigame cắt cảnh
        Simulation,    // Chế độ tương tác sâu (hack terminal, điều khiển máy móc...)
        Paused,         // Tạm dừng game
        Setting,        //Màn Hình cài đặt.
        SaveLoad,        //Màn hình save load
        Intro            //Màn hình intro

    }
}