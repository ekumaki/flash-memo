# FlashMemo パフォーマンステストスクリプト
# Windows PowerShell 5.1以降で実行

param(
    [string]$ExePath = "build\output\Release\FlashMemo.exe",
    [int]$TestCount = 5
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "FlashMemo Performance Test" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

# 前提条件チェック
if (-not (Test-Path $ExePath)) {
    Write-Error "実行ファイルが見つかりません: $ExePath"
    Write-Host "先にビルドを実行してください: build\build.bat"
    exit 1
}

# テスト結果保存用
$TestResults = @{
    StartupTimes = @()
    MemoryUsage = @()
    Success = $true
}

Write-Host "テスト対象: $ExePath" -ForegroundColor Yellow
Write-Host "テスト回数: $TestCount" -ForegroundColor Yellow
Write-Host ""

# 1. 起動時間テスト
Write-Host "1. 起動時間テスト" -ForegroundColor Green
Write-Host "----------------------------------------"

for ($i = 1; $i -le $TestCount; $i++) {
    Write-Host "テスト $i/$TestCount..." -NoNewline
    
    # 既存プロセスを終了
    Get-Process -Name "FlashMemo" -ErrorAction SilentlyContinue | Stop-Process -Force
    Start-Sleep -Milliseconds 500
    
    # 起動時間測定
    $StartTime = Get-Date
    $Process = Start-Process -FilePath $ExePath -PassThru -WindowStyle Hidden
    
    # プロセスがメモリを使用し始めるまで待機（初期化完了の目安）
    $TimeoutMs = 10000
    $ElapsedMs = 0
    
    while ($ElapsedMs -lt $TimeoutMs) {
        try {
            $WorkingSet = $Process.WorkingSet64
            if ($WorkingSet -gt (10 * 1024 * 1024)) { # 10MB以上で初期化完了とみなす
                break
            }
        }
        catch {
            # プロセスがまだ完全に開始していない場合
        }
        
        Start-Sleep -Milliseconds 50
        $ElapsedMs += 50
    }
    
    $EndTime = Get-Date
    $StartupTime = ($EndTime - $StartTime).TotalMilliseconds
    $TestResults.StartupTimes += $StartupTime
    
    Write-Host " $([math]::Round($StartupTime, 0))ms" -ForegroundColor $(if ($StartupTime -le 500) { "Green" } else { "Red" })
    
    # メモリ使用量測定（5秒後）
    Start-Sleep -Seconds 2
    try {
        $MemoryMB = [math]::Round($Process.WorkingSet64 / 1MB, 1)
        $TestResults.MemoryUsage += $MemoryMB
    }
    catch {
        Write-Warning "メモリ使用量の取得に失敗しました"
        $TestResults.MemoryUsage += 0
    }
    
    # プロセス終了
    $Process | Stop-Process -Force -ErrorAction SilentlyContinue
    Start-Sleep -Milliseconds 200
}

Write-Host ""

# 2. 結果分析
Write-Host "2. テスト結果" -ForegroundColor Green
Write-Host "----------------------------------------"

# 起動時間の統計
$AvgStartup = [math]::Round(($TestResults.StartupTimes | Measure-Object -Average).Average, 0)
$MaxStartup = [math]::Round(($TestResults.StartupTimes | Measure-Object -Maximum).Maximum, 0)
$MinStartup = [math]::Round(($TestResults.StartupTimes | Measure-Object -Minimum).Minimum, 0)

Write-Host "起動時間:" -ForegroundColor White
Write-Host "  平均: ${AvgStartup}ms" -ForegroundColor $(if ($AvgStartup -le 500) { "Green" } else { "Red" })
Write-Host "  最速: ${MinStartup}ms" -ForegroundColor $(if ($MinStartup -le 500) { "Green" } else { "Red" })
Write-Host "  最遅: ${MaxStartup}ms" -ForegroundColor $(if ($MaxStartup -le 500) { "Green" } else { "Red" })

# メモリ使用量の統計
$ValidMemory = $TestResults.MemoryUsage | Where-Object { $_ -gt 0 }
if ($ValidMemory.Count -gt 0) {
    $AvgMemory = [math]::Round(($ValidMemory | Measure-Object -Average).Average, 1)
    $MaxMemory = [math]::Round(($ValidMemory | Measure-Object -Maximum).Maximum, 1)
    $MinMemory = [math]::Round(($ValidMemory | Measure-Object -Minimum).Minimum, 1)
    
    Write-Host "メモリ使用量:" -ForegroundColor White
    Write-Host "  平均: ${AvgMemory}MB" -ForegroundColor $(if ($AvgMemory -le 30) { "Green" } else { "Red" })
    Write-Host "  最大: ${MaxMemory}MB" -ForegroundColor $(if ($MaxMemory -le 30) { "Green" } else { "Red" })
    Write-Host "  最小: ${MinMemory}MB" -ForegroundColor $(if ($MinMemory -le 30) { "Green" } else { "Red" })
}

Write-Host ""

# 3. 要件チェック
Write-Host "3. 要件適合性チェック" -ForegroundColor Green
Write-Host "----------------------------------------"

$StartupOK = $AvgStartup -le 500
$MemoryOK = $ValidMemory.Count -eq 0 -or $AvgMemory -le 30

Write-Host "起動時間 (≤500ms): " -NoNewline
Write-Host $(if ($StartupOK) { "✓ PASS" } else { "✗ FAIL" }) -ForegroundColor $(if ($StartupOK) { "Green" } else { "Red" })

Write-Host "メモリ使用量 (≤30MB): " -NoNewline
Write-Host $(if ($MemoryOK) { "✓ PASS" } else { "✗ FAIL" }) -ForegroundColor $(if ($MemoryOK) { "Green" } else { "Red" })

# 4. 機能テスト（手動）
Write-Host ""
Write-Host "4. 手動テスト手順" -ForegroundColor Green
Write-Host "----------------------------------------"
Write-Host "以下の項目を手動でテストしてください:"
Write-Host ""
Write-Host "□ アプリ起動後、システムトレイにアイコンが表示される"
Write-Host "□ Ctrl+Shift+M でメモウィンドウが表示される"
Write-Host "□ メモウィンドウが画面中央に表示される"
Write-Host "□ テキストボックスに日本語入力ができる"
Write-Host "□ 「コピー&閉じる」ボタンでクリップボードにコピーされる"
Write-Host "□ Ctrl+Enter でもクリップボードにコピーされる"
Write-Host "□ Escapeキーでウィンドウが閉じる"
Write-Host "□ ウィンドウ外をクリックすると自動的に閉じる"
Write-Host "□ システムトレイの右クリックメニューが表示される"
Write-Host "□ 「終了」でアプリが完全に終了する"

# 5. 最終判定
Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
$OverallResult = $StartupOK -and $MemoryOK
Write-Host "総合結果: " -NoNewline
Write-Host $(if ($OverallResult) { "✓ PASS" } else { "✗ FAIL" }) -ForegroundColor $(if ($OverallResult) { "Green" } else { "Red" })
Write-Host "========================================" -ForegroundColor Cyan

if (-not $OverallResult) {
    Write-Host ""
    Write-Host "改善提案:" -ForegroundColor Yellow
    if (-not $StartupOK) {
        Write-Host "- 起動時間を短縮するため、不要な初期化処理を見直してください"
        Write-Host "- AOTコンパイル（ReadyToRun）が有効になっているか確認してください"
    }
    if (-not $MemoryOK) {
        Write-Host "- メモリ使用量を削減するため、不要なリソースの読み込みを避けてください"
        Write-Host "- ガベージコレクションの設定を確認してください"
    }
}

# ログファイル出力
$LogFile = "test\performance-results-$(Get-Date -Format 'yyyyMMdd-HHmmss').log"
@"
FlashMemo Performance Test Results
==================================
Test Date: $(Get-Date)
Test Count: $TestCount
Executable: $ExePath

Startup Times (ms): $($TestResults.StartupTimes -join ', ')
Memory Usage (MB): $($TestResults.MemoryUsage -join ', ')

Average Startup: ${AvgStartup}ms
Average Memory: ${AvgMemory}MB

Requirements Check:
- Startup Time (≤500ms): $(if ($StartupOK) { 'PASS' } else { 'FAIL' })
- Memory Usage (≤30MB): $(if ($MemoryOK) { 'PASS' } else { 'FAIL' })

Overall Result: $(if ($OverallResult) { 'PASS' } else { 'FAIL' })
"@ | Out-File -FilePath $LogFile -Encoding UTF8

Write-Host ""
Write-Host "詳細ログ: $LogFile" -ForegroundColor Gray

exit $(if ($OverallResult) { 0 } else { 1 })