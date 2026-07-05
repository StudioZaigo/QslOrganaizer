Imports System.Drawing.Imaging
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text.Json
Imports System.Text.RegularExpressions
Imports System.Windows
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Button
Imports Microsoft.VisualBasic
'Imports OpenCvSharp
Imports Tesseract


Public Enum DateFormats As Integer
    InvalidFormat       ' 無効な形式
    IsoFormat           ' 2014年8月22日
    BritishFormat       ' 22/8/2014
    AmericanFormat      ' 8/22/2014
    MilitaryFormat      ' 22 Aug 2014 順としては日 月 年の順だが、月名を英語で表す形式もあるので、月を先に探す
End Enum

Public Enum BandFormats As Integer
    Unknown       ' 不明
    MHz           ' 周波数(MHz)
    kHz           ' 周波数(kHz)
    WaveLength    ' 波長
End Enum

Public Class frmMain

    Private currentImagePath As String = ""
    Public InputFileName As String
    Private FileList As List(Of String)
    Private FileIndex As Integer = 0

    'Private CurrentAutoMode As AutoMode = AutoMode.AutoOff

    Public InputFileFormat As System.Drawing.Imaging.ImageFormat
    Private Shared MyCallsigns As String
    Private Shared InputFolder As String
    Private Shared OutputFolder As String

    '    New AppSettings With
    '    {.MyCallsigns = "User1", .InputFolder = "C: \Input", .OutputFolder = "C:\Output", .CtyTimeStamp = DateTime.MinValue}
    Private ArrayMyCallsigns() As String

    Public Shared qsoCallsign As String
    Public Shared qsoDate As String
    Public Shared qsoTime As String
    Public Shared mode As String
    Public Shared freq As String

    Private Sub PicQsl_Click(sender As Object, e As EventArgs) Handles PicQsl.Click
        If (Control.ModifierKeys And Keys.Shift) = Keys.Shift Then
            PicQsl.Image.RotateFlip(RotateFlipType.Rotate90FlipNone)
            PicQsl.Refresh()
        End If
    End Sub


    Private Sub RunAutoOCR()
        If PicQsl.Image Is Nothing Then Exit Sub

        ' 既存の OCR 処理を呼ぶ
        btnOfflineOcr_Click(Nothing, Nothing)
    End Sub


    Private Sub SetImage(img As Image)
        ' PictureBox に画像があるかどうかでボタンの状態を更新
        Dim hasImage As Boolean = (img IsNot Nothing)

        btnOfflineOcr.Enabled = hasImage
        btnOnlineOcr.Enabled = hasImage
        btnSave.Enabled = hasImage
    End Sub

    Private Sub SetImageFile(fileName As String)

        Dim fileInfo As New System.IO.FileInfo(fileName)

        lstFileInfo.Items.Clear()

        If System.IO.File.Exists(fileName) Then
            '        Dim fileNameInfo As New System.IO.FileInfo(fileName)
            lstFileInfo.Items.Add("ファイル名: " & fileInfo.Name)
            '   s1 = fileInfo.Length.ToString("#,0")
            lstFileInfo.Items.Add("サイズ: " & fileInfo.Length.ToString("#,0") & " byte")

            ' Imageオブジェクトを作成
            Using img As Image = Image.FromFile(fileName)
                '     Using img

                For Each prop As PropertyItem In img.PropertyItems
                    If prop.Id = &H112 Then ' Orientation
                        Dim orientation As Integer = BitConverter.ToInt16(prop.Value, 0)

                        Select Case orientation
                            Case 2
                                'If img.Width < img.Height Then
                                '    img.RotateFlip(RotateFlipType.Rotate270FlipNone)
                                'End If
                            Case 3
                                img.RotateFlip(RotateFlipType.Rotate180FlipNone)
                            Case 6
                                img.RotateFlip(RotateFlipType.Rotate90FlipNone)
                            Case 8
                                img.RotateFlip(RotateFlipType.Rotate270FlipNone)
                            Case Else
                                'If img.Width < img.Height Then
                                '    img.RotateFlip(RotateFlipType.Rotate270FlipNone)
                                'End If
                        End Select

                        Exit For
                    End If
                Next

                PicQsl.Image = CType(img.Clone(), Image)

                ' 幅（Width）と高さ（Height）を取得
                ' 1. ピクセルサイズ
                Dim widthPx As Integer = img.Width
                Dim heightPx As Integer = img.Height
                ' 2. 解像度（DPI: Dots Per Inch）を取得
                Dim dpiX As Single = img.HorizontalResolution
                Dim dpiY As Single = img.VerticalResolution

                lstFileInfo.Items.Add("大きさ: " & widthPx.ToString("#") & "×" & heightPx.ToString("#") & " px")
                lstFileInfo.Items.Add("解像度: " & dpiX.ToString("#") & "x" & dpiY.ToString("#") & " dpi")

                Dim widthInch As Single = widthPx / dpiX
                Dim heightInch As Single = heightPx / dpiY
                lstFileInfo.Items.Add("横縦長: " & (widthInch * 2.54).ToString("#.0") & "×" & (heightInch * 2.54).ToString("#.0") & " mm")

            End Using
            lstFileInfo.Items.Add("作成日時: " & fileInfo.CreationTime)
        End If

        ' Return img
    End Sub

    Private Sub LoadNextFile()
        If FileList Is Nothing OrElse FileList.Count = 0 Then Exit Sub
        If FileIndex >= FileList.Count Then
            MessageBox.Show("すべてのファイルを処理しました")
            Exit Sub
        End If

        Dim file = FileList(FileIndex)
        FileIndex += 1

        ' 既存の画像読み込み処理を呼ぶ
        SetImageFile(file)
        currentImagePath = file
        InputFileName = file

        CheckFileName(InputFileName)
        cmbCallsign.Select()

        Text = "QslOrganizer - " & file
    End Sub


    ' 画像の上下左右を自動判断し,方向をそろえる機能
    ' 処理が重いので組み込まない
    'Private Shared Function AutoRotateImageByOCR(src As Bitmap, tessdataPath As String) As Bitmap
    '    Dim bestAngle As Integer = 0
    '    Dim bestScore As Integer = -1
    '    Dim angles() As Integer = {0, 90, 180, 270}

    '    Using engine As New TesseractEngine(tessdataPath, "eng", EngineMode.Default)
    '        For Each angle In angles
    '            Using rotated As Bitmap = RotateBitmap(src, angle)
    '                Using pix = BitmapToPix(rotated)
    '                    Using page = engine.Process(pix, PageSegMode.SingleBlock)

    '                        Dim text As String = page.GetText()

    '                        ' 読めた文字数をスコアとする
    '                        Dim score As Integer = text.Count(Function(c) Char.IsLetterOrDigit(c))

    '                        If score > bestScore Then
    '                            bestScore = score
    '                            bestAngle = angle
    '                        End If
    '                    End Using
    '                End Using
    '            End Using
    '        Next
    '    End Using

    '    Return RotateBitmap(src, bestAngle)
    'End Function

    'Private Shared Function BitmapToPix(bmp As Bitmap) As Pix
    '    Using ms As New MemoryStream()
    '        bmp.Save(ms, Imaging.ImageFormat.Png)
    '        ms.Position = 0
    '        Return Pix.LoadFromMemory(ms.ToArray())
    '    End Using
    'End Function

    'Private Shared Function RotateBitmap(src As Bitmap, angle As Integer) As Bitmap
    '    Dim rotated As New Bitmap(src.Height, src.Width)
    '    rotated.SetResolution(src.HorizontalResolution, src.VerticalResolution)

    '    Using g = Graphics.FromImage(rotated)
    '        g.TranslateTransform(rotated.Width / 2, rotated.Height / 2)
    '        g.RotateTransform(angle)
    '        g.TranslateTransform(-src.Width / 2, -src.Height / 2)
    '        g.DrawImage(src, New System.Drawing.Point(0, 0))
    '    End Using

    '    Return rotated
    'End Function



    Public Class ExplorerSort
        <DllImport("shlwapi.dll", CharSet:=CharSet.Unicode)>
        Private Shared Function StrCmpLogicalW(x As String, y As String) As Integer
        End Function

        Public Shared Function SortLikeExplorer(files As List(Of String)) As List(Of String)
            files.Sort(Function(a, b) StrCmpLogicalW(a, b))
            Return files
        End Function
    End Class

    Public Function ToHalfWidthAscii(s As String) As String
        Dim sb As New System.Text.StringBuilder(s.Length)
        For Each c In s
            Dim code = AscW(c)
            ' 全角英数字（FF01〜FF5E）を半角に変換
            If code >= &HFF01 AndAlso code <= &HFF5E Then
                sb.Append(ChrW(code - &HFEE0))
            Else
                sb.Append(c)
            End If
        Next
        Return sb.ToString()
    End Function



    Function CheckFileName(FileName As String) As Boolean

        Dim year, month, day, hour, minute As String
        Dim pattern As String

        Dim upper As String = Dir(FileName).ToUpper()           ' ファイル名を抜き出す 先にファイル名を抜き出し、その後大文字にする必要がある
        upper = ToHalfWidthAscii(upper)                         ' 全角文字を半角文字に

        If Microsoft.VisualBasic.Left(upper, 4) = "IMG_" Then Return False

        ' "A35TR_20140822_0022_15M_CW.jpg"の型式  日付と時刻の間に"_"がある
        pattern = "^([A-Z0-9]+)_([0-9]{8})_([0-9]{4})_([0-9]{1,4}[MGC])_([A-Z0-9]+)\.(JPG|JPEG|PNG|BMP)$"
        Dim m = Regex.Match(upper, pattern, RegexOptions.IgnoreCase)
        If m.Success Then
            cmbCallsign.Text = m.Groups(1).Value
            cmbCallsign.Items.Add(cmbCallsign.Text)
            year = m.Groups(2).Value.Substring(0, 4)
            month = m.Groups(2).Value.Substring(4, 2)
            day = m.Groups(2).Value.Substring(6, 2)
            hour = m.Groups(3).Value.Substring(0, 2)
            minute = m.Groups(3).Value.Substring(2, 2)
            txtDate.Text = $"{year:0000}/{month:00}/{day:00}"
            txtTime.Text = $"{hour:00}:{minute:00}"
            txtBand.Text = m.Groups(4).Value
            txtMode.Text = m.Groups(5).Value
            Return True
        End If

        ' "A31MM_201710290514_15M_SSB.JPG"の型式  日付と時刻の間に"_"がない
        pattern = "^([A-Z0-9]+)_([0-9]{8})([0-9]{4})_([0-9]{1,4}[MGC])_([A-Z0-9]+)\.(JPG|JPEG|PNG|BMP)$"
        m = Regex.Match(upper, pattern, RegexOptions.IgnoreCase)
        If m.Success Then
            cmbCallsign.Text = m.Groups(1).Value
            cmbCallsign.Items.Add(cmbCallsign.Text)
            year = m.Groups(2).Value.Substring(0, 4)
            month = m.Groups(2).Value.Substring(4, 2)
            day = m.Groups(2).Value.Substring(6, 2)
            hour = m.Groups(3).Value.Substring(0, 2)
            minute = m.Groups(3).Value.Substring(2, 2)
            txtDate.Text = $"{year:0000}/{month:00}/{day:00}"
            txtTime.Text = $"{hour:00}:{minute:00}"
            txtBand.Text = m.Groups(4).Value
            txtMode.Text = m.Groups(5).Value
            Return True
        End If

        ' "A35TR_140822_0022_21.200_CW_JA7FKF.jpg"の型式  ｈQSLの形式
        pattern = "^([A-Z0-9]+)_([0-9]{6})_([0-9]{4})_(\d+(\.\d+)?)_([A-Z0-9]+)_([A-Z0-9]+)\.JPG$"
        m = Regex.Match(upper, pattern, RegexOptions.IgnoreCase)
        If m.Success Then
            cmbCallsign.Text = m.Groups(1).Value
            cmbCallsign.Items.Add(cmbCallsign.Text)
            Dim s1 As String = Now().ToShortDateString.Substring(2, 2)
            year = m.Groups(2).Value.Substring(0, 2)
            If year > s1 Then
                year = "19" & year
            Else
                year = "20" & year
            End If
            month = m.Groups(2).Value.Substring(2, 2)
            day = m.Groups(2).Value.Substring(4, 2)
            hour = m.Groups(3).Value.Substring(0, 2)
            minute = m.Groups(3).Value.Substring(2, 2)
            txtDate.Text = $"{year:0000}/{month:00}/{day:00}"

            txtTime.Text = $"{hour:00}:{minute:00}"
            txtBand.Text = ConvertFreqToBand(m.Groups(4).Value)
            txtMode.Text = m.Groups(6).Value
            Return True
        End If

        ' "A35TR_001.jpg"の型式  Callsignと連続番号だけの形式
        pattern = "^([A-Z0-9]+)[_-]([0-9]{1,3})\.(JPG|JPEG|PNG|BMP)$"
        m = Regex.Match(upper, pattern, RegexOptions.IgnoreCase)
        If m.Success Then
            If CheckCallsignFormat(m.Groups(1).Value) Then
                cmbCallsign.Text = m.Groups(1).Value
                cmbCallsign.Items.Add(cmbCallsign.Text)
                Return True
            End If
        End If

        ' "A35TR(1).jpg"の型式  Callsignと連続番号だけの形式
        'pattern = "^([A-Z0-9]+)[(]([0-9]{1,3}[)])\.(JPG|JPEG|PNG|BMP)$"
        'm = Regex.Match(upper, pattern, RegexOptions.IgnoreCase)            
        If m.Success Then
            If CheckCallsignFormat(m.Groups(1).Value) Then
                cmbCallsign.Text = m.Groups(1).Value
                cmbCallsign.Items.Add(cmbCallsign.Text)
                Return True
            End If
        End If

        ' "JA7FKF-???.jpg"の型式  Callsignだけの形式 あるいはCallsign＋区切り記号＋その他の情報の形式
        pattern = "^([A-Z0-9]{1,10})([ _(\-].+\.)(JPG|JPEG|PNG|BMP)$"
        m = Regex.Match(upper, pattern, RegexOptions.IgnoreCase)
        If m.Success Then
            If CheckCallsignFormat(m.Groups(1).Value) Then
                cmbCallsign.Text = m.Groups(1).Value
                cmbCallsign.Items.Add(cmbCallsign.Text)
                Return True
            End If
        End If

        ' "JA7FKF.jpg"の型式  Callsignだけの形式 あるいはCallsign＋区切り記号＋その他の情報の形式
        pattern = "^([A-Z0-9]{1,10})\.(JPG|JPEG|PNG|BMP)$"
        m = Regex.Match(upper, pattern, RegexOptions.IgnoreCase)
        If m.Success Then
            If CheckCallsignFormat(m.Groups(1).Value) Then
                cmbCallsign.Text = m.Groups(1).Value
                cmbCallsign.Items.Add(cmbCallsign.Text)
                Return True
            End If
        End If

        Return False
    End Function




    ' 傾き補正　役立たず
    'Private Sub btnDeskew_Click(sender As Object, e As EventArgs) Handles btnDeskew.Click, btnDeskew.Click
    '    If currentImagePath = "" Then
    '        MessageBox.Show("先に画像を開いてください")
    '        Exit Sub
    '    End If

    '    Dim outputPath = IO.Path.Combine(
    '    IO.Path.GetDirectoryName(currentImagePath),
    '    "deskewed_" & IO.Path.GetFileName(currentImagePath)
    ')

    '    If ImagePreprocessor.Deskew(currentImagePath, outputPath) Then
    '        PicQsl.Image = Image.FromFile(outputPath)
    '        currentImagePath = outputPath   ' 補正後の画像を現在の画像として扱う
    '        MessageBox.Show("傾き補正が完了しました")
    '    End If
    'End Sub

    'Public Class ImagePreprocessor

    '    ' 傾き補正を行う関数
    '    Public Shared Function Deskew(inputPath As String, outputPath As String) As Boolean
    '        Try
    '            ' 画像読み込み（グレースケール）
    '            Using src As Mat = Cv2.ImRead(inputPath, ImreadModes.Grayscale)

    '                ' エッジ検出
    '                Dim edges As New Mat()
    '                Cv2.Canny(src, edges, 50, 200)

    '                ' Hough変換で直線検出
    '                Dim lines() As LineSegmentPolar = Cv2.HoughLines(edges, 1, Math.PI / 180, 150)

    '                If lines.Length = 0 Then
    '                    ' 傾きが検出できない場合はそのまま保存
    '                    Cv2.ImWrite(outputPath, src)
    '                    Return True
    '                End If

    '                ' 平均角度を求める
    '                Dim angle As Double = 0
    '                For Each line In lines
    '                    angle += line.Theta
    '                Next
    '                angle /= lines.Length

    '                ' ラジアン → 度に変換
    '                Dim angleDeg As Double = angle * 180 / Math.PI

    '                ' 画像の中心
    '                Dim center As New Point2f(src.Width / 2, src.Height / 2)

    '                ' 回転行列
    '                Dim rotMat As Mat = Cv2.GetRotationMatrix2D(center, angleDeg - 90, 1)

    '                ' 出力画像
    '                Dim dst As New Mat()
    '                Cv2.WarpAffine(src, dst, rotMat, src.Size())

    '                ' 保存
    '                Cv2.ImWrite(outputPath, dst)

    '                Return True
    '            End Using
    '        Catch ex As Exception
    '            MessageBox.Show("傾き補正中にエラー:    " & ex.Message)
    '            Return False
    '        End Try
    '    End Function

    'End Class



    Public Class OcrEngine
        Private ReadOnly _dataPath As String

        Public Sub New()
            ' 実行ファイルと同じフォルダに tessdata がある前提
            _dataPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata")
        End Sub

        Public Function RecognizeText(imagePath As String) As String
            Try
                Using engine As New TesseractEngine(_dataPath, "eng", EngineMode.Default)

                    Using img = Pix.LoadFromFile(imagePath)
                        Using page = engine.Process(img)
                            Return page.GetText()
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("OCR中にエラー: " & ex.Message)
                Return ""
            End Try
        End Function

        Public Function RecognizeTextFromImage(img As Image) As String
            Try
                Using engine As New TesseractEngine(_dataPath, "eng", EngineMode.Default)

                    ' ★ 英小文字を読ませない（大文字＋数字だけ許可）
                    '    engine.SetVariable("tessedit_char_whitelist", " ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789/.#-")
                    ' 結果悪し

                    ' Image → Pix に変換
                    Using bmp As New Bitmap(img)
                        Using ms As New MemoryStream()
                            bmp.Save(ms, Imaging.ImageFormat.Bmp)
                            ms.Position = 0
                            Using pix = Tesseract.Pix.LoadFromMemory(ms.ToArray())
                                Using page = engine.Process(pix)
                                    Return page.GetText()
                                End Using
                            End Using
                        End Using
                    End Using

                End Using

            Catch ex As Exception
                MessageBox.Show("OCR中にエラー: " & ex.Message)
                Return ""
            End Try
        End Function

    End Class


    Private ocr As New OcrEngine()
    Private Function RunOCR(img As Image) As String
        If chkAuto.Checked Then
            Return ocr.RecognizeTextFromImage(img)
            '               Return GoogleOcr(img)   ' ← 後で実装
            Return ""
        Else
            Return ocr.RecognizeTextFromImage(img)
        End If
        Return ""
    End Function




    Private Sub SetButtonColer()
        If chkAuto.Checked Then
            For Each ctrl As Control In pnlBtn.Controls
                If TypeOf ctrl Is Button Then
                    ctrl.BackColor = Color.AliceBlue
                End If
            Next
        Else
            For Each ctrl As Control In pnlBtn.Controls
                If TypeOf ctrl Is Button Then
                    ctrl.BackColor = Color.Honeydew
                End If
            Next
        End If
    End Sub

    Private Sub Control_Enter(sender As Object, e As EventArgs)
        If TypeOf sender Is TextBox OrElse TypeOf sender Is ComboBox Then
            sender.BackColor = SystemColors.GradientInactiveCaption
        End If
    End Sub

    Private Sub Control_Leave(sender As Object, e As EventArgs)
        If TypeOf sender Is TextBox OrElse TypeOf sender Is ComboBox Then
            sender.BackColor = SystemColors.Window
        End If
    End Sub



    Private Sub ComboBox_KeyPress(sender As Object, e As KeyPressEventArgs)
        ' 英字は大文字に変換
        If Char.IsLetter(e.KeyChar) Then
            e.KeyChar = Char.ToUpper(e.KeyChar)
        End If
    End Sub

    Private Sub SetComboPlaceholder(cb As ComboBox, placeholder As String)
        cb.Tag = placeholder
    End Sub

    Private Sub ComboBox_DrawItem(sender As Object, e As DrawItemEventArgs)
        Dim cb = CType(sender, ComboBox)

        e.DrawBackground()

        Dim text = ""
        Dim colors = Color.Black

        If e.Index < 0 Then
            ' --- プレースホルダー表示 ---
            If cb.Text = "" Then
                text = cb.Tag.ToString
                colors = Color.Gray
            Else
                text = cb.Text
                colors = cb.ForeColor
            End If
        Else
            ' --- 通常のアイテム ---
            text = cb.Items(e.Index).ToString
            colors = cb.ForeColor
        End If

        Using b As New SolidBrush(colors)
            e.Graphics.DrawString(text, cb.Font, b, e.Bounds)
        End Using

        e.DrawFocusRectangle()
    End Sub


    Private Function GetPrevInputControl(current As Control) As Control
        Dim parent As Control = current.Parent
        Dim controls = parent.Controls.Cast(Of Control)().
                   OrderBy(Function(c) c.TabIndex).ToList()

        Dim currentIndex = controls.IndexOf(current)

        ' 前方向
        For i As Integer = currentIndex - 1 To 0 Step -1
            If TypeOf controls(i) Is TextBox OrElse TypeOf controls(i) Is ComboBox Then
                Return controls(i)
            End If
        Next

        ' 最後に戻る
        For i As Integer = controls.Count - 1 To currentIndex + 1 Step -1
            If TypeOf controls(i) Is TextBox OrElse TypeOf controls(i) Is ComboBox Then
                Return controls(i)
            End If
        Next

        Return Nothing
    End Function


    Private Function GetNextInputControl(current As Control) As Control
        Dim parent As Control = current.Parent
        Dim controls = parent.Controls.Cast(Of Control)().
                   OrderBy(Function(c) c.TabIndex).ToList()

        Dim currentIndex = controls.IndexOf(current)

        ' 次のコントロールを順にチェック
        For i As Integer = currentIndex + 1 To controls.Count - 1
            If TypeOf controls(i) Is TextBox OrElse TypeOf controls(i) Is ComboBox Then
                Return controls(i)
            End If
        Next

        ' 見つからなければ先頭に戻る
        For i As Integer = 0 To currentIndex - 1
            If TypeOf controls(i) Is TextBox OrElse TypeOf controls(i) Is ComboBox Then
                Return controls(i)
            End If
        Next

        Return Nothing
    End Function



    Private Function SaveImageWithRules(inputImagePath As String) As Boolean

        Dim cs As String = cmbCallsign.Text.Trim().ToUpper()
        Dim dt As String = txtDate.Text.Trim()
        Dim tm As String = txtTime.Text.Trim()
        Dim bd As String = txtBand.Text.Trim()
        Dim md As String = txtMode.Text.Trim()

        '=== 入力チェック ===
        If cs = "" Then
            MessageBox.Show("Callsign が空です。保存できません。")
            Return False
        End If

        Dim allFilled As Boolean =
        (cs <> "" AndAlso dt <> "" AndAlso tm <> "" AndAlso bd <> "" AndAlso md <> "")

        Dim onlyCallsign As Boolean =
        (cs <> "" AndAlso dt = "" AndAlso tm = "" AndAlso bd = "" AndAlso md = "")

        If Not allFilled AndAlso Not onlyCallsign Then
            MessageBox.Show("Callsign 以外の項目が一部だけ入力されています。保存できません。")
            Return False
        End If

        '=== 保存先フォルダ決定 ===
        Dim baseFolder As String = OutputFolder
        If baseFolder = "" Then
            MessageBox.Show("OutputFolder が設定されていません。")
            Return False
        End If

        Dim prefix As String = cs.Substring(0, 2)
        Dim subFolder As String

        If (prefix Like "J[A-S]") OrElse (prefix Like "7[JKLNM]" OrElse prefix Like "8[JKLNM]") Then
            subFolder = cs.Substring(0, 3)      ' 7J, 8J, JA〜JS は3文字目まで、その他は1文字目までをサブフォルダにする
        Else
            subFolder = cs.Substring(0, 1)
        End If

        Dim saveFolder As String = Path.Combine(baseFolder, subFolder)

        If Not Directory.Exists(saveFolder) Then
            Directory.CreateDirectory(saveFolder)
        End If

        '=== 拡張子を入力ファイルから取得 ===
        Dim ext As String = Path.GetExtension(inputImagePath).ToLower()
        If ext = "" Then ext = ".jpg"

        '=== 保存ファイル名作成 ===
        Dim saveName As String = ""

        If allFilled Then
            ' 日付と時刻の記号を除去
            dt = dt.Replace("/", "")
            tm = tm.Replace(":", "")

            saveName = $"{cs}_{dt}_{tm}_{bd}_{md}{ext}"

        ElseIf onlyCallsign Then
            ' シリアル番号決定
            Dim pattern As String = $"{cs}_*.jpg"
            Dim files = Directory.GetFiles(saveFolder, pattern)

            Dim maxSerial As Integer = 0
            For Each f In files
                Dim fn = Path.GetFileNameWithoutExtension(f)
                Dim parts = fn.Split("_"c)
                If parts.Length = 2 Then
                    Dim num As Integer
                    If Integer.TryParse(parts(1), num) Then
                        If num > maxSerial Then maxSerial = num
                    End If
                End If
            Next

            Dim nextSerial As Integer = maxSerial + 1
            saveName = $"{cs}_{nextSerial.ToString("000")}{ext}"
        End If

        Dim savePath As String = Path.Combine(saveFolder, saveName)

        If File.Exists(savePath) Then
            MessageBox.Show($"{savePath} ファイルは既に存在します。")
            Return False
        End If

        '=== 画像保存 ===
        Try
            ' PNG形式で保存する場合
            Dim img As Image = PicQsl.Image
            InputFileFormat = img.RawFormat
            img.Save(savePath, InputFileFormat)

            img.Dispose()

            'File.Copy(inputImagePath, savePath, overwrite:=False)
        Catch ex As Exception
            MessageBox.Show("保存に失敗しました: " & ex.Message)
            Return False
        End Try


        '=== 元画像削除 ===
        Try
            File.Delete(inputImagePath)
        Catch ex As Exception
            MessageBox.Show("元画像の削除に失敗しました: " & ex.Message)
            ' 保存は成功しているので True を返す
        End Try


        Return True
    End Function

    Private Sub cmbCallsign_Leave(sender As Object, e As EventArgs)

        If Trim(cmbCallsign.Text) <> "" Then
            If Not CheckCallsignFormat(cmbCallsign.Text) Then
                MessageBox.Show("コールサインの形式が正しくありません。", "入力エラー",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)

                cmbCallsign.Focus()
                cmbCallsign.SelectAll()
            End If
        End If

    End Sub

    Private Function CheckCallsignFormat(cs As String) As Boolean
        Dim pattern As String = "^[1-9]?[A-Za-z]{1,5}[0-9]{1,5}[A-Za-z0-9]{1,8}$"

        Return System.Text.RegularExpressions.Regex.IsMatch(cs, pattern)

    End Function

    Private Sub txtDate_Leave(sender As Object, e As EventArgs)
        'If txtDate.Text <> "" Then
        '    Dim input = txtDate.Text.Trim
        '    Dim year As Integer
        '    Dim month As UInteger
        '    Dim day As Integer
        '    Dim parts = input.Split("/"c)

        '    ' 正規表現：YYYY/MM/DD または YY/MM/DD
        '    Dim pattern = "^(\d{2}|\d{4})/(0?[1-9]|1[0-2])/(0?[1-9]|[12]\d|3[01])$"
        '    If Not Regex.IsMatch(input, pattern) Then
        '        '         MessageBox.Show("日付の形式が正しくありません（YYYY/MM/DD）。", "入力エラー",
        '        '        MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '        '                txtDate.Focus()
        '        '                txtDate.SelectAll()
        '        '                Exit Sub


        '        If input.Length = 8 Then
        '            year = Mid(input, 1, 4)
        '            month = Mid(input, 5, 2)
        '            day = Mid(input, 7, 2)
        '        End If
        '        If input.Length = 6 Then
        '            year = Mid(input, 1, 2)
        '            month = Mid(input, 3, 2)
        '            day = Mid(input, 5, 2)
        '        End If
        '    Else

        '        ' --- ここから YY → YYYY の変換 ---
        '        year = Integer.Parse(parts(0))
        '        month = Integer.Parse(parts(1))
        '        day = Integer.Parse(parts(2))
        '    End If

        '    If year < 100 Then
        '        ' 00〜49 → 2000〜2049、50〜99 → 1950〜1999 とする
        '        If year <= 49 Then
        '            year += 2000
        '        Else
        '            year += 1900
        '        End If
        '    End If

        '    Dim dt As Date
        '    Try
        '        dt = New Date(year, month, day)
        '    Catch ex As Exception
        '        MessageBox.Show("存在しない日付です。", "入力エラー",
        '                    MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '        txtDate.Focus()
        '        txtDate.SelectAll()
        '        Exit Sub
        '    End Try

        '    ' --- 範囲チェック ---
        '    Dim minDate = New Date(1952, 7, 29)
        '    Dim maxDate = Date.Today

        '    If dt < minDate OrElse dt > maxDate Then
        '        MessageBox.Show("日付は 1952/07/29 〜 今日 の範囲で入力してください。", "入力エラー",
        '                    MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '        txtDate.Focus()
        '        txtDate.SelectAll()
        '        Exit Sub
        '    End If

        '    ' 正常 → YYYY/MM/DD に整形して戻す
        '    txtDate.Text = dt.ToString("yyyy/MM/dd")

        'End If
    End Sub

    Private Sub txtTime_Leave(sender As Object, e As EventArgs)
        If txtTime.Text <> "" Then

            Dim input = txtTime.Text.Trim

            ' 数字3～4桁のみ許可
            Dim pattern = "^(\d{1,2}:\d{2}|\d{3,4})$"
            If Not Regex.IsMatch(input, pattern) Then
                MessageBox.Show("時刻は HHMM の形式で入力してください。", "入力エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtTime.Focus()
                txtTime.SelectAll()
                Exit Sub
            End If

            ' --- HMM → HHMM に補正 ---
            If input.Length = 3 Then
                input = "0" & input
            End If
            ' とりあえず":"を削除する
            input = Replace(input, ":", "")

            ' --- 時と分を分解 ---
            Dim hour = Integer.Parse(input.Substring(0, 2))
            Dim minute = Integer.Parse(input.Substring(2, 2))

            ' --- 範囲チェック ---
            If hour < 0 OrElse hour > 23 OrElse minute < 0 OrElse minute > 59 Then
                MessageBox.Show("時刻が正しくありません（00:00～23:59）。", "入力エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtTime.Focus()
                txtTime.SelectAll()
                Exit Sub
            End If

            ' --- 正常なら HHMM に整形して戻す ---
            txtTime.Text = hour.ToString("00") & ":" & minute.ToString("00")
        End If
    End Sub

    Private Sub txtBand_Leave(sender As Object, e As EventArgs)
        If txtBand.Text <> "" Then

            Dim input = txtBand.Text.Trim.ToUpper

            ' 一致チェック
            If Not Bands.Contains(input) Then
                MessageBox.Show("バンドの入力が正しくありません。", "入力エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtBand.Focus()
                txtBand.SelectAll()
                Exit Sub
            End If

            ' 正常 → 正式表記に整形して戻す
            txtBand.Text = input
        End If
    End Sub

    Private Sub txtMode_Leave(sender As Object, e As EventArgs)
        If txtMode.Text <> "" Then

            Dim input = txtMode.Text.Trim.ToUpper

            ' 1. 安全に値を取得する（推奨）
            Dim val = txtMode.Text
            If AliasModes.TryGetValue(input, val) Then
                input = val
            End If

            If Not Modes.Contains(input) Then
                MessageBox.Show("モードの入力が正しくありません。", "入力エラー",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtMode.Focus()
                txtMode.SelectAll()
                Exit Sub
            End If

            ' 正常 → 正式表記に整形
            txtMode.Text = input
        End If
    End Sub


    Private Sub ClearForm(allClear As Boolean)

        btnOfflineOcr.Enabled = True
        If allClear Then
            Me.Text = My.Application.Info.ProductName
            PicQsl.Image = Nothing
            btnOfflineOcr.Enabled = False
        End If

        cmbCallsign.Text = ""
        cmbCallsign.Items.Clear()
        txtDate.Clear()
        txtTime.Clear()
        txtBand.Clear()
        txtMode.Clear()
        '    TxtOcrResult.Text = ""
        LstOcrResult.Items.Clear()
        lstFileInfo.Items.Clear()
    End Sub

    Private Sub SetButtons()
        If PicQsl.Image Is Nothing Then
            btnOfflineOcr.Enabled = False
            btnSave.Enabled = False
        Else
            btnOfflineOcr.Enabled = True
            If ((cmbCallsign.Text <> "") AndAlso (txtDate.Text <> "") AndAlso (txtTime.Text <> "") AndAlso (txtBand.Text <> "") AndAlso (txtMode.Text <> "")) _
            OrElse ((cmbCallsign.Text <> "") AndAlso (txtDate.Text = "") AndAlso (txtTime.Text = "") AndAlso (txtBand.Text = "") AndAlso (txtMode.Text = "")) Then
                btnSave.Enabled = True
            Else
                btnSave.Enabled = False
            End If

        End If
    End Sub

    'Private Sub SetPlaceholder(tb As TextBox, placeholder As String)
    '    tb.Tag = placeholder
    '    '      tb.ForeColor = Color.Gray
    '    tb.Text = placeholder
    'End Sub

    Private Sub TextBox_Enter(sender As Object, e As EventArgs)
        Dim tb As TextBox = CType(sender, TextBox)

        '      If tb.ForeColor = Color.Gray Then
        'tb.Text = ""
        tb.ForeColor = Color.Black
        '   End If
    End Sub

    Private Sub TextBox_Leave(sender As Object, e As EventArgs)
        Dim tb As TextBox = CType(sender, TextBox)

        'If tb.Text.Trim() = "" Then
        '    tb.ForeColor = Color.Gray
        '    tb.Text = tb.Tag.ToString()
        'End If
    End Sub

    Private Sub frmSettingOpen()
        Using form As New frmSetting()

            ' 設定を読み込み
            AppSettings.LoadJson(AppSettings.SettingsFile)

            form.txtMyCallsigns.Text = AppSettings.GetJson("General", "MyCallsigns", "")
            form.txtInputFolder.Text = AppSettings.GetJson("General", "InputFolder", "")
            form.txtOutputFolder.Text = AppSettings.GetJson("General", "OutputFolder", "")

            If form.ShowDialog() = DialogResult.OK Then
                ' 設定を保存
                AppSettings.SetJson("General", "MyCallsigns", form.txtMyCallsigns.Text)
                AppSettings.SetJson("General", "InputFolder", form.txtInputFolder.Text)
                AppSettings.SetJson("General", "OutputFolder", form.txtOutputFolder.Text)

                AppSettings.SaveJson(AppSettings.SettingsFile)

                MyCallsigns = form.txtMyCallsigns.Text
                InputFolder = form.txtInputFolder.Text
                OutputFolder = form.txtOutputFolder.Text

            End If
        End Using
    End Sub

    Private Sub frmAboutOpen()
        Using form As New AboutBox1
            form.ShowDialog()
        End Using
    End Sub

    Public Class AppSettings

        'Public Shared exePath = Process.GetCurrentProcess().MainModule.FileName
        'Public Shared AsyncbasePath = Path.GetDirectoryName(exePath)
        'Public Shared SettingsFile As String = Path.Combine(SettingsFile, "QslOrganizer.json")


        Public Shared basePath As String = AppDomain.CurrentDomain.BaseDirectory
        Public Shared SettingsFile As String = Path.Combine(basePath, My.Application.Info.Title & ".json")
        Public Shared JsonSettings As Dictionary(Of String, Object)

        Public Shared Sub LoadJson(FileName As String)

            Dim json As String
            If System.IO.File.Exists(AppSettings.SettingsFile) Then
                json = File.ReadAllText(FileName)
            Else
                json = "{ ""General"": {  }}"
            End If


            JsonSettings = JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(json,
        New JsonSerializerOptions With {.PropertyNameCaseInsensitive = True}
        )

            For Each section In JsonSettings.Keys.ToList()
                Dim sec = CType(JsonSettings(section), JsonElement)
                Dim dict = JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(sec.GetRawText())
                JsonSettings(section) = dict
            Next
        End Sub

        Public Shared Sub SaveJson(FileName As String)
            Dim jsonString As String = JsonSerializer.Serialize(JsonSettings,
        New JsonSerializerOptions With {.WriteIndented = True}
    )
            File.WriteAllText(FileName, jsonString)
        End Sub

        Public Shared Function GetJson(section As String, key As String, Initial As String) As String
            If Not JsonSettings.ContainsKey(section) Then Return Initial

            Dim sec = CType(JsonSettings(section), Dictionary(Of String, Object))

            If Not sec.ContainsKey(key) Then Return Initial

            Return sec(key).ToString()
        End Function

        Public Shared Function SetJson(section As String, key As String, value As String) As Boolean
            ' セクションがなければ作る
            If Not JsonSettings.ContainsKey(section) Then
                JsonSettings(section) = New Dictionary(Of String, Object)
            End If

            Dim sec = CType(JsonSettings(section), Dictionary(Of String, Object))

            ' キーを更新
            sec(key) = value

            Return True
        End Function

    End Class


    Private Sub TxtOcrResult_TextChanged(sender As Object, e As EventArgs)

    End Sub



    Shared Function Levenshtein(a As String, b As String) As Integer
        Dim dp(a.Length, b.Length) As Integer

        For i = 0 To a.Length
            dp(i, 0) = i
        Next
        For j = 0 To b.Length
            dp(0, j) = j
        Next

        For i = 1 To a.Length
            For j = 1 To b.Length
                Dim cost = If(a(i - 1) = b(j - 1), 0, 1)
                dp(i, j) = Math.Min(Math.Min(
                dp(i - 1, j) + 1,
                dp(i, j - 1) + 1),
                dp(i - 1, j - 1) + cost)
            Next
        Next

        Return dp(a.Length, b.Length)
    End Function

    Shared Function Similarity(a As String, b As String) As Double
        Dim d = Levenshtein(a, b)
        Dim maxLen = Math.Max(a.Length, b.Length)
        Return 1 - d / maxLen
    End Function








    Private Sub SetSaveButton()
        If ((cmbCallsign.Text <> "") AndAlso (txtDate.Text <> "") AndAlso (txtTime.Text <> "") AndAlso (txtBand.Text <> "") AndAlso (txtMode.Text <> "")) _
            OrElse ((cmbCallsign.Text <> "") AndAlso (txtDate.Text = "") AndAlso (txtTime.Text = "") AndAlso (txtBand.Text = "") AndAlso (txtMode.Text = "")) Then
            btnSave.Enabled = True
        Else
            btnSave.Enabled = False
        End If
    End Sub

    Private Sub frmMain_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        PicQsl.Top = 0

        With PicQsl
            .Top = 4
            .Left = 4
            .Height = PnlPic.Height - 8
            .Width = PnlPic.Width - 8
            .BackColor = SystemColors.Menu
        End With

        With LstOcrResult
            .Top = 4
            .Left = 4
            .Height = PnlList.Height - 8
            .Width = PnlList.Width - 8
            .BackColor = SystemColors.Menu
        End With

        With pnlBtn
            .BackColor = SystemColors.Menu
        End With

        With PnlList
            .BackColor = SystemColors.Menu
        End With
        With PnlPic
            .BackColor = SystemColors.Menu
        End With

    End Sub


    '------------------------------------------------------------------------------------------------------------------
    '
    ' ここから先、コントロールのイベント処理
    '
    '---------------------------------------------------------------------------   ------------------------------------

    Private Sub frmMain_Activated(sender As Object, e As EventArgs) Handles MyBase.Activated
        Debug.Print(Me.ActiveControl.Name)

        If (TypeOf Me.ActiveControl IsNot TextBox) AndAlso (TypeOf Me.ActiveControl IsNot ComboBox) Then
            cmbCallsign.Select()

        End If

    End Sub

    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' 設定を読み込み
        AppSettings.LoadJson(AppSettings.SettingsFile)

        Dim AppWindowTop = Me.Top
        Dim AppWindowLeft = Me.Left

        AppSettings.SetJson(Me.Name, "Top", AppWindowTop.ToString())            ' 設定を保存
        AppSettings.SetJson(Me.Name, "Left", AppWindowLeft.ToString())

        AppSettings.SaveJson(AppSettings.SettingsFile)                          ' 設定を書き込み
    End Sub

    Private Sub frmMain_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.Control Then
            If e.KeyCode = Keys.Enter Then
                ' 警告音（ピピッという音）を鳴らさないようにする
                e.SuppressKeyPress = True

                ' ここに実行したい処理を書く
                btnSave.PerformClick()
                '           MessageBox.Show("Ctrl + Enter が押されました！") 
            ElseIf e.KeyCode = Keys.Delete Then
                If txtDate.Text <> "" OrElse txtTime.Text <> "" OrElse txtBand.Text <> "" OrElse txtMode.Text <> "" Then
                    txtDate.Text = ""
                    txtTime.Text = ""
                    txtBand.Text = ""
                    txtMode.Text = ""
                ElseIf cmbCallsign.Text <> "" AndAlso txtDate.Text = "" AndAlso txtTime.Text = "" AndAlso txtBand.Text = "" AndAlso txtMode.Text = "" Then
                    cmbCallsign.Text = ""
                End If
                cmbCallsign.Select()
            End If
        End If
    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.Height = 600
        PnlList.Height = PnlPic.Height
        LstOcrResult.Dock = DockStyle.Fill

        ' 設定を読み込み


        AppSettings.LoadJson(AppSettings.SettingsFile)
            'Else
            '    AppSettings.SettingsFile
            'End If

            Dim DisplayWorkRect As Rectangle
        DisplayWorkRect = Screen.PrimaryScreen.WorkingArea

        Dim AppWindowTop = AppSettings.GetJson(Me.Name, "Top", "-1")
        Dim AppWindowLeft = AppSettings.GetJson("frmMain", "Left", "-1")
        If (AppWindowTop = -1) AndAlso (AppWindowLeft = -1) Then
            Me.Top = (DisplayWorkRect.Height - Me.Height) / 2
            Me.Left = (DisplayWorkRect.Width - Me.Width) / 2
        Else
            Me.Top = AppWindowTop
            Me.Left = AppWindowLeft
        End If

        Dim IntFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
        MyCallsigns = AppSettings.GetJson("General", "MyCallsigns", "")
        InputFolder = AppSettings.GetJson("General", "InputFolder", IntFolder)

        OutputFolder = AppSettings.GetJson("General", "OutputFolder", IntFolder)

        'If InputFolder.Trim = "" Then
        '    InputFolder = IntFolder
        'End If
        'If OutputFolder.Trim = "" Then
        '    OutputFolder = IntFolder
        'End If

        LoadCtyDatabase()               ' cty.json 読み込み

        ArrayMyCallsigns = MyCallsigns.Split(","c)

        For Each c As Control In Controls
            If TypeOf c Is ComboBox Then
                AddHandler c.KeyPress, AddressOf ComboBox_KeyPress
            End If
        Next

        For Each c As Control In Controls
            If TypeOf c Is TextBox OrElse TypeOf c Is ComboBox Then
                AddHandler c.Enter, AddressOf Control_Enter
                AddHandler c.Leave, AddressOf Control_Leave
                AddHandler c.KeyDown, AddressOf Control_KeyDown
            End If
        Next

        ' 例：コールサイン欄
        'SetPlaceholder(cmbCallsign, "コールサインを入力")

        ' 例：日付欄
        'SetPlaceholder(txtDate, "YYYY/MM/DD")

        ' 例：時刻欄
        'SetPlaceholder(txtTime, "HH:MM")
        'SetPlaceholder(txtBand, "Band")
        'SetPlaceholder(txtMode, "Mode")

        ' イベント割り当て（TextBox 全体に適用）
        For Each c As Control In Controls
            If TypeOf c Is TextBox Then
                AddHandler c.Enter, AddressOf TextBox_Enter
                AddHandler c.Leave, AddressOf TextBox_Leave
            End If
        Next

        SetButtonColer()

        ClearForm(True)

        If MyCallsigns = "" OrElse InputFolder = "" OrElse OutputFolder = "" Then
            frmSettingOpen()
        End If

        chkAuto.Checked = False

    End Sub

    Private Sub LstOcrResult_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LstOcrResult.SelectedIndexChanged
        ' "コピーするテキスト" をクリップボードに保存
        Clipboard.SetText(String.Join(Environment.NewLine, LstOcrResult.Items.Cast(Of String)))

    End Sub

    Private Sub cmbCallsign_TextChanged(sender As Object, e As EventArgs) Handles cmbCallsign.TextChanged
        cmbCallsign.Text = cmbCallsign.Text.Trim

        Dim cb = DirectCast(sender, ComboBox)
        Dim start As Integer = cb.SelectionStart
        cb.Text = cb.Text.ToUpper() ' 小文字にする場合は .ToLower()
        cb.SelectionStart = start

        qsoCallsign = cmbCallsign.Text

        SetSaveButton()
    End Sub

    Private Sub ComboBox_Leave(sender As Object, e As EventArgs) Handles cmbCallsign.Leave
        Dim cb As ComboBox = CType(sender, ComboBox)

        If cb.Text.Trim() = "" Then
            cb.Text = ""
        End If
    End Sub

    Private Sub txtDate_TextChanged(sender As Object, e As EventArgs) Handles txtDate.TextChanged
        txtDate.Text = txtDate.Text.Trim
        SetSaveButton()
    End Sub

    Private Sub txtDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtDate.Validating

        txtDate.Text = txtDate.Text.Trim.ToUpper

        If txtDate.Text = "" Then Exit Sub

        txtDate.Text = txtDate.Text.Replace("-", "/")

        Dim year As String = "0000"
        Dim month As String = "00"
        Dim day As String = "00"
        Dim ymd As String

        Dim parts = txtDate.Text.Split("/"c)

        If parts.Count = 3 Then
            year = parts(0)
            month = parts(1)
            day = parts(2)
        ElseIf parts.Count = 2 Then
            month = parts(0)
            day = parts(1)
            If (month > Now.Month) OrElse ((month = Now.Month) AndAlso (day > Now.Day)) Then
                year = Now.Year - 1
            Else
                year = Now.Year
            End If
        ElseIf parts.Count = 1 Then
            ymd = "0000" & txtDate.Text
            day = ymd.Substring(ymd.Length - 2, 2)
            month = ymd.Substring(ymd.Length - 4, 2)
            Select Case txtDate.Text.Length
                Case 3, 4
                    year = Now.Year
                Case 6
                    year = Now.Year - (Now.Year Mod 100) + txtDate.Text.Substring(0, 2)

                    '       dt = New Date(year, month, day)
                    If New Date(year, month, day) > Now Then
                        year = (Now.Year - (Now.Year Mod 100) - 1).ToString().Substring(0, 2) & txtDate.Text.Substring(0, 2)

                    End If
                    'If (month > Now.Month) OrElse ((month = Now.Month) AndAlso (day > Now.Day)) Then
                    '    year = Now.Year - 1
                    'Else
                    '    year = Now.Year
                    '    year = Now.Year - (Now.Year Mod 100) + txtDate.Text.Substring(0, 2)
                    'End If


                Case 8
                    year = txtDate.Text.Substring(0, 4)
            End Select
        End If

        If year.Length = 2 Then
            year = Now.Year - (Now.Year Mod 100) + year
        End If
        If month.Length <> 2 Then
            month = ("00" & month)
            month = month.Substring(month.Length - 2, 2)
        End If
        If day.Length <> 2 Then
            day = ("00" & day)
            day = day.Substring(day.Length - 2, 2)
        End If

        ymd = year & "/" & month & "/" & day

        ' 正規表現：YYYY/MM/DD または YY/MM/DD
        Dim pattern = "^(19|20)\d{2}/(0[1-9]|1[0-2])/([0-2]\d|3[01])$"
        If Not Regex.IsMatch(ymd, pattern) Then
            MessageBox.Show("日付の形式が正しくありません（YYYY/MM/DD）。", "入力エラー")
            e.Cancel = True
            Exit Sub
        End If

        Dim dt As Date
        Try
            dt = New Date(year, month, day)
            If dt > Now Then
                MessageBox.Show("日付は未来の日付に設定できません。", "入力エラー")
                e.Cancel = True
                Exit Sub
            End If
        Catch ex As Exception
            MessageBox.Show("存在しない日付です。", "入力エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            e.Cancel = True
            Exit Sub
        End Try

        ' --- 範囲チェック ---
        Dim minDate = New Date(1952, 7, 29)
        Dim maxDate = Date.Today

        If dt < minDate OrElse dt > maxDate Then
            MessageBox.Show("日付は 1952/07/29 〜 今日 の範囲で入力してください。", "入力エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            e.Cancel = True
            Exit Sub
        End If

        ' 正常 → YYYY/MM/DD に整形して戻す
        txtDate.Text = dt.ToString("yyyy/MM/dd")
    End Sub

    Private Sub txtTime_TextChanged(sender As Object, e As EventArgs) Handles txtTime.TextChanged
        txtTime.Text = txtTime.Text.Trim
        SetSaveButton()
    End Sub

    Private Sub txtTime_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtTime.Validating
        txtTime.Text = txtTime.Text.Trim.ToUpper

        If txtTime.Text = "" Then Exit Sub

        Dim hour As String
        Dim minutes As String

        Dim foundIndex = txtTime.Text.IndexOf(":")
        If foundIndex >= 0 Then
            hour = "00" & txtTime.Text.Substring(0, foundIndex)
            minutes = "00" & txtTime.Text.Substring(foundIndex + 1, txtTime.Text.Length - foundIndex - 1)
            hour = hour.Substring(hour.Length - 2, 2)
            minutes = minutes.Substring(minutes.Length - 2, 2)
        Else
            Dim t = "0000" & txtTime.Text
            hour = t.Substring(t.Length - 4, 2)
            minutes = t.Substring(t.Length - 2, 2)

        End If
        txtTime.Text = hour & ":" & minutes

        ' 正規表現：HH:MM
        Dim pattern = "^([0-1]\d|2[0-3]):([0-5]\d)$"
        If Not Regex.IsMatch(txtTime.Text, pattern) Then
            MessageBox.Show("時間の形式が正しくありません（HH:MM）。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            e.Cancel = True
        End If

    End Sub

    Private Sub txtBand_TextChanged(sender As Object, e As EventArgs) Handles txtBand.TextChanged
        txtBand.Text = txtBand.Text.Trim
        SetSaveButton()
    End Sub

    Private Sub txtBand_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtBand.Validating
        txtBand.Text = txtBand.Text.Trim.ToUpper

        If txtBand.Text = "" Then Exit Sub

        If txtBand.Text.Substring(txtBand.Text.Length - 1, 1) = "M" Then
            For Each b In BandList.Bands        ' 40M, 2M 等の波長表示
                If txtBand.Text.Contains(b) Then Exit Sub
            Next
        Else
            Dim b = ConvertFreqToBand(txtBand.Text)
            If b <> "" Then
                txtBand.Text = b
                Exit Sub
            End If

        End If

        Dim ErrMess1 = "Bandの値が誤りです"
        Dim ErrMess2 = "エラー"
        MessageBox.Show(ErrMess1, ErrMess2, MessageBoxButtons.OK, MessageBoxIcon.Error)
        e.Cancel = True
    End Sub

    Private Sub txtMode_TextChanged(sender As Object, e As EventArgs) Handles txtMode.TextChanged
        txtMode.Text = txtMode.Text.Trim
        SetSaveButton()
    End Sub

    Private Sub txtMode_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtMode.Validating
        txtMode.Text = txtMode.Text.Trim.ToUpper

        If txtMode.Text = "" Then Exit Sub

        For Each m In ModeList.Modes        ' 40M, 2M 等の波長表示
            If txtMode.Text.Contains(m) Then Exit Sub
        Next

        If ModeList.AliasModes.ContainsKey(txtMode.Text) Then
            txtMode.Text = ModeList.AliasModes(txtMode.Text)
            Exit Sub
        End If

        Dim ErrMess1 = "Modeの値が誤りです"
        Dim ErrMess2 = "エラー"
        MessageBox.Show(ErrMess1, ErrMess2, MessageBoxButtons.OK, MessageBoxIcon.Error)
        e.Cancel = True

    End Sub

    Private Sub btnOpenImage_Click(sender As Object, e As EventArgs) Handles btnOpenImage.Click

        Static InitSub = True

        Dim dlg As New OpenFileDialog
        Dim input = "qsl_original.jpg"
        Dim output = "qsl_corrected.jpg"

        dlg.Filter = "画像ファイル|*.jpg;*.jpeg;*.png;*.bmp"
        If InitSub Then
            dlg.InitialDirectory = InputFolder
            InitSub = False
        End If

        ' 古い画像を開放
        If PicQsl.Image IsNot Nothing Then
            PicQsl.Image.Dispose()
            PicQsl.Image = Nothing
        End If

        ClearForm(False)

        If dlg.ShowDialog = DialogResult.OK Then
            currentImagePath = dlg.FileName

            ' 新しい画像を安全に読み込む
            Dim img = Image.FromFile(dlg.FileName)
            SetImageFile(dlg.FileName)

            'Using img = Image.FromFile(dlg.FileName)
            '    ' PictureBoxに表示する場合
            '    'PictureBox1.Image = New Bitmap(img)


            '    SetImageFile(dlg.FileName)
            'End Using ' ここで自動的に開放される

            ' 画像の上下左右を自動判断し,方向をそろえる機能
            ' 処理が重いので組み込まない
            'PicQsl.Image = AutoRotateImageByOCR(PicQsl.Image, System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata"))

            InputFileName = dlg.FileName
            InputFileFormat = img.RawFormat

            img.Dispose()

            If InputFileName = "" Then
                Text = "QslOrganizer"
            Else
                Text = "QslOrganizer - " & InputFileName
            End If

            CheckFileName(dlg.FileName)
            cmbCallsign.Select()
            '     End If

            Dim folder = Path.GetDirectoryName(dlg.FileName)
            Dim files = Directory.GetFiles(folder, "*.jpg").ToList()

            ' エクスプローラと同じ順番に並べる
            FileList = ExplorerSort.SortLikeExplorer(files)

            ' 現在のファイルの位置をセット
            FileIndex = FileList.IndexOf(dlg.FileName) + 1

            If chkAuto.Checked Then
                LoadNextFile()
                CheckFileName(InputFileName)
                RunAutoOCR()
            End If
        End If

    End Sub

    Private Sub Control_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbCallsign.KeyDown, btnOpenImage.KeyDown, btnOfflineOcr.KeyDown, btnSave.KeyDown, btnOnlineOcr.KeyDown, txtDate.KeyDown, txtTime.KeyDown, txtBand.KeyDown, txtMode.KeyDown, btnOnlineOcr.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True

            Dim current = CType(sender, Control)
            Dim nextCtrl As Control

            If e.Shift Then
                ' Shift + Enter → 前へ
                nextCtrl = GetPrevInputControl(current)
            Else
                ' Enter → 次へ
                nextCtrl = GetNextInputControl(current)
            End If

            If nextCtrl IsNot Nothing Then
                nextCtrl.Focus()
            End If
        End If
    End Sub

    Private Sub btnOfflineOcr_Click(sender As Object, e As EventArgs) Handles btnOfflineOcr.Click

        If currentImagePath = "" Then
            MessageBox.Show("先に画像を開いてください")
            Exit Sub
        End If

        'Dim text = ocr.RecognizeText(currentImagePath)
        'Dim text = ocr.RecognizeTextFromImage(PicQsl.Image)
        Dim text = RunOCR(PicQsl.Image)

        If text = "" Then
            MessageBox.Show("OCR結果が空です")
            Exit Sub
        End If

        ' とりあえずメッセージで全文を確認5
        Dim preview = If(text.Length > 500, text.Substring(0, 500) & " ...", text)
        Dim s = preview.Split(vbCr)

        LstOcrResult.Items.Clear()
        LstOcrResult.Items.AddRange(text.Split(New String() {vbLf}, StringSplitOptions.RemoveEmptyEntries))

        ' 全体としての正規化
        text = Trim(text.Trim.ToUpper)
        text = text.Replace("　", " ")               ' 全角スペースを半角スペースに置き換え 
        text = text.Replace("|", " ")               ' 縦罫線を半角スペースに置き換え
        text = text.Replace("[", " ")
        text = text.Replace("]", "J")
        text = text.Replace("{", " ")

        text = text.Replace("€", "0")               ' €はCの誤読にも
        text = text.Replace("§", "8")
        text = text.Replace(ChrW(&H2014), "-"c)     '  EM DASHを半角"-"に置き換える
        'Dim dashChars = {ChrW(&H2014), ChrW(&H2013), ChrW(&H2212), ChrW(&H2010), ChrW(&H2015)}     ' 今後のため残す

        text = Regex.Replace(text, " +", " ")       ' 連続する複数のスペースをひとつにする

        UsedRanges.Clear()      ' OCR → Extract の処理へ    ' 既出の文字列リストをクリアする

        Dim candidates As New List(Of String)
        If cmbCallsign.Text <> "" Then candidates.Add(cmbCallsign.Text)
        Dim lst = RegexExtractor.ExtractCallsignCandidates(text)         ' Callsign項目を抽出
        candidates.AddRange(lst)         ' Callsign項目を抽出
        candidates = RegexExtractor.RemoveMyCallsign(MyCallsigns, candidates)    ' 自分のコールサインを除外
        Dim callsign = RegexExtractor.ChooseBestCallsign(candidates, ArrayMyCallsigns(0))         ' 最も正しいものを選ぶ
        qsoCallsign = callsign

        qsoDate = RegexExtractor.ExtractDate(text)
        qsoTime = RegexExtractor.ExtractTime(text)
        mode = RegexExtractor.ExtractMode(text)
        freq = RegexExtractor.ExtractBand(text)

        ' UI に反映
        cmbCallsign.Items.Clear()

        For Each cs In candidates
            cmbCallsign.Items.Add(cs)
        Next
        If cmbCallsign.Text = "" Then
            cmbCallsign.Text = callsign
        End If
        cmbCallsign.Text = callsign
        txtDate.Text = qsoDate
        txtTime.Text = qsoTime
        txtMode.Text = mode
        txtBand.Text = freq

        SetButtons()

        cmbCallsign.Select()
    End Sub

    Private Sub btnOnlineOcr_Click(sender As Object, e As EventArgs) Handles btnOnlineOcr.Click

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        SaveImageWithRules(InputFileName)
        PicQsl.Image = Nothing
        SetImage(Nothing)
        ClearForm(True)

        ' ★ 保存が成功したら次のファイルを読み込む
        If chkAuto.Checked Then
            LoadNextFile()
            RunAutoOCR()
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Application.Exit()
    End Sub

    Private Sub btnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click

        Dim pattern = "(?<=\s|:|;|\b)(\d{1,7})([.,\.]{0,2})(\d{0,6})(?=[\s\]MKG]|$)"           ' 直前が空白であり、直後が数字以外であること
        Dim text = "7 DEC'24 09:20 +14 7 F18
"
        '     ' まず変換する前に実行する
        For Each m In Regex.Matches(text, pattern)

            MessageBox.Show(m.Groups(0).Value & " index=" & m.groups(0).index & " length=" & m.groups(0).length)

        Next
    End Sub

    Private Sub mnuSetting_Click(sender As Object, e As EventArgs) Handles mnuSetting.Click
        frmSettingOpen()
    End Sub

    Private Sub mnuAbout_Click(sender As Object, e As EventArgs) Handles mnuAbout.Click
        Using form As New AboutBox1
            form.ShowDialog()
        End Using
    End Sub

End Class
