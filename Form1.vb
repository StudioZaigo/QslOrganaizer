Imports System.IO
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Reflection
Imports System.Text.Json
Imports System.Text.RegularExpressions
Imports OpenCvSharp
Imports Tesseract

'Imports Newtonsoft.Json.Linq

Public Module BandList
    Public ReadOnly Bands As String() = {
        "160M", "80M", "60M", "40M", "30M", "20M",
        "17M", "15M", "12M", "10M", "6M", "2M", "70CM",
        "23CM", "2200M", "630M", "12CM", "5CM", "3CM"
    }
End Module

Module ModeList
    Public ReadOnly Modes As String() = {
        "CW", "SSB", "AM", "FM",
        "RTTY", "FT8", "FT4", "PSK31",
        "JT65", "JT9", "SSTV", "ATV",
        "AMTOR", "PACKET", "FSK", "MFSK",
        "C4FM", "D-STAR"
    }
End Module



Public Class Form1

    Private currentImagePath As String = ""
    Public MyCallsign As String
    Public InputFolder As String
    Public OutputFolder As String
    Public InputFileName As String

    Private Sub btnOpenImage_Click(sender As Object, e As EventArgs) Handles btnOpenImage.Click

        Dim dlg As New OpenFileDialog
        Dim input = "qsl_original.jpg"
        Dim output = "qsl_corrected.jpg"

        dlg.Filter = "画像ファイル|*.jpg;*.jpeg;*.png;*.bmp"

        If dlg.ShowDialog = DialogResult.OK Then
            currentImagePath = dlg.FileName
            PicQsl.Image = Image.FromFile(dlg.FileName)
            LstOcrResult.Items.Clear()
            cmbCallsign.Text = ""
            cmbCallsign.Items.Clear()
            txtDate.Clear()
            txtTime.Clear()
            txtBand.Clear()
            txtMode.Clear()

            InputFileName = dlg.FileName

            SetImage(PicQsl.Image)
        End If
    End Sub

    Private Sub SetImage(img As Image)
        PicQsl.Image = img

        ' PictureBox に画像があるかどうかでボタンの状態を更新
        Dim hasImage As Boolean = (img IsNot Nothing)

        btnOfflineOcr.Enabled = hasImage
        btnOnlineOcr.Enabled = hasImage
        btnSave.Enabled = hasImage
    End Sub

    Private Sub btnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click
        ''   
        'Dim qsoDate = RegexExtractor.ExtractDate("2015/05/10")
        'txtDate.Text = qsoDate
    End Sub



    Private Sub btnDeskew_Click(sender As Object, e As EventArgs) Handles btnDeskew.Click, btnDeskew.Click
        If currentImagePath = "" Then
            MessageBox.Show("先に画像を開いてください")
            Exit Sub
        End If

        Dim outputPath = IO.Path.Combine(
        IO.Path.GetDirectoryName(currentImagePath),
        "deskewed_" & IO.Path.GetFileName(currentImagePath)
    )

        If ImagePreprocessor.Deskew(currentImagePath, outputPath) Then
            PicQsl.Image = Image.FromFile(outputPath)
            currentImagePath = outputPath   ' 補正後の画像を現在の画像として扱う
            MessageBox.Show("傾き補正が完了しました")
        End If
    End Sub

    Public Class ImagePreprocessor

        ' 傾き補正を行う関数
        Public Shared Function Deskew(inputPath As String, outputPath As String) As Boolean
            Try
                ' 画像読み込み（グレースケール）
                Dim src As Mat = Cv2.ImRead(inputPath, ImreadModes.Grayscale)

                ' エッジ検出
                Dim edges As New Mat()
                Cv2.Canny(src, edges, 50, 200)

                ' Hough変換で直線検出
                Dim lines() As LineSegmentPolar = Cv2.HoughLines(edges, 1, Math.PI / 180, 150)

                If lines.Length = 0 Then
                    ' 傾きが検出できない場合はそのまま保存
                    Cv2.ImWrite(outputPath, src)
                    Return True
                End If

                ' 平均角度を求める
                Dim angle As Double = 0
                For Each line In lines
                    angle += line.Theta
                Next
                angle /= lines.Length

                ' ラジアン → 度に変換
                Dim angleDeg As Double = angle * 180 / Math.PI

                ' 画像の中心
                Dim center As New Point2f(src.Width / 2, src.Height / 2)

                ' 回転行列
                Dim rotMat As Mat = Cv2.GetRotationMatrix2D(center, angleDeg - 90, 1)

                ' 出力画像
                Dim dst As New Mat()
                Cv2.WarpAffine(src, dst, rotMat, src.Size())

                ' 保存
                Cv2.ImWrite(outputPath, dst)

                Return True

            Catch ex As Exception
                MessageBox.Show("傾き補正中にエラー:    " & ex.Message)
                Return False
            End Try
        End Function

    End Class



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

    End Class

    Private ocr As New OcrEngine()

    Private Sub btnOfflineOcr_Click(sender As Object, e As EventArgs) Handles btnOfflineOcr.Click

        If currentImagePath = "" Then
            MessageBox.Show("先に画像を開いてください")
            Exit Sub
        End If

        Dim text = ocr.RecognizeText(currentImagePath)

        If text = "" Then
            MessageBox.Show("OCR結果が空です")
        Else
            ' とりあえずメッセージで全文を確認5
            Dim preview = If(text.Length > 500, text.Substring(0, 500) & " ...", text)
            Dim s = preview.Split(vbCr)

            LstOcrResult.Items.Clear()
            LstOcrResult.Items.AddRange(text.Split(New String() {vbLf}, StringSplitOptions.RemoveEmptyEntries))
            Debug.Print(text)
            '           MessageBox.Show(preview, "OCR結果（先頭のみ）")
        End If

        text = ocr.RecognizeText(currentImagePath)

        ' Callsign項目を抽出
        Dim candidates = RegexExtractor.ExtractCallsignCandidates(text)

        ' 自分のコールサインを除外
        '      candidates = RegexExtractor.RemoveMyCallsign(candidates)

        ' 最も正しいものを選ぶ
        Dim callsign = RegexExtractor.ChooseBestCallsign(candidates)

        For Each cs In candidates
            'Dim fixedCs = FixCallsign(m.Value)
            'List.Add(fixedCs)
            cmbCallsign.Items.Add(cs)
        Next


        Dim qsoDate = RegexExtractor.ExtractDate(text)
        Dim qsoTime = RegexExtractor.ExtractTime(text)
        Dim mode = RegexExtractor.ExtractMode(text)
        Dim freq = RegexExtractor.ExtractBand(text)

        ' UI に反映
        cmbCallsign.Text = callsign
        txtDate.Text = qsoDate
        txtTime.Text = qsoTime
        txtMode.Text = mode
        txtBand.Text = freq
    End Sub


    '**********************************************************************************************
    '
    '   OCR結果から項目を注出する
    '
    '**********************************************************************************************

    Public Class RegexExtractor
        Public Shared Function ExtractCallsignCandidates(text As String) As List(Of String)
            Dim list As New List(Of String)

            ' OCR誤認識補正前のテキストを大文字化
            Dim upper = text.ToUpper()

            ' ゆるめのコールサインパターン
            '           Dim pattern As String = "[A-Z0-9/]{3,10}"
            Dim pattern As String = "^[1-9]?[A-Z]{1,5}[0-9]{1,5}[A-Z0-9]{1,8}$"

            For Each m As Match In Regex.Matches(upper, pattern)
                Dim fixedCs = FixCallsign(m.Value)
                If fixedCs <> "" Then
                    list.Add(fixedCs)
                End If
            Next

            Return list
        End Function




        Public Shared Function RemoveMyCallsign(MyCallsign As String, candidates As List(Of String)) As List(Of String)
            Dim result As New List(Of String)

            For Each cs In candidates
                If cs <> MyCallsign Then
                    result.Add(cs)
                End If
            Next

            Return result
        End Function

        Public Shared Function ChooseBestCallsign(candidates As List(Of String)) As String
            For Each cs In candidates
                If Regex.IsMatch(cs, "^[1-9]?[A-Z]{1,3}\d[A-Z0-9/]{1,6}$") Then
                    Return cs
                End If
            Next

            ' それでも決まらない場合は最初の候補
            If candidates.Count > 0 Then
                Return candidates(0)
            End If

            Return ""
        End Function

        ' Callsign 抽出
        Public Shared Function ExtractCallsign(text As String) As String
            Dim pattern As String = "[A-Z0-9/]{3,10}"
            Dim m = Regex.Match(text.ToUpper(), pattern)
            If m.Success Then
                Return FixCallsign(m.Value)
            End If

            Return ""
        End Function

        ' Callsign の誤認識補正
        Public Shared Function FixCallsign(raw As String) As String
            Dim s As String = raw.ToUpper()

            ' よくある誤認識の補正
            s = s.Replace("O", "0")   ' O → 0
            s = s.Replace("D", "0")   ' D → 0
            s = s.Replace("I", "1")   ' I → 1
            s = s.Replace("L", "1")   ' L → 1
            s = s.Replace("S", "5")   ' S → 5
            s = s.Replace("B", "8")   ' B → 8

            Return s
        End Function

        ' 月名 → 数字 の辞書
        Private Shared ReadOnly MonthNames As New Dictionary(Of String, Integer) From {
                {"JANUARY", 1}, {"JAN", 1},
                {"FEBRUARY", 2}, {"FEB", 2},
                {"MARCH", 3}, {"MAR", 3},
                {"APRIL", 4}, {"APR", 4},
                {"MAY", 5},
                {"JUNE", 6}, {"JUN", 6},
                {"JULY", 7}, {"JUL", 7},
                {"AUGUST", 8}, {"AUG", 8},
                {"SEPTEMBER", 9}, {"SEP", 9},
                {"OCTOBER", 10}, {"OCT", 10},
                {"NOVEMBER", 11}, {"NOV", 11},
                {"DECEMBER", 12}, {"DEC", 12}
            }

        ' 日付抽出のメイン関数

        Public Shared Function ExtractDate(text As String) As String
            Dim cleaned = CleanText(text)

            ' 1. 月名を含む形式を探す
            Dim dateFromMonth = ExtractDateWithMonthName(text)
            If dateFromMonth <> "" Then Return dateFromMonth

            ' 2. 数字だけの形式を探す
            Dim dateFromNumbers = ExtractDateNumeric(cleaned)
            If dateFromNumbers <> "" Then Return dateFromNumbers

            Return ""
        End Function

        ' OCR誤認識の補正
        Private Shared Function CleanText(text As String) As String
            Dim s = text.ToUpper()
            s = s.Replace("O", "0")
            s = s.Replace("I", "1")
            s = s.Replace(",", "/")
            s = s.Replace(".", "/")
            Return s
        End Function

        ' 月名を含む日付
        Private Shared Function ExtractDateWithMonthName(text As String) As String
            text = text.ToUpper
            For Each key In MonthNames.Keys
                If text.Contains(key) Then
                    Dim pattern = key & "\s+(\d{1,2}),?\s+(\d{2,4})"
                    Dim m = Regex.Match(text, pattern)

                    If m.Success Then
                        Dim month = MonthNames(key)
                        Dim day = Integer.Parse(m.Groups(1).Value)
                        Dim year = FixYear(m.Groups(2).Value)

                        Return $"{year:0000}/{month:00}/{day:00}"
                    End If
                End If
            Next

            Return ""
        End Function

        ' 数字だけの日付
        Private Shared Function ExtractDateNumeric(text As String) As String
            Dim pattern = "(\d{2,4})[\/\-,\.\s]+(\d{1,2})[\/\-,\.\s]+(\d{1,2})"


            Dim m = Regex.Match(text, pattern)

            If m.Success Then
                Dim a = m.Groups(1).Value
                Dim b = m.Groups(2).Value
                Dim c = m.Groups(3).Value

                Dim year As Integer
                Dim month As Integer
                Dim day As Integer

                ' yyyy/mm/dd
                If a.Length = 4 Then
                    year = Integer.Parse(a)
                    month = Integer.Parse(b)
                    day = Integer.Parse(c)
                ElseIf c.Length = 4 Then
                    year = Integer.Parse(c)
                    month = Integer.Parse(a)
                    day = Integer.Parse(b)
                Else
                    ' mm/dd/yy or yy/mm/dd の判定

                    ' 月は 1〜12 の範囲で判断
                    If Integer.Parse(b) <= 12 Then      ' 第二要素が１から12なら月
                        year = Integer.Parse(c)
                        month = Integer.Parse(b)
                        day = Integer.Parse(c)
                    Else
                        year = Integer.Parse(a)
                        month = Integer.Parse(b)
                        day = Integer.Parse(c)
                    End If

                    '                    year = yy
                End If
                year = FixYear(year)

                Return $"{year:0000}/{month:00}/{day:00}"
            End If

            Return ""
        End Function

        ' 年の補正（2桁 → 4桁）
        Private Shared Function FixYear(y As Integer) As Integer
            Dim yy = Integer.Parse(y)
            If yy >= 100 Then Return yy

            Dim currentYY = Integer.Parse(DateTime.Now.Year.ToString().Substring(2))

            If yy > currentYY Then
                Return 1900 + yy
            Else
                Return 2000 + yy
            End If
        End Function


        ' 時刻抽出のメイン関数
        Public Shared Function ExtractTime(text As String) As String
            Dim cleaned = CleanTimeText(text)

            ' 時刻パターン（例：9:3, 09:30, 21:5 など）
            Dim pattern = "(\d{1,2})[:\.](\d{1,2})"
            Dim m = Regex.Match(cleaned, pattern)

            If m.Success Then
                Dim hour = Integer.Parse(m.Groups(1).Value)
                Dim minute = Integer.Parse(m.Groups(2).Value)

                ' 時刻の妥当性チェック
                If hour < 0 Or hour > 23 Then Return ""
                If minute < 0 Or minute > 59 Then Return ""

                ' ゼロ埋めして返す
                Return $"{hour:00}:{minute:00}"
            End If

            Return ""
        End Function

        ' OCR誤認識の補正
        Private Shared Function CleanTimeText(text As String) As String
            Dim s = text.ToUpper()

            ' よくある誤認識の補正
            s = s.Replace("O", "0")
            s = s.Replace("I", "1")
            s = s.Replace("L", "1")
            s = s.Replace(";", ":")
            s = s.Replace(".", ":")
            s = s.Replace(",", ":")

            Return s
        End Function

        '' 代表的な通信モード 一覧
        'Private Shared ReadOnly ModeList As String() = {
        '    "CW", "SSB", "FM",
        '    "FT8", "FT4", "JT65",
        '    "RTTY", "PSK31", "SSTV",
        '    "AM"
        '}

        ' メイン関数
        Public Shared Function ExtractMode(text As String) As String
            Dim cleaned = CleanModeText(text)

            ' まず既知モードが含まれているかチェック
            For Each m In Modes
                If cleaned.Contains(" " & m & " ") Then
                    Return m
                End If
            Next

            ' 既知モードが見つからない場合は類似文字列を探す
            Dim candidate = FindSimilarMode(cleaned)
            Return candidate
        End Function

        ' OCR誤認識の補正
        Private Shared Function CleanModeText(text As String) As String
            Dim s = text.ToUpper()

            s = s.Replace("O", "0")
            s = s.Replace("I", "1")
            s = s.Replace("L", "1")
            s = s.Replace("B", "8")
            s = s.Replace("S", "5") ' SSB → 55B などの逆補正も後で対応

            Return s
        End Function

        ' 類似モードを探す（簡易的な距離判定）
        Private Shared Function FindSimilarMode(text As String) As String
            For Each m In Modes
                ' 文字数が近いものを優先
                If Math.Abs(text.Length - m.Length) <= 2 Then
                    If text.Contains(m.Substring(0, 1)) Then
                        Return m
                    End If
                End If
            Next

            Return ""
        End Function

        ' 周波数抽出（例：7.074）

        ' 周波数 → バンド変換のメイン関数

        ' メイン関数
        Public Shared Function ExtractBand(text As String) As String

            ' 1. まず Band 表記があるか探す
            Dim band = ExtractBandDirect(text)
            If band <> "" Then Return band

            ' 2. なければ周波数から Band を推定
            Dim freq = ExtractFrequency(text)
            If freq > 0 Then
                Return ConvertFreqToBand(freq)
            End If

            Return ""
        End Function

        ' Band 表記を直接抽出
        Private Shared Function ExtractBandDirect(text As String) As String
            Dim upper = text.ToUpper()

            For Each b In BandList.Bands
                If upper.Contains(b) Then
                    Return b
                End If
            Next

            Return ""
        End Function

        ' 周波数を抽出（例：145.04 → 145.04）
        Private Shared Function ExtractFrequency(text As String) As Double
            Dim cleaned = CleanText(text)

            Dim pattern = "(\d{1,3}\.\d{1,3})"
            Dim m = Regex.Match(cleaned, pattern)

            If m.Success Then
                Dim value As Double
                If Double.TryParse(m.Value, value) Then
                    Return value
                End If
            End If

            Return 0
        End Function

        ' OCR誤認識の補正
        Private Shared Function CleanBandText(text As String) As String
            Dim s = text.ToUpper()
            s = s.Replace("O", "0")
            s = s.Replace("I", "1")
            s = s.Replace("L", "1")
            s = s.Replace(",", ".")
            Return s
        End Function



    End Class

    'Imports System.Net.Http
    'Imports System.Net.Http.Headers
    'Imports Newtonsoft.Json.Linq

    'Public Async Function AzureOCR(imagePath As String, endpoint As String, apiKey As String) As Task(Of String)

    '    Dim url = endpoint & "/computervision/imageanalysis:analyze?api-version=2023-02-01-preview&features=read"

    '    Using client As New HttpClient()
    '        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey)

    '        Dim bytes = IO.File.ReadAllBytes(imagePath)
    '        Dim content = New ByteArrayContent(bytes)
    '        content.Headers.ContentType = New MediaTypeHeaderValue("application/octet-stream")

    '        Dim response = Await client.PostAsync(url, content)
    '        Dim json = Await response.Content.ReadAsStringAsync()

    '        Dim obj = JObject.Parse(json)
    '        Dim text As String = String.Join(vbCrLf,
    '        obj("readResult")("content").Select(Function(t) t.ToString()))

    '        Return text
    '    End Using

    'End Function
    ' 周波数 → バンド変換
    Private Shared Function ConvertFreqToBand(freq As Double) As String
        If freq >= 1.8 AndAlso freq <= 2.0 Then Return "160M"
        If freq >= 3.5 AndAlso freq <= 4.0 Then Return "80M"
        If freq >= 5.3 AndAlso freq <= 5.4 Then Return "60M"
        If freq >= 7.0 AndAlso freq <= 7.3 Then Return "40M"
        If freq >= 10.1 AndAlso freq <= 10.15 Then Return "30M"
        If freq >= 14.0 AndAlso freq <= 14.35 Then Return "20M"
        If freq >= 18.068 AndAlso freq <= 18.168 Then Return "17M"
        If freq >= 21.0 AndAlso freq <= 21.45 Then Return "15M"
        If freq >= 24.89 AndAlso freq <= 24.99 Then Return "12M"
        If freq >= 28.0 AndAlso freq <= 29.7 Then Return "10M"
        If freq >= 50.0 AndAlso freq <= 54.0 Then Return "6M"
        If freq >= 144.0 AndAlso freq <= 148.0 Then Return "2M"
        If freq >= 430.0 AndAlso freq <= 440.0 Then Return "70CM"
        If freq >= 1260.0 AndAlso freq <= 1300.0 Then Return "23CM"
        If freq >= 0.1357 AndAlso freq <= 0.1388 Then Return "2200M"
        If freq >= 0.472 AndAlso freq <= 0.479 Then Return "630M"
        If freq >= 2400.0 AndAlso freq <= 2450.0 Then Return "12CM"
        If freq >= 5650.0 AndAlso freq <= 5850 Then Return "5CM"
        If freq >= 10000.0 AndAlso freq <= 10500.0 Then Return "3CM"

        Return ""
    End Function

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadSettings()

        ' 例：自分のコールサインを表示
        'MyCallsign = MyCallsign
        'InputFolder = InputFolder
        'OutputFolder = OutputFolder


        For Each c As Control In Me.Controls
            If TypeOf c Is ComboBox Then
                AddHandler c.KeyPress, AddressOf ComboBox_KeyPress
            End If
        Next

        For Each c As Control In Me.Controls
            If TypeOf c Is TextBox OrElse TypeOf c Is ComboBox Then
                AddHandler c.Enter, AddressOf Control_Enter
                AddHandler c.Leave, AddressOf Control_Leave
                AddHandler c.KeyDown, AddressOf Control_KeyDown
            End If
        Next

        ' 例：コールサイン欄
        'SetPlaceholder(cmbCallsign, "コールサインを入力")

        ' 例：日付欄
        SetPlaceholder(txtDate, "YYYY/MM/DD")

        ' 例：時刻欄
        SetPlaceholder(txtTime, "HH:MM")
        SetPlaceholder(txtBand, "Band")
        SetPlaceholder(txtMode, "Mode")

        ' イベント割り当て（TextBox 全体に適用）
        For Each c As Control In Me.Controls
            If TypeOf c Is TextBox Then
                AddHandler c.Enter, AddressOf TextBox_Enter
                AddHandler c.Leave, AddressOf TextBox_Leave
            End If
        Next

        cmbCallsign.Items.Clear()
        cmbCallsign.Text = ""
        txtDate.Clear()
        txtTime.Clear()
        txtBand.Clear()
        txtMode.Clear()

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


    Private Sub Control_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True

            Dim current As Control = CType(sender, Control)
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


    'Private Function GetNextInputControl(current As Control) As Control
    '    Dim parent As Control = current.Parent
    '    Dim controls = parent.Controls.Cast(Of Control)().
    '               OrderBy(Function(c) c.TabIndex).ToList()

    '    Dim currentIndex = controls.IndexOf(current)

    '    ' 次方向
    '    For i As Integer = currentIndex + 1 To controls.Count - 1
    '        If TypeOf controls(i) Is TextBox OrElse TypeOf controls(i) Is ComboBox Then
    '            Return controls(i)
    '        End If
    '    Next

    '    ' 先頭に戻る
    '    For i As Integer = 0 To currentIndex - 1
    '        If TypeOf controls(i) Is TextBox OrElse TypeOf controls(i) Is ComboBox Then
    '            Return controls(i)
    '        End If
    '    Next

    '    Return Nothing
    'End Function

    Private Sub ComboBox_KeyPress(sender As Object, e As KeyPressEventArgs)
        ' 英字は大文字に変換
        If Char.IsLetter(e.KeyChar) Then
            e.KeyChar = Char.ToUpper(e.KeyChar)
        End If
    End Sub

    Private Sub SetComboPlaceholder(cb As ComboBox, placeholder As String)
        cb.Tag = placeholder
    End Sub

    Private Sub ComboBox_DrawItem(sender As Object, e As DrawItemEventArgs) Handles cmbCallsign.DrawItem
        Dim cb As ComboBox = CType(sender, ComboBox)

        e.DrawBackground()

        Dim text As String = ""
        Dim color As Color = Color.Black

        If e.Index < 0 Then
            ' --- プレースホルダー表示 ---
            If cb.Text = "" Then
                text = cb.Tag.ToString()
                color = Color.Gray
            Else
                text = cb.Text
                color = cb.ForeColor
            End If
        Else
            ' --- 通常のアイテム ---
            text = cb.Items(e.Index).ToString()
            color = cb.ForeColor
        End If

        Using b As New SolidBrush(color)
            e.Graphics.DrawString(text, cb.Font, b, e.Bounds)
        End Using

        e.DrawFocusRectangle()
    End Sub

    Private Sub ComboBox_Leave(sender As Object, e As EventArgs) Handles cmbCallsign.Leave
        Dim cb As ComboBox = CType(sender, ComboBox)

        If cb.Text.Trim() = "" Then
            cb.Text = ""
        End If
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

    Private Sub PicQsl_LoadCompleted(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs) Handles PicQsl.LoadCompleted

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        PicQsl.Image = Nothing
        SaveImageWithRules(InputFileName)
        SetImage(Nothing)
    End Sub

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
        If (prefix Like "7[JKLNM]" OrElse prefix Like "8[JKLNM]" OrElse
        prefix Like "J[A-S]") Then
            subFolder = prefix
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

            saveName = $"{cs}_{dt}_{tm}_{bd}_{md}.{ext}"

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
            saveName = $"{cs}_{nextSerial}.{ext}"
        End If

        Dim savePath As String = Path.Combine(saveFolder, saveName)

        '=== 画像保存 ===
        Try
            File.Copy(inputImagePath, savePath, overwrite:=False)
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

    Private Sub cmbCallsign_Leave(sender As Object, e As EventArgs) Handles cmbCallsign.Leave
        Dim pattern As String = "^[1-9]?[A-Za-z]{1,5}[0-9]{1,5}[A-Za-z0-9]{1,8}$"

        If cmbCallsign.Text <> "" Then
            If Not System.Text.RegularExpressions.Regex.IsMatch(cmbCallsign.Text, pattern) Then
                MessageBox.Show("コールサインの形式が正しくありません。", "入力エラー",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)

                cmbCallsign.Focus()
                cmbCallsign.SelectAll()
            End If
        End If
    End Sub


    Private Sub txtDate_Leave(sender As Object, e As EventArgs) Handles txtDate.Leave
        If txtDate.Text <> "" Then
            Dim input As String = txtDate.Text.Trim()

            ' 正規表現：YYYY/MM/DD または YY/MM/DD
            Dim pattern As String = "^(\d{2}|\d{4})/(0?[1-9]|1[0-2])/(0?[1-9]|[12]\d|3[01])$"
            If Not System.Text.RegularExpressions.Regex.IsMatch(input, pattern) Then
                MessageBox.Show("日付の形式が正しくありません（YYYY/MM/DD）。", "入力エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtDate.Focus()
                txtDate.SelectAll()
                Exit Sub
            End If

            ' --- ここから YY → YYYY の変換 ---
            Dim parts = input.Split("/"c)
            Dim year As Integer = Integer.Parse(parts(0))
            Dim month As Integer = Integer.Parse(parts(1))
            Dim day As Integer = Integer.Parse(parts(2))

            If year < 100 Then
                ' 00〜49 → 2000〜2049、50〜99 → 1950〜1999 とする
                If year <= 49 Then
                    year += 2000
                Else
                    year += 1900
                End If
            End If


            Dim dt As Date
            Try
                dt = New Date(year, month, day)
            Catch ex As Exception
                MessageBox.Show("存在しない日付です。", "入力エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtDate.Focus()
                txtDate.SelectAll()
                Exit Sub
            End Try

            ' --- 範囲チェック ---
            Dim minDate As Date = New Date(1952, 7, 29)
            Dim maxDate As Date = Date.Today

            If dt < minDate OrElse dt > maxDate Then
                MessageBox.Show("日付は 1952/07/29 〜 今日 の範囲で入力してください。", "入力エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtDate.Focus()
                txtDate.SelectAll()
                Exit Sub
            End If

            ' 正常 → YYYY/MM/DD に整形して戻す
            txtDate.Text = dt.ToString("yyyy/MM/dd")

        End If
    End Sub

    Private Sub txtTime_Leave(sender As Object, e As EventArgs) Handles txtTime.Leave
        If txtTime.Text <> "" Then

            Dim input As String = txtTime.Text.Trim()

            ' 数字3～4桁のみ許可
            Dim pattern As String = "^(\d{1,2}:\d{2}|\d{3,4})$"
            If Not System.Text.RegularExpressions.Regex.IsMatch(input, pattern) Then
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
            Dim hour As Integer = Integer.Parse(input.Substring(0, 2))
            Dim minute As Integer = Integer.Parse(input.Substring(2, 2))

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

    Private Sub txtBand_Leave(sender As Object, e As EventArgs) Handles txtBand.Leave
        If txtBand.Text <> "" Then

            Dim input As String = txtBand.Text.Trim().ToUpper()

            ' 一致チェック
            If Not BandList.Bands.Contains(input) Then
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


    Private Sub txtMode_Leave(sender As Object, e As EventArgs) Handles txtMode.Leave
        If txtMode.Text <> "" Then

            Dim input As String = txtMode.Text.Trim().ToUpper()

            input = Replace(input, "DSTAR:", "D-STAR")

            If Not ModeList.Modes.Contains(input) Then
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
    'Private Sub TextBox_KeyDown(sender As Object, e As KeyEventArgs)
    '    If e.KeyCode = Keys.Enter Then
    '        e.SuppressKeyPress = True

    '        Dim tb As TextBox = DirectCast(sender, TextBox)

    '        If e.Shift Then
    '            MoveToNextTextBox(tb, forward:=False)
    '        Else
    '            MoveToNextTextBox(tb, forward:=True)
    '        End If
    '    End If
    'End Sub

    'Private Sub CombBox_KeyDown(sender As Object, e As KeyEventArgs)
    '    If e.KeyCode = Keys.Enter Then
    '        e.SuppressKeyPress = True

    '        Dim tb As ComboBox = DirectCast(sender, ComboBox)

    '        If e.Shift Then
    '            MoveToNextTextBox(tb, forward:=False)
    '        Else
    '            MoveToNextTextBox(tb, forward:=True)
    '        End If
    '    End If
    'End Sub

    'Private Sub FocusNextTextBox(current As Control, forward As Boolean)
    '    Dim parent As Control = current.Parent
    '    Dim controls = parent.Controls

    '    Dim startIndex As Integer = controls.IndexOf(current)
    '    Dim count As Integer = controls.Count

    '    Dim i As Integer = startIndex

    '    Do
    '        If forward Then
    '            i = (i + 1) Mod count
    '        Else
    '            i = (i - 1 + count) Mod count
    '        End If

    '        Dim c As Control = controls(i)

    '        ' TextBox だけにフォーカスを移動
    '        If (TypeOf c Is TextBox) Or (TypeOf c Is ComboBox) Then
    '            c.Focus()
    '            Exit Sub
    '        End If

    '    Loop While i <> startIndex
    'End Sub

    'Private Sub MoveToNextTextBox(current As Control, forward As Boolean)
    '    Dim allTextBoxes = GetAllTextBoxes(Me)

    '    ' TabIndex 順に並べ替え
    '    Dim ordered = allTextBoxes.OrderBy(Function(c) c.TabIndex).ToList()

    '    Dim index = ordered.IndexOf(current)
    '    If index = -1 Then Exit Sub

    '    Dim nextIndex As Integer

    '    If forward Then
    '        nextIndex = (index + 1) Mod ordered.Count
    '    Else
    '        nextIndex = (index - 1 + ordered.Count) Mod ordered.Count
    '    End If

    '    ordered(nextIndex).Focus()
    'End Sub

    'Private Function GetAllTextBoxes(parent As Control) As List(Of TextBox)
    '    Dim list As New List(Of TextBox)

    '    For Each ctrl As Control In parent.Controls
    '        If (TypeOf ctrl Is TextBox) OrElse (TypeOf ctrl Is TextBox) Then
    '            list.Add(DirectCast(ctrl, TextBox))
    '        End If

    '        If ctrl.HasChildren Then
    '            list.AddRange(GetAllTextBoxes(ctrl))
    '        End If
    '    Next

    '    Return list
    'End Function

    Private Sub SetPlaceholder(tb As TextBox, placeholder As String)
        tb.Tag = placeholder
        tb.ForeColor = Color.Gray
        tb.Text = placeholder
    End Sub

    Private Sub TextBox_Enter(sender As Object, e As EventArgs)
        Dim tb As TextBox = CType(sender, TextBox)

        If tb.ForeColor = Color.Gray Then
            tb.Text = ""
            tb.ForeColor = Color.Black
        End If
    End Sub

    Private Sub TextBox_Leave(sender As Object, e As EventArgs)
        Dim tb As TextBox = CType(sender, TextBox)

        If tb.Text.Trim() = "" Then
            tb.ForeColor = Color.Gray
            tb.Text = tb.Tag.ToString()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form2Open()
    End Sub

    Private Sub SettingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SettingToolStripMenuItem.Click
        Form2Open()

    End Sub

    Private Sub Form2Open()
        Dim form As New Form2

        MyCallsign = MyCallsign
        InputFolder = InputFolder
        OutputFolder = OutputFolder

        If form.ShowDialog() = DialogResult.OK Then
            MyCallsign = MyCallsign
            InputFolder = InputFolder
            OutputFolder = OutputFolder
            SaveSettings()
        End If

    End Sub
End Class

Public Class AppConfig
    Public Property MyCallsign As String
    Public Property InputFolder As String
    Public Property OutputFolder As String
End Class

Module AppSetting
    Private ReadOnly SettingsFile As String =
        Path.Combine(Application.StartupPath, "settings.json")

    'Public AppConfig As New AppConfig With {
    '    .MyCallsign = "",
    '    .InputFolder = "",
    '    .OutputFolder = ""
    '}

    ' 設定を読み込む
    Public Sub LoadSettings()
        Dim AppConfig As AppConfig = New AppConfig()

        If File.Exists(SettingsFile) Then
            Dim json = File.ReadAllText(SettingsFile)
            AppConfig = JsonSerializer.Deserialize(Of AppConfig)(json)
            '            AppConfig = JsonSerializer.Deserialize(Of AppSettings)(json)

            Form1.MyCallsign = AppConfig.MyCallsign
            Form1.InputFolder = AppConfig.InputFolder
            Form1.OutputFolder = AppConfig.OutputFolder
        End If
    End Sub

    ' 設定を保存する
    Public Sub SaveSettings()
        Dim AppConfig As AppConfig = New AppConfig()
        AppConfig.MyCallsign = Form1.MyCallsign
        AppConfig.InputFolder = Form1.InputFolder
        AppConfig.OutputFolder = Form1.OutputFolder


        Dim json = JsonSerializer.Serialize(AppConfig, New JsonSerializerOptions With {.WriteIndented = True})
        File.WriteAllText(SettingsFile, json)
    End Sub

End Module
