# FlashMemo - パッとメモ

[![Windows](https://img.shields.io/badge/Windows-11-blue.svg)](https://www.microsoft.com/windows)
[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

Windows 11対応の超軽量・高速メモアプリです。グローバルホットキーでいつでも瞬時にメモを取り、クリップボードにコピーして即座に他のアプリで活用できます。

## ✨ 特徴

- 🚀 **超高速起動**: 500ms以内でウィンドウが表示
- ⌨️ **グローバルホットキー**: `Ctrl+Shift+M` でいつでも起動
- 📋 **ワンクリックコピー**: テキストをクリップボードにコピーして即座に終了
- 🪶 **軽量設計**: 常駐時のメモリ使用量 < 30MB
- 🎯 **フォーカス重視**: ウィンドウ外クリックで自動的に非表示
- 🔧 **設定不要**: インストール後すぐに使用可能

## 📱 スクリーンショット

![FlashMemo起動画面](docs/screenshot.png)

*グローバルホットキーでメモウィンドウが瞬時に表示されます*

## 🚀 クイックスタート

### システム要件

- Windows 11 (64-bit)
- .NET 8.0 Runtime (自己完結版は不要)

### インストール方法

1. **ポータブル版（推奨）**
   ```
   1. Releases から FlashMemo-Portable.zip をダウンロード
   2. 任意のフォルダに解凍
   3. FlashMemo.exe を実行
   ```

2. **自己完結版（.NET Runtime不要）**
   ```
   1. Releases から FlashMemo-Standalone.zip をダウンロード
   2. 任意のフォルダに解凍
   3. FlashMemo.exe を実行
   ```

### 使い方

1. **アプリ起動**: `FlashMemo.exe` を実行
2. **メモ作成**: `Ctrl+Shift+M` を押す
3. **メモ入力**: 表示されたウィンドウにテキストを入力
4. **コピー**: 「コピー&閉じる」ボタンを押すか `Ctrl+Enter`
5. **他アプリで使用**: クリップボードの内容を `Ctrl+V` で貼り付け

### キーボードショートカット

| キー | 動作 |
|------|------|
| `Ctrl+Shift+M` | メモウィンドウを表示 |
| `Ctrl+Enter` | テキストをコピーして閉じる |
| `Escape` | ウィンドウを閉じる（コピーなし） |

## 🔧 開発者向け情報

### ビルド方法

```bash
# リポジトリをクローン
git clone https://github.com/your-username/FlashMemo.git
cd FlashMemo

# ビルドスクリプトを実行（Windows）
build\build.bat

# または手動ビルド
dotnet restore src/FlashMemo/FlashMemo.csproj
dotnet build src/FlashMemo/FlashMemo.csproj -c Release
```

### プロジェクト構造

```
FlashMemo/
├── src/
│   ├── FlashMemo/           # メインアプリケーション
│   │   ├── Services/        # サービスクラス
│   │   ├── *.xaml          # WPF UI定義
│   │   └── *.cs            # C# ソースコード
│   └── Assets/             # アイコンなどのリソース
├── build/                  # ビルドスクリプト
├── test/                   # テストスクリプト
└── docs/                   # ドキュメント
```

### アーキテクチャ

- **フロントエンド**: WPF (.NET 8)
- **ホットキー**: Win32 API（RegisterHotKey）
- **クリップボード**: Windows Clipboard API
- **UI**: モダンなフラットデザイン

### パフォーマンス最適化

- 単一ファイル出力で起動時間を短縮
- ReadyToRun（AOT）コンパイルで実行速度向上
- 最小限のUI要素で軽量化
- バックグラウンド初期化の遅延実行

## 🧪 テスト

パフォーマンステストを実行:

```powershell
# Windows PowerShell
.\test\test-performance.ps1
```

テスト項目:
- 起動時間測定（500ms以内）
- メモリ使用量測定（30MB以内）
- クリップボード機能テスト
- ホットキー応答テスト

## 📝 ライセンス

MIT License - 詳細は [LICENSE](LICENSE) ファイルを参照してください。

## 🤝 コントリビューション

Issues とPull Requestsを歓迎します！

### 開発ガイドライン

- コメントは日本語、変数名・関数名は英語で記載
- パフォーマンスを最優先に考慮
- シンプルで読みやすいコードを心がける

## 📞 サポート

- [Issues](https://github.com/your-username/FlashMemo/issues) - バグ報告・機能要望
- [Discussions](https://github.com/your-username/FlashMemo/discussions) - 質問・アイデア

## 🎯 ロードマップ

- [x] 基本的なメモ機能
- [x] グローバルホットキー
- [x] クリップボード連携
- [ ] ホットキーのカスタマイズ設定
- [ ] メモ履歴機能（オプション）
- [ ] ダークテーマ対応
- [ ] Microsoft Store配布

---

**FlashMemo** - 思いついたアイデアを瞬時に記録、即座に活用。