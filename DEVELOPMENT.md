# FlashMemo 開発ガイドライン

## 🌳 ブランチ戦略

### メインブランチ
- **`main`** - 本番用安定版（リリース済みコード）
- **`develop`** - 開発統合ブランチ（次期リリース準備中）

### フィーチャーブランチ
- **`feature/hotkey-customization`** - ホットキーカスタマイズ機能
- **`feature/memo-history`** - メモ履歴機能  
- **`feature/dark-theme`** - ダークテーマ対応

## 🔄 開発ワークフロー

### 1. 新機能開発の場合

```bash
# developブランチから新しいフィーチャーブランチを作成
git checkout develop
git pull origin develop
git checkout -b feature/new-feature-name

# 開発作業実施
# コミット & プッシュ

# developにマージ
git checkout develop
git merge feature/new-feature-name
git branch -d feature/new-feature-name
```

### 2. バグ修正の場合

```bash
# mainまたはdevelopから修正ブランチを作成
git checkout main  # または develop
git checkout -b bugfix/fix-description

# 修正作業実施
# テスト実行
# コミット & プッシュ

# 該当ブランチにマージ
git checkout main  # または develop
git merge bugfix/fix-description
git branch -d bugfix/fix-description
```

### 3. リリース準備

```bash
# developからリリースブランチを作成
git checkout develop
git checkout -b release/v1.1.0

# バージョン番号更新、最終テスト
# コミット

# mainにマージしてタグ付け
git checkout main
git merge release/v1.1.0
git tag -a v1.1.0 -m "Release version 1.1.0"

# developにもマージ
git checkout develop
git merge release/v1.1.0

# リリースブランチ削除
git branch -d release/v1.1.0
```

## 🧪 開発前のテスト

新機能開発前に現在のコードが正常動作することを確認：

```bash
# ビルドテスト
build\build.bat

# パフォーマンステスト
powershell -ExecutionPolicy Bypass -File test\test-performance.ps1

# 手動テスト項目確認（README参照）
```

## 📝 コミットメッセージ規約

```
<type>(<scope>): <subject>

<body>

<footer>
```

### Type
- `feat`: 新機能
- `fix`: バグ修正
- `docs`: ドキュメント更新
- `style`: コードスタイル修正
- `refactor`: リファクタリング
- `test`: テスト追加・修正
- `chore`: その他の作業

### 例
```
feat(hotkey): ホットキーカスタマイズ機能を追加

- ユーザー設定でホットキーを変更可能に
- 設定画面の追加
- レジストリへの設定保存機能

Closes #123
```

## 🎯 次期開発予定

### v1.1.0 計画
- [ ] ホットキーカスタマイズ設定
- [ ] 設定画面UI
- [ ] レジストリ設定保存

### v1.2.0 計画  
- [ ] メモ履歴機能（オプション）
- [ ] 履歴の検索・フィルタ
- [ ] エクスポート機能

### v1.3.0 計画
- [ ] ダークテーマ対応
- [ ] テーマ切り替え設定
- [ ] システムテーマ自動追従

## 🐛 既知の課題

- [ ] 一部の環境でホットキー登録が失敗する場合がある
- [ ] 高DPI環境でのUI表示調整が必要
- [ ] Windows 10での動作確認が不十分

## 🚀 パフォーマンス目標

- 起動時間: < 500ms（現在達成済み）
- メモリ使用量: < 30MB（現在達成済み） 
- インストーラサイズ: < 20MB
- バイナリサイズ: < 10MB

---

**現在のブランチ**: `develop`
**次の作業**: 機能拡張またはバグ修正を選択して対応するブランチで開発開始