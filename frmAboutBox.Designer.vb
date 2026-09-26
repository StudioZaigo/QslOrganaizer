<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAboutBox
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        TableLayoutPanel = New TableLayoutPanel()
        Button1 = New Button()
        lstInformation = New ListBox()
        TextBoxDescription = New Label()
        TableLayoutPanel.SuspendLayout()
        SuspendLayout()
        ' 
        ' TableLayoutPanel
        ' 
        TableLayoutPanel.ColumnCount = 3
        TableLayoutPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 18F))
        TableLayoutPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 18F))
        TableLayoutPanel.Controls.Add(Button1, 1, 2)
        TableLayoutPanel.Controls.Add(lstInformation, 1, 0)
        TableLayoutPanel.Controls.Add(TextBoxDescription, 1, 1)
        TableLayoutPanel.Dock = DockStyle.Fill
        TableLayoutPanel.Location = New Point(10, 10)
        TableLayoutPanel.Margin = New Padding(4)
        TableLayoutPanel.Name = "TableLayoutPanel"
        TableLayoutPanel.RowCount = 3
        TableLayoutPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 50.0600052F))
        TableLayoutPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 40.85413F))
        TableLayoutPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 9.08587F))
        TableLayoutPanel.Size = New Size(463, 382)
        TableLayoutPanel.TabIndex = 0
        ' 
        ' Button1
        ' 
        Button1.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        Button1.DialogResult = DialogResult.Cancel
        Button1.Location = New Point(353, 353)
        Button1.Margin = New Padding(4)
        Button1.Name = "Button1"
        Button1.Size = New Size(88, 25)
        Button1.TabIndex = 2
        Button1.Text = "&OK"
        ' 
        ' lstInformation
        ' 
        lstInformation.BackColor = SystemColors.Menu
        lstInformation.Font = New Font("Yu Gothic UI", 9F)
        lstInformation.FormattingEnabled = True
        lstInformation.Location = New Point(21, 3)
        lstInformation.Name = "lstInformation"
        lstInformation.Size = New Size(419, 184)
        lstInformation.TabIndex = 4
        ' 
        ' TextBoxDescription
        ' 
        TextBoxDescription.AutoSize = True
        TextBoxDescription.Location = New Point(21, 191)
        TextBoxDescription.Name = "TextBoxDescription"
        TextBoxDescription.Size = New Size(419, 30)
        TextBoxDescription.TabIndex = 3
        TextBoxDescription.Text = "説明 :  (ランタイムに、ラベルのテキストはアプリケーションのアセンブリ情報に置き換えられます。"
        ' 
        ' frmAboutBox
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(483, 402)
        Controls.Add(TableLayoutPanel)
        FormBorderStyle = FormBorderStyle.FixedDialog
        Margin = New Padding(4)
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmAboutBox"
        Padding = New Padding(10)
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterParent
        Text = "AboutBox1"
        TableLayoutPanel.ResumeLayout(False)
        TableLayoutPanel.PerformLayout()
        ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel As TableLayoutPanel
    Friend WithEvents Button1 As Button
    Friend WithEvents TextBoxDescription As Label
    Friend WithEvents lstInformation As ListBox

End Class
