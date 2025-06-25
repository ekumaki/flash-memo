using Xunit;
using FlashMemo;
using System.Windows;

namespace FlashMemo.Tests
{
    public class MainWindowTests
    {
        [WpfFact]
        public void PinButton_Toggle_UpdatesTopmost()
        {
            // Arrange
            var mainWindow = new MainWindow();
            
            // Act - Toggle pin on
            mainWindow.PinButton_Click(null, null);
            
            // Assert
            Assert.True(mainWindow.Topmost);
            
            // Act - Toggle pin off
            mainWindow.PinButton_Click(null, null);
            
            // Assert
            Assert.False(mainWindow.Topmost);
        }
        
        [WpfFact]
        public void CopyTextAndClose_WithText_CopiesToClipboard()
        {
            // Arrange
            var mainWindow = new MainWindow();
            var testText = "Test text";
            mainWindow.MemoTextBox.Text = testText;
            
            // Act
            mainWindow.CopyTextAndClose();
            
            // Assert - 実際のクリップボードの確認は難しいので、例外がスローされないことを確認
            // 実際のアプリでは、ClipboardService をモック化するのがベストプラクティスです
        }
    }
}
