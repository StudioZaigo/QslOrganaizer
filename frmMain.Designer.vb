<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        MenuStrip1 = New MenuStrip()
        mnuSetting = New ToolStripMenuItem()
        DeskewToolStripMenuItem = New ToolStripMenuItem()
        mnuAbout = New ToolStripMenuItem()
        ImAGEToolStripMenuItem = New ToolStripMenuItem()
        pnlBtn = New Panel()
        chkAuto = New CheckBox()
        lstFileInfo = New ListBox()
        Label5 = New Label()
        Label4 = New Label()
        Label3 = New Label()
        Label2 = New Label()
        Label1 = New Label()
        btnExit = New Button()
        Button1 = New Button()
        cmbCallsign = New ComboBox()
        btnTest = New Button()
        btnDeskew = New Button()
        btnOnlineOcr = New Button()
        btnOfflineOcr = New Button()
        txtBand = New TextBox()
        txtTime = New TextBox()
        btnSave = New Button()
        btnOpenImage = New Button()
        txtMode = New TextBox()
        txtDate = New TextBox()
        PnlPic = New Panel()
        PicQsl = New PictureBox()
        PnlList = New Panel()
        LstOcrResult = New ListBox()
        MenuStrip1.SuspendLayout()
        pnlBtn.SuspendLayout()
        PnlPic.SuspendLayout()
        CType(PicQsl, ComponentModel.ISupportInitialize).BeginInit()
        PnlList.SuspendLayout()
        SuspendLayout()
        ' 
        ' MenuStrip1
        ' 
        MenuStrip1.ImageScalingSize = New Size(20, 20)
        MenuStrip1.Items.AddRange(New ToolStripItem() {mnuSetting, DeskewToolStripMenuItem, mnuAbout, ImAGEToolStripMenuItem})
        MenuStrip1.Location = New Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.Padding = New Padding(6, 3, 0, 3)
        MenuStrip1.Size = New Size(1014, 30)
        MenuStrip1.TabIndex = 16
        MenuStrip1.Text = "MenuStrip1"
        ' 
        ' mnuSetting
        ' 
        mnuSetting.Name = "mnuSetting"
        mnuSetting.ShortcutKeys = Keys.Control Or Keys.S
        mnuSetting.Size = New Size(70, 24)
        mnuSetting.Text = "Setting"
        ' 
        ' DeskewToolStripMenuItem
        ' 
        DeskewToolStripMenuItem.Enabled = False
        DeskewToolStripMenuItem.Name = "DeskewToolStripMenuItem"
        DeskewToolStripMenuItem.ShortcutKeys = Keys.Control Or Keys.D
        DeskewToolStripMenuItem.Size = New Size(74, 24)
        DeskewToolStripMenuItem.Text = "Deskew"
        ' 
        ' mnuAbout
        ' 
        mnuAbout.Name = "mnuAbout"
        mnuAbout.Size = New Size(64, 24)
        mnuAbout.Text = "About"
        ' 
        ' ImAGEToolStripMenuItem
        ' 
        ImAGEToolStripMenuItem.Name = "ImAGEToolStripMenuItem"
        ImAGEToolStripMenuItem.ShortcutKeys = Keys.Control Or Keys.O
        ImAGEToolStripMenuItem.Size = New Size(89, 24)
        ImAGEToolStripMenuItem.Text = "Open QSL"
        ImAGEToolStripMenuItem.Visible = False
        ' 
        ' pnlBtn
        ' 
        pnlBtn.BackgroundImageLayout = ImageLayout.None
        pnlBtn.Controls.Add(chkAuto)
        pnlBtn.Controls.Add(lstFileInfo)
        pnlBtn.Controls.Add(Label5)
        pnlBtn.Controls.Add(Label4)
        pnlBtn.Controls.Add(Label3)
        pnlBtn.Controls.Add(Label2)
        pnlBtn.Controls.Add(Label1)
        pnlBtn.Controls.Add(btnExit)
        pnlBtn.Controls.Add(Button1)
        pnlBtn.Controls.Add(cmbCallsign)
        pnlBtn.Controls.Add(btnTest)
        pnlBtn.Controls.Add(btnDeskew)
        pnlBtn.Controls.Add(btnOnlineOcr)
        pnlBtn.Controls.Add(btnOfflineOcr)
        pnlBtn.Controls.Add(txtBand)
        pnlBtn.Controls.Add(txtTime)
        pnlBtn.Controls.Add(btnSave)
        pnlBtn.Controls.Add(btnOpenImage)
        pnlBtn.Controls.Add(txtMode)
        pnlBtn.Controls.Add(txtDate)
        pnlBtn.Dock = DockStyle.Bottom
        pnlBtn.Location = New Point(0, 596)
        pnlBtn.Name = "pnlBtn"
        pnlBtn.Size = New Size(1014, 143)
        pnlBtn.TabIndex = 25
        ' 
        ' chkAuto
        ' 
        chkAuto.AutoSize = True
        chkAuto.Location = New Point(855, 99)
        chkAuto.Name = "chkAuto"
        chkAuto.Size = New Size(63, 24)
        chkAuto.TabIndex = 45
        chkAuto.Text = "&Auto"
        chkAuto.UseVisualStyleBackColor = True
        ' 
        ' lstFileInfo
        ' 
        lstFileInfo.BackColor = SystemColors.Menu
        lstFileInfo.BorderStyle = BorderStyle.None
        lstFileInfo.FormattingEnabled = True
        lstFileInfo.Location = New Point(15, 15)
        lstFileInfo.Name = "lstFileInfo"
        lstFileInfo.Size = New Size(216, 120)
        lstFileInfo.TabIndex = 43
        lstFileInfo.TabStop = False
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Font = New Font("Yu Gothic UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        Label5.ForeColor = SystemColors.MenuHighlight
        Label5.Location = New Point(855, 13)
        Label5.Name = "Label5"
        Label5.Size = New Size(54, 23)
        Label5.TabIndex = 33
        Label5.Text = "&Mode"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Font = New Font("Yu Gothic UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        Label4.ForeColor = SystemColors.MenuHighlight
        Label4.Location = New Point(709, 13)
        Label4.Name = "Label4"
        Label4.Size = New Size(49, 23)
        Label4.TabIndex = 31
        Label4.Text = "&Band"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Font = New Font("Yu Gothic UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        Label3.ForeColor = SystemColors.MenuHighlight
        Label3.Location = New Point(554, 13)
        Label3.Name = "Label3"
        Label3.Size = New Size(47, 23)
        Label3.TabIndex = 29
        Label3.Text = "&Time"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Yu Gothic UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        Label2.ForeColor = SystemColors.MenuHighlight
        Label2.Location = New Point(411, 13)
        Label2.Name = "Label2"
        Label2.Size = New Size(46, 23)
        Label2.TabIndex = 27
        Label2.Text = "&Date"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Yu Gothic UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        Label1.ForeColor = SystemColors.MenuHighlight
        Label1.Location = New Point(237, 13)
        Label1.Name = "Label1"
        Label1.Size = New Size(69, 23)
        Label1.TabIndex = 25
        Label1.Text = "&Callsign"
        ' 
        ' btnExit
        ' 
        btnExit.Location = New Point(720, 87)
        btnExit.Name = "btnExit"
        btnExit.Padding = New Padding(3)
        btnExit.Size = New Size(114, 43)
        btnExit.TabIndex = 39
        btnExit.TabStop = False
        btnExit.Text = "&Exit"
        btnExit.UseVisualStyleBackColor = True
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(967, 51)
        Button1.Name = "Button1"
        Button1.Size = New Size(123, 31)
        Button1.TabIndex = 42
        Button1.TabStop = False
        Button1.Text = "Open Form2"
        Button1.UseVisualStyleBackColor = True
        Button1.Visible = False
        ' 
        ' cmbCallsign
        ' 
        cmbCallsign.Font = New Font("メイリオ", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        cmbCallsign.FormattingEnabled = True
        cmbCallsign.ImeMode = ImeMode.Disable
        cmbCallsign.Location = New Point(237, 40)
        cmbCallsign.Name = "cmbCallsign"
        cmbCallsign.Size = New Size(151, 33)
        cmbCallsign.Sorted = True
        cmbCallsign.TabIndex = 26
        ' 
        ' btnTest
        ' 
        btnTest.Font = New Font("Yu Gothic UI", 10.2F)
        btnTest.Location = New Point(917, 87)
        btnTest.Name = "btnTest"
        btnTest.Size = New Size(123, 43)
        btnTest.TabIndex = 41
        btnTest.TabStop = False
        btnTest.Text = "Test"
        btnTest.UseVisualStyleBackColor = True
        btnTest.Visible = False
        ' 
        ' btnDeskew
        ' 
        btnDeskew.Location = New Point(967, 5)
        btnDeskew.Name = "btnDeskew"
        btnDeskew.Size = New Size(123, 43)
        btnDeskew.TabIndex = 40
        btnDeskew.TabStop = False
        btnDeskew.Text = "Deskew"
        btnDeskew.UseVisualStyleBackColor = True
        btnDeskew.Visible = False
        ' 
        ' btnOnlineOcr
        ' 
        btnOnlineOcr.Enabled = False
        btnOnlineOcr.Location = New Point(477, 87)
        btnOnlineOcr.Name = "btnOnlineOcr"
        btnOnlineOcr.Size = New Size(114, 43)
        btnOnlineOcr.TabIndex = 37
        btnOnlineOcr.TabStop = False
        btnOnlineOcr.Text = "Online OCR(&N)"
        btnOnlineOcr.UseVisualStyleBackColor = True
        ' 
        ' btnOfflineOcr
        ' 
        btnOfflineOcr.Font = New Font("Yu Gothic UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        btnOfflineOcr.Location = New Point(357, 87)
        btnOfflineOcr.Name = "btnOfflineOcr"
        btnOfflineOcr.Size = New Size(114, 43)
        btnOfflineOcr.TabIndex = 36
        btnOfflineOcr.TabStop = False
        btnOfflineOcr.Text = "Offline OCR(&F)"
        btnOfflineOcr.UseVisualStyleBackColor = True
        ' 
        ' txtBand
        ' 
        txtBand.BorderStyle = BorderStyle.FixedSingle
        txtBand.CharacterCasing = CharacterCasing.Upper
        txtBand.Font = New Font("メイリオ", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        txtBand.ImeMode = ImeMode.Disable
        txtBand.Location = New Point(709, 40)
        txtBand.Name = "txtBand"
        txtBand.Size = New Size(125, 33)
        txtBand.TabIndex = 32
        ' 
        ' txtTime
        ' 
        txtTime.BorderStyle = BorderStyle.FixedSingle
        txtTime.CharacterCasing = CharacterCasing.Upper
        txtTime.Font = New Font("メイリオ", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        txtTime.ImeMode = ImeMode.Disable
        txtTime.Location = New Point(554, 40)
        txtTime.Name = "txtTime"
        txtTime.Size = New Size(125, 33)
        txtTime.TabIndex = 30
        ' 
        ' btnSave
        ' 
        btnSave.Location = New Point(597, 87)
        btnSave.Name = "btnSave"
        btnSave.Size = New Size(114, 43)
        btnSave.TabIndex = 38
        btnSave.TabStop = False
        btnSave.Text = "&Save"
        btnSave.UseVisualStyleBackColor = True
        ' 
        ' btnOpenImage
        ' 
        btnOpenImage.Font = New Font("Yu Gothic UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        btnOpenImage.Location = New Point(237, 87)
        btnOpenImage.Name = "btnOpenImage"
        btnOpenImage.Size = New Size(114, 43)
        btnOpenImage.TabIndex = 35
        btnOpenImage.TabStop = False
        btnOpenImage.Text = "&Open QSL"
        btnOpenImage.UseVisualStyleBackColor = True
        ' 
        ' txtMode
        ' 
        txtMode.BorderStyle = BorderStyle.FixedSingle
        txtMode.CharacterCasing = CharacterCasing.Upper
        txtMode.Font = New Font("メイリオ", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        txtMode.ImeMode = ImeMode.Disable
        txtMode.Location = New Point(855, 40)
        txtMode.Name = "txtMode"
        txtMode.Size = New Size(108, 33)
        txtMode.TabIndex = 34
        ' 
        ' txtDate
        ' 
        txtDate.BackColor = SystemColors.Window
        txtDate.BorderStyle = BorderStyle.FixedSingle
        txtDate.CharacterCasing = CharacterCasing.Upper
        txtDate.Font = New Font("メイリオ", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        txtDate.ImeMode = ImeMode.Disable
        txtDate.Location = New Point(411, 40)
        txtDate.Name = "txtDate"
        txtDate.Size = New Size(125, 33)
        txtDate.TabIndex = 28
        ' 
        ' PnlPic
        ' 
        PnlPic.BackColor = SystemColors.GrayText
        PnlPic.BorderStyle = BorderStyle.FixedSingle
        PnlPic.Controls.Add(PicQsl)
        PnlPic.Dock = DockStyle.Fill
        PnlPic.Location = New Point(0, 30)
        PnlPic.Margin = New Padding(3, 0, 3, 0)
        PnlPic.Name = "PnlPic"
        PnlPic.Padding = New Padding(3, 0, 3, 0)
        PnlPic.Size = New Size(775, 566)
        PnlPic.TabIndex = 26
        ' 
        ' PicQsl
        ' 
        PicQsl.Anchor = AnchorStyles.None
        PicQsl.BackColor = SystemColors.HotTrack
        PicQsl.BackgroundImageLayout = ImageLayout.None
        PicQsl.Location = New Point(59, 18)
        PicQsl.Margin = New Padding(0)
        PicQsl.Name = "PicQsl"
        PicQsl.Size = New Size(545, 321)
        PicQsl.SizeMode = PictureBoxSizeMode.Zoom
        PicQsl.TabIndex = 1
        PicQsl.TabStop = False
        ' 
        ' PnlList
        ' 
        PnlList.Controls.Add(LstOcrResult)
        PnlList.Dock = DockStyle.Right
        PnlList.Location = New Point(775, 30)
        PnlList.Margin = New Padding(3, 0, 3, 0)
        PnlList.Name = "PnlList"
        PnlList.Padding = New Padding(3, 0, 3, 0)
        PnlList.Size = New Size(239, 566)
        PnlList.TabIndex = 27
        ' 
        ' LstOcrResult
        ' 
        LstOcrResult.BackColor = SystemColors.Control
        LstOcrResult.BorderStyle = BorderStyle.FixedSingle
        LstOcrResult.Font = New Font("メイリオ", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        LstOcrResult.FormattingEnabled = True
        LstOcrResult.HorizontalScrollbar = True
        LstOcrResult.Location = New Point(3, 69)
        LstOcrResult.Margin = New Padding(3, 0, 3, 0)
        LstOcrResult.Name = "LstOcrResult"
        LstOcrResult.ScrollAlwaysVisible = True
        LstOcrResult.SelectionMode = SelectionMode.None
        LstOcrResult.Size = New Size(233, 462)
        LstOcrResult.TabIndex = 16
        LstOcrResult.TabStop = False
        ' 
        ' frmMain
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1014, 739)
        Controls.Add(PnlPic)
        Controls.Add(PnlList)
        Controls.Add(pnlBtn)
        Controls.Add(MenuStrip1)
        FormBorderStyle = FormBorderStyle.FixedSingle
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        KeyPreview = True
        MainMenuStrip = MenuStrip1
        MaximizeBox = False
        Name = "frmMain"
        Text = "QslOrganizer"
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        pnlBtn.ResumeLayout(False)
        pnlBtn.PerformLayout()
        PnlPic.ResumeLayout(False)
        CType(PicQsl, ComponentModel.ISupportInitialize).EndInit()
        PnlList.ResumeLayout(False)
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents Button3 As Button
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents mnuSetting As ToolStripMenuItem
    Friend WithEvents DeskewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents mnuAbout As ToolStripMenuItem
    Friend WithEvents ImAGEToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents pnlBtn As Panel
    Friend WithEvents lstFileInfo As ListBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents btnExit As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents cmbCallsign As ComboBox
    Friend WithEvents btnTest As Button
    Friend WithEvents btnDeskew As Button
    Friend WithEvents btnOnlineOcr As Button
    Friend WithEvents btnOfflineOcr As Button
    Friend WithEvents txtBand As TextBox
    Friend WithEvents txtTime As TextBox
    Friend WithEvents btnSave As Button
    Friend WithEvents btnOpenImage As Button
    Friend WithEvents txtMode As TextBox
    Friend WithEvents txtDate As TextBox
    Friend WithEvents PnlPic As Panel
    Friend WithEvents PicQsl As PictureBox
    Friend WithEvents PnlList As Panel
    Friend WithEvents LstOcrResult As ListBox
    Friend WithEvents chkAuto As CheckBox

End Class
