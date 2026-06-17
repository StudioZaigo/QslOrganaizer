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
        txtMyCallsigns = New TextBox()
        txtOutputHolder = New TextBox()
        txtInputFolder = New TextBox()
        btnOK = New Button()
        btnCancel = New Button()
        txtOutputFolder = New TextBox()
        btnInputFolder = New Button()
        btnOutputFolder = New Button()
        lblCallsign = New Label()
        Label2 = New Label()
        Label3 = New Label()
        Label1 = New Label()
        SuspendLayout()
        ' 
        ' txtMyCallsigns
        ' 
        txtMyCallsigns.AcceptsReturn = True
        txtMyCallsigns.BorderStyle = BorderStyle.FixedSingle
        txtMyCallsigns.CharacterCasing = CharacterCasing.Upper
        txtMyCallsigns.Font = New Font("Yu Gothic UI", 10.2F)
        txtMyCallsigns.ImeMode = ImeMode.Disable
        txtMyCallsigns.Location = New Point(53, 41)
        txtMyCallsigns.Name = "txtMyCallsigns"
        txtMyCallsigns.Size = New Size(295, 30)
        txtMyCallsigns.TabIndex = 1
        ' 
        ' txtOutputHolder
        ' 
        txtOutputHolder.Location = New Point(0, 0)
        txtOutputHolder.Name = "txtOutputHolder"
        txtOutputHolder.Size = New Size(100, 27)
        txtOutputHolder.TabIndex = 0
        ' 
        ' txtInputFolder
        ' 
        txtInputFolder.BorderStyle = BorderStyle.FixedSingle
        txtInputFolder.Font = New Font("Yu Gothic UI", 10.2F)
        txtInputFolder.ImeMode = ImeMode.Disable
        txtInputFolder.Location = New Point(53, 103)
        txtInputFolder.Name = "txtInputFolder"
        txtInputFolder.Size = New Size(632, 30)
        txtInputFolder.TabIndex = 3
        ' 
        ' btnOK
        ' 
        btnOK.Font = New Font("Yu Gothic UI", 10.2F)
        btnOK.Location = New Point(559, 213)
        btnOK.Name = "btnOK"
        btnOK.Size = New Size(92, 43)
        btnOK.TabIndex = 9
        btnOK.Text = "OK"
        btnOK.UseVisualStyleBackColor = True
        ' 
        ' btnCancel
        ' 
        btnCancel.Font = New Font("Yu Gothic UI", 10.2F)
        btnCancel.Location = New Point(676, 213)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(92, 43)
        btnCancel.TabIndex = 10
        btnCancel.Text = "Cancel"
        btnCancel.UseVisualStyleBackColor = True
        ' 
        ' txtOutputFolder
        ' 
        txtOutputFolder.BorderStyle = BorderStyle.FixedSingle
        txtOutputFolder.Font = New Font("Yu Gothic UI", 10.2F)
        txtOutputFolder.ImeMode = ImeMode.Disable
        txtOutputFolder.Location = New Point(53, 168)
        txtOutputFolder.Name = "txtOutputFolder"
        txtOutputFolder.Size = New Size(632, 30)
        txtOutputFolder.TabIndex = 6
        ' 
        ' btnInputFolder
        ' 
        btnInputFolder.BackColor = SystemColors.Control
        btnInputFolder.Image = CType(resources.GetObject("btnInputFolder.Image"), Image)
        btnInputFolder.Location = New Point(702, 93)
        btnInputFolder.Name = "btnInputFolder"
        btnInputFolder.Size = New Size(47, 49)
        btnInputFolder.TabIndex = 4
        btnInputFolder.UseVisualStyleBackColor = False
        ' 
        ' btnOutputFolder
        ' 
        btnOutputFolder.Image = CType(resources.GetObject("btnOutputFolder.Image"), Image)
        btnOutputFolder.Location = New Point(702, 158)
        btnOutputFolder.Name = "btnOutputFolder"
        btnOutputFolder.Size = New Size(47, 49)
        btnOutputFolder.TabIndex = 8
        btnOutputFolder.UseVisualStyleBackColor = False
        ' 
        ' lblCallsign
        ' 
        lblCallsign.AutoSize = True
        lblCallsign.Font = New Font("Yu Gothic UI", 10.2F)
        lblCallsign.ForeColor = SystemColors.MenuHighlight
        lblCallsign.Location = New Point(40, 9)
        lblCallsign.Name = "lblCallsign"
        lblCallsign.Size = New Size(69, 23)
        lblCallsign.TabIndex = 0
        lblCallsign.Text = "&Callsign"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Yu Gothic UI", 10.2F)
        Label2.ForeColor = SystemColors.MenuHighlight
        Label2.Location = New Point(40, 74)
        Label2.Name = "Label2"
        Label2.Size = New Size(102, 23)
        Label2.TabIndex = 2
        Label2.Text = "&Input Folder"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Font = New Font("Yu Gothic UI", 10.2F)
        Label3.ForeColor = SystemColors.MenuHighlight
        Label3.Location = New Point(40, 139)
        Label3.Name = "Label3"
        Label3.Size = New Size(116, 23)
        Label3.TabIndex = 5
        Label3.Text = "&Output Folder"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(373, 46)
        Label1.Name = "Label1"
        Label1.Size = New Size(220, 20)
        Label1.TabIndex = 11
        Label1.Text = "複数Callsinは、カンマ区切りで入力"
        ' 
        ' frmSetting
        ' 
        AcceptButton = btnOK
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnCancel
        ClientSize = New Size(800, 268)
        Controls.Add(Label1)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(lblCallsign)
        Controls.Add(btnOutputFolder)
        Controls.Add(btnInputFolder)
        Controls.Add(txtOutputFolder)
        Controls.Add(btnCancel)
        Controls.Add(btnOK)
        Controls.Add(txtInputFolder)
        Controls.Add(txtMyCallsigns)
        ForeColor = SystemColors.ControlDarkDark
        FormBorderStyle = FormBorderStyle.FixedDialog
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MaximizeBox = False
        MinimizeBox = False
        Name = "Form2"
        StartPosition = FormStartPosition.CenterParent
        Text = "Setting"
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents txtOutputHolder As TextBox
    Friend WithEvents btnOK As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnInputFolder As Button
    Friend WithEvents btnOutputFolder As Button
    Friend WithEvents lblCallsign As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label1 As Label
    Public WithEvents txtMyCallsigns As TextBox
    Public WithEvents txtInputFolder As TextBox
    Public WithEvents txtOutputFolder As TextBox
End Class
