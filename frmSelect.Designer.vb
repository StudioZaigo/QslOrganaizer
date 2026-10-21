<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmSelect
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSelect))
        rbtnOldFile = New RadioButton()
        rbtnNewFile = New RadioButton()
        rbtnBotthFile = New RadioButton()
        btnOk = New Button()
        Label1 = New Label()
        Panel2 = New Panel()
        Panel1 = New Panel()
        Label2 = New Label()
        PicOldQsl = New PictureBox()
        btnCancel = New Button()
        Panel2.SuspendLayout()
        Panel1.SuspendLayout()
        CType(PicOldQsl, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' rbtnOldFile
        ' 
        rbtnOldFile.AutoSize = True
        rbtnOldFile.Checked = True
        rbtnOldFile.Location = New Point(21, 38)
        rbtnOldFile.Name = "rbtnOldFile"
        rbtnOldFile.Size = New Size(104, 19)
        rbtnOldFile.TabIndex = 0
        rbtnOldFile.TabStop = True
        rbtnOldFile.Text = "旧ファイルを保存"
        rbtnOldFile.UseVisualStyleBackColor = True
        ' 
        ' rbtnNewFile
        ' 
        rbtnNewFile.AutoSize = True
        rbtnNewFile.Location = New Point(139, 38)
        rbtnNewFile.Name = "rbtnNewFile"
        rbtnNewFile.Size = New Size(104, 19)
        rbtnNewFile.TabIndex = 1
        rbtnNewFile.Text = "新ファイルを保存"
        rbtnNewFile.UseVisualStyleBackColor = True
        ' 
        ' rbtnBotthFile
        ' 
        rbtnBotthFile.AutoSize = True
        rbtnBotthFile.Location = New Point(260, 38)
        rbtnBotthFile.Name = "rbtnBotthFile"
        rbtnBotthFile.Size = New Size(104, 19)
        rbtnBotthFile.TabIndex = 2
        rbtnBotthFile.Text = "両ファイルを保存"
        rbtnBotthFile.UseMnemonic = False
        rbtnBotthFile.UseVisualStyleBackColor = True
        ' 
        ' btnOk
        ' 
        btnOk.Location = New Point(403, 21)
        btnOk.Margin = New Padding(3, 3, 3, 10)
        btnOk.Name = "btnOk"
        btnOk.Size = New Size(95, 32)
        btnOk.TabIndex = 3
        btnOk.Text = "OK"
        btnOk.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(9, 12)
        Label1.Name = "Label1"
        Label1.Size = New Size(117, 15)
        Label1.TabIndex = 5
        Label1.Text = "保存ファイルを指定する"
        ' 
        ' Panel2
        ' 
        Panel2.AutoSize = True
        Panel2.Controls.Add(btnCancel)
        Panel2.Controls.Add(Label1)
        Panel2.Controls.Add(btnOk)
        Panel2.Controls.Add(rbtnBotthFile)
        Panel2.Controls.Add(rbtnNewFile)
        Panel2.Controls.Add(rbtnOldFile)
        Panel2.Dock = DockStyle.Bottom
        Panel2.Location = New Point(0, 362)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(618, 63)
        Panel2.TabIndex = 1
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(Label2)
        Panel1.Controls.Add(PicOldQsl)
        Panel1.Dock = DockStyle.Fill
        Panel1.Font = New Font("Yu Gothic UI", 14F)
        Panel1.Location = New Point(0, 0)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(618, 425)
        Panel1.TabIndex = 0
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.BackColor = Color.Transparent
        Label2.Font = New Font("HG丸ｺﾞｼｯｸM-PRO", 14F, FontStyle.Italic)
        Label2.ForeColor = SystemColors.ControlLightLight
        Label2.Location = New Point(59, 78)
        Label2.Name = "Label2"
        Label2.Size = New Size(104, 19)
        Label2.TabIndex = 1
        Label2.Text = "旧ファイル"
        ' 
        ' PicOldQsl
        ' 
        PicOldQsl.BackColor = SystemColors.Control
        PicOldQsl.BorderStyle = BorderStyle.FixedSingle
        PicOldQsl.Location = New Point(47, 60)
        PicOldQsl.Margin = New Padding(6)
        PicOldQsl.Name = "PicOldQsl"
        PicOldQsl.Padding = New Padding(6)
        PicOldQsl.Size = New Size(490, 222)
        PicOldQsl.SizeMode = PictureBoxSizeMode.Zoom
        PicOldQsl.TabIndex = 0
        PicOldQsl.TabStop = False
        ' 
        ' btnCancel
        ' 
        btnCancel.Location = New Point(504, 21)
        btnCancel.Margin = New Padding(3, 3, 3, 10)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(95, 32)
        btnCancel.TabIndex = 6
        btnCancel.Text = "Cancel"
        btnCancel.UseVisualStyleBackColor = True
        ' 
        ' frmSelect
        ' 
        AcceptButton = btnOk
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnCancel
        ClientSize = New Size(618, 425)
        Controls.Add(Panel2)
        Controls.Add(Panel1)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Name = "frmSelect"
        Text = "Select　Image file "
        TopMost = True
        Panel2.ResumeLayout(False)
        Panel2.PerformLayout()
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        CType(PicOldQsl, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents rbtnOldFile As RadioButton
    Friend WithEvents rbtnNewFile As RadioButton
    Friend WithEvents rbtnBotthFile As RadioButton
    Friend WithEvents btnOk As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents PicOldQsl As PictureBox
    Friend WithEvents Label2 As Label
    Friend WithEvents btnCancel As Button


End Class
