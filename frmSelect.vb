Imports QslOrganizer.frmMain


Partial Class frmSelect

    Private Sub frmSelect_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PicOldQsl.Dock = DockStyle.Fill

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

        ' ラベルのBackColorをTransparentに設定
        Label1.BackColor = Color.Transparent

        ' 親コントロールをPictureBoxにする
        Label1.Parent = PicOldQsl

        ' PictureBoxを基準にした位置に座標を調整
        Dim point1 = PicOldQsl.Location
        Label1.Location = point1
    End Sub


    Private Sub frmSelect_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' 設定を保存
        AppSettings.LoadJson(AppSettings.SettingsFile)

        Dim AppWindowTop = Me.Top
        Dim AppWindowLeft = Me.Left

        AppSettings.SetJson(Me.Name, "Top", AppWindowTop.ToString())
        AppSettings.SetJson(Me.Name, "Left", AppWindowLeft.ToString())

        AppSettings.SaveJson(AppSettings.SettingsFile)

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs)
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        DialogResult = DialogResult.OK
        Close
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub
End Class