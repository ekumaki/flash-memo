using System;
using System.Drawing;
using System.Windows;
using FlashMemo.Services;

namespace FlashMemo
{
    /// <summary>
    /// パッとメモのメインアプリケーションクラス
    /// 起動時にグローバルホットキーを登録し、常駐モードで動作
    /// </summary>
    public partial class App : Application
    {
        private GlobalHotkeyService? _hotkeyService;
        private MainWindow? _mainWindow;
        private System.Windows.Forms.NotifyIcon? _notifyIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // メインウィンドウを非表示で作成
            _mainWindow = new MainWindow();
            
            // システムトレイアイコンを設定
            SetupNotifyIcon();
            
            // グローバルホットキーサービスを初期化
            _hotkeyService = new GlobalHotkeyService();
            _hotkeyService.HotkeyPressed += OnHotkeyPressed;
            
            // ホットキー機能を一時的に無効化
            // if (!_hotkeyService.RegisterHotkey(System.Windows.Forms.Keys.M, 
            //     GlobalHotkeyService.MOD_CONTROL | GlobalHotkeyService.MOD_SHIFT))
            // {
            //     MessageBox.Show("ホットキーの登録に失敗しました。他のアプリケーションが同じキーを使用している可能性があります。",
            //         "FlashMemo", MessageBoxButton.OK, MessageBoxImage.Warning);
            // }

            // メインウィンドウを表示しない（常駐モード）
            MainWindow = null;
        }

        /// <summary>
        /// システムトレイアイコンの設定
        /// </summary>
        private void SetupNotifyIcon()
        {
            _notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Icon = SystemIcons.Application,
                Text = "パッとメモ (クリックで開く)",
                Visible = true
            };

            // 右クリックメニューを設定
            var contextMenu = new System.Windows.Forms.ContextMenuStrip();
            contextMenu.Items.Add("メモを開く (&O)", null, (s, e) => ShowMainWindow());
            contextMenu.Items.Add("-");
            contextMenu.Items.Add("終了 (&X)", null, (s, e) => ExitApplication());
            _notifyIcon.ContextMenuStrip = contextMenu;

            // ダブルクリックでメモを開く
            _notifyIcon.DoubleClick += (s, e) => ShowMainWindow();
            
            // シングルクリックでも開く（より使いやすく）
            _notifyIcon.Click += (s, e) =>
            {
                var mouseEvent = e as System.Windows.Forms.MouseEventArgs;
                if (mouseEvent?.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    ShowMainWindow();
                }
            };
        }

        /// <summary>
        /// ホットキーが押されたときの処理
        /// </summary>
        private void OnHotkeyPressed(object? sender, EventArgs e)
        {
            ShowMainWindow();
        }

        /// <summary>
        /// メインウィンドウを表示
        /// </summary>
        private void ShowMainWindow()
        {
            if (_mainWindow == null)
            {
                _mainWindow = new MainWindow();
            }

            _mainWindow.Show();
            _mainWindow.Activate();
            _mainWindow.Focus();
        }

        /// <summary>
        /// アプリケーション終了処理
        /// </summary>
        private void ExitApplication()
        {
            _hotkeyService?.Dispose();
            _notifyIcon?.Dispose();
            Shutdown();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ExitApplication();
            base.OnExit(e);
        }
    }
}