<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        PicQsl = New PictureBox()
        txtDate = New TextBox()
        txtMode = New TextBox()
        btnOpenImage = New Button()
        btnSave = New Button()
        txtTime = New TextBox()
        txtBand = New TextBox()
        btnOfflineOcr = New Button()
        btnOnlineOcr = New Button()
        btnDeskew = New Button()
        LstOcrResult = New ListBox()
        MenuStrip1 = New MenuStrip()
        mnuSetting = New ToolStripMenuItem()
        DeskewToolStripMenuItem = New ToolStripMenuItem()
        AboutToolStripMenuItem = New ToolStripMenuItem()
        ImAGEToolStripMenuItem = New ToolStripMenuItem()
        btnTest = New Button()
        cmbCallsign = New ComboBox()
        Button1 = New Button()
        btnExit = New Button()
        Label1 = New Label()
        Label2 = New Label()
        Label3 = New Label()
        Label4 = New Label()
        Label5 = New Label()
        lstFileInfo = New ListBox()
        TxtOcrResult = New TextBox()
        CType(PicQsl, ComponentModel.ISupportInitialize).BeginInit()
        MenuStrip1.SuspendLayout()
        SuspendLayout()
        ' 
        ' PicQsl
        ' 
        PicQsl.BackgroundImageLayout = ImageLayout.None
        PicQsl.BorderStyle = BorderStyle.FixedSingle
        PicQsl.Location = New Point(12, 36)
        PicQsl.Name = "PicQsl"
        PicQsl.Size = New Size(800, 580)
        PicQsl.SizeMode = PictureBoxSizeMode.StretchImage
        PicQsl.TabIndex = 0
        PicQsl.TabStop = False
        ' 
        ' txtDate
        ' 
        txtDate.BackColor = SystemColors.Window
        txtDate.BorderStyle = BorderStyle.FixedSingle
        txtDate.CharacterCasing = CharacterCasing.Upper
        txtDate.Font = New Font("Yu Gothic UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        txtDate.ImeMode = ImeMode.Disable
        txtDate.Location = New Point(404, 652)
        txtDate.Name = "txtDate"
        txtDate.Size = New Size(125, 30)
        txtDate.TabIndex = 4
        txtDate.Text = "DATE"
        ' 
        ' txtMode
        ' 
        txtMode.BorderStyle = BorderStyle.FixedSingle
        txtMode.CharacterCasing = CharacterCasing.Upper
        txtMode.Font = New Font("Yu Gothic UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        txtMode.ImeMode = ImeMode.Disable
        txtMode.Location = New Point(848, 652)
        txtMode.Name = "txtMode"
        txtMode.Size = New Size(158, 30)
        txtMode.TabIndex = 10
        txtMode.Text = "MODE"
        ' 
        ' btnOpenImage
        ' 
        btnOpenImage.Location = New Point(226, 698)
        btnOpenImage.Name = "btnOpenImage"
        btnOpenImage.Size = New Size(123, 42)
        btnOpenImage.TabIndex = 11
        btnOpenImage.TabStop = False
        btnOpenImage.Text = "&Open QSL"
        btnOpenImage.UseVisualStyleBackColor = True
        ' 
        ' btnSave
        ' 
        btnSave.Location = New Point(664, 698)
        btnSave.Name = "btnSave"
        btnSave.Size = New Size(123, 42)
        btnSave.TabIndex = 14
        btnSave.TabStop = False
        btnSave.Text = "&Save"
        btnSave.UseVisualStyleBackColor = True
        ' 
        ' txtTime
        ' 
        txtTime.BorderStyle = BorderStyle.FixedSingle
        txtTime.CharacterCasing = CharacterCasing.Upper
        txtTime.Font = New Font("Yu Gothic UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        txtTime.ImeMode = ImeMode.Disable
        txtTime.Location = New Point(547, 652)
        txtTime.Name = "txtTime"
        txtTime.Size = New Size(125, 30)
        txtTime.TabIndex = 6
        txtTime.Text = "TIME"
        ' 
        ' txtBand
        ' 
        txtBand.BorderStyle = BorderStyle.FixedSingle
        txtBand.CharacterCasing = CharacterCasing.Upper
        txtBand.Font = New Font("Yu Gothic UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        txtBand.ImeMode = ImeMode.Disable
        txtBand.Location = New Point(702, 653)
        txtBand.Name = "txtBand"
        txtBand.Size = New Size(125, 30)
        txtBand.TabIndex = 8
        txtBand.Text = "BAND"
        ' 
        ' btnOfflineOcr
        ' 
        btnOfflineOcr.Location = New Point(372, 698)
        btnOfflineOcr.Name = "btnOfflineOcr"
        btnOfflineOcr.Size = New Size(123, 42)
        btnOfflineOcr.TabIndex = 12
        btnOfflineOcr.TabStop = False
        btnOfflineOcr.Text = "Offline OCR(&F)"
        btnOfflineOcr.UseVisualStyleBackColor = True
        ' 
        ' btnOnlineOcr
        ' 
        btnOnlineOcr.Enabled = False
        btnOnlineOcr.Location = New Point(518, 698)
        btnOnlineOcr.Name = "btnOnlineOcr"
        btnOnlineOcr.Size = New Size(123, 42)
        btnOnlineOcr.TabIndex = 13
        btnOnlineOcr.TabStop = False
        btnOnlineOcr.Text = "Online OCR(&N)"
        btnOnlineOcr.UseVisualStyleBackColor = True
        ' 
        ' btnDeskew
        ' 
        btnDeskew.Location = New Point(939, 630)
        btnDeskew.Name = "btnDeskew"
        btnDeskew.Size = New Size(123, 42)
        btnDeskew.TabIndex = 21
        btnDeskew.TabStop = False
        btnDeskew.Text = "Deskew"
        btnDeskew.UseVisualStyleBackColor = True
        btnDeskew.Visible = False
        ' 
        ' LstOcrResult
        ' 
        LstOcrResult.FormattingEnabled = True
        LstOcrResult.HorizontalScrollbar = True
        LstOcrResult.Location = New Point(818, 36)
        LstOcrResult.Name = "LstOcrResult"
        LstOcrResult.ScrollAlwaysVisible = True
        LstOcrResult.SelectionMode = SelectionMode.None
        LstOcrResult.Size = New Size(271, 584)
        LstOcrResult.TabIndex = 15
        LstOcrResult.TabStop = False
        LstOcrResult.Visible = False
        ' 
        ' MenuStrip1
        ' 
        MenuStrip1.ImageScalingSize = New Size(20, 20)
        MenuStrip1.Items.AddRange(New ToolStripItem() {mnuSetting, DeskewToolStripMenuItem, AboutToolStripMenuItem, ImAGEToolStripMenuItem})
        MenuStrip1.Location = New Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.Size = New Size(1098, 28)
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
        ' AboutToolStripMenuItem
        ' 
        AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        AboutToolStripMenuItem.Size = New Size(64, 24)
        AboutToolStripMenuItem.Text = "About"
        ' 
        ' ImAGEToolStripMenuItem
        ' 
        ImAGEToolStripMenuItem.Name = "ImAGEToolStripMenuItem"
        ImAGEToolStripMenuItem.ShortcutKeys = Keys.Control Or Keys.O
        ImAGEToolStripMenuItem.Size = New Size(89, 24)
        ImAGEToolStripMenuItem.Text = "Open QSL"
        ImAGEToolStripMenuItem.Visible = False
        ' 
        ' btnTest
        ' 
        btnTest.Font = New Font("Yu Gothic UI", 10.2F)
        btnTest.Location = New Point(966, 698)
        btnTest.Name = "btnTest"
        btnTest.Size = New Size(123, 42)
        btnTest.TabIndex = 22
        btnTest.TabStop = False
        btnTest.Text = "Test"
        btnTest.UseVisualStyleBackColor = True
        btnTest.Visible = False
        ' 
        ' cmbCallsign
        ' 
        cmbCallsign.DrawMode = DrawMode.OwnerDrawFixed
        cmbCallsign.Font = New Font("Yu Gothic UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        cmbCallsign.FormattingEnabled = True
        cmbCallsign.ImeMode = ImeMode.Disable
        cmbCallsign.Location = New Point(230, 652)
        cmbCallsign.Name = "cmbCallsign"
        cmbCallsign.Size = New Size(151, 31)
        cmbCallsign.Sorted = True
        cmbCallsign.TabIndex = 2
        cmbCallsign.Text = "Callsign"
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(1012, 631)
        Button1.Name = "Button1"
        Button1.Size = New Size(130, 31)
        Button1.TabIndex = 23
        Button1.TabStop = False
        Button1.Text = "Open Form2"
        Button1.UseVisualStyleBackColor = True
        Button1.Visible = False
        ' 
        ' btnExit
        ' 
        btnExit.Location = New Point(810, 698)
        btnExit.Name = "btnExit"
        btnExit.Size = New Size(123, 42)
        btnExit.TabIndex = 15
        btnExit.TabStop = False
        btnExit.Text = "&Exit"
        btnExit.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Yu Gothic UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        Label1.ForeColor = SystemColors.MenuHighlight
        Label1.Location = New Point(230, 626)
        Label1.Name = "Label1"
        Label1.Size = New Size(69, 23)
        Label1.TabIndex = 1
        Label1.Text = "&Callsign"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Yu Gothic UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        Label2.ForeColor = SystemColors.MenuHighlight
        Label2.Location = New Point(404, 626)
        Label2.Name = "Label2"
        Label2.Size = New Size(46, 23)
        Label2.TabIndex = 3
        Label2.Text = "&Date"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Font = New Font("Yu Gothic UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        Label3.ForeColor = SystemColors.MenuHighlight
        Label3.Location = New Point(547, 626)
        Label3.Name = "Label3"
        Label3.Size = New Size(47, 23)
        Label3.TabIndex = 5
        Label3.Text = "&Time"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Font = New Font("Yu Gothic UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        Label4.ForeColor = SystemColors.MenuHighlight
        Label4.Location = New Point(702, 626)
        Label4.Name = "Label4"
        Label4.Size = New Size(49, 23)
        Label4.TabIndex = 7
        Label4.Text = "&Band"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Font = New Font("Yu Gothic UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        Label5.ForeColor = SystemColors.MenuHighlight
        Label5.Location = New Point(848, 626)
        Label5.Name = "Label5"
        Label5.Size = New Size(54, 23)
        Label5.TabIndex = 9
        Label5.Text = "&Mode"
        ' 
        ' lstFileInfo
        ' 
        lstFileInfo.BackColor = SystemColors.Menu
        lstFileInfo.BorderStyle = BorderStyle.None
        lstFileInfo.FormattingEnabled = True
        lstFileInfo.Location = New Point(25, 631)
        lstFileInfo.Name = "lstFileInfo"
        lstFileInfo.Size = New Size(178, 100)
        lstFileInfo.TabIndex = 24
        lstFileInfo.TabStop = False
        ' 
        ' TxtOcrResult
        ' 
        TxtOcrResult.AcceptsReturn = True
        TxtOcrResult.BackColor = SystemColors.Window
        TxtOcrResult.BorderStyle = BorderStyle.FixedSingle
        TxtOcrResult.Font = New Font("Yu Gothic UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        TxtOcrResult.ImeMode = ImeMode.Disable
        TxtOcrResult.Location = New Point(818, 36)
        TxtOcrResult.Multiline = True
        TxtOcrResult.Name = "TxtOcrResult"
        TxtOcrResult.ScrollBars = ScrollBars.Both
        TxtOcrResult.Size = New Size(271, 584)
        TxtOcrResult.TabIndex = 25
        TxtOcrResult.TabStop = False
        TxtOcrResult.Text = "txtOcrResult"
        TxtOcrResult.WordWrap = False
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1098, 752)
        Controls.Add(TxtOcrResult)
        Controls.Add(lstFileInfo)
        Controls.Add(Label5)
        Controls.Add(Label4)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(btnExit)
        Controls.Add(Button1)
        Controls.Add(cmbCallsign)
        Controls.Add(btnTest)
        Controls.Add(LstOcrResult)
        Controls.Add(btnDeskew)
        Controls.Add(btnOnlineOcr)
        Controls.Add(btnOfflineOcr)
        Controls.Add(txtBand)
        Controls.Add(txtTime)
        Controls.Add(btnSave)
        Controls.Add(btnOpenImage)
        Controls.Add(txtMode)
        Controls.Add(txtDate)
        Controls.Add(PicQsl)
        Controls.Add(MenuStrip1)
        FormBorderStyle = FormBorderStyle.FixedSingle
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        KeyPreview = True
        MainMenuStrip = MenuStrip1
        MaximizeBox = False
        Name = "Form1"
        Text = "QslOrganizer"
        CType(PicQsl, ComponentModel.ISupportInitialize).EndInit()
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents PicQsl As PictureBox
    Friend WithEvents txtDate As TextBox
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents txtMode As TextBox
    Friend WithEvents btnOpenImage As Button
    Friend WithEvents btnExit As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents txtTime As TextBox
    Friend WithEvents txtBand As TextBox
    Friend WithEvents btnOfflineOcr As Button
    Friend WithEvents btnOnlineOcr As Button
    Friend WithEvents btnDeskew As Button
    Friend WithEvents LstOcrResult As ListBox
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents mnuSetting As ToolStripMenuItem
    Friend WithEvents btnTest As Button
    Friend WithEvents cmbCallsign As ComboBox
    Friend WithEvents Button1 As Button
    Friend WithEvents DeskewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents lstFileInfo As ListBox
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ImAGEToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TxtOcrResult As TextBox

End Class
