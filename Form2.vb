Public Class Form2

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnOutputFolder.Text = ""
        btnOutputFolder.Text = ""

        'txtCallsign.Text = Form1.MyCallsign
        'txtInputFolder.Text = Form1.InputFolder
        'txtOutputFolder.Text = Form1.OutputFolder

    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        ' --- データチェック ---
        If txtCallsign.Text.Trim() = "" Then
            MessageBox.Show("コールサインを入力してください。", "入力エラー",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning)
            '           e.Cancel = True   ' ← クローズをキャンセル
            txtCallsign.Focus()
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

        'Form1.MyCallsign = txtCallsign.Text
        'Form1.InputFolder = txtInputFolder.Text
        'Form1.OutputFolder = txtOutputFolder.Text
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnOutputFolder_Click(sender As Object, e As EventArgs) Handles btnOutputFolder.Click
        Dim dlg As New FolderBrowserDialog With
        {
        .ShowNewFolderButton = False,
        .Description = "Output Folder"
        }
        If dlg.ShowDialog = DialogResult.OK Then
            txtOutputFolder.Text = dlg.SelectedPath
        End If
    End Sub

    Private Sub btnInputFolder_Click(sender As Object, e As EventArgs) Handles btnInputFolder.Click
        Dim dlg As New FolderBrowserDialog With
        {
        .ShowNewFolderButton = False,
        .Description = "Input Folder"
        }
        If dlg.ShowDialog = DialogResult.OK Then
            txtInputFolder.Text = dlg.SelectedPath
        End If
    End Sub
End Class