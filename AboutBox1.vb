Public NotInheritable Class AboutBox1

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
        Me.LabelProductName.Text = "製品名： " & My.Application.Info.ProductName
        Me.LabelVersion.Text = "バージョン：" & String.Format("バージョン {0}", My.Application.Info.Version.ToString)
        Me.LabelCopyright.Text = "著作権： " & My.Application.Info.Copyright
        Me.LabelCompanyName.Text = "会社名： " & My.Application.Info.CompanyName
        'Me.TextBoxDescription.Text = "説明： " & My.Application.Info.Description

        Dim s = "QslOrganizerは、QSＬカードのイメージファイルを効率よく整理・管理するためのアプリケーション（以下、アプリ）です。
紙のQSLカードを電子化した場合、
•	画像の向きがバラバラ
•	ファイル名が統一されていない
•	整理ホルダーが混乱する
といった問題が発生します。
本アプリは、これらの作業を 高速・正確・簡単 に行うために設計されています。"

        Me.TextBoxDescription.Text = "説明： " & s

    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        Me.Close()
    End Sub

End Class
