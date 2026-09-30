Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports QslOrganizer.frmMain


Public Class frmSetting

    Private Sub frmSetting_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        btnOutputFolder.Text = ""
        btnOutputFolder.Text = ""

        lblComment.Text = "複数のCallsinは、カンマ区切りで入力"
        lblGoogleNote.Text = "Google Clude Vision AI を使用する時に設定する"
        lblAzureNote.Text = "Micrsoft Azure AI を使用する時に設定する"

        lblAzureApiKey.Location = lblGoogleApiKey.Location
        lblAzureUrl.Location = lblGoogleUrl.Location
        lblAzureNote.Location = lblGoogleNote.Location
        txtAzureApiKey.Location = txtGoogleApiKey.Location
        txtAzureUrl.Location = txtGoogleURL.Location
        chkUseAzure.Location = chkUseGoogle.Location

        '' 設定を読み込み
        'AppSettings.LoadJson(AppSettings.SettingsFile)

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
        Dim GoogleUrl As String = $"https://vision.googleapis.com/v1/images:annotate?key="
        Dim AzureUrl As String = $"https://qslorganizer.cognitiveservices.azure.com/"

        txtMyCallsigns.Text = AppSettings.GetJson("General", "MyCallsigns", "")
        txtInputFolder.Text = AppSettings.GetJson("General", "InputFolder", "")
        txtOutputFolder.Text = AppSettings.GetJson("General", "OutputFolder", "")
        txtFontSize.Text = AppSettings.GetJson("General", "FontSize", "9")

        txtGoogleApiKey.Text = AppSettings.GetJson("Google", "ApiKey", "")
        txtGoogleURL.Text = AppSettings.GetJson("Google", "URL", GoogleUrl)
        Dim s = AppSettings.GetJson("Google", "UseGoogle", "True")
        chkUseGoogle.Checked = Boolean.Parse(AppSettings.GetJson("Google", "UseGoogle", "True"))

        txtAzureApiKey.Text = AppSettings.GetJson("Azure", "ApiKey", "")
        txtAzureUrl.Text = AppSettings.GetJson("Azure", "Url", AzureUrl)
        chkUseAzure.Checked = Boolean.Parse(AppSettings.GetJson("Azure", "UseAzure", "False"))

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

        txtFontSize.Text = txtFontSize.Text.Trim
        If txtFontSize.Text = "" Then
            MessageBox.Show("フォントサイズを入力してください。", "入力エラー",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtFontSize.Focus()
            Return
        ElseIf Not Double.TryParse(txtFontSize.Text, Nothing) Then
            MessageBox.Show("フォントサイズは数字のみで入力してください。", "入力エラー",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtFontSize.Focus()
        End If

        AppSettings.LoadJson(AppSettings.SettingsFile)

        AppSettings.SetJson("General", "MyCallsigns", txtMyCallsigns.Text)
        AppSettings.SetJson("General", "InputFolder", txtInputFolder.Text)
        AppSettings.SetJson("General", "OutputFolder", txtOutputFolder.Text)
        AppSettings.SetJson("General", "FontSize", txtFontSize.Text)

        AppSettings.SetJson("Google", "ApiKey", txtGoogleApiKey.Text)
        AppSettings.SetJson("Google", "URL", txtGoogleURL.Text)
        Dim s = chkUseGoogle.Checked.ToString
        AppSettings.SetJson("Google", "UseGoogle", chkUseGoogle.Checked.ToString)

        AppSettings.SetJson("Azure", "ApiKey", txtAzureApiKey.Text)
        AppSettings.SetJson("Azure", "URL", txtAzureUrl.Text)
        AppSettings.SetJson("Azure", "UseAzure", chkUseAzure.Checked.ToString)

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

    Private Sub txtFontSize_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtFontSize.KeyPress
        ' 数字、コントロールキー（バックスペースなど）、小数点を許可
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> "."c Then
            e.Handled = True
        End If

        ' 2. 小数点に関する制限
        If e.KeyChar = "."c Then
            ' すでに小数点がある場合は、2つ目を拒否
            If txtFontSize.Text.Contains(".") Then
                e.Handled = True
                Return
            End If
        End If

        ' 3. 小数点以下の桁数制限（数字が入力された場合のみチェック）
        If Char.IsDigit(e.KeyChar) Then
            Dim dotIndex As Integer = txtFontSize.Text.IndexOf("."c)

            ' すでに小数点が存在する場合
            If dotIndex >= 0 Then
                ' カーソル位置が小数点の右側にあるかチェック
                If txtFontSize.SelectionStart > dotIndex Then
                    ' 小数点以下の文字列を取得（選択反転して消える文字数は除外）
                    Dim decimalPart As String = txtFontSize.Text.Substring(dotIndex + 1)
                    Dim currentLength As Integer = decimalPart.Length - txtFontSize.SelectionLength

                    ' すでに2桁ある場合は入力を拒否
                    If currentLength >= 2 Then
                        e.Handled = True
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub chkUseGoogle_CheckedChanged(sender As Object, e As EventArgs) Handles chkUseGoogle.CheckedChanged
        chkUseAzure.Checked = Not chkUseGoogle.Checked
    End Sub

    Private Sub chkUseAzure_CheckedChanged(sender As Object, e As EventArgs) Handles chkUseAzure.CheckedChanged
        chkUseGoogle.Checked = Not chkUseAzure.Checked
    End Sub
End Class