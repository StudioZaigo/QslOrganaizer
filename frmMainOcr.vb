Imports System.CodeDom.Compiler
Imports System.Configuration
Imports System.Diagnostics.Eventing.Reader
Imports System.Diagnostics.Metrics
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Runtime.Intrinsics.Arm
Imports System.Runtime.Intrinsics.X86
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox
Imports Windows.Win32.System

Public Module callsignList
    Public ReadOnly ExcludedCallsigns As String() = {
    "HB9CV", "JAPAN"
    }
End Module

Public Module BandList
    Public ReadOnly Bands As String() = {                   ' Band順位並べる必要あり
        "2200M", "630M",
        "160M", "80M", "60M", "40M", "30M", "20M",
        "17M", "15M", "12M", "10M", "6M",
        "2M", "70CM", "23CM",
        "13CM", "5CM", "2.9CM", "2.8CM", "SAT"                     ' 稀なので後ろにした
    }

    Public ReadOnly FreqTable As (Low As Double, High As Double, Name As String)() = {
    (1.8, 1.999, "160M"),
    (3.5, 3.999, "80M"),
    (5.3, 5.4, "60M"),
    (7.0, 7.3, "40M"),
    (10.1, 10.15, "30M"),
    (10.0, 10.0, "30M"),        ' 特例
    (14.0, 14.35, "20M"),
    (18.068, 18.168, "17M"),
    (18.0, 18.0, "17M"),        ' 特例
    (21.0, 21.45, "15M"),
    (24.89, 24.99, "12M"),
    (24.0, 24.0, "12M"),        ' 特例
    (28.0, 29.7, "10M"),
    (29.0, 29.0, "10M"),        ' 特例
    (50.0, 54.0, "6M"),
    (52.0, 52.0, "6M"),         ' 特例
    (144.0, 146.0, "2M"),
    (145.0, 145.0, "2M"),       ' 特例
    (430.0, 440.0, "70CM"),
    (435.0, 435.0, "70CM"),     ' 特例
    (1200.0, 1200.0, "23CM"),   ' 特例
    (1260.0, 1300.0, "23CM"),
    (0.1357, 0.1388, "2200M"),
    (0.472, 0.479, "630M"),
    (2400.0, 2450.0, "12CM"),
    (5650.0, 5850.0, "5CM"),
    (10000.0, 10250.0, "3CM"),
    (10450.0, 10500.0, "3CM")
}

    Public ReadOnly MisReadingBands As New Dictionary(Of String, String)() From {
       {"19", "160M"}, {"35", "80M"}
   }
End Module

Module ModeList
    Public ReadOnly Modes As String() = {
        "SSB", "FT8", "CW", "FM", "AM",
        "RTTY", "FT4", "PSK31",
        "JT65", "JT9", "SSTV", "ATV",
        "AMTOR", "PACKET", "FSK", "MFSK",
        "C4FM", "D-STAR", "F2A", "PHONE", "DMR"
    }

    ' 例："DSTAR"は、正式名"D-STAR"に置き換えられる（ADIFでの"DSTAR"は誤り、JARLは放置している）
    Public ReadOnly AliasModes As New Dictionary(Of String, String)() From {
        {"J3E", "SSB"}, {"LSB", "SSB"}, {"USB", "SSB"}, {"FT-8", "FT8"}, {"A1", "CW"}, {"A1A", "CW"}, {"F3", "FM"},
        {"A3", "AM"}, {"FONE", "PHONE"}, {"JT-65", "JT65"},
        {"DSTAR", "D-STAR"}
        }

    Public ReadOnly MisReadingModes As New Dictionary(Of String, String)() From {
        {"FTS", "FT8"}, {"FTB", "FT8"}, {"FTJ", "FT8"}, {"FT&", "FT8"}, {"FTZ", "FT8"}, {"FTE", "FT8"},
        {"FIS", "FT8"}, {"FIB", "FT8"}, {"FIJ", "FT8"}, {"FI&", "FT8"}, {"FIZ", "FT8"}, {"FI8", "FT8"},
        {"F1S", "FT8"}, {"F1B", "FT8"}, {"F1J", "FT8"}, {"F1&", "FT8"}, {"F1Z", "FT8"},
        {"F7S", "FT8"}, {"F7B", "FT8"}, {"F78", "FT8"}, {"F7&", "FT8"}, {"F7T", "FT8"},
        {"E18", "FT8"}, {"FES", "FT8"}, {"PTS", "FT8"},
        {"33B", "SSB"}, {"SSE", "SSB"}, {"SS8", "SSB"}, {"\$SB", "SSB"}, {"$$B", "SSB"}, {"S88", "SSB"},
        {"SSA", "SSB"},
        {"F4M", "FM"}, {"F3M", "FM"}, {"F3N", "FM"},
        {"A3M", "AM"}, {"A3N", "AM"},
        {"RTTYS", "RTTY"}, {"RTTY8", "RTTY"},
        {"PSK3I", "PSK31"}, {"PSK31A", "PSK31"},
        {"JT65A", "JT65"}, {"JT65B", "JT65"}, {"JT65C", "JT65"},
        {"SSTV8", "SSTV"}, {"SSTV9", "SSTV"},
        {"OW", "CW"}, {"J165", "JT65"}
    }

    '{"CWS", "CW"}, {"CWB", "CW"}, {"CW8", "CW"}, {"C1W", "CW"},
End Module

Module CalendarModule                ' 月名 → 数字 の辞書
    Public ReadOnly MonthNames As New Dictionary(Of String, Integer) From {
                {"JANUARY", 1}, {"FEBRUARY", 2}, {"MARCH", 3}, {"APRIL", 4}, {"MAY", 5}, {"JUNE", 6}, {"JULY", 7},
                {"AUGUST", 8}, {"SEPTEMBER", 9}, {"OCTOBER", 10}, {"NOVEMBER", 11}, {"DECEMBER", 12},
                {"JAN", 1}, {"FEB", 2}, {"MAR", 3}, {"APR", 4}, {"JUN", 6},
                {"JUL", 7}, {"AUG", 8}, {"SEP", 9}, {"OCT", 10}, {"NOV", 11}, {"DEC", 12}
            }

    Public ReadOnly MisReadingMonth As New Dictionary(Of String, String)() From {                                   ' 置き換え前と、置き換え後は桁数を合わせる必要あり
        {"3AN", "JAN"}, {"WAR", "MAR"}, {"APE", "APR"}, {"/UL", "JUL"}, {"1UL", "JUL"}, {"SUL", "JUL"}, {"HUG", "AUG"},
        {"SEPT", "SEP "}, {"0CT", "OCT"}, {"N0V", "NOV"}, {"APR1L", "APRIL"}
    }

    Public ReadOnly AdifItemNames As String() = {
        "FROM", "DATE", "TIME", "BAND", "MODE", "TO"
    }

    Public ReadOnly QrCodePattern As String() = {
        "OPERATOR", "QSO_DATE", "TIME_ON", "BAND", "MODE"
    }

End Module


