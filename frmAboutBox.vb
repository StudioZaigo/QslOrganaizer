Public NotInheritable Class frmAboutBox

    Private Sub AboutBox1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' フォームのタイトルを設定します。
        Dim ApplicationTitle As String
        If My.Application.Info.Title <> "" Then
            ApplicationTitle = My.Application.Info.Title
        Else
            ApplicationTitle = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        Me.Text = String.Format("バージョン情報 {0}", ApplicationTitle)

        ' バージョン情報ボックスに表示されたテキストをすべて初期化します。
        ' TODO: [プロジェクト] メニューの下にある [プロジェクト プロパティ] ダイアログの [アプリケーション] ペインで、アプリケーションのアセンブリ情報を 
        '    カスタマイズします。

        ' --- 見た目の調整（お好みで） ---
        lstInformation.BorderStyle = BorderStyle.None    ' 枠線を消してすっきりさせる
        lstInformation.SelectionMode = SelectionMode.None ' クリックしても青く選択されないようにする
        'lstInformation.Font = New Font("MS UI Gothic", 10) ' 見やすいフォントに変更

        ' --- ListBox タブ幅（Offset）の設定 ---
        lstInformation.UseTabStops = True
        lstInformation.CustomTabOffsets.Clear()

        '' ★ここを調整します（40〜60くらいが目安です。文字が重なる場合は大きくしてください）
        lstInformation.CustomTabOffsets.Add(150)

        ' --- データの追加 ---
        ' 「項目名」と「値」の間には必ず「vbTab」を挟んでください
        lstInformation.Items.Add("製品名" & vbTab & My.Application.Info.Title)
        lstInformation.Items.Add("組織名" & vbTab & My.Application.Info.CompanyName)
        lstInformation.Items.Add("著作権" & vbTab & My.Application.Info.Copyright)
        lstInformation.Items.Add("バージョン" & vbTab & String.Format("バージョン {0}", My.Application.Info.Version.ToString))


        ' 今後項目が増えても、このように1行追加するだけで自動的に整列します
        'lstInformation.Items.Add("ライセンス" & vbTab & "個人利用限定ライセンス")
        'lstInformation.Items.Add("リリース日" & vbTab & "2026/09/16")

        ' mscorlib（.NETの基盤アセンブリ）のバージョンを取得
        ' これにより、どの世代のコンパイラ環境でビルドされたかが分かります
        Dim vbRuntimeVer As String = GetType(String).Assembly.GetName().Version.ToString()
        lstInformation.Items.Add("CORE Lib" & vbTab & vbRuntimeVer)

        ' 表示例: ".NET Framework 4.8.9139.0" など
        Dim netVer As String = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription
        lstInformation.Items.Add(".NET" & vbTab & netVer)

        Dim TessVersion = frmMain.OcrEngine.GetTesseractVersion()
        lstInformation.Items.Add("Tesseract" & vbTab & TessVersion)

        Dim ZXingVersion = frmMain.GetZXingVersion()
        lstInformation.Items.Add("ZXing" & vbTab & ZXingVersion)

        'lstInformation.Padding()

        Dim s = "QslOrganizerは、QSＬカードのイメージファイルを効率よく整理・管理するためのアプリケーション（以下、アプリ）です。
紙のQSLカードを電子化した場合、
•	画像の向きがバラバラ
•	ファイル名が統一されていない
•	整理ホルダーが混乱する
といった問題が発生します。
本アプリは、これらの作業を 高速・正確・簡単 に行うために設計されています。"

        Me.TextBoxDescription.Text = "説明： 　　" & s

    End Sub

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        Close()
    End Sub

End Class
