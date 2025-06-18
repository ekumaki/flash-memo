using System.Windows;

namespace FlashMemo
{
    /// <summary>
    /// パッとメモのメインアプリケーションクラス
    /// 起動時に即座にメモウィンドウを表示
    /// </summary>
    public partial class App : Application
    {
        private MainWindow? _mainWindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // メインウィンドウを作成して即座に表示
            _mainWindow = new MainWindow();
            MainWindow = _mainWindow;
            
            // ウィンドウを表示
            _mainWindow.Show();
        }

    }
}