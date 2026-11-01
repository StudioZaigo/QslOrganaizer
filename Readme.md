# 1．はじめに

QslOrganizerは、QSＬカードのイメージファイルを効率よく整理- 管理するためのアプリケーション（以下、アプリ）です。<br>紙のQSLカードを電子化した場合、

- 画像の向きがバラバラ2026-09-30 第6版	Ver 1.1.3.0対応

- ファイル名が統一されていない

- 整理ホルダーが混乱する

といった問題が発生します。  
本アプリは、これらの作業を 高速- 正確- 簡単 に行うために設計されています。



# 2．アプリ概要

主な機能

- 画像の回転（Shift＋クリック）

- オフラインOCR（Tesseract）

- オンラインOCR(Google, Azure)

- QRコード読み取り(ZXing)

- Callsign / Date / Time / Band / Mode の自動抽出

- ファイル名の自動生成

- 国内局- 海外局に応じたホルダー自動仕分け

- Autoモードによる連続処理

- キーボード操作による高速入力


# 3．対応画像形式

- JPEG / JPG / PNG / BMP / GIF / TIFF

（すべての形式を完全にテストしているわけではありません）



# 4．使用技術

- OCR：Google, Azuru, Tesseract

- Callsign判定：CTY.CSV（country-files.com）
- 正規表現


# 5． 導入方法
- ブラウザーで[StudioZaigo/QslOrganizer](https://www7b.biglobe.ne.jp/~studiozaigo/QslOrganizer.htm)のページを表示する。
- QslOrganizer V1.1.X.X.zipファイルをダウンロードする。
- zipファイルを展開する。
- 展開したフォルダー内のQslOrganizer.exeが実行ファイルです。
- 展開したフォルダーを適切なドライブ、フォルダーに移動して実行してください。



# 6．履歴

### 2026-11-01 Ver 1.1.3.2
- OCRの検出文字がキリル文字と認識されることがある。ラテン文字に置き換えるようにした。
- QRコード処理のあと、OCR処理が行われ項目の検出に誤りが出ることがあった。修正した(Many thanks JI2SVL)

### 2026-10-27 Ver 1.1.3.1
- 和暦（令和〇年〇月〇日、R〇年〇月〇日、）を読み取るようにした。
- 上向き矢印（↑）で「Shift＋Tab」機能を実行する。「Shift＋Enter」よりキー操作が楽になります。
- Save時、Callsign\_yyyymmdd\_hhnn\_bbb\_mmmm形式の同一ファイル名がすでに存在したとき、残すファイルを選択できるようにした。
- AzureでのOCR時、「429 Too many requests- - - 」のエラー発生頻度を削減した。
- Google, Azure, Studiozaigo/QslOrganizerのサイトへのリンクメニューを追加した。
- ファイル名とフォルダー名の不一致を整理するPower Shell「QslTidyup.ps1」を作成した。
- Amazon AWSのOCR機能を組み込もうとしましたが、AWSの設定が難しく諦めました。
- ”Setting”を”Settings”に変更した。
- SettingsでGoogle, AzureのURLを変更できるようにしました。（Many thanks to JI2SVL）
- 細かな誤りの修正





### 2026-09-30 Ver 1.1.3.0
- Online読み取りにMicrosoft Azure　Vision AIを利用できるようにした。
- 細かな誤りの修正

### 2026-09-26 Ver 1.1.2.5a
- Autoモードの時、対象ファイルが「jpg」のみになっていた。「jpeg、png、bmp」を追加した
- 入力ファイル名にCallsign、Date～Modeがあるとき、OCRでは上書きをしないようにした
- TextBox、ListBox項目のフォントをYu Gothic UIからConsolasに変更した。これにより英字「O」と数字「0」の&#x09;判別を容易にした
- 設定画面にフォントサイズ設定することによりメイン画面の大きさを変えられるようにした
- 今回は、Callsignの読み取り精度の向上に努めた
- さらに読み取り精度向上、誤りの訂正をした
- 履歴ファイルを出力するようにした。
- 「Auto」で処理したとき、次に呼ぼ出されたファイルは、「Offlien」処理に戻っていた。<br>　これを「Online」、「Offline」処理が継続するようにした。
- 「AboutBox」を見直した。
- 「Auto」モードの処理を見直した。

### 2026-08-31 Ver 1.1.2.4
- 複数QRコードの読み取りに対応
- Save時に同名ファイルある場合、入力ファイルを削除可能にした
- さらに読み取り精度向上を目指した

### 2026-08-15 Ver 1.1.2.2
- QRコードの読み取り(DARC,ADIF形式など)
- 画像の回転をKBで行えるようした「Ctrl＋→」「Ctrl＋←」
- 画像ファイルを直接削除を行えるようにした「Ctrl＋B」
- CallsignなしQSLカードため、Callsign\_Xの目印をつけられるようにした
- さらに読み取り精度向上を目指した

### 2026-07-21 Ver 1.1.2
- GoogleのCloud Vision APIによりOnline OCR機能の付加<br>（OfflineとOnlineでOCR結果のBack colorが変わります）
- QRコードの読み取り
- CallsignにSWL番号入力化に
- 画像ファイル一覧表作成

### 2026-06-30 Ver 1.1.0
- ctl+DEL操作の見直し
- Callsign読み取り機能　画像の縮小化,グレースケール化見直し
- 時間抽出ロジックの誤り修正



# 7．課題

- Callsignの読み取り精度向上（グレースケール化 → ノイズ除去、高品質縮小 → 輪郭が整う）

- Tessetactへ斜線付０の学習

- 同じ画像の削除（callsignのみのデータ）

- PDFへの対応

- ご読み取り辞書を外部ファイル化

