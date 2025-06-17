using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Interop;

namespace FlashMemo.Services
{
    /// <summary>
    /// グローバルホットキーを管理するサービス
    /// Win32 APIを使用してシステム全体でホットキーを登録
    /// </summary>
    public class GlobalHotkeyService : IDisposable
    {
        // Win32 API定数
        public const int MOD_ALT = 0x0001;
        public const int MOD_CONTROL = 0x0002;
        public const int MOD_SHIFT = 0x0004;
        public const int MOD_WIN = 0x0008;
        public const int WM_HOTKEY = 0x0312;

        // Win32 API関数のインポート
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private readonly int _hotkeyId;
        private readonly HiddenWindow _hiddenWindow;
        private bool _disposed = false;

        public event EventHandler? HotkeyPressed;

        public GlobalHotkeyService()
        {
            _hotkeyId = GetHashCode();
            _hiddenWindow = new HiddenWindow();
            _hiddenWindow.HotkeyPressed += OnHotkeyPressed;
        }

        /// <summary>
        /// ホットキーを登録
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="modifiers">修飾キー（MOD_CONTROL | MOD_SHIFT など）</param>
        /// <returns>登録成功時はtrue</returns>
        public bool RegisterHotkey(Keys key, int modifiers)
        {
            try
            {
                return RegisterHotKey(_hiddenWindow.Handle, _hotkeyId, modifiers, (int)key);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ホットキー登録エラー: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// ホットキーの登録を解除
        /// </summary>
        public void UnregisterHotkey()
        {
            try
            {
                UnregisterHotKey(_hiddenWindow.Handle, _hotkeyId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ホットキー解除エラー: {ex.Message}");
            }
        }

        /// <summary>
        /// ホットキーが押されたときの内部処理
        /// </summary>
        private void OnHotkeyPressed(object? sender, EventArgs e)
        {
            HotkeyPressed?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                UnregisterHotkey();
                _hiddenWindow?.Dispose();
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// ホットキーメッセージを受信するための非表示ウィンドウ
        /// </summary>
        private class HiddenWindow : NativeWindow, IDisposable
        {
            public event EventHandler? HotkeyPressed;

            public HiddenWindow()
            {
                CreateHandle(new CreateParams());
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM_HOTKEY)
                {
                    HotkeyPressed?.Invoke(this, EventArgs.Empty);
                }
                base.WndProc(ref m);
            }

            public void Dispose()
            {
                if (Handle != IntPtr.Zero)
                {
                    DestroyHandle();
                }
                GC.SuppressFinalize(this);
            }
        }
    }
}