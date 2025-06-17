# アイコンファイルについて

本プロジェクトでは以下のサイズのアイコンファイルが必要です：

- app-16.ico (16x16px) - システムトレイ用
- app-32.ico (32x32px) - ウィンドウタイトルバー用  
- app-48.ico (48x48px) - タスクバー用
- app-256.ico (256x256px) - アプリケーション用

## アイコンの作成方法

1. デザインソフトウェア（Figma、Adobe Illustrator等）で以下のデザインを作成：
   - 背景：明るい青色（#0078D4）
   - メインアイコン：白色のメモ帳またはノートのシルエット
   - シンプルで視認性の高いデザイン

2. 各サイズでPNGファイルをエクスポート

3. PNGファイルを.icoファイルに変換
   - オンラインツール: https://convertio.co/png-ico/
   - またはWindows SDK付属のrc.exeを使用

## 暫定対応

開発中は以下のコマンドでWindows標準アイコンをコピーして使用できます：

```bash
# Windows環境での例
copy "%SystemRoot%\System32\shell32.dll,70" app-16.ico
copy "%SystemRoot%\System32\shell32.dll,70" app-32.ico  
copy "%SystemRoot%\System32\shell32.dll,70" app-48.ico
copy "%SystemRoot%\System32\shell32.dll,70" app-256.ico
```

実際のリリース時には専用のアイコンに差し替えてください。