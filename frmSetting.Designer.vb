<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSetting
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSetting))
        txtOutputHolder = New TextBox()
        Panel1 = New Panel()
        btnOK = New Button()
        btnCancel = New Button()
        TabControl1 = New TabControl()
        TabPage1 = New TabPage()
        lblCallsign = New Label()
        lblInputFolder = New Label()
        lblOutputFolder = New Label()
        lblComment = New Label()
        txtMyCallsigns = New TextBox()
        txtInputFolder = New TextBox()
        btnInputFolder = New Button()
        txtOutputFolder = New TextBox()
        btnOutputFolder = New Button()
        TabPage2 = New TabPage()
        lblGoogleApiKey = New Label()
        lblGoogleUrl = New Label()
        lblNote = New Label()
        txtGoogleApiKey = New TextBox()
        txtGoogleURL = New TextBox()
        Panel1.SuspendLayout()
        TabControl1.SuspendLayout()
        TabPage1.SuspendLayout()
        TabPage2.SuspendLayout()
        SuspendLayout()
        ' 
        ' txtOutputHolder
        ' 
        txtOutputHolder.Location = New Point(0, 0)
        txtOutputHolder.Name = "txtOutputHolder"
        txtOutputHolder.Size = New Size(100, 23)
        txtOutputHolder.TabIndex = 0
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(btnOK)
        Panel1.Controls.Add(btnCancel)
        Panel1.Dock = DockStyle.Bottom
        Panel1.Location = New Point(0, 210)
        Panel1.Margin = New Padding(3, 2, 3, 2)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(700, 52)
        Panel1.TabIndex = 15
        ' 
        ' btnOK
        ' 
        btnOK.Font = New Font("Yu Gothic UI", 10.2F)
        btnOK.Location = New Point(500, 12)
        btnOK.Margin = New Padding(3, 2, 3, 2)
        btnOK.Name = "btnOK"
        btnOK.Size = New Size(80, 30)
        btnOK.TabIndex = 11
        btnOK.Text = "OK"
        btnOK.UseVisualStyleBackColor = True
        ' 
        ' btnCancel
        ' 
        btnCancel.Font = New Font("Yu Gothic UI", 10.2F)
        btnCancel.Location = New Point(603, 12)
        btnCancel.Margin = New Padding(3, 2, 3, 2)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(80, 30)
        btnCancel.TabIndex = 12
        btnCancel.Text = "Cancel"
        btnCancel.UseVisualStyleBackColor = True
        ' 
        ' TabControl1
        ' 
        TabControl1.Controls.Add(TabPage1)
        TabControl1.Controls.Add(TabPage2)
        TabControl1.Dock = DockStyle.Fill
        TabControl1.Location = New Point(0, 0)
        TabControl1.Margin = New Padding(3, 2, 3, 2)
        TabControl1.Name = "TabControl1"
        TabControl1.SelectedIndex = 0
        TabControl1.Size = New Size(700, 210)
        TabControl1.TabIndex = 1
        ' 
        ' TabPage1
        ' 
        TabPage1.Controls.Add(lblCallsign)
        TabPage1.Controls.Add(lblInputFolder)
        TabPage1.Controls.Add(lblOutputFolder)
        TabPage1.Controls.Add(lblComment)
        TabPage1.Controls.Add(txtMyCallsigns)
        TabPage1.Controls.Add(txtInputFolder)
        TabPage1.Controls.Add(btnInputFolder)
        TabPage1.Controls.Add(txtOutputFolder)
        TabPage1.Controls.Add(btnOutputFolder)
        TabPage1.Location = New Point(4, 24)
        TabPage1.Margin = New Padding(3, 2, 3, 2)
        TabPage1.Name = "TabPage1"
        TabPage1.Padding = New Padding(3, 2, 3, 2)
        TabPage1.Size = New Size(692, 182)
        TabPage1.TabIndex = 0
        TabPage1.Text = "General"
        TabPage1.UseVisualStyleBackColor = True
        ' 
        ' lblCallsign
        ' 
        lblCallsign.AutoSize = True
        lblCallsign.Font = New Font("Yu Gothic UI", 10.2F)
        lblCallsign.ForeColor = SystemColors.MenuHighlight
        lblCallsign.Location = New Point(23, 10)
        lblCallsign.Name = "lblCallsign"
        lblCallsign.Size = New Size(56, 19)
        lblCallsign.TabIndex = 12
        lblCallsign.Text = "&Callsign"
        ' 
        ' lblInputFolder
        ' 
        lblInputFolder.AutoSize = True
        lblInputFolder.Font = New Font("Yu Gothic UI", 10.2F)
        lblInputFolder.ForeColor = SystemColors.MenuHighlight
        lblInputFolder.Location = New Point(23, 59)
        lblInputFolder.Name = "lblInputFolder"
        lblInputFolder.Size = New Size(83, 19)
        lblInputFolder.TabIndex = 14
        lblInputFolder.Text = "&Input Folder"
        ' 
        ' lblOutputFolder
        ' 
        lblOutputFolder.AutoSize = True
        lblOutputFolder.Font = New Font("Yu Gothic UI", 10.2F)
        lblOutputFolder.ForeColor = SystemColors.MenuHighlight
        lblOutputFolder.Location = New Point(23, 108)
        lblOutputFolder.Name = "lblOutputFolder"
        lblOutputFolder.Size = New Size(95, 19)
        lblOutputFolder.TabIndex = 17
        lblOutputFolder.Text = "&Output Folder"
        ' 
        ' lblComment
        ' 
        lblComment.AutoSize = True
        lblComment.Location = New Point(314, 38)
        lblComment.Name = "lblComment"
        lblComment.Size = New Size(176, 15)
        lblComment.TabIndex = 20
        lblComment.Text = "複数Callsinは、カンマ区切りで入力"
        ' 
        ' txtMyCallsigns
        ' 
        txtMyCallsigns.AcceptsReturn = True
        txtMyCallsigns.BorderStyle = BorderStyle.FixedSingle
        txtMyCallsigns.CharacterCasing = CharacterCasing.Upper
        txtMyCallsigns.Font = New Font("Yu Gothic UI", 10.2F)
        txtMyCallsigns.ImeMode = ImeMode.Disable
        txtMyCallsigns.Location = New Point(34, 34)
        txtMyCallsigns.Margin = New Padding(3, 2, 3, 2)
        txtMyCallsigns.Name = "txtMyCallsigns"
        txtMyCallsigns.Size = New Size(258, 26)
        txtMyCallsigns.TabIndex = 13
        ' 
        ' txtInputFolder
        ' 
        txtInputFolder.BorderStyle = BorderStyle.FixedSingle
        txtInputFolder.Font = New Font("Yu Gothic UI", 10.2F)
        txtInputFolder.ImeMode = ImeMode.Disable
        txtInputFolder.Location = New Point(34, 81)
        txtInputFolder.Margin = New Padding(3, 2, 3, 2)
        txtInputFolder.Name = "txtInputFolder"
        txtInputFolder.Size = New Size(553, 26)
        txtInputFolder.TabIndex = 15
        ' 
        ' btnInputFolder
        ' 
        btnInputFolder.BackColor = SystemColors.Control
        btnInputFolder.Image = CType(resources.GetObject("btnInputFolder.Image"), Image)
        btnInputFolder.Location = New Point(602, 74)
        btnInputFolder.Margin = New Padding(3, 2, 3, 2)
        btnInputFolder.Name = "btnInputFolder"
        btnInputFolder.Size = New Size(41, 37)
        btnInputFolder.TabIndex = 16
        btnInputFolder.UseVisualStyleBackColor = False
        ' 
        ' txtOutputFolder
        ' 
        txtOutputFolder.BorderStyle = BorderStyle.FixedSingle
        txtOutputFolder.Font = New Font("Yu Gothic UI", 10.2F)
        txtOutputFolder.ImeMode = ImeMode.Disable
        txtOutputFolder.Location = New Point(34, 130)
        txtOutputFolder.Margin = New Padding(3, 2, 3, 2)
        txtOutputFolder.Name = "txtOutputFolder"
        txtOutputFolder.Size = New Size(553, 26)
        txtOutputFolder.TabIndex = 18
        ' 
        ' btnOutputFolder
        ' 
        btnOutputFolder.Image = CType(resources.GetObject("btnOutputFolder.Image"), Image)
        btnOutputFolder.Location = New Point(602, 122)
        btnOutputFolder.Margin = New Padding(3, 2, 3, 2)
        btnOutputFolder.Name = "btnOutputFolder"
        btnOutputFolder.Size = New Size(41, 37)
        btnOutputFolder.TabIndex = 19
        btnOutputFolder.UseVisualStyleBackColor = False
        ' 
        ' TabPage2
        ' 
        TabPage2.Controls.Add(lblGoogleApiKey)
        TabPage2.Controls.Add(lblGoogleUrl)
        TabPage2.Controls.Add(lblNote)
        TabPage2.Controls.Add(txtGoogleApiKey)
        TabPage2.Controls.Add(txtGoogleURL)
        TabPage2.Location = New Point(4, 24)
        TabPage2.Margin = New Padding(3, 2, 3, 2)
        TabPage2.Name = "TabPage2"
        TabPage2.Padding = New Padding(3, 2, 3, 2)
        TabPage2.Size = New Size(692, 182)
        TabPage2.TabIndex = 1
        TabPage2.Text = "Google"
        TabPage2.UseVisualStyleBackColor = True
        ' 
        ' lblGoogleApiKey
        ' 
        lblGoogleApiKey.AutoSize = True
        lblGoogleApiKey.Font = New Font("Yu Gothic UI", 10.2F)
        lblGoogleApiKey.ForeColor = SystemColors.MenuHighlight
        lblGoogleApiKey.Location = New Point(24, 14)
        lblGoogleApiKey.Name = "lblGoogleApiKey"
        lblGoogleApiKey.Size = New Size(56, 19)
        lblGoogleApiKey.TabIndex = 16
        lblGoogleApiKey.Text = "API Key"
        ' 
        ' lblGoogleUrl
        ' 
        lblGoogleUrl.AutoSize = True
        lblGoogleUrl.Enabled = False
        lblGoogleUrl.Font = New Font("Yu Gothic UI", 10.2F)
        lblGoogleUrl.ForeColor = SystemColors.MenuHighlight
        lblGoogleUrl.Location = New Point(24, 66)
        lblGoogleUrl.Name = "lblGoogleUrl"
        lblGoogleUrl.Size = New Size(34, 19)
        lblGoogleUrl.TabIndex = 20
        lblGoogleUrl.Text = "URL"
        ' 
        ' lblNote
        ' 
        lblNote.AutoSize = True
        lblNote.Font = New Font("Yu Gothic UI", 10.2F)
        lblNote.ForeColor = Color.OrangeRed
        lblNote.Location = New Point(38, 137)
        lblNote.Name = "lblNote"
        lblNote.RightToLeft = RightToLeft.Yes
        lblNote.Size = New Size(159, 19)
        lblNote.TabIndex = 15
        lblNote.Text = "Google Vision を使用する"
        ' 
        ' txtGoogleApiKey
        ' 
        txtGoogleApiKey.BorderStyle = BorderStyle.FixedSingle
        txtGoogleApiKey.Font = New Font("Yu Gothic UI", 10.2F)
        txtGoogleApiKey.ImeMode = ImeMode.Disable
        txtGoogleApiKey.Location = New Point(38, 33)
        txtGoogleApiKey.Margin = New Padding(3, 2, 3, 2)
        txtGoogleApiKey.Name = "txtGoogleApiKey"
        txtGoogleApiKey.Size = New Size(553, 26)
        txtGoogleApiKey.TabIndex = 17
        ' 
        ' txtGoogleURL
        ' 
        txtGoogleURL.BorderStyle = BorderStyle.FixedSingle
        txtGoogleURL.Enabled = False
        txtGoogleURL.Font = New Font("Yu Gothic UI", 10.2F)
        txtGoogleURL.ImeMode = ImeMode.Disable
        txtGoogleURL.Location = New Point(38, 86)
        txtGoogleURL.Margin = New Padding(3, 2, 3, 2)
        txtGoogleURL.Name = "txtGoogleURL"
        txtGoogleURL.Size = New Size(553, 26)
        txtGoogleURL.TabIndex = 19
        ' 
        ' frmSetting
        ' 
        AcceptButton = btnOK
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnCancel
        ClientSize = New Size(700, 262)
        Controls.Add(TabControl1)
        Controls.Add(Panel1)
        ForeColor = SystemColors.ControlDarkDark
        FormBorderStyle = FormBorderStyle.FixedDialog
        Margin = New Padding(3, 2, 3, 2)
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmSetting"
        StartPosition = FormStartPosition.CenterParent
        Text = "Setting"
        Panel1.ResumeLayout(False)
        TabControl1.ResumeLayout(False)
        TabPage1.ResumeLayout(False)
        TabPage1.PerformLayout()
        TabPage2.ResumeLayout(False)
        TabPage2.PerformLayout()
        ResumeLayout(False)
    End Sub
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents txtOutputHolder As TextBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnOK As Button
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents lblComment As Label
    Friend WithEvents lblOutputFolder As Label
    Friend WithEvents lblInputFolder As Label
    Friend WithEvents lblCallsign As Label
    Friend WithEvents btnOutputFolder As Button
    Friend WithEvents btnInputFolder As Button
    Public WithEvents txtOutputFolder As TextBox
    Public WithEvents txtInputFolder As TextBox
    Public WithEvents txtMyCallsigns As TextBox
    Friend WithEvents TabPage2 As TabPage
    Public WithEvents txtGoogleApiKey As TextBox
    Friend WithEvents lblNote As Label
    Public WithEvents txtGoogleURL As TextBox
    Friend WithEvents lblGoogleApiKey As Label
    Friend WithEvents lblGoogleUrl As Label
End Class
