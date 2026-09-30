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
        Label1 = New Label()
        lblFontSize = New Label()
        txtFontSize = New TextBox()
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
        chkUseGoogle = New CheckBox()
        lblGoogleApiKey = New Label()
        lblGoogleUrl = New Label()
        lblGoogleNote = New Label()
        txtGoogleApiKey = New TextBox()
        txtGoogleURL = New TextBox()
        TabPage3 = New TabPage()
        chkUseAzure = New CheckBox()
        lblAzureApiKey = New Label()
        lblAzureUrl = New Label()
        lblAzureNote = New Label()
        txtAzureApiKey = New TextBox()
        txtAzureUrl = New TextBox()
        Panel1.SuspendLayout()
        TabControl1.SuspendLayout()
        TabPage1.SuspendLayout()
        TabPage2.SuspendLayout()
        TabPage3.SuspendLayout()
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
        Panel1.Location = New Point(0, 251)
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
        TabControl1.Controls.Add(TabPage3)
        TabControl1.Dock = DockStyle.Fill
        TabControl1.Location = New Point(0, 0)
        TabControl1.Margin = New Padding(3, 2, 3, 2)
        TabControl1.Name = "TabControl1"
        TabControl1.SelectedIndex = 0
        TabControl1.Size = New Size(700, 251)
        TabControl1.TabIndex = 1
        ' 
        ' TabPage1
        ' 
        TabPage1.Controls.Add(Label1)
        TabPage1.Controls.Add(lblFontSize)
        TabPage1.Controls.Add(txtFontSize)
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
        TabPage1.Size = New Size(692, 223)
        TabPage1.TabIndex = 0
        TabPage1.Text = "General"
        TabPage1.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Yu Gothic UI", 10.2F)
        Label1.ForeColor = SystemColors.MenuHighlight
        Label1.Location = New Point(108, 184)
        Label1.Name = "Label1"
        Label1.Size = New Size(106, 19)
        Label1.TabIndex = 23
        Label1.Text = "規定値は9ptです"
        ' 
        ' lblFontSize
        ' 
        lblFontSize.AutoSize = True
        lblFontSize.Font = New Font("Yu Gothic UI", 10.2F)
        lblFontSize.ForeColor = SystemColors.MenuHighlight
        lblFontSize.Location = New Point(23, 160)
        lblFontSize.Name = "lblFontSize"
        lblFontSize.Size = New Size(62, 19)
        lblFontSize.TabIndex = 21
        lblFontSize.Text = "Font size"
        ' 
        ' txtFontSize
        ' 
        txtFontSize.BorderStyle = BorderStyle.FixedSingle
        txtFontSize.Font = New Font("Yu Gothic UI", 10.2F)
        txtFontSize.ImeMode = ImeMode.Disable
        txtFontSize.Location = New Point(34, 180)
        txtFontSize.Margin = New Padding(3, 2, 3, 2)
        txtFontSize.Name = "txtFontSize"
        txtFontSize.Size = New Size(72, 26)
        txtFontSize.TabIndex = 22
        txtFontSize.Text = "9"
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
        TabPage2.Controls.Add(chkUseGoogle)
        TabPage2.Controls.Add(lblGoogleApiKey)
        TabPage2.Controls.Add(lblGoogleUrl)
        TabPage2.Controls.Add(lblGoogleNote)
        TabPage2.Controls.Add(txtGoogleApiKey)
        TabPage2.Controls.Add(txtGoogleURL)
        TabPage2.Location = New Point(4, 24)
        TabPage2.Margin = New Padding(3, 2, 3, 2)
        TabPage2.Name = "TabPage2"
        TabPage2.Padding = New Padding(3, 2, 3, 2)
        TabPage2.Size = New Size(692, 223)
        TabPage2.TabIndex = 1
        TabPage2.Text = "Google"
        TabPage2.UseVisualStyleBackColor = True
        ' 
        ' chkUseGoogle
        ' 
        chkUseGoogle.AutoSize = True
        chkUseGoogle.Location = New Point(38, 126)
        chkUseGoogle.Name = "chkUseGoogle"
        chkUseGoogle.Size = New Size(86, 19)
        chkUseGoogle.TabIndex = 27
        chkUseGoogle.Text = "Use Google"
        chkUseGoogle.UseVisualStyleBackColor = True
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
        ' lblGoogleNote
        ' 
        lblGoogleNote.AutoSize = True
        lblGoogleNote.Font = New Font("Yu Gothic UI", 10.2F)
        lblGoogleNote.ForeColor = Color.OrangeRed
        lblGoogleNote.Location = New Point(38, 162)
        lblGoogleNote.Name = "lblGoogleNote"
        lblGoogleNote.RightToLeft = RightToLeft.Yes
        lblGoogleNote.Size = New Size(159, 19)
        lblGoogleNote.TabIndex = 15
        lblGoogleNote.Text = "Google Vision を使用する"
        ' 
        ' txtGoogleApiKey
        ' 
        txtGoogleApiKey.BorderStyle = BorderStyle.FixedSingle
        txtGoogleApiKey.Font = New Font("Yu Gothic UI", 10.2F)
        txtGoogleApiKey.ImeMode = ImeMode.Disable
        txtGoogleApiKey.Location = New Point(38, 33)
        txtGoogleApiKey.Margin = New Padding(3, 2, 3, 2)
        txtGoogleApiKey.Name = "txtGoogleApiKey"
        txtGoogleApiKey.ShortcutsEnabled = False
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
        ' TabPage3
        ' 
        TabPage3.Controls.Add(chkUseAzure)
        TabPage3.Controls.Add(lblAzureApiKey)
        TabPage3.Controls.Add(lblAzureUrl)
        TabPage3.Controls.Add(lblAzureNote)
        TabPage3.Controls.Add(txtAzureApiKey)
        TabPage3.Controls.Add(txtAzureUrl)
        TabPage3.Location = New Point(4, 24)
        TabPage3.Name = "TabPage3"
        TabPage3.Padding = New Padding(3)
        TabPage3.Size = New Size(692, 223)
        TabPage3.TabIndex = 2
        TabPage3.Text = "Azure"
        TabPage3.UseVisualStyleBackColor = True
        ' 
        ' chkUseAzure
        ' 
        chkUseAzure.AutoSize = True
        chkUseAzure.Location = New Point(77, 155)
        chkUseAzure.Name = "chkUseAzure"
        chkUseAzure.Size = New Size(78, 19)
        chkUseAzure.TabIndex = 26
        chkUseAzure.Text = "Use Azure"
        chkUseAzure.UseVisualStyleBackColor = True
        ' 
        ' lblAzureApiKey
        ' 
        lblAzureApiKey.AutoSize = True
        lblAzureApiKey.Font = New Font("Yu Gothic UI", 10.2F)
        lblAzureApiKey.ForeColor = SystemColors.MenuHighlight
        lblAzureApiKey.Location = New Point(63, 40)
        lblAzureApiKey.Name = "lblAzureApiKey"
        lblAzureApiKey.Size = New Size(56, 19)
        lblAzureApiKey.TabIndex = 22
        lblAzureApiKey.Text = "API Key"
        ' 
        ' lblAzureUrl
        ' 
        lblAzureUrl.AutoSize = True
        lblAzureUrl.Enabled = False
        lblAzureUrl.Font = New Font("Yu Gothic UI", 10.2F)
        lblAzureUrl.ForeColor = SystemColors.MenuHighlight
        lblAzureUrl.Location = New Point(63, 92)
        lblAzureUrl.Name = "lblAzureUrl"
        lblAzureUrl.Size = New Size(34, 19)
        lblAzureUrl.TabIndex = 25
        lblAzureUrl.Text = "URL"
        ' 
        ' lblAzureNote
        ' 
        lblAzureNote.AutoSize = True
        lblAzureNote.Font = New Font("Yu Gothic UI", 10.2F)
        lblAzureNote.ForeColor = Color.OrangeRed
        lblAzureNote.Location = New Point(37, 177)
        lblAzureNote.Name = "lblAzureNote"
        lblAzureNote.RightToLeft = RightToLeft.Yes
        lblAzureNote.Size = New Size(150, 19)
        lblAzureNote.TabIndex = 21
        lblAzureNote.Text = "Azure Vision を使用する"
        ' 
        ' txtAzureApiKey
        ' 
        txtAzureApiKey.BorderStyle = BorderStyle.FixedSingle
        txtAzureApiKey.Font = New Font("Yu Gothic UI", 10.2F)
        txtAzureApiKey.ImeMode = ImeMode.Disable
        txtAzureApiKey.Location = New Point(77, 59)
        txtAzureApiKey.Margin = New Padding(3, 2, 3, 2)
        txtAzureApiKey.Name = "txtAzureApiKey"
        txtAzureApiKey.Size = New Size(553, 26)
        txtAzureApiKey.TabIndex = 23
        ' 
        ' txtAzureUrl
        ' 
        txtAzureUrl.BorderStyle = BorderStyle.FixedSingle
        txtAzureUrl.Enabled = False
        txtAzureUrl.Font = New Font("Yu Gothic UI", 10.2F)
        txtAzureUrl.ImeMode = ImeMode.Disable
        txtAzureUrl.Location = New Point(77, 112)
        txtAzureUrl.Margin = New Padding(3, 2, 3, 2)
        txtAzureUrl.Name = "txtAzureUrl"
        txtAzureUrl.Size = New Size(553, 26)
        txtAzureUrl.TabIndex = 24
        ' 
        ' frmSetting
        ' 
        AcceptButton = btnOK
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnCancel
        ClientSize = New Size(700, 303)
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
        TabPage3.ResumeLayout(False)
        TabPage3.PerformLayout()
        ResumeLayout(False)
    End Sub
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
    Friend WithEvents lblGoogleNote As Label
    Public WithEvents txtGoogleURL As TextBox
    Friend WithEvents lblGoogleApiKey As Label
    Friend WithEvents lblGoogleUrl As Label
    Friend WithEvents lblFontSize As Label
    Public WithEvents txtFontSize As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents chkUseAzure As CheckBox
    Friend WithEvents lblAzureApiKey As Label
    Friend WithEvents lblAzureUrl As Label
    Friend WithEvents lblAzureNote As Label
    Public WithEvents txtAzureApiKey As TextBox
    Public WithEvents txtAzureUrl As TextBox
    Friend WithEvents chkUseGoogle As CheckBox
End Class
