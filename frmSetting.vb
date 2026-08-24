Imports QslOrganizer.frmMain


Public Class frmSetting

    Private Sub frmSetting_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        btnOutputFolder.Text = ""
        btnOutputFolder.Text = ""

        lblComment.Text = "複数のCallsinは、カンマ区切りで入力"
        lblNote.Text = "Google Clude Vision AI を使用する時に設定する"

        ' 設定を読み込み
        AppSettings.LoadJson(AppSettings.SettingsFile)

        Dim DisplayWorkRect As Rectangle
        DisplayWorkRect = Screen.PrimaryScreen.WorkingArea

        Dim AppWindowTop = AppSettings.GetJson(Me.Name, "Top", "-1")
        Dim AppWindowLeft = AppSettings.GetJson(Me.Name, "Left", "-1")
        Dim TabIndex = AppSettings.GetJson(Me.Name, "TabIndex", "0")

        If (AppWindowTop = -1) AndAlso (AppWindowLeft = -1) Then
            Me.Top = (DisplayWorkRect.Height - Me.Height) / 2
            Me.Left = (DisplayWorkRect.Width - Me.Width) / 2
        Else
            Me.Top = AppWindowTop
            Me.Left = AppWindowLeft
        End If

        ' 設定を読み込み
        AppSettings.LoadJson(AppSettings.SettingsFile)
        Dim url As String = $"https://vision.googleapis.com/v1/images:annotate?key="

        txtMyCallsigns.Text = AppSettings.GetJson("General", "MyCallsigns", "")
        txtInputFolder.Text = AppSettings.GetJson("General", "InputFolder", "")
        txtOutputFolder.Text = AppSettings.GetJson("General", "OutputFolder", "")
        txtGoogleApiKey.Text = AppSettings.GetJson("Google", "ApiKey", "")
        txtGoogleURL.Text = AppSettings.GetJson("Google", "URL", url)
        TabControl1.TabIndex = TabIndex
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        ' --- データチェック ---
        If txtMyCallsigns.Text.Trim = "" Then
            MessageBox.Show("コールサインを入力してください。", "入力エラー",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning)
            '           e.Cancel = True   ' ← クローズをキャンセル
            txtMyCallsigns.Focus()
            Return
        End If

        If txtInputFolder.Text.Trim = "" Then
            MessageBox.Show("入力フォルダを入力してください。", "入力エラー",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            '           e.Cancel = True   ' ← クローズをキャンセル
            txtInputFolder.Focus()
            Return
        End If

        If txtOutputFolder.Text.Trim = "" Then
            MessageBox.Show("出力フォルダを入力してください。", "入力エラー",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            '          e.Cancel = True   ' ← クローズをキャンセル
            txtOutputFolder.Focus()
            Return
        End If

        AppSettings.LoadJson(AppSettings.SettingsFile)

        AppSettings.SetJson("General", "MyCallsigns", txtMyCallsigns.Text)
        AppSettings.SetJson("General", "InputFolder", txtInputFolder.Text)
        AppSettings.SetJson("General", "OutputFolder", txtOutputFolder.Text)
        AppSettings.SetJson("Google", "ApiKey", txtGoogleApiKey.Text)
        AppSettings.SetJson("Google", "URL", txtGoogleURL.Text)

        AppSettings.SaveJson(AppSettings.SettingsFile)

        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

    Private Sub btnOutputFolder_Click(sender As Object, e As EventArgs) Handles btnOutputFolder.Click
        If txtOutputFolder.Text = "" Then
            txtOutputFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
        End If
        Dim dlg As New FolderBrowserDialog With
        {
        .SelectedPath = txtOutputFolder.Text,
        .ShowNewFolderButton = False,
        .Description = "Output Folder"
        }
        If dlg.ShowDialog = DialogResult.OK Then
            txtOutputFolder.Text = dlg.SelectedPath
        End If
    End Sub

    Private Sub btnInputFolder_Click(sender As Object, e As EventArgs) Handles btnInputFolder.Click
        If txtInputFolder.Text = "" Then
            txtInputFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
        End If
        Dim dlg As New FolderBrowserDialog With
        {
        .SelectedPath = txtInputFolder.Text,
        .ShowNewFolderButton = False,
        .Description = "Input Folder"
        }
        If dlg.ShowDialog = DialogResult.OK Then
            txtInputFolder.Text = dlg.SelectedPath
        End If
    End Sub

    Private Sub frmSetting_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' 設定を保存
        AppSettings.LoadJson(AppSettings.SettingsFile)

        Dim AppWindowTop = Me.Top
        Dim AppWindowLeft = Me.Left

        AppSettings.SetJson(Me.Name, "Top", AppWindowTop.ToString())
        AppSettings.SetJson(Me.Name, "Left", AppWindowLeft.ToString())
        AppSettings.SetJson(Me.Name, "TabIndex", TabControl1.TabIndex.ToString())

        AppSettings.SaveJson(AppSettings.SettingsFile)

    End Sub
End Class