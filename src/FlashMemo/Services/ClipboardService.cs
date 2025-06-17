using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FlashMemo.Services
{
    /// <summary>
    /// クリップボード操作を管理するサービス
    /// 安全で信頼性の高いクリップボード操作を提供
    /// </summary>
    public class ClipboardService
    {
        private const int MaxRetryAttempts = 3;
        private const int RetryDelayMs = 100;

        /// <summary>
        /// テキストをクリップボードに設定
        /// リトライ機能付きで信頼性を向上
        /// </summary>
        /// <param name="text">設定するテキスト</param>
        public async Task SetTextAsync(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            await RetryClipboardOperation(() => 
            {
                Clipboard.SetText(text);
                return Task.CompletedTask;
            });
        }

        /// <summary>
        /// テキストをクリップボードに設定（同期版）
        /// </summary>
        /// <param name="text">設定するテキスト</param>
        public void SetText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            RetryClipboardOperation(() => 
            {
                Clipboard.SetText(text);
                return Task.CompletedTask;
            }).Wait();
        }

        /// <summary>
        /// クリップボードからテキストを取得
        /// </summary>
        /// <returns>クリップボード内のテキスト</returns>
        public async Task<string> GetTextAsync()
        {
            string result = string.Empty;
            
            await RetryClipboardOperation(() => 
            {
                if (Clipboard.ContainsText())
                {
                    result = Clipboard.GetText();
                }
                return Task.CompletedTask;
            });

            return result;
        }

        /// <summary>
        /// クリップボードからテキストを取得（同期版）
        /// </summary>
        /// <returns>クリップボード内のテキスト</returns>
        public string GetText()
        {
            return GetTextAsync().Result;
        }

        /// <summary>
        /// クリップボード操作のリトライ処理
        /// クリップボードが他のアプリケーションによってロックされている場合に対応
        /// </summary>
        /// <param name="operation">実行する操作</param>
        private async Task RetryClipboardOperation(Func<Task> operation)
        {
            for (int attempt = 0; attempt < MaxRetryAttempts; attempt++)
            {
                try
                {
                    // UIスレッドで実行する必要がある
                    if (Application.Current?.Dispatcher.CheckAccess() == true)
                    {
                        await operation();
                        return;
                    }
                    else
                    {
                        await Application.Current.Dispatcher.InvokeAsync(async () => await operation());
                        return;
                    }
                }
                catch (Exception ex) when (IsRetryableException(ex))
                {
                    if (attempt == MaxRetryAttempts - 1)
                    {
                        // 最後の試行でも失敗した場合は例外を再スロー
                        throw;
                    }

                    // 次の試行前に短時間待機
                    await Task.Delay(RetryDelayMs);
                }
            }
        }

        /// <summary>
        /// リトライ可能な例外かどうかを判定
        /// </summary>
        /// <param name="ex">発生した例外</param>
        /// <returns>リトライ可能な場合はtrue</returns>
        private static bool IsRetryableException(Exception ex)
        {
            // クリップボードがロックされている、または一時的にアクセスできない場合
            return ex is System.Runtime.InteropServices.COMException ||
                   ex is System.Runtime.InteropServices.ExternalException ||
                   ex.Message.Contains("OpenClipboard") ||
                   ex.Message.Contains("clipboard");
        }

        /// <summary>
        /// クリップボードが利用可能かどうかを確認
        /// </summary>
        /// <returns>利用可能な場合はtrue</returns>
        public bool IsClipboardAvailable()
        {
            try
            {
                return Clipboard.ContainsText() || !Clipboard.ContainsText();
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// クリップボードをクリア
        /// </summary>
        public async Task ClearAsync()
        {
            await RetryClipboardOperation(() => 
            {
                Clipboard.Clear();
                return Task.CompletedTask;
            });
        }

        /// <summary>
        /// クリップボードをクリア（同期版）
        /// </summary>
        public void Clear()
        {
            ClearAsync().Wait();
        }
    }
}