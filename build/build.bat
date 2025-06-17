@echo off
REM FlashMemo（パッとメモ）ビルドスクリプト
REM Windows 11 x64対応の高速メモアプリ

echo ========================================
echo FlashMemo Build Script
echo ========================================

REM 前提条件チェック
where dotnet >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: .NET 8 SDK がインストールされていません
    echo https://dotnet.microsoft.com/download/dotnet/8.0 からダウンロードしてください
    pause
    exit /b 1
)

REM プロジェクトディレクトリに移動
cd /d "%~dp0.."
if not exist "src\FlashMemo\FlashMemo.csproj" (
    echo ERROR: プロジェクトファイルが見つかりません
    pause
    exit /b 1
)

echo 現在のディレクトリ: %CD%

REM 出力ディレクトリをクリア
echo 出力ディレクトリをクリアしています...
if exist "build\output" rmdir /s /q "build\output"
mkdir "build\output"

REM リストア実行
echo NuGetパッケージを復元しています...
dotnet restore "src\FlashMemo\FlashMemo.csproj"
if %errorlevel% neq 0 (
    echo ERROR: パッケージの復元に失敗しました
    pause
    exit /b 1
)

REM デバッグビルド
echo デバッグビルドを実行しています...
dotnet build "src\FlashMemo\FlashMemo.csproj" -c Debug -o "build\output\Debug"
if %errorlevel% neq 0 (
    echo ERROR: デバッグビルドに失敗しました
    pause
    exit /b 1
)

REM リリースビルド（単一ファイル出力）
echo リリースビルドを実行しています...
dotnet publish "src\FlashMemo\FlashMemo.csproj" ^
    -c Release ^
    -r win-x64 ^
    --self-contained false ^
    -p:PublishSingleFile=true ^
    -p:PublishReadyToRun=true ^
    -p:EnableCompressionInSingleFile=true ^
    -o "build\output\Release"

if %errorlevel% neq 0 (
    echo ERROR: リリースビルドに失敗しました
    pause
    exit /b 1
)

REM ポータブル版作成
echo ポータブル版を作成しています...
mkdir "build\output\Portable"
copy "build\output\Release\FlashMemo.exe" "build\output\Portable\"
copy "README.md" "build\output\Portable\"

REM 自己完結型ビルド（.NET Runtime不要版）
echo 自己完結型ビルドを実行しています...
dotnet publish "src\FlashMemo\FlashMemo.csproj" ^
    -c Release ^
    -r win-x64 ^
    --self-contained true ^
    -p:PublishSingleFile=true ^
    -p:PublishReadyToRun=true ^
    -p:EnableCompressionInSingleFile=true ^
    -p:PublishTrimmed=true ^
    -o "build\output\Standalone"

if %errorlevel% neq 0 (
    echo WARNING: 自己完結型ビルドに失敗しました（オプション）
)

REM ファイルサイズ確認
echo.
echo ========================================
echo ビルド結果:
echo ========================================
if exist "build\output\Release\FlashMemo.exe" (
    for %%f in ("build\output\Release\FlashMemo.exe") do echo リリース版: %%~zf bytes
)
if exist "build\output\Standalone\FlashMemo.exe" (
    for %%f in ("build\output\Standalone\FlashMemo.exe") do echo 自己完結版: %%~zf bytes
)

echo.
echo ビルド完了しました！
echo 出力ディレクトリ: %CD%\build\output
echo.
echo 実行方法:
echo - デバッグ版: build\output\Debug\FlashMemo.exe
echo - リリース版: build\output\Release\FlashMemo.exe
echo - ポータブル版: build\output\Portable\FlashMemo.exe
echo - 自己完結版: build\output\Standalone\FlashMemo.exe
echo.
pause