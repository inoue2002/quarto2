# Quarto Unity ゲーム

Quartoを実装した Unity ゲームです。

## 概要

Quartoは2人用の戦略ボードゲームです。16個のピースを4×4の盤面に配置し、4つ並べることで勝利します。

## 操作方法

### 基本操作

ゲーム内での操作は以下の通りです。

#### キーボード操作
- **W/A/S/D** - カーソル移動
- **Q/E** - ピース選択
- **スペースキー** - ピース配置/決定
- **P** - パス（スキップ）

#### マウス操作
- **左クリック + ドラッグ** - カメラ回転
- **右クリック + ドラッグ** - カメラ移動

### ゲームルール
- マウスでピースを選択して盤面に配置します。

## ファイル構成 

- `Assets/Scripts/Backend/` - ゲームロジック
- `Assets/Scripts/Frontend/` - UI関連
- `Assets/Scripts/Frontend/UIScripts/CameraMover.cs` - カメラ操作

## 環境

- Unity 6000.0.32f1
- Visual Studio 2022

## ビルド手順

1. Unityでプロジェクトを開く
2. File > Build Settings を選択
3. プラットフォームを選択してビルド
