# FlashMemo - パッとメモ

[![Windows](https://img.shields.io/badge/Windows-10%2F11-blue.svg)](https://www.microsoft.com/windows)
[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Release](https://img.shields.io/github/v/release/ekumaki/flash-memo)](https://github.com/ekumaki/flash-memo/releases)

Windows対応の超軽量・高速メモアプリです。ピン留め機能でいつでも参照でき、クリップボードにコピーして即座に他のアプリで活用できます。

## ✨ 特徴

- 🚀 **超高速起動**: 500ms以内でウィンドウが表示
- 📌 **ピン留め機能**: ウィンドウを最前面に固定可能
- ⌨️ **キーボードショートカット**: `Ctrl+Enter` でコピー、`Escape` で終了
- 📋 **ワンクリックコピー**: テキストをクリップボードにコピーして即座に終了
- 🪶 **軽量設計**: メモリ使用量 < 30MB
- 🔧 **簡単セットアップ**: ZIPファイルを展開するだけで使用可能

## 📱 スクリーンショット

![FlashMemo起動画面](docs/screenshot.png)

*ピン留め機能でメモを常に表示し、キーボードショートカットで高速操作*

## 🚀 クイックスタート

### システム要件

- Windows 10 バージョン1809 以降 / Windows 11
- .NET 8.0 Runtime ([ダウンロード](https://dotnet.microsoft.com/download/dotnet/8.0))

### セットアップ方法

1. **[Releases](https://github.com/ekumaki/flash-memo/releases)** から `FlashMemo-Portable-v1.1.zip` をダウンロード
2. 任意のフォルダに解凍（例：`C:\FlashMemo\`）
3. `FlashMemo.exe` をダブルクリックで起動

> **💡 ヒント**: デスクトップにショートカットを作成すると便利です

### 使い方

#### 基本操作
1. **起動**: `FlashMemo.exe` をダブルクリック
2. **メモ入力**: 表示されたウィンドウにテキストを入力
3. **コピー**: 「コピー&閉じる」ボタンを押すか `Ctrl+Enter`
4. **他アプリで使用**: `Ctrl+V` で貼り付け

#### 🆕 新機能：ピン留め
1. **ピン留め**: 📌 ボタンをクリック
2. **常に最前面**: 他のアプリを使用中も表示
3. **解除**: 📌 ボタンを再度クリック

### キーボードショートカット

| キー | 動作 |
|------|------|
| `Ctrl+Enter` | テキストをコピーして閉じる |
| `Escape` | ウィンドウを閉じる（コピーなし） |

### 操作パターン

| 用途 | 操作 |
|------|------|
| **一時メモ** | 入力 → `Ctrl+Enter` → 他アプリで貼り付け |
| **参照用** | 📌 ピン留め → 他の作業をしながら参照 |
| **下書き** | 入力 → `Escape` で破棄 |

## 🆕 v1.1.0の新機能

### 📌 ピン留め機能
- ウィンドウを最前面に固定
- 他のアプリ使用中も常に表示
- ボタンの透明度で状態を視覚的に確認

### ⌨️ キーボードショートカット強化
- `Ctrl+Enter`: 高速コピー&終了
- `Escape`: 高速終了
- より効率的な操作フロー

### 🎨 UI/UX改善
- ピン留めToggleButton追加
- ボタンレイアウト最適化
- ツールチップで操作説明

## 🔧 開発者向け情報

### ビルド方法

```bash
# リポジトリをクローン
git clone https://github.com/ekumaki/flash-memo.git
cd flash-memo

# 依存関係を復元
dotnet restore src/FlashMemo/FlashMemo.csproj

# リリースビルド
dotnet publish src/FlashMemo/FlashMemo.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -p:PublishReadyToRun=true -o build/output/Release
```

### プロジェクト構造

```
flash-memo/
├── src/
│   └── FlashMemo/           # メインアプリケーション
│       ├── Services/        # クリップボードサービス
│       ├── MainWindow.xaml  # メインUI
│       └── MainWindow.xaml.cs # メインロジック
├── tests/
│   └── FlashMemo.Tests/     # ユニットテスト
├── .github/workflows/       # CI/CD
└── docs/                    # ドキュメント
```

### アーキテクチャ

- **フロントエンド**: WPF (.NET 8)
- **クリップボード**: Windows Clipboard API
- **UI**: モダンなフラットデザイン
- **テスト**: xUnit + WPF Test Framework

### パフォーマンス最適化

- 単一ファイル出力で起動時間を短縮
- ReadyToRun（AOT）コンパイルで実行速度向上
- 最小限のUI要素で軽量化
- 効率的なメモリ管理

## 🧪 テスト

```bash
# ユニットテストを実行
dotnet test tests/FlashMemo.Tests/FlashMemo.Tests.csproj
```

テスト対象:
- ピン留め機能の動作確認
- キーボードショートカット
- クリップボード操作
- ウィンドウ初期化

## 📝 ライセンス

MIT License - 詳細は [LICENSE](LICENSE) ファイルを参照してください。

## 🤝 コントリビューション

Issues とPull Requestsを歓迎します！

### 開発ガイドライン

- コメントは日本語、変数名・関数名は英語で記載
- パフォーマンスを最優先に考慮
- シンプルで読みやすいコードを心がける
- テストカバレッジの維持

## 📞 サポート

- [Issues](https://github.com/ekumaki/flash-memo/issues) - バグ報告・機能要望
- [Discussions](https://github.com/ekumaki/flash-memo/discussions) - 質問・アイデア

## 🎯 ロードマップ

- [x] 基本的なメモ機能
- [x] クリップボード連携
- [x] ピン留め機能 **🆕 v1.1.0**
- [x] キーボードショートカット強化 **🆕 v1.1.0**
- [ ] ホットキーのカスタマイズ設定
- [ ] メモ履歴機能（オプション）
- [ ] ダークテーマ対応
- [ ] Microsoft Store配布

## ❓ よくある質問

**Q: 起動しない / エラーが出る**  
A: .NET 8.0 Runtimeがインストールされているか確認してください

**Q: ピン留めが効かない**  
A: 他のアプリが最前面固定されている可能性があります

**Q: 設定は保存される？**  
A: 現在のバージョンでは設定保存は非対応です（軽量化のため）

**Q: アンインストール方法は？**  
A: フォルダごと削除するだけでOKです（レジストリ変更なし）

---

**FlashMemo** - 思いついたアイデアを瞬時に記録、即座に活用。