using System;
using System.Windows;
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
        private DateTime _windowShowTime;

        public MainWindow()
        {
            InitializeComponent();
            _clipboardService = new ClipboardService();
            
            // ウィンドウの初期設定
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        /// <summary>
        /// ウィンドウが表示されたときの処理
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _windowShowTime = DateTime.Now;
            
            // テキストボックスにフォーカスを設定
            MemoTextBox.Focus();
            MemoTextBox.CaretIndex = MemoTextBox.Text.Length;
            
            // ウィンドウを最前面に表示
            Topmost = true;
            Activate();
        }

        /// <summary>
        /// ウィンドウが非アクティブになったときの処理（自動的に閉じる）
        /// </summary>
        private void Window_Deactivated(object sender, EventArgs e)
        {
            // ウィンドウが表示されてから500ms以降であれば自動的に閉じる
            var elapsed = DateTime.Now - _windowShowTime;
            if (elapsed.TotalMilliseconds > 500)
            {
                Hide();
            }
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
            Hide();
        }

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
                Hide();
                e.Handled = true;
            }
        }

        /// <summary>
        /// テキストをクリップボードにコピーしてウィンドウを閉じる
        /// </summary>
        private void CopyTextAndClose()
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
                
                // ウィンドウを隠す
                Hide();
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
            
            // 最前面に表示してフォーカスを設定
            Topmost = true;
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