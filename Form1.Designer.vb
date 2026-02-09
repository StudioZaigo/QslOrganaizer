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
        SettingToolStripMenuItem = New ToolStripMenuItem()
        btnTest = New Button()
        cmbCallsign = New ComboBox()
        Button1 = New Button()
        CType(PicQsl, ComponentModel.ISupportInitialize).BeginInit()
        MenuStrip1.SuspendLayout()
        SuspendLayout()
        ' 
        ' PicQsl
        ' 
        PicQsl.BackgroundImageLayout = ImageLayout.None
        PicQsl.BorderStyle = BorderStyle.FixedSingle
        PicQsl.Location = New Point(12, 38)
        PicQsl.Name = "PicQsl"
        PicQsl.Size = New Size(830, 590)
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
        txtDate.Location = New Point(187, 646)
        txtDate.Name = "txtDate"
        txtDate.Size = New Size(125, 30)
        txtDate.TabIndex = 2
        txtDate.Text = "DATE"
        ' 
        ' txtMode
        ' 
        txtMode.BorderStyle = BorderStyle.FixedSingle
        txtMode.CharacterCasing = CharacterCasing.Upper
        txtMode.Font = New Font("Yu Gothic UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        txtMode.ImeMode = ImeMode.Disable
        txtMode.Location = New Point(631, 646)
        txtMode.Name = "txtMode"
        txtMode.Size = New Size(158, 30)
        txtMode.TabIndex = 5
        txtMode.Text = "MODE"
        ' 
        ' btnOpenImage
        ' 
        btnOpenImage.Location = New Point(9, 690)
        btnOpenImage.Name = "btnOpenImage"
        btnOpenImage.Size = New Size(123, 42)
        btnOpenImage.TabIndex = 6
        btnOpenImage.Text = "&Open QSL"
        btnOpenImage.UseVisualStyleBackColor = True
        ' 
        ' btnSave
        ' 
        btnSave.Location = New Point(461, 688)
        btnSave.Name = "btnSave"
        btnSave.Size = New Size(123, 42)
        btnSave.TabIndex = 9
        btnSave.Text = "&Save"
        btnSave.UseVisualStyleBackColor = True
        ' 
        ' txtTime
        ' 
        txtTime.BorderStyle = BorderStyle.FixedSingle
        txtTime.CharacterCasing = CharacterCasing.Upper
        txtTime.Font = New Font("Yu Gothic UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        txtTime.ImeMode = ImeMode.Disable
        txtTime.Location = New Point(330, 646)
        txtTime.Name = "txtTime"
        txtTime.Size = New Size(125, 30)
        txtTime.TabIndex = 3
        txtTime.Text = "TIME"
        ' 
        ' txtBand
        ' 
        txtBand.BorderStyle = BorderStyle.FixedSingle
        txtBand.CharacterCasing = CharacterCasing.Upper
        txtBand.Font = New Font("Yu Gothic UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        txtBand.ImeMode = ImeMode.Disable
        txtBand.Location = New Point(485, 647)
        txtBand.Name = "txtBand"
        txtBand.Size = New Size(125, 30)
        txtBand.TabIndex = 4
        txtBand.Text = "BAND"
        ' 
        ' btnOfflineOcr
        ' 
        btnOfflineOcr.Location = New Point(155, 692)
        btnOfflineOcr.Name = "btnOfflineOcr"
        btnOfflineOcr.Size = New Size(123, 42)
        btnOfflineOcr.TabIndex = 7
        btnOfflineOcr.Text = "Offline OCR(&F)"
        btnOfflineOcr.UseVisualStyleBackColor = True
        ' 
        ' btnOnlineOcr
        ' 
        btnOnlineOcr.Location = New Point(303, 692)
        btnOnlineOcr.Name = "btnOnlineOcr"
        btnOnlineOcr.Size = New Size(123, 42)
        btnOnlineOcr.TabIndex = 8
        btnOnlineOcr.Text = "Online OCR(&N)"
        btnOnlineOcr.UseVisualStyleBackColor = True
        ' 
        ' btnDeskew
        ' 
        btnDeskew.Location = New Point(628, 686)
        btnDeskew.Name = "btnDeskew"
        btnDeskew.Size = New Size(123, 42)
        btnDeskew.TabIndex = 10
        btnDeskew.Text = "Deskew"
        btnDeskew.UseVisualStyleBackColor = True
        ' 
        ' LstOcrResult
        ' 
        LstOcrResult.Enabled = False
        LstOcrResult.FormattingEnabled = True
        LstOcrResult.Location = New Point(860, 36)
        LstOcrResult.Name = "LstOcrResult"
        LstOcrResult.Size = New Size(259, 584)
        LstOcrResult.TabIndex = 15
        LstOcrResult.TabStop = False
        ' 
        ' MenuStrip1
        ' 
        MenuStrip1.ImageScalingSize = New Size(20, 20)
        MenuStrip1.Items.AddRange(New ToolStripItem() {SettingToolStripMenuItem})
        MenuStrip1.Location = New Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.Size = New Size(1129, 28)
        MenuStrip1.TabIndex = 16
        MenuStrip1.Text = "MenuStrip1"
        ' 
        ' SettingToolStripMenuItem
        ' 
        SettingToolStripMenuItem.Name = "SettingToolStripMenuItem"
        SettingToolStripMenuItem.Size = New Size(70, 24)
        SettingToolStripMenuItem.Text = "Setting"
        ' 
        ' btnTest
        ' 
        btnTest.Font = New Font("Yu Gothic UI", 10.2F)
        btnTest.Location = New Point(801, 687)
        btnTest.Name = "btnTest"
        btnTest.Size = New Size(123, 42)
        btnTest.TabIndex = 11
        btnTest.Text = "Test"
        btnTest.UseVisualStyleBackColor = True
        ' 
        ' cmbCallsign
        ' 
        cmbCallsign.DrawMode = DrawMode.OwnerDrawFixed
        cmbCallsign.Font = New Font("Yu Gothic UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        cmbCallsign.FormattingEnabled = True
        cmbCallsign.ImeMode = ImeMode.Disable
        cmbCallsign.Location = New Point(13, 646)
        cmbCallsign.Name = "cmbCallsign"
        cmbCallsign.Size = New Size(151, 31)
        cmbCallsign.TabIndex = 1
        cmbCallsign.Text = "Callsign"
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(936, 693)
        Button1.Name = "Button1"
        Button1.Size = New Size(130, 31)
        Button1.TabIndex = 17
        Button1.Text = "Button1"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(8.0F, 20.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1129, 744)
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
        MainMenuStrip = MenuStrip1
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
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents txtTime As TextBox
    Friend WithEvents txtBand As TextBox
    Friend WithEvents btnOfflineOcr As Button
    Friend WithEvents btnOnlineOcr As Button
    Friend WithEvents btnDeskew As Button
    Friend WithEvents LstOcrResult As ListBox
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents SettingToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents btnTest As Button
    Friend WithEvents cmbCallsign As ComboBox
    Friend WithEvents Button1 As Button

End Class