Partial Class frmMain

    Private Shared UsedRanges As New List(Of (value As String, Start As Integer, Length As Integer))

    Public Structure Detection
        Public Value As String
        Public Index As Integer
        Public Length As Integer
        Public Row As Integer
        Public Column As Integer
    End Structure

    Private Shared DateBase As Detection                        ' 検出されたDateの値、位置
    Private Shared DateBaseTitle As Detection                   ' Dateを検出したTitleをもとに
    Private Shared DetectionDates As New List(Of Detection)     ' Dateに一致するものが2個以上あったとき

    Private Shared TimeBase As Detection
    Private Shared TimeBaseTitle As Detection                   ' Timeに一致するものが2個以上あったとき
    Private Shared DetectionTimes As New List(Of Detection)

    Private Shared ModeBase As Detection
    Private Shared ModeBaseTitle As Detection                   ' Modeを検出したTitleをもとに
    Private Shared DetectionModes As New List(Of Detection)     ' Modeに一致するものが2個以上あったとき

    Private Shared BandBase As Detection
    Private Shared BandBaseTitle As Detection                   ' Bandを検出したTitleをもとに
    Private Shared DetectionBands As New List(Of Detection)     ' Band、FREQに一致するものが2個以上あったとき

    Public Enum DateFormats As Integer
        InvalidFormat       ' 無効な形式
        IsoFormat           ' 2014年8月22日
        BritishFormat       ' 22/8/2014
        AmericanFormat      ' 8/22/2014
        MilitaryFormat      ' 22 Aug 2014 順としては日 月 年の順だが、月名を英語で表す形式もあるので、月を先に探す
    End Enum

    Private Shared DateFormat As DateFormats

    Public Enum BandFormats As Integer
        Unknown       ' 不明
        MHz           ' 周波数(MHz)
        kHz           ' 周波数(kHz)
        WaveLength    ' 波長
    End Enum


    Public Shared isPhone As Boolean           ' ReportからModeを類推する、Phoneの時とき　Bandが２M以上の時、FMにModeを置き換える

    '**********************************************************************************************
    '
    '   OCR結果から項目を注出する
    '
    '**********************************************************************************************

    Public Class RegexExtractor

        Public Shared Function ExtractFromQrCode(text As String) As List(Of String)
            Dim results As New List(Of String)
            Dim items() As String
            Dim dt As String

            ' HAMLOGのQRコードの形式 hamlogでは交信時刻がUTCだった場合は、JSTに変換され記録します。
            ' Exsampl
            ' !!JH3OGT_250627_1751_50.3143_FT8_JA7FKF
            ' !!JG1MOU_230205_1532_10.136_FT8_JI1ILB 
            If text.Substring(0, 2) = "!!" Then
                items = text.Substring(2).Split("_")
                results.Add(items(0))                                                   ' Callsign
                dt = items(1)
                dt = ConverTo4degitsYear(dt.Substring(0, 2)) & "/" & dt.Substring(2, 2) & "/" & dt.Substring(4, 2)
                results.Add(dt)                                                         ' date
                results.Add(items(2).Substring(0, 2) & ":" & items(2).Substring(2, 2))  ' time
                results.Add(RegexExtractor.ConvertFreqToBand(items(3)))                 ' Band 周波数なので波長に変換
                results.Add(items(4))                                                   ' Mode
                results.Add(items(5))                                                   ' To Callsign
                Return results
            End If

            ' DARCのQRコードの形式
            ' example
            ' From:DL6UCK To: JA7FKF
            ' Date: 23.12.24 Time: 08:38 Band: 17m Mode: FT8 RST:  -04 QSL: PSE

            Dim pattern As String
            Dim i As String

            results.Clear()
            For Each a In AdifItemNames
                pattern = "\b" & a & "[:;] ([A-Z0-9.:]+)\b"         ' :があり、そのあと空白、そのあとデータ
                Dim m As Match = Regex.Match(text, pattern, RegexOptions.IgnoreCase)
                If m.Success Then
                    If a = "DATE" Then
                        i = m.Groups(1).Value.Replace(".", "/")
                        If i.Length = 8 Then
                            i = ConverTo4degitsYear(i.Substring(6, 2)) & i.Substring(2, 4) & i.Substring(0, 2)      ' BritishuFormat から IsoFprmt変換
                        ElseIf i.Length = 10 Then
                            i = i.Substring(6, 4) & i.Substring(2, 4) & i.Substring(0, 2)      ' BritishuFormat から IsoFprmt変換
                        End If
                    Else
                        i = m.Groups(1).Value
                    End If
                    results.Add(i)
                End If
            Next
            If results.Count = 5 Then
                Return results
            End If

            ' ADIFのQRコードの形式
            ' <CALL:6>IW1QLH<QSO_DATE:8:D>20000101<TIME_ON:4>0000<BAND:3>20M<MODE:3>USB<EOR>" 73 de Claudio - IW1QLH
            results.Clear()
            Dim r = (RegexExtractor.ExtractFromADIF("CALL", text))    ' Callsign
            If r <> "" Then
                results.Add(r)

                r = RegexExtractor.ExtractFromADIF("QSO_DATE", text)     ' Date
                If r.Length = 8 OrElse r.Length = 6 Then
                    dt = ConverTo4degitsYear(r.Substring(0, 2)) & "/" & r.Substring(2, 2) & "/" & r.Substring(4, 2)
                    If r <> "" Then results.Add(dt)
                End If

                If r.Length = 4 Then
                    r = RegexExtractor.ExtractFromADIF("TIME_ON", text)      ' Time
                    Dim tm = r.Substring(0, 2) & ":" & r.Substring(2, 2)
                    If r <> "" Then results.Add(tm)
                End If

                r = RegexExtractor.ExtractFromADIF("BAND", text)         ' Band
                If r <> "" Then results.Add(r)

                r = RegexExtractor.ExtractFromADIF("MODE", text)         ' Mode
                If Not Modes.Contains(r) Then
                    If AliasModes.ContainsKey(r) Then r = AliasModes(r)
                End If
                If r <> "" Then results.Add(r)

                If results.Count = 5 Then
                    Return results
                End If
            End If


            ' example
            ' Operator;QSO_DATE;TIME_ON;BAND;MODE;RST_SENT;QSL_RCVD;
            ' DL2RMM;10.06.18;06:01;17M;FT8;-10;PSE;

            results.Clear()
            items = text.Replace(vbLf, "").Split(";")
            Dim c = (items.Count \ 2)
            For j = 0 To c - 1
                If j > QrCodePattern.Count - 1 Then Exit For
                If items(j) = QrCodePattern(j) Then
                    Dim a = QrCodePattern(j)
                    i = items(j + c)
                    If a = "QSO_DATE" Then
                        i = i.Replace(".", "/")
                        If i.Length = 8 Then
                            i = ConverTo4degitsYear(i.Substring(6, 2)) & i.Substring(2, 4) & i.Substring(0, 2)      ' BritishuFormat から IsoFprmt変換
                        ElseIf i.Length = 10 Then
                            i = i.Substring(6, 4) & i.Substring(2, 4) & i.Substring(0, 2)      ' BritishuFormat から IsoFprmt変換
                        End If
                    End If
                    results.Add(i)
                End If
            Next
            If results.Count = 5 Then
                Return results
            End If

            ' HRD Labelの型式  仕様が不明、HRDのラベル印刷のQRコードの形式は、HRDのバージョンによって変わる可能性がある
            ' Example
            ' 00000:60JA7FKF000000000:8:D02025021900000000 : 40075100000:3012000000:30FT800000000000000000:50DK6AH0EOR0

            items = text.Split(":")
            If items(0) = "00000" Then

                Dim lng = items(7).Substring(0, 1)          ' Calllsign
                Dim val = items(7).Substring(2, lng)
                results.Add(val)

                lng = items(2).Substring(0, 1)              ' DATE
                val = items(3).Substring(2, lng)
                val = val.Substring(0, 4) & "/" & val.Substring(4, 2) & "/" & val.Substring(6, 2)
                results.Add(val)

                lng = items(4).Substring(0, 1)              ' TIME
                val = items(4).Substring(2, lng)
                val = val.Substring(0, 2) & ":" & val.Substring(2, 2)
                results.Add(val)

                lng = items(5).Substring(0, 1)              ' BAND
                val = items(5).Substring(2, lng - 1) & "M"
                results.Add(val)

                lng = items(6).Substring(0, 1)              ' MODE
                val = items(6).Substring(2, lng)
                results.Add(val)

                If results.Count = 5 Then
                    Return results
                End If
            End If

            Return results
        End Function

        Public Shared Function ExtractFromADIF(ItemName As String, adifText As String) As String
            ' <CALL:6>IW1QLH<QSO_DATE:8:D>20000101<TIME_ON:4>0000<BAND:3>20M<MODE:3>USB<EOR>" 73 de Claudio - IW1QLH
            ' <([A-Za-z0-9_]+):(\d+)(?::[A-Za-z])?  

            Dim pattern = "<" & ItemName & ":(\d+)>(?::[A-Z0-9])?"
            Dim m = Regex.Match(adifText, pattern, RegexOptions.IgnoreCase)
            If m.Success Then

                Return adifText.Substring(m.Index + m.Length, m.Groups(1).Value)

            End If
            Return ""
        End Function

        Public Shared Function ExtractCallsignCandidates(text As String) As List(Of String)
            ' callsignの候補を抽出する処理

            Dim list As New List(Of String)
            Dim pattern As String

            ' 厳しめで検索、第１候補とする
            pattern = "\b([A-Z]{1,2}|[1-9][A-Z]|[A-Z]\d|3DA)(\d+)([A-Z][A-Z0-9]{0,8})\b"    ' prefix:　英字1文字または2文字(K,JA)　数字1文字＋英字1文字(1S)　英字1文字＋数字1文字(E5)　3DA　　　
            '                                                                               ' Area:　数字1文字　　　　sffix:英字1文字＋英数字0～8文字

            For Each m As Match In Regex.Matches(text, pattern)
                Dim part1 = m.Groups(1).Value
                Dim part2 = m.Groups(2).Value
                Dim part3 = m.Groups(3).Value

                Dim fixedCs = part1 & part2 & part3
                If fixedCs <> "" Then
                    If Not isExcludedCallsigns(fixedCs) Then
                        If Not isGridLoc(fixedCs) Then
                            If list.Contains(fixedCs) = False Then
                                list.Add(fixedCs)
                            End If
                        End If
                    End If
                End If
            Next

            pattern = "\b(J[A-S]|7[K-N])([A-Z0-9])([A-Z0-9]{2,3})\b"        ' 国内のCallsign（Area,Suffixの誤読も補正）
            For Each m As Match In Regex.Matches(text, pattern)
                Dim part1 = m.Groups(1).Value
                Dim part2 = m.Groups(2).Value
                Dim part3 = m.Groups(3).Value

                part2 = ReplaceAtoN(part2)          ' Area codeの誤認識補正
                part3 = ReplaceNtoA(part3)          ' Suffixの誤認識補正

                Dim fixedCs = part1 & part2 & part3
                If fixedCs <> "" Then
                    If Not isExcludedCallsigns(fixedCs) Then
                        If list.Contains(fixedCs) = False Then
                            list.Add(fixedCs)
                        End If
                    End If
                End If
            Next

            ' ゆるめのコールサインパターン　1文字目：英数字、　2文字目：数字(0,1桁),  その後：英字(1～5桁)　数字(1～5桁)　。。。。
            pattern = "(?<![A-Z1-9])[0-9]?[A-Z]{1,5}[0-9]{1,5}[A-Z0-9]{1,8}(?![A-Z0-9])"
            '   pattern = "[1-9]?[A-Z]{1,5}[0-9]{1,5}[A-Z0-9]{1,8}\s"
            For Each m As Match In Regex.Matches(text, pattern, RegexOptions.Multiline)
                Dim fixedCs = m.Value
                If fixedCs <> "" Then
                    If Not isExcludedCallsigns(fixedCs) Then
                        If Not isGridLoc(fixedCs) Then                          ' GridLocatorは除外する
                            If list.Contains(fixedCs) = False Then
                                list.Add(fixedCs)
                            End If
                        End If
                    End If
                End If
            Next

            ' hQSL対応 JA局のみ（Fromがあったらその後ろはCallsign）
            'pattern = "(^|\s)FR[O|0]M[:;]?([A-Z0-9]{5,6})(\s|$)"

            pattern = "\bFR[O|0]M[:;]?([A-Z0-9]{5,6})\b"
            For Each m As Match In Regex.Matches(text, pattern, RegexOptions.Multiline)
                Dim fixedCs = m.Groups(1).Value
                Dim c1 = fixedCs.Substring(0, 2)
                Dim c2 = fixedCs.Substring(2, 1)
                Dim c3 = fixedCs.Substring(3, fixedCs.Length - 3)

                c1 = c1.Replace("T", "7")   ' O → 0
                c2 = ReplaceAtoN(c2)
                c3 = ReplaceNtoA(c3)
                Dim prefix = c1 & c2
                fixedCs = prefix & c3

                If (c1 Like "J[A-S]") OrElse (c1 Like "7[JKLNM]") OrElse (c1 Like "8[JKLNM]") Then
                    ' NOP
                Else
                    Continue For
                End If

                If fixedCs <> "" Then
                    If Not isExcludedCallsigns(fixedCs) Then
                        'If fixedCs Like "*[0-9]*" Then
                        If list.Contains(fixedCs) = False Then
                            list.Add(fixedCs)
                        End If
                        'End If
                    End If
                End If
            Next

            pattern = "\b(JA[0-9]-?[0-9]{3,6})\b"               ' JAのSWLのみ　海外のSWL番号の体系不明
            For Each m As Match In Regex.Matches(text, pattern, RegexOptions.Multiline)
                Dim fixedCs = m.Groups(1).Value

                If fixedCs <> "" Then
                    'If fixedCs Like "*[0-9]*" Then
                    If list.Contains(fixedCs) = False Then
                        list.Add(fixedCs)
                    End If
                End If
            Next

            Return list
        End Function

        Public Shared Function isGridLoc(Callsign) As Boolean
            ' Grid Locatorのパターン
            Dim pattern = "[A-R]{2}[0-9]{2}[A-X]{2}"
            If Regex.IsMatch(Callsign, pattern) Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function isExcludedCallsigns(Callsign) As Boolean
            For Each cs In ExcludedCallsigns
                If Callsign = cs Then Return true
            Next
            Return False
        End Function


        Public Shared Function RemoveMyCallsign(MyCallsigns As String, candidates As List(Of String)) As List(Of String)
            ' Callsignの候補から自局のコールサインを除外する処理
            Dim result As New List(Of String)

            Dim Callsigns As String() = MyCallsigns.Split(","c)
            For Each cs In candidates
                'If candidates.Contains(cs) = False Then
                '    result.Add(cs)
                'End If
                Dim r As Boolean = False
                For Each mcs In Callsigns
                    If cs = mcs Then
                        r = True
                        Exit For
                    End If
                Next
                If Not r Then
                    result.Add(cs)
                End If
            Next

            Return result
        End Function

        Public Shared Function ChooseBestCallsign(candidates As List(Of String), myCall As String) As String

            Dim callPattern As String = "^([A-Z]{1,2}|[1-9][A-Z]|[A-Z]\d|3DA)\d[A-Z]{1,8}$"
            Dim SwlPattern As String = "^(JA[0-9]-?[0-9]{3,6})$"
            Dim bestCall As String = ""
            Dim bestDistance As Integer = 9999
            Dim cs As String

            For Each c In candidates
                cs = c

                ' 典型的なコールサインパターンに合致するものだけ採用
                If Not Regex.IsMatch(cs, callPattern) Then
                    Continue For
                End If
                bestCall = cs
                Return bestCall
            Next

            '    'If Regex.IsMatch(cs, callPattern) Then
            '    Return cs
            '    'End If

            '    'Dim JApattern As String = "^(J[A-S]|7[K-N]|[8[J-N])"      ' JA局を優先
            '    'If Regex.IsMatch(cs, JApattern) Then
            '    '    Return cs
            '    'End If

            '    '' MyCallsign とどれだけ違うか（距離が大きいほど違う）
            '    'Dim d As Integer = Levenshtein(cs, myCall)

            '    'If d < bestDistance Then
            '    '    bestDistance = d
            '    '    bestCall = cs
            '    'End If
            'Next
            'If bestCall <> "" Then
            '    Return bestCall
            'End If

            For Each c In candidates                                    ' SWLの選択
                cs = c
                ' 典型的なSWLパターンに合致するものだけ採用
                If Not Regex.IsMatch(cs, SwlPattern) Then
                    Continue For
                End If
                bestCall = cs
                Return bestCall
            Next

            Return ""
        End Function
        ']

        ' Callsign の誤認識補正
        Public Shared Function ReplaceAtoN(s As String) As String
            ' よくある誤認識の補正
            s = s.Replace("O", "0")   ' O → 0
            s = s.Replace("D", "0")   ' D → 0
            s = s.Replace("I", "1")   ' I → 1
            s = s.Replace("L", "1")   ' L → 1
            s = s.Replace("Z", "7")   ' Z → 7
            s = s.Replace("T", "7")   ' T → 7
            s = s.Replace("B", "8")   ' B → 8
            s = s.Replace("G", "0")   ' B → 8

            Return s
        End Function

        Public Shared Function ReplaceNtoA(s As String) As String
            ' よくある誤認識の補正
            s = s.Replace("0", "O")   ' O → 0
            s = s.Replace("1", "I")   ' D → 0
            s = s.Replace("2", "Z")   ' I → 1
            s = s.Replace("3", "B")   ' L → 1
            s = s.Replace("5", "S")   ' S → 5
            s = s.Replace("7", "T")   ' L → 1

            Return s
        End Function


        '＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
        '
        '   日付の抽出
        '
        '＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊


        ' 日付抽出のメイン関数
        Public Shared Function ExtractDate(text As String) As String
            Dim pattern As String
            Dim m As Match

            text = text.Replace(vbLf, " ")
            text = CleanTextDateSymbol(text)
            Dim cleaned = CleanTextDate(text)

            ClearBaseDetection(DateBase)
            DateBaseTitle = New Detection With {.Value = "", .Index = -1, .Length = 0, .Row = 0, .Column = 0}
            ClearBaseDetection(DateBaseTitle)
            pattern = "(DATE|(YY)?YYMMDD|YEAR|YYYY|YY)"
            m = Regex.Match(cleaned, pattern)
            If m.Success = True Then
                Dim index As Integer = m.Index
                ' 行と列を計算するメソッドを呼び出す
                Dim row As Integer = 0
                Dim col As Integer = 0
                GetRowAndColumn(cleaned, index, row, col)
                SetBaseDetection(DateBaseTitle, m.Groups(0).Value, m.Index, m.Length, row, col)
            End If

            DateFormat = DateFormats.InvalidFormat
            pattern = "([YF][EAR]{0,3}|D[DAY]{0,2})[- /:;]{1,3}(M[MONTH]{0,4})[- /:;]{1,3}([YF][EAR]{0,3}|D[AY]{0,2})"
            m = Regex.Match(cleaned, pattern)
            If m.Success = True Then
                Debug.Print($"{m.Groups(1).Value} - {m.Groups(2).Value} - {m.Groups(3).Value}")
                Dim v1 = m.Groups(1).Value.Substring(0, 1)
                Dim v2 = m.Groups(2).Value.Substring(0, 1)
                Dim v3 = m.Groups(3).Value.Substring(0, 1)
                If (v1 = "Y" OrElse v1 = "F") AndAlso v2 = "M" AndAlso v3 = "D" Then
                    DateFormat = DateFormats.IsoFormat
                ElseIf v1 = "D" AndAlso v2 = "M" AndAlso (v3 = "Y" OrElse v3 = "F") Then
                    DateFormat = DateFormats.BritishFormat
                ElseIf v1 = "M" AndAlso v2 = "D" AndAlso (v3 = "Y" OrElse v3 = "F") Then
                    DateFormat = DateFormats.AmericanFormat
                End If
            Else
                pattern = "([YF][EAR]{0,3}|YY)[- /:;]{1,3](M[ONTH]{0,4}|MM)[- /:;]{1, 3}(D[AY]{0,2}|DD)"
                m = Regex.Match(cleaned, pattern)
                If m.Success = True Then
                    DateFormat = DateFormats.IsoFormat
                Else
                    pattern = "(D[AY]{0,2}|DD)[- /:;]{1,3}(M[ONTH]{0,4}|MM)[- /:;]{1,3}([YF][EAR]{0,3}|YY)"
                    m = Regex.Match(cleaned, pattern)
                    If m.Success = True Then
                        DateFormat = DateFormats.BritishFormat
                    Else
                        pattern = "(M[ONTH]{0,4}|MM)[- /:;]{1,3}(D[AY]{0,2}|DD)[- /:;]{1,3}([YF][EAR]{0,3}|YY)"
                        m = Regex.Match(cleaned, pattern)
                        If m.Success = True Then
                            DateFormat = DateFormats.AmericanFormat
                        End If
                    End If
                End If
            End If

            'If DateFormat = DateFormats.InvalidFormat Then
            '    'DateFormat = JugementDateFormat(text)               ' yyyy/mm/dd、dd/mm/yy等の順から日付のフォーマットを設定
            '    DateFormat = JugementDateFormatFromEntity(qsoCallsign)      ' Enthityから日付のファーマっとを設
            'End If

            ' 1. 月名を含む形式を探す
            Dim dateFromMonth = ExtractDateWithMonthName(cleaned)
            If dateFromMonth <> "" Then Return dateFromMonth

            '2. 誤読した英月名で探す
            dateFromMonth = ExtractMisReadDate(cleaned)
            If dateFromMonth <> "" Then Return dateFromMonth

            ' 3. 数字だけの形式を探す
            Dim dateFromNumbers = ExtractDateNumeric(text)
            If dateFromNumbers <> "" Then Return dateFromNumbers

            dateFromNumbers = ExtractDateNumeric(cleaned)
            If dateFromNumbers <> "" Then Return dateFromNumbers


            ' 3.1 hamlog形式の数字だけの形式を探す
            dateFromNumbers = ExtractDateHamlog(cleaned)
            If dateFromNumbers <> "" Then Return dateFromNumbers

            '  4. 日本語の年、月、日を誤読したとして
            dateFromMonth = ExtractNenTsukiHiDate(cleaned)
            If dateFromMonth <> "" Then Return dateFromMonth

            dateFromMonth = ExtractDateSpace(cleaned)
            If dateFromMonth <> "" Then Return dateFromMonth

            dateFromMonth = ExtractDateWithMonthName2Line(cleaned)
            If dateFromMonth <> "" Then Return dateFromMonth

            ' 5. 日本語の年、月、日を誤読したとして
            dateFromMonth = ExtractJapaneseDate(cleaned)
            If dateFromMonth <> "" Then Return dateFromMonth

            dateFromMonth = ExtractYYMMDDDDate(cleaned)
            If dateFromMonth <> "" Then Return dateFromMonth

            If DateBaseTitle.Value <> "" Then
                Dim bestAwnser As String() = ChooseBestDetection(DetectionDates, DateBaseTitle, False)
                If bestAwnser(0) <> "" Then
                    AddUsed(bestAwnser(0), bestAwnser(1), bestAwnser(2))
                    SetBaseDetection(DateBase, bestAwnser(0), bestAwnser(1), bestAwnser(2), 0, 0)
                    Return bestAwnser(0)
                End If
            End If

            Return ""
        End Function

        ' 月名を含む日付
        Private Shared Function ExtractDateWithMonthName(text As String) As String
            ' 月名の順に整合しているので、複数行の年月日がある場合、1番目の行が選択されると限らない

            text = CleanTextMisreadingDate(text)        ' 月名をミスリードしたものを補正する
            Dim pattern As String
            Dim m As Match
            Dim month As String
            Dim day As String
            Dim year As String
            Dim sm As String

            For Each key In MonthNames.Keys
                If text.Contains(key) = False Then Continue For

                If DateFormat = DateFormats.BritishFormat Then
                    pattern = "(\b|\n|\s|\D)([0-2]\d|3[01]|[1-9])[- :;/\.]{0,3}" & key & "[-. :;\.]{0,3}(['’\""]?)((19|20)?\d{1,2})(\b|\n|\s|\D)"       'BritishFormat 日、月、年の並び順 縦罫線がJと読まれることがあるので、Jも許容する
                    m = Regex.Match(text, pattern)
                    If m.Success Then
                        'If (m.Groups(1).Value = "'") OrElse (m.Groups(2).Value.Length = 4) Then
                        month = MonthNames(key)
                        day = Integer.Parse(m.Groups(2).Value)
                        Dim y = m.Groups(4).Value
                        If y.Length = 3 Then     ' 3桁の年は誤読している。下2桁を年とする (例: '123 → 2023)
                            year = y.Substring(1, 2)
                        Else
                            year = m.Groups(4).Value
                        End If
                        year = FixYear(year)           ' 年が2桁なら2000年代とみなす
                        If isValidDate(year, month, day) Then
                            sm = FormatDate(year, month, day)
                            UsedRanges.Add((sm, m.Index, m.Length))
                            SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, 0, 0)
                            Return sm
                        End If
                        'DateFormat = DateFormats.InvalidFormat
                    End If
                ElseIf DateFormat = DateFormats.AmericanFormat Then
                    pattern = "(\b|\n|\s)" & key & "[-/. :;]{1,4}(\d{1,2})[-/\. :;]{1,4}(['‘’]?)(\d{1,4})(\b|^n|\s)"       'AmericanFormat 月、日、年の並び順
                    m = Regex.Match(text, pattern)
                    If m.Success Then
                        'If (m.Groups(1).Value = "'") OrElse (m.Groups(2).Value.Length = 4) Then
                        month = MonthNames(key)
                        day = Integer.Parse(m.Groups(2).Value)
                        Dim y = m.Groups(4).Value
                        If y.Length = 3 Then     ' 3桁の年は誤読している。下2桁を年とする (例: '123 → 2023)
                            year = y.Substring(1, 2)
                        Else
                            year = m.Groups(4).Value
                        End If
                        year = FixYear(year)          ' 年が2桁なら2000年代とみなす
                        If isValidDate(year, month, day) Then
                            sm = FormatDate(year, month, day)
                            UsedRanges.Add((sm, m.Index, m.Length))
                            SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, 0, 0)
                            Return sm
                        End If
                        'DateFormat = DateFormats.InvalidFormat
                    End If
                    'DateFormat = DateFormats.InvalidFormat
                ElseIf DateFormat = DateFormats.IsoFormat Then
                    pattern = "\b(['’‘]?)(\d{1,4})[-/. :;]{1,4}" & key & "[-/\. :;']{1,4}(\d{1,2})\b"       'IsoFormat年、月、日の並び順

                    'text = " 2025 APR 22 16:06 -06 18 FT8"
                    m = Regex.Match(text, pattern)
                    If m.Success Then
                        Debug.Print(m.Groups(2).Value & "-" & m.Groups(3).Value & "-" & m.Groups(4).Value)
                        'Debug.Print(BitConverter.ToString(System.Text.Encoding.UTF8.GetBytes(text))) ' 結果: "41-42-43"

                        month = MonthNames(key)
                        day = m.Groups(3).Value
                        Dim y = m.Groups(2).Value
                        If y.Length = 3 Then     ' 3桁の年は誤読している。下2桁を年とする (例: '123 → 2023)
                            year = y.Substring(1, 2)
                        Else
                            year = y
                        End If
                        year = FixYear(year)           ' 年が2桁なら2000年代とみなす
                        If isValidDate(year, month, day) Then
                            sm = FormatDate(year, month, day)
                            AddUsed(sm, m.Index, m.Length)
                            SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, 0, 0)
                            Return sm
                        End If
                        'DateFormat = DateFormats.InvalidFormat
                    End If
                    'DateFormat = DateFormats.InvalidFormat
                    'End If
                Else                 ' DateFormatがDateFormats.InvalidFormatの時
                    ' DateFormat = DateFormats.InvalidFormat の場合　BritishformatまたはisoFormatと判断
                    pattern = "\b(['’‘""]?) {0,1}(\d{1,4})[-/\. :;]{0,4}" & key & "[-/. :;]{0,4}(['’‘""])? {0,2}(\d{1,4})(\b|\s|\D)"       '年、月、日または日、月、年の並び順で試す　頭文字は他の項目との重なりを考慮し、空白1文字にした
                    m = Regex.Match(text, pattern)
                    If m.Success Then
                        Dim v1 = Math.Abs(m.Groups(2).Value - Now.Year.ToString.Substring(2, 2))
                        Dim v2 = Math.Abs(m.Groups(4).Value - Now.Year.ToString.Substring(2, 2))
                        If (m.Groups(2).Value.Length >= 3 AndAlso m.Groups(4).Value.Length <= 2) OrElse         ' 長さで年を判断
                           (m.Groups(1).Value = "'") OrElse                                                     ' 年を省略の’を付けた
                           (m.Groups(2).Value > 31) Then                                                ' 日の最終日31より大きい  
                            Dim y = m.Groups(2).Value
                            month = MonthNames(key)
                            day = Integer.Parse(m.Groups(4).Value)          ' 年、月、日

                            If y.Length = 3 Then     ' 3桁の年は誤読している。下2桁を年とする (例: '123 → 2023)
                                year = y.Substring(1, 2)
                            Else
                                year = m.Groups(2).Value
                            End If
                            If isValidDate(year, month, day) Then
                                sm = FormatDate(year, month, day)
                                UsedRanges.Add((sm, m.Index, m.Length))
                                SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, 0, 0)
                                Return sm
                            End If
                        ElseIf (m.Groups(2).Value.Length <= 2 AndAlso m.Groups(4).Value.Length >= 3) OrElse         '　日、月、年
                            (m.Groups(3).Value = "'") OrElse
                            (m.Groups(4).Value > 31) Then    ' britishFormat nobaai 
                            '(m.Groups(2).Value > Now.Year.ToString().Substring(2, 2)) Then
                            month = MonthNames(key)
                            day = Integer.Parse(m.Groups(2).Value)
                            Dim y = m.Groups(4).Value
                            If (y.Length = 3) AndAlso (DateFormat = DateFormats.IsoFormat) Then     ' 3桁の年は誤読している。下2桁を年とする (例: '123 → 2023)
                                year = y.Substring(1, 2)
                            Else
                                year = m.Groups(4).Value
                            End If
                            year = FixYear(year)          ' 年が2桁なら2000年代とみなす
                            If isValidDate(year, month, day) Then
                                sm = FormatDate(year, month, day)
                                AddUsed(m.Value, m.Index, m.Length)
                                SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, 0, 0)
                                Return sm
                            End If
                        ElseIf v1 < v2 Then                             ' 決定できないときは、現在年に近いほうを年とする
                            month = MonthNames(key)
                            day = Integer.Parse(m.Groups(4).Value)
                            year = m.Groups(2).Value
                            year = FixYear(year)          ' 年が2桁なら2000年代とみなす
                            If isValidDate(year, month, day) Then
                                sm = FormatDate(year, month, day)
                                UsedRanges.Add((sm, m.Index, m.Length))
                                SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, 0, 0)
                                Return sm
                            Else
                                month = MonthNames(key)
                                day = Integer.Parse(m.Groups(2).Value)
                                year = m.Groups(4).Value
                                year = FixYear(year)          ' 年が2桁なら2000年代とみなす
                                If isValidDate(year, month, day) Then
                                    sm = FormatDate(year, month, day)
                                    UsedRanges.Add((sm, m.Index, m.Length))
                                    SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, 0, 0)
                                    Return sm
                                End If
                            End If
                        Else
                            month = MonthNames(key)
                            day = Integer.Parse(m.Groups(2).Value)
                            year = m.Groups(4).Value
                            year = FixYear(year)          ' 年が2桁なら2000年代とみなす
                            If isValidDate(year, month, day) Then
                                sm = FormatDate(year, month, day)
                                UsedRanges.Add((sm, m.Index, m.Length))
                                SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, 0, 0)
                                Return sm
                            Else
                                month = MonthNames(key)
                                day = Integer.Parse(m.Groups(4).Value)
                                year = m.Groups(2).Value
                                year = FixYear(year)          ' 年が2桁なら2000年代とみなす
                                If isValidDate(year, month, day) Then
                                    sm = FormatDate(year, month, day)
                                    UsedRanges.Add((sm, m.Index, m.Length))
                                    SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, 0, 0)
                                    Return sm
                                End If
                            End If
                        End If
                    End If
                End If

                pattern = "\b" & key & "[- .,/:;]{0,4}(\d{1,2})[- .,/:;]{1,4}(\d{2,4})\b"              '月、日、年の並び順     American Format の場合
                m = Regex.Match(text, pattern)
                If m.Success Then
                    month = MonthNames(key)
                    day = Integer.Parse(m.Groups(1).Value)
                    year = FixYear(m.Groups(2).Value)
                    If isValidDate(year, month, day) Then
                        Dim s As String = FormatDate(year, month, day)
                        UsedRanges.Add((s, m.Index, m.Length))
                        SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, 0, 0)
                        Return s
                    End If
                End If
            Next

            Return ""
        End Function

        '  2. 誤読した英月名で探す(Levenshtein)
        Private Shared Function ExtractMisReadDate(text As String) As String
            Dim dateFromMonth As String

            Dim pattern = "[A-Z0-9]{4,10}"
            For Each key In MonthNames.Keys
                If key.Length <= 3 Then Continue For
                Dim ms = Regex.Matches(text, pattern)

                For Each m As Match In ms
                    Dim d As Double = Similarity(key, m.Value)
                    If d > 0.74 Then
                        text = text.Replace(m.Value, key)
                        If IsUsed(m.Index, m.Length) Then
                            dateFromMonth = ExtractDateWithMonthName(text)
                            If dateFromMonth <> "" Then
                                AddUsed(dateFromMonth, m.Index, m.Length)
                                SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, 0, 0)
                                Return dateFromMonth
                            End If
                        End If
                    End If
                Next
            Next

            Return ""
        End Function


        ' 数字だけの日付
        Private Shared Function ExtractDateNumeric(text As String) As String
            text = CleanTextDateNumeric(text)

            ' 年が真ん中に来るはずない
            'Dim pattern = "(^|\s|\D|:|;)(')?(\d{2}|\d{4}|\d)[/\-\. ]{1,2}(\d{1,2})[/\-\. ]{1,2}(')?(\d{2}|\d{4}|\d)(\s|\D|$)"
            'Dim pattern = "(^|\s|\D|:|;)(')?(\d{2}|\d{4}|\d)([/\-\.\/ ])(\d{1,2})\4(')?(\d{2}|\d{4}|\d)(\s|\D|$)"
            'Dim pattern = "\b(')?(\d{1,4}) {0,2}[-/. ]{1,3} {0,2}(\d{1,2}) {0,2}[-/. ']{1,3}? {0,2}(')? ?(\d{1,4})\b"

            'Dim ms = Regex.Matches(text, pattern)
            'Dim m As Match = Nothing
            'For Each m In ms
            ''If (m.Index < DateBase.Index) AndAlso (DateBase.Index = 0) Then Continue For

            ''    Dim part1 = m.Groups(2).Value
            ''    Dim part2 = m.Groups(3).Value
            ''    Dim part3 = m.Groups(5).Value

            Dim year As String = ""
            Dim month As String = ""
            Dim day As String = ""
            Dim ymd As String = ""
            If (DateFormat = DateFormats.BritishFormat) Then     '  British Format の場合
                Dim pattern = "\b([012]\d|3[01]|[1-9]) {0,2}[-/. ]{1,3} {0,2}(0\d|1[0-2]|[1-9]) {0,2}[-/. ']{1,3}? {0,2}(')? ?((19|20)?\d{2})\b"

                Dim ms = Regex.Matches(text, pattern)
                Dim m As Match = Nothing
                For Each m In ms
                    If (m.Index < DateBase.Index) AndAlso (DateBase.Index = 0) Then Continue For

                    year = m.Groups(4).Value
                    month = m.Groups(2).Value
                    day = m.Groups(1).Value
                    If Not isValidDate(year, month, day) Then Continue For
                    ymd = FormatDate(year, month, day)

                    If Not IsUsed(m.Index, m.Length) Then
                        DetectionDates.Add(New Detection With {.Value = ymd, .Index = m.Groups(0).Index, .Length = m.Groups(0).Length, .Row = 0, .Column = 0})
                    End If
                Next
            ElseIf (DateFormat = DateFormats.americanFormat) Then
                Dim pattern = "\b(0\d|1[0-2]) {0,2}[-/. ]{1,3} {0,2}([012]\d|3[01]) {0,2}[-/. ']{1,3}? {0,2}(')? ?(19|20\d{2})\b"

                Dim ms = Regex.Matches(text, pattern)
                Dim m As Match = Nothing
                For Each m In ms
                    If (m.Index < DateBase.Index) AndAlso (DateBase.Index = 0) Then Continue For

                    year = m.Groups(4).Value
                    month = m.Groups(1).Value
                    day = m.Groups(2).Value
                    If Not isValidDate(year, month, day) Then Continue For
                    ymd = FormatDate(year, month, day)

                    If Not IsUsed(m.Index, m.Length) Then
                        DetectionDates.Add(New Detection With {.Value = ymd, .Index = m.Groups(0).Index, .Length = m.Groups(0).Length, .Row = 0, .Column = 0})
                    End If
                Next
            ElseIf (DateFormat = DateFormats.IsoFormat) Then                                                      ' コロンがない場合 年，月、日の順とみなす  ISO Format
                Dim pattern = "\b(')? ?([19|20]?\d{2}) {0,2}[-/. ]? {0,2}(0\d|1[0-2]) {0,2}[-/. ']? {0,2}([012]\d|3[01])\b"

                Dim ms = Regex.Matches(text, pattern)
                Dim m As Match = Nothing
                For Each m In ms
                    If (m.Index < DateBase.Index) AndAlso (DateBase.Index = 0) Then Continue For

                    year = m.Groups(2).Value
                    month = m.Groups(3).Value
                    day = m.Groups(4).Value
                    If Not isValidDate(year, month, day) Then Continue For
                    ymd = FormatDate(year, month, day)

                    If Not IsUsed(m.Index, m.Length) Then
                        DetectionDates.Add(New Detection With {.Value = ymd, .Index = m.Groups(0).Index, .Length = m.Groups(0).Length, .Row = 0, .Column = 0})
                    End If
                Next
            Else                ' DateFormat = DateFormats.invalidの場合 
                Dim p1 As String = "(')?(\d{1,4})"
                Dim p2 As String = " {0,2}[-/., ]{1,2} {0,2}"
                Dim pattern = "\b" & p1 & p2 & p1 & p2 & p1 & p2 & p1 & "?\b"

                Dim ms = Regex.Matches(text, pattern)
                Dim m As Match = Nothing
                For Each m In ms
                    If (m.Groups(1).Value = "'") OrElse (m.Groups(2).Length > 2) OrElse (m.Groups(2).Value > 31) Then   'm.Groups(2)が年なら、年、月、日の並び 
                        year = m.Groups(2).Value
                        month = m.Groups(4).Value
                        day = m.Groups(6).Value
                    ElseIf (m.Groups(5).Value = "'") OrElse (m.Groups(6).Length > 2) OrElse (m.Groups(6).Value > 31) Then       ' m.Groups(6)が年の場合
                        year = m.Groups(6).Value
                        If m.Groups(2).Value > 12 Then          ' 12より大きければ月でない
                            month = m.Groups(4).Value
                            day = m.Groups(2).Value
                        ElseIf m.Groups(4).Value > 12 Then
                            month = m.Groups(2).Value
                            day = m.Groups(4).Value
                        Else                                    ' どちらも12以下であれば、BritishFormatとみなす
                            month = m.Groups(4).Value
                            day = m.Groups(2).Value
                        End If
                    Else
                        If (m.Groups(4).Value > 12) Then        ' 2番目が12以上だと、それは日か年だが2番目が年になることはない
                            year = m.Groups(6).Value
                            month = m.Groups(4).Value
                            day = m.Groups(2).Value
                        Else
                            year = m.Groups(2).Value            ' 判断できないので年、月、日とする
                            month = m.Groups(4).Value
                            day = m.Groups(6).Value
                        End If
                    End If

                    If isValidDate(year, month, day) Then
                        ymd = FormatDate(year, month, day)
                        Dim len = m.Groups(6).Index - m.Groups(2).Index + m.Groups(6).Length
                        If Not IsUsed(m.Groups(2).Index, len) Then
                            DetectionDates.Add(New Detection With {.Value = ymd, .Index = m.Groups(2).Index, .Length = len, .Row = 0, .Column = 0})
                            Continue For
                        End If
                    End If

                    If m.Groups(8).Value <> "" Then
                        If (m.Groups(3).Value = "'") OrElse (m.Groups(4).Length > 2) Then           ' 項目を1個づらしでcheck 
                            year = m.Groups(4).Value
                            month = m.Groups(6).Value
                            day = m.Groups(8).Value
                        ElseIf (m.Groups(7).Value = "'") OrElse (m.Groups(8).Length > 2) Then
                            year = m.Groups(8).Value
                            If m.Groups(4).Value > 12 Then
                                month = m.Groups(6).Value
                                day = m.Groups(4).Value
                            Else
                                month = m.Groups(4).Value
                                day = m.Groups(6).Value
                            End If
                        End If
                        If isValidDate(year, month, day) Then
                            ymd = FormatDate(year, month, day)
                            Dim len = m.Groups(8).Index - m.Groups(4).Index + m.Groups(8).Length
                            If Not IsUsed(m.Groups(4).Index, len) Then
                                DetectionDates.Add(New Detection With {.Value = ymd, .Index = m.Groups(4).Index, .Length = len, .Row = 0, .Column = 0})
                                Continue For
                            End If
                        End If

                        'If Not IsUsed(m.Index, m.Length) Then
                        '    DetectionDates.Add(New Detection With {.Value = ymd, .Index = m.Groups(0).Index, .Length = m.Groups(0).Length, .Row = 0, .Column = 0})
                        'End If
                    End If
                Next
            End If

            'Dim part1 = m.Groups(2).Value
            '        Dim part2 = m.Groups(3).Value
            '        Dim part3 = m.Groups(5).Value
            '        year =                 Dim part1 = m.Groups(2).Value
            '    Dim part2 = m.Groups(3).Value
            '        Dim part3 = m.Groups(5).Value
            '        Dim year As String = ""
            '        Dim month As String = ""
            '        Dim day As String = ""
            '        Dim month As String = ""
            '        Dim day As String = m.Groups(5).Value


            '        year = FixYear(year)           ' 年が2桁なら2000年代とみなす

            '        If Not isValidDate(year, month, day) Then
            '            Dim temp As String = month                  ' 年と日を入れ替えてみる
            '            month = day
            '            day = temp
            '            If Not isValidDate(year, month, day) Then Continue For

            '        End If

            '        Dim ymd = FormatDate(year, month, day)

            '        If Not IsUsed(m.Index, m.Length) Then
            '            'AddUsed(m.Groups(0).Value, m.Index, m.Length)
            '            '                    SetBaseDetection(DetectionDates, ymd, m.Index, m.Length, 0, 0)
            '            DetectionDates.Add(New Detection With {.Value = ymd, .Index = m.Groups(0).Index, .Length = m.Groups(0).Length, .Row = 0, .Column = 0})

            '        End If
            '    Next
            'End If
            ''Next

            If DetectionDates.Count = 1 Then
                AddUsed(DetectionDates(0).Value, DetectionDates(0).Index, DetectionDates(0).Length)
                SetBaseDetection(DateBase, DetectionDates(0).Value, DetectionDates(0).Index, DetectionDates(0).Length, 0, 0)
                Return DetectionDates(0).Value
            End If

            If DateBaseTitle.Value <> "" Then
                Dim bestAwnser As String() = ChooseBestDetection(DetectionDates, DateBaseTitle, False)
                If bestAwnser(0) <> "" Then
                    AddUsed(bestAwnser(0), bestAwnser(1), bestAwnser(2))
                    SetBaseDetection(DateBase, bestAwnser(0), bestAwnser(1), bestAwnser(2), 0, 0)
                    Return bestAwnser(0)
                End If
            End If

            If DetectionDates.Count > 1 Then
                AddUsed(DetectionDates(0).Value, DetectionDates(0).Index, DetectionDates(0).Length)
                SetBaseDetection(DateBase, DetectionDates(0).Value, DetectionDates(0).Index, DetectionDates(0).Length, 0, 0)
                Return DetectionDates(0).Value
            End If

            ' 日付のTitleがない場合、Titleより後で100文字以内の位置にある日付を採用する
            'If m IsNot Nothing Then
            '    If (DateBaseTitle.Index <= 0) OrElse ((m.Index > DateBaseTitle.Index) AndAlso (m.Index < DateBaseTitle.Index + 100)) Then
            '        AddUsed(m.Groups(0).Value, m.Index, m.Length)     ' yyyy/mm/dd
            '        SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, 0, 0)
            '        a = m.Groups(1).Value
            '        b = m.Groups(2).Value
            '        c = m.Groups(3).Value
            '        Dim d = FormatDate(a, b, c)
            '        Return d            ' Return $"{a:0000}/{b:00}/{c:00}"
            '    End If
            'End If

            Return ""
        End Function

        Private Shared Function ExtractDateHamlog(text As String) As String

            Dim pattern = "(\d{2})([0][0-9]|[1][0-2])([0-2][0-9]|3[0-1])"

            Dim m = Regex.Match(text, pattern)
            If m.Success Then
                Dim year = m.Groups(1).Value
                Dim month = m.Groups(2).Value
                Dim day = m.Groups(3).Value
                If year.Length = 3 Then    ' 3桁の年は誤読している。下2桁を年とする (例: '123 → 2023)
                    year = year.Substring(1, 2)
                End If

                year = FixYear(year)           ' 年が2桁なら2000年代とみなす
                If isValidDate(year, month, day) Then
                    Return FormatDate(year, month, day)           ' Return $"{a:0000}/{b:00}/{c:00}"
                End If
            End If

            Return ""
        End Function

        '  4. 日本語の年、月、日を誤読したとして
        Private Shared Function ExtractNenTsukiHiDate(Text As String) As String

            'Dim pattern = "(19|20)\d{2}.{1,3}([01]\d).{1,3}([0-3]?\d)"     ' Copilot提案
            Dim pattern = "((19|20)\d{2}).{1,2}([01]\d).{1,2}([0-3]\d)"

            Dim ms = Regex.Matches(Text, pattern)

            For Each m As Match In ms
                Dim year = m.Groups(1).Value
                Dim month = m.Groups(3).Value
                Dim day = m.Groups(4).Value

                If Not isValidDate(year, month, day) Then Continue For
                If Not IsUsed(m.Index, m.Length) Then
                    AddUsed(m.Groups(0).Value, m.Index, m.Length)
                    SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, 0, 0)
                    Return FormatDate(year, month, day)
                End If
            Next

            Return ""
        End Function


        '  4. 日本語の年、月、日を誤読したとして
        Private Shared Function ExtractDateSpace(text As String) As String
            Dim pattern As String
            Dim ms As MatchCollection
            Dim year, month, day As String
            If DateFormat = DateFormats.BritishFormat Then
                pattern = "([012]\d|3[0-1])[.:;\s]?(0\d|1[0-2])[.:;\s]?((19|20)?\d{2})"

            ElseIf DateFormat = DateFormats.AmericanFormat Then
                pattern = "(0\d|1[0-2])[.:;\s]?([012]\d|3[0-1])[.:;\s]?((19|20)?\d{2})"
            Else
                pattern = "((19|20)?\d{2})[.:;\s]?(0\d|1[0-2])[.:;\s]?([012]\d|3[0-1])"      ' IsoFormatまたはその他
            End If

            ms = Regex.Matches(text, pattern)
            For Each m As Match In ms
                If DateFormat = DateFormats.BritishFormat Then
                    year = m.Groups(3).Value
                    month = m.Groups(2).Value
                    day = m.Groups(1).Value
                ElseIf DateFormat = DateFormats.AmericanFormat Then
                    year = m.Groups(3).Value
                    month = m.Groups(1).Value
                    day = m.Groups(2).Value
                Else
                    year = m.Groups(1).Value
                    month = m.Groups(2).Value
                    day = m.Groups(3).Value
                End If

                If Not isValidDate(year, month, day) Then Continue For
                If Not IsUsed(m.Index, m.Length) Then
                    AddUsed(m.Groups(0).Value, m.Index, m.Length)
                    SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, 0, 0)
                    Return FormatDate(year, month, day)
                End If
            Next

            Return ""
        End Function

        Private Shared Function ExtractDateWithMonthName2Line(text As String) As String
            'text = CleanTextDate(text)
            Dim pattern As String
            Dim m As Match
            Dim month As String
            Dim day As String
            Dim year As String = ""
            Dim sm As String
            Dim yearIndex As Integer
            Dim yearLength As Integer

            Dim DateFormat = JugementDateFormatFromEntity(qsoCallsign)      ' Enthityから日付のファーマっとを設

            Dim y As String
            Dim yDef As Integer = Integer.MaxValue
            pattern = "\b?(19|20)(\d{2})\b"              '19xx,20xxのみ
            Dim ms = Regex.Matches(text, pattern)
            For Each m In ms
                If m.Success Then
                    y = m.Groups(1).Value & m.Groups(2).Value
                    y = FixYear(y)           ' 年が2桁なら2000年代とみなす

                    If Math.Abs(y - Now.Year) > yDef Then Continue For
                    yearIndex = m.Index
                    yearLength = m.Length
                    year = y
                    yDef = Math.Abs(y - Now.Year)
                End If
            Next
            If yDef = Integer.MaxValue Then Return ""

            For Each key In MonthNames.Keys
                If Not text.Contains(key) Then Continue For
                '         pattern = "\b?(\d{1,2})?[\s/-]*" & key & "[\.\s/-']*(\d{1,2})?\b"              '月、日または日、月の並び順
                pattern = "\b([0-2]\d|[3][0-1])?[ /\-]{1,3}" & key & "\b"
                m = Regex.Match(text, pattern)
                If m.Success Then
                    If m.Groups(1).Value <> "" Then         ' (yy)yy-mm-dd  Japanese Format の場合
                        month = MonthNames(key)
                        day = Integer.Parse(m.Groups(1).Value)
                        If isValidDate(year, month, day) Then
                            sm = FormatDate(year, month, day)
                            AddUsed(sm, m.Index, m.Length)
                            AddUsed(year, yearIndex, yearLength)
                            SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, yearIndex, yearLength)
                            Return sm
                        End If
                    End If
                End If

                pattern = "\b?" & key & "[ /\-]{0,3}([0-2]\d|[3][0-1])\b"
                m = Regex.Match(text, pattern)
                If m.Success Then
                    If m.Groups(1).Value <> "" Then         ' (yy)yy-mm-dd  Japanese Format の場合
                        month = MonthNames(key)
                        day = Integer.Parse(m.Groups(1).Value)
                        If isValidDate(year, month, day) Then
                            sm = FormatDate(year, month, day)
                            AddUsed(sm, m.Index, m.Length)
                            AddUsed(year, yearIndex, yearLength)
                            SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, yearIndex, yearLength)
                            Return sm
                        End If
                    End If
                End If
            Next

            pattern = "\b(\d{1,2})(月|日) {0,2}(\d{1,2})(日|月)"          ' 日、月の順
            m = Regex.Match(text, pattern)
            If m.Success Then
                If m.Groups(2).Value = "月" Then
                    month = m.Groups(1).Value
                    day = m.Groups(3).Value
                Else
                    month = m.Groups(3).Value
                    day = m.Groups(1).Value
                End If

                If yearIndex < m.Index Then
                    If isValidDate(year, month, day) Then
                        sm = FormatDate(year, month, day)
                        AddUsed(sm, m.Index, m.Length)
                        AddUsed(year, yearIndex, yearLength)
                        SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, yearIndex, yearLength)
                        Return sm
                    End If
                End If
            End If

            pattern = "\b([0-2]\d|[3][0-1]|[1-9])[ /\-]{1,3}(0[1-9]|1[0-2]|[1-9])\b"            ' 日、月の順
            m = Regex.Match(text, pattern)
            If m.Success Then
                If m.Groups(1).Value <> "" Then         ' (yy)yy-mm-dd  Japanese Format の場合
                    month = Integer.Parse(m.Groups(2).Value)
                    day = Integer.Parse(m.Groups(1).Value)

                    If yearIndex < m.Index Then
                        If isValidDate(year, month, day) Then
                            sm = FormatDate(year, month, day)
                            AddUsed(sm, m.Index, m.Length)
                            AddUsed(year, yearIndex, yearLength)
                            SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, yearIndex, yearLength)
                            Return sm
                        End If
                    End If
                End If
            End If

            pattern = "\b(0[1-9]|1[0-2]|[1-9])[ /\-]{0,3}([0-2]\d|[3][0-1]|[1-9])\b"               ' 月、日の順
            m = Regex.Match(text, pattern)
            If m.Success Then
                If m.Groups(1).Value <> "" Then         ' (yy)yy-mm-dd  Japanese Format の場合
                    month = Integer.Parse(m.Groups(1).Value)
                    day = Integer.Parse(m.Groups(2).Value)

                    If yearIndex < m.Index Then
                        If isValidDate(year, month, day) Then
                            sm = FormatDate(year, month, day)
                            AddUsed(sm, m.Index, m.Length)
                            AddUsed(year, yearIndex, yearLength)
                            SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, yearIndex, yearLength)
                            Return sm
                        End If
                    End If
                End If
            End If

            Return ""
        End Function

        Private Shared Function ExtractJapaneseDate(text As String) As String
            ' 日本語で年、月、日が書かれている場合の抽出
            Dim yearPattern As String = "(?<Year>\d{4}|\d{2})(?=年)"
            Dim monthPattern As String = "(?<Month>\d{1,2})(?=月)"
            Dim dayPattern As String = "(?<Day>\d{1,2})(?=日)"

            Dim yearMatch = Regex.Match(text, yearPattern)
            Dim monthMatch = Regex.Match(text, monthPattern)
            Dim dayMatch = Regex.Match(text, dayPattern)

            Dim year As String = If(yearMatch.Success, yearMatch.Groups("Year").Value, "")
            Dim month As String = If(monthMatch.Success, monthMatch.Groups("Month").Value, "")
            Dim day As String = If(dayMatch.Success, dayMatch.Groups("Day").Value, "")

            If year <> "" AndAlso month <> "" AndAlso day <> "" Then
                If isValidDate(year, month, day) Then
                    Dim formattedDate = FormatDate(year, month, day)
                    AddUsed(formattedDate, yearMatch.Index, yearMatch.Length)
                    AddUsed(formattedDate, monthMatch.Index, monthMatch.Length)
                    AddUsed(formattedDate, dayMatch.Index, dayMatch.Length)
                    SetBaseDetection(DateBase, formattedDate, yearMatch.Index, yearMatch.Length, monthMatch.Index, monthMatch.Length)
                    Return formattedDate
                End If
            End If
            Return ""
        End Function

        Private Shared Function ExtractYYMMDDDDate(text As String) As String
            ' 日本語でyear,month,day付の年月日の場合の抽出
            Dim yearPattern As String = "\b(Y[YEAR]{0,3})[\s\b\n]{0,2}(\d{4}|\d{2})"
            Dim monthPattern As String = "\b(M[MONTH]{0,4})[\s\b\n]{0,2}(\d{1,2})"
            Dim dayPattern As String = "\b(D[DAY]{0,2})[\s\b\n]{0,2}(\d{1,2})"

            Dim yearMatch = Regex.Match(text, yearPattern)
            Dim monthMatch = Regex.Match(text, monthPattern)
            Dim dayMatch = Regex.Match(text, dayPattern)

            Dim year As String = If(yearMatch.Success, yearMatch.Groups(2).Value, "")
            Dim month As String = If(monthMatch.Success, monthMatch.Groups(2).Value, "")
            Dim day As String = If(dayMatch.Success, dayMatch.Groups(2).Value, "")

            If year <> "" AndAlso month <> "" AndAlso day <> "" Then
                If isValidDate(year, month, day) Then
                    Dim formattedDate = FormatDate(year, month, day)
                    AddUsed(formattedDate, yearMatch.Index, yearMatch.Length)
                    AddUsed(formattedDate, monthMatch.Index, monthMatch.Length)
                    AddUsed(formattedDate, dayMatch.Index, dayMatch.Length)
                    SetBaseDetection(DateBase, formattedDate, yearMatch.Index, yearMatch.Length, monthMatch.Index, monthMatch.Length)
                    Return formattedDate
                End If
            End If

            For Each key In MonthNames.Keys
                If text.Contains(key) = False Then Continue For

                monthPattern = "\b(M[MONTH]{0,4})[\s\b\n]{0,2}(" & key & ")"
                monthMatch = Regex.Match(text, monthPattern)
                'month = If(monthMatch.Success, monthMatch.Groups(2).Value, "")
                month = MonthNames(key)

                If year <> "" AndAlso month <> "" AndAlso day <> "" Then
                    If isValidDate(year, month, day) Then
                        Dim formattedDate = FormatDate(year, month, day)
                        AddUsed(formattedDate, yearMatch.Index, yearMatch.Length)
                        AddUsed(formattedDate, monthMatch.Index, monthMatch.Length)
                        AddUsed(formattedDate, dayMatch.Index, dayMatch.Length)
                        SetBaseDetection(DateBase, formattedDate, yearMatch.Index, yearMatch.Length, monthMatch.Index, monthMatch.Length)
                        Return formattedDate
                    End If
                End If
            Next

            Return ""
        End Function

        Private Shared Function JugementDateFormat(text As String) As DateFormats

            Dim pattern = "\b(?:YEAR|YYYY|YYY|YY|Y|MONTH|MON|MM|M|DAY|DD|D)\b"
            Dim matches = Regex.Matches(text, pattern)

            Dim s As String = ""
            For Each m As Match In matches
                Dim t = m.Value
                If Regex.IsMatch(t, "^(YEAR|YYYY|YYY|YY|Y)$") Then
                    s += "Y"
                ElseIf Regex.IsMatch(t, "^(MONTH|MON|MM|M)$") Then
                    s += "M"
                ElseIf Regex.IsMatch(t, "^(DAY|DD|D)$") Then
                    s += "D"
                End If
            Next

            Select Case s
                Case "YMD" : Return DateFormats.IsoFormat
                Case "DMY" : Return DateFormats.BritishFormat
                Case "MDY" : Return DateFormats.AmericanFormat
            End Select

            Return DateFormats.InvalidFormat
        End Function

        Private Shared Function JugementDateFormatFromEntity(Callsign) As DateFormats
            Dim DateFormat As DateFormats
            Dim result = LookupEntityyAndContinent(Callsign)
            Dim country = result.Entity
            Dim continent = result.Continent

            If country = "" Then
                DateFormat = DateFormats.InvalidFormat
            Else
                DateFormat = GetDateFormat(country, continent)      ' Enthityから日付のファーマっとを設定
            End If
            Return DateFormat

        End Function

        Private Shared Function ConverTo4degitsYear(yy As Integer) As Integer
            If yy < 50 Then
                Return yy + 2000
            ElseIf yy < 100 Then
                Return yy + 1900
            Else
                Return yy
            End If
        End Function

        Private Shared Function isValidDate(Year As String, Month As String, Day As String) As Boolean
            Dim y, m, d As Integer

            ' 数字（整数）かどうかを判定する場合
            If Not Integer.TryParse(Year, y) Then Return False
            If Not Integer.TryParse(Month, m) Then Return False
            If Not Integer.TryParse(Day, d) Then Return False
            Return isValidDate(y, m, d)
        End Function

        Private Shared Function isValidDate(Year As Integer, Month As Integer, Day As Integer) As Boolean

            If Year < 50 Then
                Year = Year + 2000
            ElseIf Year < 100 Then
                Year = Year + 1900
            End If
            ' DateSerialは自動で日付を補正するため、
            ' 元の数値と比較して妥当性を判定する必要がある
            Dim targetDate As Date
            Try
                targetDate = DateSerial(Year, Month, Day)
                ' 補正された結果、元の入力と異なる場合は不正とする
                If targetDate.Year = Year AndAlso targetDate.Month = Month AndAlso targetDate.Day = Day Then
                    If (targetDate < New Date(1952, 7, 29)) OrElse (targetDate > Date.Today) Then
                        Return False
                    Else
                        Return True
                    End If
                Else
                    Return False
                End If
            Catch ex As Exception
                Return False
            End Try
        End Function

        Private Shared Function FormatDate(year As String, month As String, day As String) As String
            ' スラッシュ(/)で連結
            If year = "0" OrElse month = "0" OrElse "0" Then Return ""

            If year.Length = 4 Then
                year = ConverTo4degitsYear(year)
            End If

            Dim dateString As String = $"{year:0000}/{month:MM}/{day:00}"

            ' Date型に変換
            Dim dt As DateTime = Date.Parse(dateString)

            Return dt.ToString("yyyy/MM/dd")
        End Function

        Private Shared Function CleanTextMisreadingDate(text As String) As String
            Dim s = text

            For Each keys In MisReadingMonth
                s = s.Replace(keys.Key, keys.Value)
            Next

            Return s
        End Function


        Private Shared Function CleanTextDate(text As String) As String
            ' OCR誤認識の補正
            Dim s = text


            s = s.Replace(vbLf, " ")

            ' s = s.Replace("'", " ")           ' 年の基準にするため,置き換えない

            s = s.Replace("|", " ")
            s = s.Replace("I", "1")     ' 月名にIはないので、1に変換
            s = s.Replace(",", " ")     ' 年月日なので、スラッシュに変換
            s = s.Replace(".", " ")
            s = s.Replace("[", " ")
            s = s.Replace("]", " ")

            s = s.Replace(";", "/")
            s = s.Replace("\", "/")
            s = s.Replace("(", " ")
            s = s.Replace(")", " ")
            s = s.Replace("{", " ")
            s = s.Replace("}", " ")
            s = s.Replace("_", " ")

            s = s.Replace("""", "'")　　　　' 年の省略記号
            s = s.Replace("’", "'")


            Return s
        End Function


        Private Shared Function CleanTextDateSymbol(text As String) As String
            Dim s As String

            s = text.Replace("‘", "'")
            s = text.Replace("””‘", "'")

            Return s
        End Function

        Private Shared Function CleanTextDateNumeric(text As String) As String
            ' 日付が数字のみで構成されているとき
            Dim s = text

            s = s.Replace("O", "0")
            s = s.Replace("I", "1")
            s = s.Replace("J", " ")     ' ]がJに誤認識されることがある
            s = s.Replace("L", "1")     ' Lの小文字

            Return s
        End Function

        Private Shared Function FixYear(y As String) As Integer
            Return FixYear(CInt("0" & y))
        End Function

        Private Shared Function FixYear(y As Integer) As Integer
            ' 年の補正（2桁 → 4桁）
            Dim yy = Integer.Parse(y)
            If yy >= 100 Then Return yy

            Dim currentYY = Integer.Parse(DateTime.Now.Year.ToString().Substring(2))

            If yy > currentYY Then
                Return 1900 + yy
            Else
                Return 2000 + yy
            End If
        End Function


        '＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
        '
        '       時間抽出
        '
        '＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊

        ' 時刻抽出のメイン関数
        Public Shared Function ExtractTime(text As String) As String

            Dim pattern As String
            Dim m As Match
            Dim mc As MatchCollection

            text = text.Replace(vbLf, " ")

            DetectionTimes.Clear()
            ClearBaseDetection(TimeBaseTitle)
            pattern = "(TIME|HHMM|JST|J0T|AST|UTC|T1ME|T1NE|UST)"       '　誤読を考慮
            m = Regex.Match(text, pattern)
            If m.Success = True Then
                Dim index As Integer = m.Index

                ' 行と列を計算するメソッドを呼び出す
                Dim row As Integer = 0
                Dim col As Integer = 0
                GetRowAndColumn(text, index, row, col)

                SetBaseDetection(TimeBaseTitle, m.Groups(0).Value, m.Index, m.Length, row, col)
            End If

            Dim cleaned = CleanTextTime(text)
            DetectionTimes.Clear()

            pattern = "\b(AT|TIME)?([-:\. ])? {0,3}([01]\d|2[0-3]|[1-9]) {0,3}([-:\.])? {0,3}([0-5]\d)(\s{0,2})?(JS|UT|Z|TI|AS)?"       ' TIはTIMEの一部,ASは誤読対策
            mc = Regex.Matches(cleaned, pattern)
            For Each m In mc
                If m.Success Then
                    If IsUsed(m.Index, m.Length) Then Continue For

                    If (m.Groups(1).Value <> "") OrElse (m.Groups(7).Value <> "") Then
                        Dim s = m.Groups(3).Value & ":" & m.Groups(5).Value
                        ' 時刻の妥当性チェック
                        If IsValidTime(m.Groups(3).Value, m.Groups(5).Value) Then
                            DetectionTimes.Add(New Detection With {.Value = s, .Index = m.Groups(0).Index, .Length = s.Length, .Row = 0, .Column = 0})
                        End If
                    End If
                End If
            Next



            ' 時刻パターン（例：09:00JST 9:00など）　厳密パターン1
            ' 文字区切りがあり、時と分の間は区切符号が1個 UTC,JSZ,Zの１文字がある
            pattern = "(\b)([01]\d|2[0-3]|[1-9]) {0,3}([-:\.]) {0,3}([0-5]\d)(\s){0,2}?(JS|UT|Z|TI|AS)"       ' TIはTIMEの一部,ASは誤読対策
            mc = Regex.Matches(cleaned, pattern)
            For Each m In mc
                If m.Success Then
                    If IsUsed(m.Index, m.Length) Then Continue For

                    Dim s = m.Groups(2).Value & ":" & m.Groups(4).Value
                    ' 時刻の妥当性チェック
                    If IsValidTime(m.Groups(2).Value, m.Groups(4).Value) Then
                        DetectionTimes.Add(New Detection With {.Value = s, .Index = m.Groups(0).Index, .Length = s.Length, .Row = 0, .Column = 0})
                    End If
                End If
            Next
            If DetectionTimes.Count = 1 Then        ' 厳密チェックで１つしか見つからない場合、それをそのまま答えとする
                AddUsed(DetectionTimes(0).Value, DetectionTimes(0).Index, DetectionTimes(0).Length)
                SetBaseDetection(TimeBase, DetectionTimes(0).Value, DetectionTimes(0).Index, DetectionTimes(0).Length, 0, 0)
                Return FormatTime(DetectionTimes(0).Value)
            End If

            ' 時刻パターン（例：09:00JST 9:00など）　厳密パターン2
            ' 文字区切りがあり、時と分の間は区切符号が1個 UTC,JSZ,Zの１文字がある
            pattern = "\b([01]\d|2[0-3]|[0-9]) {0,3}([-:\.]) {0,3}([0-5]\d)\s?[JUZTA]?\b"
            mc = Regex.Matches(cleaned, pattern)
            For Each m In mc
                Debug.Print($"({m.Value}, {m.Index}, {m.Length})")
                If m.Success Then
                    If IsUsed(m.Index, m.Length) Then Continue For

                    Dim before = text.Substring(m.Index - 1, 1).Trim        ' 時間前の空白もm.valueに含むため
                    If before >= "0" AndAlso before <= "9" Then Continue For

                    Dim after As String
                    If m.Groups(3).Index + m.Groups(3).Length < text.Length Then                      ' 分の後ろの空白もMATCHに含むので、Groups(3)で処理
                        after = text.Substring(m.Groups(3).Index + m.Groups(3).Length, 1).Trim        ' 時間後の空白もm.valueに含むため
                    Else
                        after = ""
                    End If
                    If after >= "0" AndAlso after <= "9" Then Continue For

                    If m.Groups(2).Value = before Then Continue For        ' 時間の区切り符号と前後の文字が同じなら、別な文字の可能性があるので除外する
                    If m.Groups(2).Value = after Then Continue For

                    Dim s = m.Groups(1).Value & ":" & m.Groups(3).Value
                    ' 時刻の妥当性チェック
                    If IsValidTime(m.Groups(1).Value, m.Groups(3).Value) Then
                        DetectionTimes.Add(New Detection With {.Value = s, .Index = m.Groups(0).Index, .Length = s.Length, .Row = 0, .Column = 0})
                    End If
                End If
            Next
            If DetectionTimes.Count = 1 Then        ' 厳密チェックで１つしか見つからない場合、それをそのまま答えとする
                AddUsed(DetectionTimes(0).Value, DetectionTimes(0).Index, DetectionTimes(0).Length)
                SetBaseDetection(TimeBase, DetectionTimes(0).Value, DetectionTimes(0).Index, DetectionTimes(0).Length, 0, 0)
                Return FormatTime(DetectionTimes(0).Value)
            End If

            ' 時刻パターン（例：09:00JST 9:00など）
            ' 文字区切りがあり、時と分の間は区切符号が0個から3個 UTC,JSZ,Zの0個又は１個がある
            pattern = "\b([01]\d|2[0-3]|[1-9]) {0,2}[- :;./]{0,3} {0,2}([0-5]\d)([ JUZMA])?\b"      ' 時と分の間は0個か1個 UTC,JSZ,Zが続く
            mc = Regex.Matches(cleaned, pattern)
            For Each m In mc
                'm = Regex.Match(text, pattern, RegexOptions.IgnoreCase)
                If m.Success Then

                    ' 時刻の妥当性チェック
                    If IsValidTime(m.Groups(1).Value, m.Groups(2).Value) Then
                        If IsUsed(m.Index, m.Length) Then Continue For
                        Dim s = m.Groups(1).Value & ":" & m.Groups(2).Value
                        s = FormatTime(s)
                        If Not IsUsed(m.Index, m.Length) Then
                            Dim before, after As String
                            If m.Groups(0).Index = 0 Then
                                before = ""
                            Else
                                before = text.Substring(m.Groups(0).Index - 1, 1).Trim
                            End If
                            If m.Groups(0).Index + m.Groups(0).Length >= text.Length Then
                                after = ""
                            Else
                                after = text.Substring(m.Groups(0).Index + m.Groups(0).Length, 1).Trim
                            End If
                            If Not (before <= "0" OrElse before >= "9") Then Continue For
                            If Not (after <= "0" OrElse after >= "9") Then Continue For

                            DetectionTimes.Add(New Detection With {.Value = s, .Index = m.Groups(0).Index, .Length = s.Length, .Row = 0, .Column = 0})
                        End If
                    End If
                End If
            Next


            ' 時刻パターン（例：0900 など）
            ' 文字区切りがあり、4個の数字 文字区切りがある
            pattern = "\b([01]\d|2[0-3])([0-5]\d)\b"     ' 数字4桁 HAMLOGの時間も
            mc = Regex.Matches(cleaned, pattern)
            For Each m In mc
                ' m = Regex.Match(text, pattern, RegexOptions.IgnoreCase)
                If m.Success Then
                    Dim s = m.Groups(1).Value & ":" & m.Groups(2).Value
                    ' 時刻の妥当性チェック
                    If IsValidTime(m.Groups(1).Value, m.Groups(2).Value) Then
                        If Not IsUsed(m.Index, m.Length) Then
                            DetectionTimes.Add(New Detection With {.Value = s, .Index = m.Groups(0).Index, .Length = s.Length, .Row = 0, .Column = 0})
                        End If
                    End If
                End If
            Next

            'If DetectionTimes.Count = 0 Then
            ' 時刻パターン（例：0900 など）
            ' 文字区切りなし　時刻数字4桁
            pattern = "([01]\d|2[0-3]) {0,2}[-:./]? {0,2}([0-5]\d)"     ' 数字4桁
            mc = Regex.Matches(cleaned, pattern)
            For Each m In mc
                ' m = Regex.Match(c leaned, pattern, RegexOptions.IgnoreCase)
                If m.Success Then
                        Dim s = m.Groups(1).Value & ":" & m.Groups(2).Value
                        ' 時刻の妥当性チェック
                        If IsValidTime(m.Groups(1).Value, m.Groups(2).Value) Then
                            If Not IsUsed(m.Index, m.Length) Then
                                DetectionTimes.Add(New Detection With {.Value = s, .Index = m.Groups(0).Index, .Length = s.Length, .Row = 0, .Column = 0})
                            End If
                        End If
                    End If
                Next
            'End If

            If DateBase.Value <> "" Then
                Dim bestAwnser As String() = ChooseBestDetection(DetectionTimes, DateBase, False)
                If bestAwnser(0) <> "" Then
                    AddUsed(bestAwnser(0), bestAwnser(1), bestAwnser(2))
                    SetBaseDetection(DateBase, bestAwnser(0), bestAwnser(1), bestAwnser(2), 0, 0)
                    Return FormatTime(bestAwnser(0))
                End If
            End If
            If TimeBaseTitle.Value <> "" Then
                Dim bestAwnser As String() = ChooseBestDetection(DetectionTimes, TimeBaseTitle, False)
                If bestAwnser(0) <> "" Then
                    AddUsed(bestAwnser(0), bestAwnser(1), bestAwnser(2))
                    SetBaseDetection(TimeBaseTitle, bestAwnser(0), bestAwnser(1), bestAwnser(2), 0, 0)
                    Return FormatTime(bestAwnser(0))
                End If
            End If
            If DateBaseTitle.Value <> "" Then
                Dim bestAwnser As String() = ChooseBestDetection(DetectionTimes, DateBaseTitle, False)
                If bestAwnser(0) <> "" Then
                    AddUsed(bestAwnser(0), bestAwnser(1), bestAwnser(2))
                    SetBaseDetection(DateBaseTitle, bestAwnser(0), bestAwnser(1), bestAwnser(2), 0, 0)
                    Return FormatTime(bestAwnser(0))
                End If
            End If
            If DetectionTimes.Count <> 0 Then
                Return DetectionTimes(0).Value
            End If

            Return ""
        End Function

        ' OCR誤認識の補正
        Private Shared Function CleanTextTime(text As String) As String
            Dim s = text
            ' よくある誤認識の補正
            s = s.Replace("|", " ")
            s = s.Replace("[", " ")
            s = s.Replace("]", " ")
            s = s.Replace("(", " ")
            s = s.Replace(")", " ")
            s = s.Replace("_", " ")

            s = s.Replace("=", ":")

            s = s.Replace(";", ":")
            s = s.Replace(".", ":")
            s = s.Replace(",", ":")

            's = s.Replace(" :", ":")
            's = s.Replace(": ", ":")

            s = s.Replace("O", "0")         ' 時刻の抽出なので英字に誤読されている可能性があるものを
            s = s.Replace("I", "1")
            s = s.Replace("L", "1")
            s = s.Replace("S", "8")

            s = s.Replace("J", " ")      ' ]がJに誤認識されることがある,これを空白に置換することで、時刻の抽出が可能になる場合がある

            Return s
        End Function

        Private Shared Function FormatTime(time As String) As String
            Dim i As Integer = time.IndexOf(":")
            If i < 0 Then i = time.IndexOf(" ")
            If i < 0 Then i = time.IndexOf("-")

            Dim h, n As String
            If i >= 0 Then
                h = time.Substring(0, i)
                n = time.Substring(i + 1, time.Length - i - 1)
            Else
                h = time.Substring(0, time.Length - 2)
                n = time.Substring(time.Length - 2, 2)
            End If

            ' コロン(:)で連結
            Dim timeString As String = $"{h:00}:{n:00}"
            ' 変換に成功したか判定
            Dim result As DateTime
            If DateTime.TryParse(timeString, result) Then
                Return result.ToString("HH:mm")
            Else
                Return ""
            End If
        End Function

        Private Shared Function IsValidTime(HH As String, DD As String) As Boolean
            Dim hour = Integer.Parse("0" & HH)
            Dim minute = Integer.Parse("0" & DD)

            ' 時刻の妥当性チェック
            If hour < 0 Or hour > 23 Then Return False
            If minute < 0 Or minute > 59 Then Return False
            Return True

        End Function



        '＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
        '
        '       モード抽出
        '
        '＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊

        Public Shared Function ExtractMode(text As String) As String

            text = text.Replace(vbLf, " ")
            Dim cleaned = CleanTextMode(text)

            isPhone = False

            Dim pattern As String
            Dim idx As Integer
            Dim m As Match

            Dim bestMode As String = ""
            Dim IndexOfBestMode As Integer = 0
            Dim lengthOfBestMode As Integer = 0
            Dim shortestDistance As Integer = Integer.MaxValue

            ClearBaseDetection(ModeBase)
            pattern = "\s(MODE|2WAY|2-WAY)[:;\s]"
            m = Regex.Match(cleaned, pattern, RegexOptions.IgnoreCase)
            If m.Success = True Then
                Dim index As Integer = m.Index

                ' 行と列を計算するメソッドを呼び出す
                Dim row As Integer = 0
                Dim col As Integer = 0
                GetRowAndColumn(cleaned, index, row, col)
                SetBaseDetection(ModeBaseTitle, m.Groups(1).Value, m.Groups(1).Index, m.Groups(1).Length, row, col)  ' Modeが複数見つかったときのため "2way"or"Mode"を基準にする
            End If

            DetectionModes.Clear()

            ' 1, まずそのまま、チェックする
            Debug.Print($"Checking for mode: {Modes(1)}")
            For Each key In Modes
                pattern = "\b(" & key & ")_?\b"                    ' Yは2WayのY、EはModeのEと誤認識する可能性があるため、前に:;YE\sを追加
                Dim mdx = Regex.Matches(text, pattern)
                For Each m In mdx
                    If m.Success = True Then
                        If IsUsed(m.Groups(1).Index, m.Groups(1).Length) Then Continue For
                        DetectionModes.Add(New Detection With {.Value = m.Groups(1).Value, .Index = m.Groups(1).Index, .Length = m.Groups(1).Length, .Row = 0, .Column = 0})
                    End If
                Next
            Next

            ' 2, エイリアスモードが含まれているかチェック
            For Each keys In AliasModes
                pattern = "[:;\s](" & keys.Key & ")(?=\s|\b|$)"
                Dim mdx = Regex.Matches(text, pattern)
                For Each m In mdx
                    If m.Success = True Then
                        If IsUsed(m.Index, m.Length) Then Continue For
                        Dim Index As Integer = m.Groups(1).Index            ' 前後の空白分を除外して登録
                        Dim Length As Integer = m.Groups(1).Length
                        DetectionModes.Add(New Detection With {.Value = keys.Value, .Index = m.Groups(1).Index, .Length = m.Groups(1).Length, .Row = 0, .Column = 0})
                    End If
                Next
            Next

            ' 3, 誤読辞書に含まれているかチェック
            For Each keys In MisReadingModes
                '                pattern = "[:;\s](" & keys.Key & ")(?=\s|\b|$)"
                pattern = "[:;\s](" & keys.Key & ")([$|\s|\b|\n])"
                m = Regex.Match(text, pattern)
                If m.Success = True Then
                    If IsUsed(m.Index, m.Length) Then Continue For
                    Dim index = m.Groups(1).Index
                    Dim length = m.Groups(1).Length
                    'UsedRanges.Add((index, length))
                    Dim row = text.LastIndexOf(ControlChars.Lf, index)
                    Dim column = index - text.LastIndexOf(ControlChars.Lf, index)

                    DetectionModes.Add(New Detection With {.Value = keys.Value, .Index = m.Groups(1).Index, .Length = m.Groups(1).Length, .Row = 0, .Column = 0})
                    'Return keys.Value
                End If
            Next

            ' 4. 誤読したモードで探す(Levenshtein distanceで判断)
            pattern = "[:;\s]([A-Z0-9]{4,8})(?=\s|\b|$)"
            Dim md = Regex.Matches(text, pattern)
            For Each key In Modes
                If key.Length <= 3 Then Continue For

                For Each m In md
                    Dim d As Double = Similarity(key, m.Value)
                    If d > 0.74 Then
                        idx = m.Groups(1).Index
                        If idx >= 0 Then
                            Dim Index = m.Groups(1).Index
                            Dim length = m.Groups(1).Length
                            'UsedRanges.Add((Index, length))
                            Dim row = text.LastIndexOf(ControlChars.Lf, Index)
                            Dim column = Index - text.LastIndexOf(ControlChars.Lf, Index)
                            DetectionModes.Add(New Detection With {.Value = key, .Index = m.Groups(1).Index, .Length = m.Groups(1).Length, .Row = 0, .Column = 0})
                        End If
                    End If
                Next
            Next

            ' 6. Modeの前後いずれかが空白を救う
            For Each key In Modes           ' Modesの中で、誤認識されやすいものを抽出する
                pattern = "(^|\s)(" & key & "|" & key & ") (/=\s|\d|$)"
                m = Regex.Match(text, pattern)
                If m.Success = True Then
                    If IsUsed(m.Index, m.Length) Then Continue For
                    If ModeBase.Index <> 0 AndAlso ((m.Index < ModeBase.Index) OrElse (m.Index > ModeBase.Index + 40)) Then Continue For
                    'UsedRanges.Add((m.Index, m.Length))
                    Dim Index = m.Groups(2).Index
                    Dim Length = m.Groups(2).Length
                    Dim row = text.LastIndexOf(ControlChars.Lf, Index)
                    Dim column = Index - text.LastIndexOf(ControlChars.Lf, Index)
                    DetectionModes.Add(New Detection With {.Value = m.Groups(2).Value, .Index = m.Groups(2).Index, .Length = m.Groups(2).Length, .Row = row, .Column = column})


                End If
            Next

            ' 7, 誤読辞書に含まれているかチェック　前後の文字を無視
            For Each keys In MisReadingModes
                pattern = "(" & keys.Key & ")"
                m = Regex.Match(cleaned, pattern)
                If m.Success = True Then
                    If IsUsed(m.Index, m.Length) Then Continue For
                    If ModeBaseTitle.Index <> 0 AndAlso ((m.Index < ModeBaseTitle.Index) OrElse (m.Index > ModeBaseTitle.Index + 100)) Then Continue For

                    Dim Index = m.Groups(1).Index
                    Dim Length = m.Groups(1).Length

                    Dim row = text.LastIndexOf(ControlChars.Lf, m.Index)
                    Dim column = m.Index - text.LastIndexOf(ControlChars.Lf, m.Index)
                    DetectionModes.Add(New Detection With {.Value = keys.Value, .Index = m.Groups(1).Index, .Length = m.Groups(1).Length, .Row = row, .Column = column})
                End If
            Next

            If DetectionModes.Count = 0 Then                                            ' Modeが検知されないとき、Keyの前後を無視してkeyで検索
                If (DetectionDates.Count <> 0) OrElse (DetectionTimes.Count <> 0) Then  ' ただし、日付、時間が検知されているときのみ、Modeを検知する
                    For Each key In Modes
                        pattern = "(\s" & key & "|" & key & "\s)"     ' Keyのどちらかが空白の場合 
                        Dim mdx = Regex.Matches(cleaned, pattern)
                        For Each m In mdx
                            If m.Success = True Then
                                If IsUsed(m.Index, m.Length) Then Continue For
                                DetectionModes.Add(New Detection With {.Value = m.Groups(0).Value, .Index = m.Groups(0).Index, .Length = m.Groups(0).Length, .Row = 0, .Column = 0})
                            End If
                        Next
                    Next
                End If
            End If

            If TimeBase.Value <> "" Then
                Dim bestAwnser As String() = ChooseBestDetection(DetectionModes, TimeBase, False)
                If bestAwnser(0) <> "" Then
                    UsedRanges.Add((bestAwnser(0), bestAwnser(1), bestAwnser(2)))
                    SetBaseDetection(ModeBase, bestAwnser(0), bestAwnser(1), bestAwnser(2), 0, 0)
                    Return bestAwnser(0)
                End If
            End If
            'If ModeBaseTitle.Value <> "" Then
            '    Dim bestAwnser As String() = ChooseBestDetection(DetectionModes, ModeBaseTitle, True)
            '    If bestAwnser(0) <> "" Then
            '        UsedRanges.Add((bestAwnser(0), bestAwnser(1), bestAwnser(2)))
            '        SetBaseDetection(ModeBase, bestAwnser(0), bestAwnser(1), bestAwnser(2), 0, 0)
            '        Return bestAwnser(0)
            '    End If
            'End If

            If DateBase.Value <> "" Then
                Dim bestAwnser As String() = ChooseBestDetection(DetectionModes, DateBase, False)
                If bestAwnser(0) <> "" Then
                    UsedRanges.Add((bestAwnser(0), bestAwnser(1), bestAwnser(2)))
                    SetBaseDetection(ModeBase, bestAwnser(0), bestAwnser(1), bestAwnser(2), 0, 0)
                    Return bestAwnser(0)
                End If
            End If

            If ModeBaseTitle.Value <> "" Then
                Dim bestAwnser As String() = ChooseBestDetection(DetectionModes, ModeBaseTitle, False)
                If bestAwnser(0) <> "" Then
                    UsedRanges.Add((bestAwnser(0), bestAwnser(1), bestAwnser(2)))
                    SetBaseDetection(ModeBase, bestAwnser(0), bestAwnser(1), bestAwnser(2), 0, 0)
                    Return bestAwnser(0)
                End If
            End If

            If TimeBaseTitle.Value <> "" Then
                Dim bestAwnser As String() = ChooseBestDetection(DetectionModes, TimeBaseTitle, False)
                If bestAwnser(0) <> "" Then
                    UsedRanges.Add((bestAwnser(0), bestAwnser(1), bestAwnser(2)))
                    SetBaseDetection(ModeBase, bestAwnser(0), bestAwnser(1), bestAwnser(2), 0, 0)
                    Return bestAwnser(0)
                End If
            End If
            If DetectionModes.Count <> 0 Then
                Return DetectionModes(0).Value
            End If

            ' 5. 見つからない場合、signalレポートからMode:を推測する 最初デジタルのレポート探す
            pattern = "[\s\b](SIGS?|DB)*[ :;]*([\+\-][0-5]\d)(DB)*[\s\b]"  '±50dBならデジタルする
            Dim ms = Regex.Matches(cleaned, pattern)
            For Each m In ms
                Dim sr As String = m.Groups(2).Value

                If Strings.Left(sr, 1) = "-" OrElse Strings.Left(sr, 1) = "+" Then
                    If IsNumeric(Strings.Mid(sr, 2)) Then
                        If qsoDate > "2017/07/31" Then
                            Return "FT8"
                        Else
                            Return "JT65"
                        End If
                    End If
                End If
            Next
            'End If

            '次にSB,CWなどレポートを探す
            pattern = "[\s\b](SIGS?|DB)*[ :;]*([3-5][5-9]|[3-5][5-9]{2})(DB)*[\s\b]"  '35から59ならフォン、355から599ならCWと推測する
            ms = Regex.Matches(cleaned, pattern)
            For Each m In ms
                Dim sr As String = m.Groups(2).Value
                If IsNumeric(sr) Then
                    If sr.Length = 2 Then
                        Return "SSB"
                        isPhone = True
                    ElseIf sr.Length = 3 Then
                        Return "CW"
                    End If
                End If
            Next

            Return ""
        End Function


        Private Shared Function CleanTextMode(text As String) As String
            Dim s = text
            ' よくある誤認識の補正
            s = s.Replace("|", " ")
            s = s.Replace("[", " ")
            s = s.Replace("]", " ")
            s = s.Replace("'", " ")

            s = s.Replace(";", " ")
            s = s.Replace(".", " ")
            s = s.Replace(",", " ")

            Return s
        End Function



        Private Shared Sub GetRowAndColumn(ByVal text As String, ByVal index As Integer, ByRef row As Integer, ByRef col As Integer)
            ' マッチ箇所までの文字列を取得
            Dim substring As String = text.Substring(0, index)

            ' 改行コード (\r\n または \r または \n) で分割して行数を特定
            Dim lines As String() = substring.Split(New Char() {ControlChars.Cr, ControlChars.Lf}, StringSplitOptions.None)

            ' 行番号 (1始まり)
            row = lines.Length

            ' 列番号 (1始まり)
            ' 最後の行の長さに1を加える
            col = lines(lines.Length - 1).Length + 1
        End Sub

        Private Shared Sub GetModeBase(text As String, m As Match)
            Dim index As Integer = m.Index

            '' 行と列を計算するメソッドを呼び出す
            Dim row As Integer = 0
            Dim col As Integer = 0
            GetRowAndColumn(text, index, row, col)

            SetModeBase("MODE", m.Groups(1).Index, m.Groups(1).Length, row, col)        ' Modeが複数見つかったときのため "2way"or"Mode"を基準にする
        End Sub
        Private Shared Sub AddModeBase(val As String, idx As Integer, Len As Integer, row As Integer, column As Integer)
            Dim md As Detection
            With md
                .Value = val
                .Index = idx
                .Length = Len
                .Row = row
                .Column = column
            End With
            DetectionModes.Add(md)
        End Sub

        Private Shared Sub SetModeBase(val As String, idx As Integer, Len As Integer, row As Integer, column As Integer)
            With ModeBase
                .Value = val
                .Index = idx
                .Length = Len
                .Row = row
                .Column = column
            End With
        End Sub



        '＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
        '
        '       Band 抽出関数
        '
        '＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊

        ' 周波数抽出（例：7.074）
        ' 周波数 → バンド変換のメイン関数
        Public Shared Function ExtractBand(text As String) As String
            Dim pattern As String
            Dim band As String
            Dim Bandformat As BandFormats

            text = text.Replace(vbLf, " ")
            text = text.Replace(",", ".")
            Dim cleaned = CleanTextBandCommon(text)

            Bandformat = BandFormats.Unknown

            SetBandBase("", 0, 0, 0, 0)      ' BandBaseを初期化する)
            DetectionBands.Clear()
            ClearBaseDetection(BandBaseTitle)
            pattern = "(BAND|FREQ|MHZ|FREQUENCY|QRG|MC)"
            Dim ms = Regex.Matches(text, pattern)
            For Each m As Match In ms
                If m.Success = True Then
                    Dim b = m.Groups(1).Value
                    Dim index As Integer = m.Index
                    Dim before As String = ""
                    before = text.Substring(Math.Max(0, index - 1), 1).Trim
                    If before >= "0" AndAlso before <= "9" Then Continue For

                    If (b = "FREQ") OrElse (b = "MHZ") OrElse (b = "FREQUENCY") OrElse (b = "QRG") OrElse (b = "MC") Then        ' 面倒なのでMHzに統一
                        Bandformat = BandFormats.MHz
                    End If

                    ' 行と列を計算するメソッドを呼び出す
                    Dim row As Integer = 0
                    Dim col As Integer = 0
                    GetRowAndColumn(text, index, row, col)
                    'AddBandBase(b, index, m.Length, row, col)
                    SetBaseDetection(BandBaseTitle, m.Groups(1).Value, m.Groups(1).Index, m.Groups(1).Length, row, col)
                    Exit For
                End If
            Next

            ' サテライト通信かどうか？
            band = ExtractSat(text)
            If band <> "" Then Return band

            band = ExtractDirectBand(text)
            If band <> "" Then
                Return band
            End If

            ' 0. 周波数の直接表記があるか探す 7.123MHZ, 145.04Mhz、136KHZ, 2.4GHZなど 周波数単位がある
            band = ExtractDirectFrequency(text)
            If band <> "" Then
                Return band
            End If

            ' 1. 波長表示の Band 表記があるか探す           ' 40M, 2M 等の波長表示
            'If Bandformat <> BandFormats.MHz Then    ' MHz表記の時は波長表記の処理を飛ばす→波長表示でもMHZ記入がある場合があるので、波長表示の処理を飛ばすと、Bandが検知されないことがある
            band = ExtractBandDirect(text)
            If band <> "" Then
                Return band
            End If

            ' 2. 周波数を求め、Bandにする        ' 周波数抽出（例：7.074）
            band = ExtractFrequency(text)
            'If band <> "" Then
            '    Return band
            'End If

            ' 3. 周波数の誤読補正をし、周波数を求め,Bandにする        ' 周波数抽出（例：7.074）
            If DetectionBands.Count = 0 Then
                '   If band = "" Then
                band = ExtractMisreadFrequency(text)
            End If

            If DetectionBands.Count = 0 Then
                'If band = "" Then
                band = ExtractMisreadBand(text)         ' 3.5を35と誤認識することがあるので、周波数抽出の前に、誤認識のBand表記を先に抽出する
            End If

            If ModeBase.Value <> "" Then
                Dim bestAwnser As String() = ChooseBestDetection(DetectionBands, ModeBase, True)
                If bestAwnser(0) <> "" Then
                    UsedRanges.Add((bestAwnser(0), bestAwnser(1), bestAwnser(2)))
                    Return bestAwnser(0)
                End If
            End If

            If ModeBase.Value <> "" Then
                Dim bestAwnser As String() = ChooseBestDetection(DetectionBands, ModeBase, False)
                If bestAwnser(0) <> "" Then
                    UsedRanges.Add((bestAwnser(0), bestAwnser(1), bestAwnser(2)))
                    Return bestAwnser(0)
                End If
            End If

            If TimeBase.Value <> "" Then
                Dim bestAwnser As String() = ChooseBestDetection(DetectionBands, TimeBase, False)
                If bestAwnser(0) <> "" Then
                    UsedRanges.Add((bestAwnser(0), bestAwnser(1), bestAwnser(2)))
                    Return bestAwnser(0)
                End If
            End If

            If BandBaseTitle.Value <> "" Then
                Dim bestAwnser As String() = ChooseBestDetection(DetectionBands, BandBaseTitle, False)
                If bestAwnser(0) <> "" Then
                    UsedRanges.Add((bestAwnser(0), bestAwnser(1), bestAwnser(2)))
                    Return bestAwnser(0)
                End If
            End If

            If DateBase.Value <> "" Then
                Dim bestAwnser As String() = ChooseBestDetection(DetectionBands, DateBase, False)
                If bestAwnser(0) <> "" Then
                    UsedRanges.Add((bestAwnser(0), bestAwnser(1), bestAwnser(2)))
                    Return bestAwnser(0)
                End If
            End If

            Dim bestBand As String = ""
            Dim IndexOfBestFreq As Integer
            Dim lengthOfBestFreq As Integer
            If DetectionBands.Count = 1 Then
                bestBand = DetectionBands(0).Value
                IndexOfBestFreq = DetectionBands(0).Index
                lengthOfBestFreq = DetectionBands(0).Length
            End If

            If bestBand <> "" Then
                UsedRanges.Add((bestBand, IndexOfBestFreq, lengthOfBestFreq))
                Return bestBand
            End If


            Return ""
        End Function

        Private Shared Function ExtractSat(text As String) As String
            Dim m As Match

            Dim p1 = "(14[45]|43[035]|1200)"
            Dim pattern = "\b" & p1 & "[/|]" & p1
            m = Regex.Match(text, pattern)
            If m.Success Then
                Return "SAT"
            End If
            Return ""
        End Function

        Private Shared Function ExtractDirectBand(text As String) As String

            text = CleanTextFreq(text)

            Dim pattern As String
            Dim m As Match
            Dim bn As String
            Dim unit As String = ""

            pattern = "\bBAND( {0,2})(\d+(\.\d+)?)( ?)([M:CM])\b"                   ' eQSlにある”Band: 16０M”　のパターン Bandのあとの：はCleanTextFreqで空白に置き換えてある
            For Each m In Regex.Matches(text, pattern)
                If IsUsed(m.Index, m.Length) Then Continue For

                Dim f As Double
                If Not Double.TryParse(m.Groups(2).Value, f) Then Continue For

                Dim s As String = f.ToString() & m.Groups(5).Value
                If Array.IndexOf(Bands, s) < 0 Then Continue For     ' Bandのリストにない

                ' 行とカラムを計算するメソッドを呼び出す
                Dim row As Integer = 0
                Dim col As Integer = 0
                GetRowAndColumn(text, m.Index, row, col)
                DetectionBands.Add(New Detection With {.Value = s, .Index = m.Index, .Length = m.Length, .Row = row, .Column = col})
            Next

            If DetectionBands.Count = 1 Then    ' 完全系が1個なので、そのまま返す
                Return DetectionBands(0).Value
            End If

            Return ""
        End Function

        Private Shared Function ExtractDirectFrequency(text As String) As String

            text = CleanTextFreq(text)

            Dim pattern As String
            Dim m As Match
            Dim bn As String
            Dim unit As String = ""

            pattern = "\b(\d+(\.\d+)?) ?([KMG](HZ|C))"                   ' 28.152MHZ など完全系であること
            For Each m In Regex.Matches(text, pattern)
                If IsUsed(m.Index, m.Length) Then Continue For
                If Math.Abs(m.Index - BandBaseTitle.Index) > 200 Then Continue For

                Dim f As Double
                unit = m.Groups(3).Value
                If Not Double.TryParse(m.Groups(1).Value, f) Then Continue For

                If Math.Abs(m.Index - BandBaseTitle.Index) > 100 Then Continue For
                If unit = "KHZ" Then
                    f = f / 1000
                ElseIf unit = "GHZ" Then
                    f = f * 1000
                End If
                bn = ConvertFreqToBand(f)
                If bn = "" Then Continue For

                'Return bn          

                ' 行とカラムを計算するメソッドを呼び出す
                Dim row As Integer = 0
                Dim col As Integer = 0
                GetRowAndColumn(text, m.Index, row, col)
                DetectionBands.Add(New Detection With {.Value = bn, .Index = m.Index, .Length = m.Length, .Row = row, .Column = col})
            Next

            If DetectionBands.Count = 1 Then    ' 完全系が1個なので、そのまま返す
                Return DetectionBands(0).Value
            End If

            Return ""
        End Function


        ' Band 表記を直接抽出
        Private Shared Function ExtractBandDirect(text As String) As String
            ' 1. 波長表示の Band 表記があるか探す           ' 40M, 2M 等の波長表示

            Dim upper = CleanTextBand(text)

            For Each b In BandList.Bands        ' 40M, 2M 等の波長表示
                If b = "SAT" Then Continue For
                If upper.Contains(b) Then
                    '                    Dim pattern = "[^:;\(\s](" & b & ")[$\)\s\b]"
                    Dim pattern = "\b" & b & "\b"
                    Dim ms = Regex.Matches(upper, pattern)
                    For Each m As Match In ms
                        If IsUsed(m.Index, m.Length) Then
                            Continue For
                        End If

                        Dim before, after As String
                        If m.Groups(0).Index = 0 Then
                            before = ""
                        Else
                            before = upper.Substring(m.Groups(0).Index - 1, 1).Trim
                        End If

                        If m.Groups(0).Index + m.Groups(0).Length >= upper.Length Then
                            after = ""
                        Else
                            after = upper.Substring(m.Groups(0).Index + m.Groups(0).Length, 1).Trim
                        End If

                        If Not (before < "0" OrElse before = "9") Then Continue For
                        If Not (after < "0" OrElse after = "9") Then Continue For

                        DetectionBands.Add(New Detection With {.Value = b, .Index = m.Index, .Length = m.Length, .Row = 0, .Column = 0})
                    Next
                End If
            Next

            If DetectionBands.Count = 1 Then    ' 完全系が1個なので、そのまま返す
                Return DetectionBands(0).Value
            End If

            Return ""
        End Function

        Private Shared Function ExtractFrequency(text As String) As String

            text = CleanTextFreq(text)

            'DetectionBands.Clear()
            Dim bestBand As String = ""
            Dim IndexOfBestFreq As Integer = 0
            Dim lengthOfBestFreq As Integer = 0
            Dim shortestDistance As Integer = Integer.MaxValue
            Dim pattern As String
            Dim m As Match
            Dim bn As String

            text = text.Replace("BAND", "    ")

            Dim unit As String = ""
            pattern = "(\s|\d)(MHZ|KHZ|GHZ)[:;\s]"
            m = Regex.Match(text, pattern, RegexOptions.IgnoreCase)
            If m.Success = True Then
                unit = m.Groups(2).Value
            End If

            'text = text.Normalize(NormalizationForm.FormKC)
            'text = Regex.Replace(text, "[\u0300-\u036F\u2000-\u200F]", "")

            pattern = "(\d+(\.\d+)?)"                   ' とりあえず数字のみで検出、あとで前後の文字で判定

            ' 直前が空白であり、直後が数字以外であること 整数部7桁、小数部6桁
            ' [+-]はデジタルのレポートを除くため  

            For Each m In Regex.Matches(text, pattern)
                Debug.Print(m.Value)
                If IsUsed(m.Index, m.Length) Then Continue For

                ' 前の1文字（行頭なら BOF）
                Dim before As String
                If m.Index > 0 Then
                    before = text(m.Index - 1)
                    before = before.Trim
                Else
                    before = "BOF"   ' 行頭の印
                End If

                ' 後ろの1文字（行末なら EOF）
                Dim after As String
                If m.Index + m.Length < text.Length Then
                    after = text(m.Index + m.Length)
                    after = after.Trim
                Else
                    after = "EOF"    ' 行末の印
                End If

                If m.Groups(1).Value = "0" Then Continue For

                Debug.Print($" before: {before} value: {m.Groups(1).Value} after: {after} * Index: {m.Groups(1).Index}")

                If before = "-" OrElse before = "+" Then Continue For       ' 数字の直前が空白以外は除く、デジタルのレポート-10などを除く
                If before <> "" AndAlso after <> "" Then Continue For       ' 数字の直前,直後両方が空白以外は除く、
                If before >= "0" AndAlso before <= "9" Then Continue For    ' 前も数字な対象外
                If after >= "0" AndAlso after <= "9" Then Continue For      ' 後ろが数字な対象外

                Dim v3 = after.Trim
                If v3 = "W" Then Continue For                               ' 2Way,50Wの表記などを除く

                'If Not ((v3.Trim = "") OrElse (v3.Trim <> "M") OrElse (v3.Trim <> "K") OrElse (v3.Trim <> "G")) Then Continue For      ' 

                If Not (before = "" OrElse after = "" OrElse after = "M") Then Continue For

                Dim Zone As String = ""                                     ' 15文字以内ITU & CQ Zoneがあれば除く  
                If m.Groups(1).Index > 15 Then
                    Zone = text.Substring(m.Groups(1).Index - 15, 15)
                    If Zone.Contains("ZONE") OrElse Zone.Contains("Z0NE") Then Continue For
                End If

                Dim b = m.Groups(1).Value　　　　' 全桁（整数部＋小数部）を取得
                Dim d = m.Groups(2).Value        ' 少数部のみ

                Dim f As Double
                If Not Double.TryParse(m.Groups(1).Value, f) Then Continue For  ' 実数に変換できなければ除く

                If unit = "KHZ" Then
                    f = f / 1000
                ElseIf unit = "GHZ" Then
                    f = f * 1000
                End If

                bn = ConvertFreqToBand(f)
                If bn = "" AndAlso b.Length > 1 AndAlso d = "" Then           ' Band に変換できなくて、小数点以下がない場合
                    Dim i = b.Length
                    For i = b.Length To 2 Step -1
                        Dim subFreq = b.Substring(0, i - 1) & "." & b.Substring(i - 1)
                        If Double.TryParse(subFreq, f) Then
                            bn = ConvertFreqToBand(f)
                            If bn <> "" Then Exit For
                        End If
                    Next ' 周波数範囲以外は除く

                End If
                If bn = "" Then Continue For

                ' 行とカラムを計算するメソッドを呼び出す
                Dim row As Integer = 0
                Dim col As Integer = 0
                GetRowAndColumn(text, m.Index, row, col)
                DetectionBands.Add(New Detection With {.Value = bn, .Index = m.Index, .Length = m.Length, .Row = row, .Column = col})
            Next

            If DetectionBands.Count = 1 Then    ' 完全系が1個なので、そのまま返す
                Return DetectionBands(0).Value
            End If

            Return ""
        End Function

        Private Shared Function ExtractMisreadFrequency(text As String) As String

            Dim cleaned = CleanTextFreq(text)       ' OCR誤認識の補正

            Return ExtractFrequency(cleaned)

        End Function

        Private Shared Function ExtractMisreadBand(text As String) As String

            Dim cleaned = text

            For Each d In MisReadingBands
                If cleaned.Contains(d.Key) Then
                    If Not IsUsed(cleaned.IndexOf(d.Key), d.Key.Length) Then
                        Dim idx = cleaned.IndexOf(d.Key)
                        Dim len = d.Key.Length
                        ' 行とカラムを計算するメソッドを呼び出す
                        Dim row As Integer = 0
                        Dim col As Integer = 0
                        GetRowAndColumn(text, idx, row, col)
                        DetectionBands.Add(New Detection With {.Value = d.Value, .Index = idx, .Length = len, .Row = row, .Column = col})
                        'UsedRanges.Add((d.Value, cleaned.IndexOf(d.Key), d.Key.Length))
                        'Return d.Value
                    End If
                End If
            Next

            Return ""
        End Function

        Private Shared Function ConvertTypicalFreqToBand(freq As String) As String
            ' 周波数(文字列) → バンド変換
            Dim f As Double
            If Double.TryParse(freq, f) Then

            Else
                Return ""
            End If

            Return ConvertTypicalFreqToBand(f)
        End Function

        Private Shared Function ConvertTypicalFreqToBand(freq As Double) As String
            ' 周波数(数値) → バンド変換
            For Each b In FreqTable
                If freq = b.Low Then
                    Return b.Name
                End If
            Next

            Return ""
        End Function

        Public Shared Function ConvertFreqToBand(freq As String) As String          ' MainFormでも使用するのでPublicにする
            ' 周波数(文字列) → バンド変換
            Dim f As Double
            If Double.TryParse(freq, f) Then

            Else
                Return ""
            End If

            Return ConvertFreqToBand(f)
        End Function

        Public Shared Function ConvertFreqToBand(freq As Double) As String
            ' 周波数(数値) → バンド変換
            For Each b In FreqTable
                If freq >= b.Low AndAlso freq <= b.High Then
                    Return b.Name
                End If
            Next

            Return ""
        End Function


        ' OCR誤認識の補正
        Private Shared Function CleanTextBandCommon(text As String) As String
            Dim s = text

            s = s.Replace("FREA", "FREQ")
            s = s.Replace("HHZ", "MHZ")
            Return s
        End Function

        Private Shared Function CleanTextFreq(text As String) As String
            Dim s = text

            s = s.Replace("(", " ")
            s = s.Replace("@", "0")

            s = s.Replace("O", "0")
            's = s.Replace("I", "1")
            's = s.Replace("L", "1")
            's = s.Replace("S", "7")
            's = s.Replace("T", "7")
            s = s.Replace(",", ".")
            '          s = s.Replace("/", ".")
            's = s.Replace("-", " ")
            s = s.Replace("¥", "7")
            s = s.Replace(":", " ")

            s = s.Replace(".", ".")
            '         s = s.Replace("9", "0")     ' 9の誤認識が多いので0に変換する 周波数に"9"はほぼない　TESSERACTに学習を追加するしかない？

            Return s
        End Function

        ' OCR誤認識の補正
        Private Shared Function CleanTextBand(text As String) As String
            Dim s = text

            s = s.Replace("O", "0")
            s = s.Replace("I", "1")
            s = s.Replace("L", "1")
            s = s.Replace("N", "M")
            s = s.Replace(",", ".")
            Return s
        End Function


        Public Shared Sub SetBaseDetection(ByRef Base As Detection, val As String, idx As Integer, Len As Integer, row As Integer, column As Integer)
            With Base
                .Value = val
                .Index = idx
                .Length = Len
                .Row = row
                .Column = column
            End With
        End Sub

        Public Shared Sub ClearBaseDetection(ByRef Base As Detection)
            With Base
                .Value = ""
                .Index = 0
                .Length = 0
                .Row = 0
                .Column = 0
            End With
        End Sub

        Public Shared Sub SetDateBaseDetection(val As String, idx As Integer, Len As Integer, row As Integer, column As Integer)
            With DateBaseTitle
                .Value = val
                .Index = idx
                .Length = Len
                .Row = row
                .Column = column
            End With
        End Sub

        Public Shared Sub AddBaseDetections(Base As Detection, val As String, idx As Integer, Len As Integer, row As Integer, column As Integer)
            With Base
                .Value = val
                .Index = idx
                .Length = Len
                .Row = row
                .Column = column
            End With
            DetectionBands.Add(Base)
        End Sub

        Public Shared Function ChooseBestDetection(ByVal detects As List(Of Detection), detect As Detection, front As Boolean) As String()
            ' FronがTrueなら前にあり、Falseなら後にある
            If detect.Value = "" Then
                Return New String() {"", "", ""}
            End If
            If detects.Count = 0 Then
                Return New String() {"", "", ""}
            End If

            Debug.Print(detect.Value & " Index=" & detect.Index & " Length=" & detect.Length)
            For Each d In detects
                Debug.Print(d.Value & " Index=" & d.Index & " Length=" & d.Length)
            Next
            Debug.Print(front.ToString)

            Dim bestValue As String = ""
            Dim IndexOfBestValue As Integer = 0
            Dim lengthOfBestValue As Integer = 0
            Dim shortestDistance As Integer = Integer.MaxValue
            For Each f In detects
                If IsUsed(f.Index, f.Length) Then Continue For
                With detect
                    Dim s As Integer = Math.Abs(.Index - f.Index)  ' 演算子のOverloadがわからないので
                    If front AndAlso f.Index < .Index Then
                        If (s < shortestDistance) AndAlso (s < 200) Then
                            'If (s < shortestDistance) Then
                            bestValue = f.Value
                            IndexOfBestValue = f.Index
                            lengthOfBestValue = f.Length
                            shortestDistance = s
                        End If
                    ElseIf Not front AndAlso f.Index > .Index Then
                        If (s < shortestDistance) AndAlso (s < 200) Then
                            bestValue = f.Value
                            IndexOfBestValue = f.Index
                            lengthOfBestValue = f.Length
                            shortestDistance = s
                        End If
                    End If
                End With
            Next
            If bestValue <> "" Then
                Return New String() {bestValue, IndexOfBestValue.ToString(), lengthOfBestValue.ToString()}
            End If
            Return New String() {"", "", ""}

        End Function


        Public Shared Sub AddBandBase(val As String, idx As Integer, Len As Integer, row As Integer, column As Integer)
            With BandBase
                .Value = val
                .Index = idx
                .Length = Len
                .Row = row
                .Column = column
            End With
            DetectionBands.Add(BandBase)
        End Sub

        Public Shared Sub SetBandBase(val As String, idx As Integer, Len As Integer, row As Integer, column As Integer)
            With BandBase
                .Value = val
                .Index = idx
                .Length = Len
                .Row = row
                .Column = column
            End With
        End Sub

    End Class

    Private Shared Sub AddUsed(value As String, start As Integer, length As Integer)
        value = value.Trim(value)
        'length = value.Length

        UsedRanges.Add((value, start, length))
    End Sub

    Private Shared Function IsUsed(start As Integer, length As Integer) As Boolean
        For Each r In UsedRanges
            Dim rStart = r.Start
            Dim rEnd = r.Start + r.Length - 1
            Dim mStart = start
            Dim mEnd = start + length - 1

            ' 範囲が重なっているか？
            If mStart < rEnd AndAlso mEnd > rStart Then
                Return True
            End If
        Next
        Return False
    End Function


End Class
