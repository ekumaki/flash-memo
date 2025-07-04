using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FlashMemo.Services;

namespace FlashMemo
{
    /// <summary>
    /// メモ入力用のメインウィンドウ
    /// 軽量で高速な起動を重視した設計
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ClipboardService _clipboardService;
        
        private bool _isPinned;

        public MainWindow()
        {
            InitializeComponent();
            _clipboardService = new ClipboardService();
            
            // ウィンドウの初期設定
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            
            // アイコンを設定
            SetWindowIcon();
            

        }

        /// <summary>
        /// ウィンドウアイコンを設定
        /// </summary>
        private void SetWindowIcon()
        {
            try
            {
                var iconPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "flashmemo-icon.ico");
                if (System.IO.File.Exists(iconPath))
                {
                    Icon = new System.Windows.Media.Imaging.BitmapImage(new Uri(iconPath));
                }
            }
            catch
            {
                // アイコンファイルがない場合はデフォルトアイコンを使用
            }
        }

        /// <summary>
        /// ウィンドウが表示されたときの処理
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            
            // テキストボックスにフォーカスを設定
            MemoTextBox.Focus();
            MemoTextBox.CaretIndex = MemoTextBox.Text.Length;
            
            UpdatePinState();
            Activate();
        }

        /// <summary>
        /// ウィンドウが閉じられるときの処理
        /// </summary>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            Application.Current.Shutdown();
        }

        /// <summary>
        /// コピー&閉じるボタンのクリック処理
        /// </summary>
        private void CopyCloseButton_Click(object sender, RoutedEventArgs e)
        {
            CopyTextAndClose();
        }

        /// <summary>
        /// 閉じるボタンのクリック処理
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// ピンボタンのクリック処理
        /// </summary>
        internal void PinButton_Click(object sender, RoutedEventArgs e)
        {
            TogglePin();
        }

        /// <summary>
        /// ピン状態を切り替える
        /// </summary>
        internal void TogglePin()
        {
            _isPinned = !_isPinned;
            UpdatePinState();
        }

        /// <summary>
        /// ピン状態を更新する
        /// </summary>
        internal void UpdatePinState()
        {
            Topmost = _isPinned;
            if (PinButton != null)
            {
                PinButton.Opacity = _isPinned ? 1.0 : 0.5;
            }
        }

        /// <summary>
        /// テスト用にMemoTextBoxを公開
        /// </summary>
        internal TextBox MemoTextBoxControl => MemoTextBox;

        /// <summary>
        /// テキストボックスでのキー入力処理
        /// </summary>
        private void MemoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Ctrl+Enter でコピー&閉じる
            if (e.Key == Key.Enter && Keyboard.Modifiers == ModifierKeys.Control)
            {
                CopyTextAndClose();
                e.Handled = true;
            }
            // Escape で閉じる
            else if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
                e.Handled = true;
            }
        }

        /// <summary>
        /// テキストをクリップボードにコピーしてウィンドウを閉じる
        /// </summary>
        internal void CopyTextAndClose()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(MemoTextBox.Text))
                {
                    _clipboardService.SetText(MemoTextBox.Text);
                    
                    // 成功のフィードバック（短時間表示）
                    ShowSuccessMessage();
                }
                
                // テキストをクリア
                MemoTextBox.Text = string.Empty;
                
                // アプリケーションを終了
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"クリップボードへのコピーに失敗しました:\n{ex.Message}", 
                    "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 成功メッセージの表示
        /// </summary>
        private void ShowSuccessMessage()
        {
            // ウィンドウタイトルを一時的に変更してフィードバック
            var originalTitle = Title;
            Title = "✓ クリップボードにコピーしました";
            
            // 500ms後に元のタイトルに戻す
            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            timer.Tick += (s, e) =>
            {
                Title = originalTitle;
                timer.Stop();
            };
            timer.Start();
        }

        /// <summary>
        /// ウィンドウを表示する際の最適化処理
        /// </summary>
        public new void Show()
        {
            // ウィンドウ状態を初期化
            ResetWindowState();
            
            base.Show();
            
            // ピン状態を反映
            UpdatePinState();
            Activate();
            Focus();
            MemoTextBox.Focus();
            
            // 画面中央に配置
            Left = (SystemParameters.PrimaryScreenWidth - Width) / 2;
            Top = (SystemParameters.PrimaryScreenHeight - Height) / 2;
        }

        /// <summary>
        /// ウィンドウ状態を初期状態にリセット
        /// </summary>
        private void ResetWindowState()
        {
            // テキストをクリア
            MemoTextBox.Text = string.Empty;
            
            // ウィンドウサイズを初期値にリセット
            Width = 500;
            Height = 400;
            
            // ウィンドウ位置を中央にリセット
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            
            // タイトルを初期状態にリセット
            Title = "パッとメモ";
        }
    }
}