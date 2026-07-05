Imports System.Diagnostics.Metrics
Imports System.Runtime.CompilerServices
Imports System.Runtime.Intrinsics.Arm
Imports System.Text.RegularExpressions
Imports Tesseract
Imports Windows.Win32.System

Public Module BandList
    Public ReadOnly Bands As String() = {                   ' Band順位並べる必要あり
        "2200M", "630M",
        "160M", "80M", "60M", "40M", "30M", "20M",
        "17M", "15M", "12M", "10M", "6M",
        "2M", "70CM", "23CM", "12CM", "5CM", "3CM"
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
    (10000.0, 10500.0, "3CM")
}

    Public ReadOnly MisReadingBands As New Dictionary(Of String, String)() From {
       {"19", "160M"}, {"18", "160M"}, {"35", "80M"}
   }
End Module

Module ModeList
    Public ReadOnly Modes As String() = {
        "SSB", "FT8", "CW", "FM", "AM",
        "RTTY", "FT4", "PSK31",
        "JT65", "JT9", "SSTV", "ATV", "SAT",
        "AMTOR", "PACKET", "FSK", "MFSK",
        "C4FM", "D-STAR", "F2A", "PHONE", "DMR"
    }

    ' 例："DSTAR"は、正式名"D-STAR"に置き換えられる（ADIFでの"DSTAR"は誤り、JARLは放置している）
    Public ReadOnly AliasModes As New Dictionary(Of String, String)() From {
        {"J3E", "SSB"}, {"LSB", "SSB"}, {"USB", "SSB"}, {"A1", "CW"}, {"A1A", "CW"}, {"F3", "FM"},
        {"A3", "AM"}, {"FONE", "PHONE"}, {"JT-65", "JT65"},
        {"DSTAR", "D-STAR"}, {"SATELLITE", "SAT"}
        }

    Public ReadOnly MisReadingModes As New Dictionary(Of String, String)() From {
        {"FTS", "FT8"}, {"FTZ", "FT8"}, {"FI8", "FT8"}, {"F18", "FT8"}, {"FIB", "FT8"}, {"EI8", "FT8"}, {"FTJ", "FT8"}, {"FT&", "FT8"},
        {"33B", "SSB"}, {"SSE", "SSB"}, {"SS8", "SSB"}, {"\$SB", "SSB"}, {"$$B", "SSB"},
        {"CWS", "CW"}, {"CWB", "CW"}, {"CW8", "CW"}, {"C1W", "CW"},
        {"F4M", "FM"}, {"F3M", "FM"}, {"F3N", "FM"},
        {"A3M", "AM"}, {"A3N", "AM"},
        {"RTTYS", "RTTY"}, {"RTTY8", "RTTY"},
        {"PSK3I", "PSK31"}, {"PSK31A", "PSK31"},
        {"JT65A", "JT65"}, {"JT65B", "JT65"}, {"JT65C", "JT65"},
        {"SSTV8", "SSTV"}, {"SSTV9", "SSTV"},
        {"OW", "CW"}, {"J165", "JT65"}
    }
End Module

Module CalendarModule                ' 月名 → 数字 の辞書
    Public ReadOnly MonthNames As New Dictionary(Of String, Integer) From {
                {"JANUARY", 1}, {"FEBRUARY", 2}, {"MARCH", 3}, {"APRIL", 4}, {"MAY", 5}, {"JUNE", 6}, {"JULY", 7},
                {"AUGUST", 8}, {"SEPTEMBER", 9}, {"OCTOBER", 10}, {"NOVEMBER", 11}, {"DECEMBER", 12},
                {"JAN", 1}, {"FEB", 2}, {"MAR", 3}, {"APR", 4}, {"JUN", 6},
                {"JUL", 7}, {"AUG", 8}, {"SEP", 9}, {"OCT", 10}, {"NOV", 11}, {"DEC", 12}
            }

    Public ReadOnly MisReadingMonth As New Dictionary(Of String, String)() From {
        {"3AN", "JAN"}, {"WAR", "MAR"}, {"APE", "APR"}, {"/UL", "JUL"}, {"1UL", "JUL"},
        {"0CT", "OCT"}, {"N0V", "NOV"}
    }

End Module


Partial Class frmMain

    Private Shared UsedRanges As New List(Of (Start As Integer, Length As Integer))

    Public Structure Detection
        Public Value As String
        Public Index As Integer
        Public Length As Integer
        Public Row As Integer
        Public Column As Integer
    End Structure

    Private Shared DateBase As Detection                   ' Dateに一致するものが2個以上あったとき
    Private Shared DateBaseTitle As Detection              ' Dateを検出したTitleをもとに
    Private Shared DetectionDates As New List(Of Detection)

    Private Shared TimeBase As Detection
    Private Shared TimeBaseTitle As Detection ' Timmeに一致するものが2個以上あったとき
    Private Shared DetectionTimes As New List(Of Detection)

    Private Shared ModeBase As Detection                   ' Modeに一致するものが2個以上あったとき
    Private Shared ModeBaseTitle As Detection              ' Modeを検出したTitleをもとに
    Private Shared DetectionModes As New List(Of Detection)

    Private Shared BandBase As Detection                   ' Band、FREQに一致するものが2個以上あったとき
    Private Shared BandBaseTitle As Detection              ' Bandを検出したTitleをもとに
    Private Shared DetectionBands As New List(Of Detection)

    Public Shared isPhone As Boolean           ' ReportからModeを類推する、Phoneの時とき　Bandが２M以上の時、FMにModeを置き換える

    '**********************************************************************************************
    '
    '   OCR結果から項目を注出する
    '
    '**********************************************************************************************

    Public Class RegexExtractor

        Public Shared Function ExtractCallsignCandidates(text As String) As List(Of String)
            Dim list As New List(Of String)
            Dim pattern As String

            pattern = "\b(J[A-S]|7[K-N])([A-Z0-9])([A-Z0-9]{2,3})\b"
            For Each m As Match In Regex.Matches(text, pattern)
                Dim part1 = m.Groups(1).Value
                Dim part2 = m.Groups(2).Value
                Dim part3 = m.Groups(3).Value

                part2 = ReplaceAtoN(part2)
                part3 = ReplaceNtoA(part3)

                Dim fixedCs = part1 & part2 & part3
                If fixedCs <> "" Then
                    If fixedCs Like "*[0-9]*" Then
                        list.Add(fixedCs)
                    End If
                End If
            Next

            ' ゆるめのコールサインパターン
            pattern = "(?<![A-Z0-9])[1-9]?[A-Z]{1,5}[0-9]{1,5}[A-Z0-9]{1,8}(?![A-Z0-9])"
            '   pattern = "[1-9]?[A-Z]{1,5}[0-9]{1,5}[A-Z0-9]{1,8}\s"
            For Each m As Match In Regex.Matches(text, pattern, RegexOptions.Multiline)
                Dim fixedCs = m.Value
                If fixedCs <> "" Then
                    If fixedCs Like "*[0-9]*" Then
                        If list.Contains(fixedCs) = False Then
                            list.Add(fixedCs)
                        End If
                    End If
                End If
            Next

            ' hQSL対応 JA局のみ

            pattern = "(^|\s)FR[O|0]M[:;]?([A-Z0-9]{5,6})(\s|$)"
            For Each m As Match In Regex.Matches(text, pattern, RegexOptions.Multiline)
                Dim fixedCs = m.Groups(2).Value
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
                    If fixedCs Like "*[0-9]*" Then
                        If list.Contains(fixedCs) = False Then
                            list.Add(fixedCs)
                        End If
                    End If
                End If
            Next


            Return list
        End Function

        Public Shared Function RemoveMyCallsign(MyCallsigns As String, candidates As List(Of String)) As List(Of String)
            Dim result As New List(Of String)

            Dim Callsigns As String() = MyCallsigns.Split(","c)
            For Each cs In candidates
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
            Dim bestCall As String = ""
            Dim bestDistance As Integer = 9999
            Dim cs As String

            For Each c In candidates
                cs = c

                ' 典型的なコールサインパターンに合致するものだけ採用
                If Not Regex.IsMatch(cs, callPattern) Then
                    Continue For
                End If

                Dim JApattern As String = "^(J[A-S]|7[K-N]|[8[J-N])"      ' JA局を優先
                If Regex.IsMatch(cs, JApattern) Then
                    Return cs
                End If

                ' MyCallsign とどれだけ違うか（距離が大きいほど違う）
                Dim d As Integer = Levenshtein(cs, myCall)

                If d < bestDistance Then
                    bestDistance = d
                    bestCall = cs
                End If
            Next

            Return bestCall
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


        ' 日付抽出のメイン関数
        Public Shared Function ExtractDate(text As String) As String
            Dim pattern As String
            Dim m As Match

            Dim cleaned = CleanTextDate(text)

            ClearBaseDetection(DateBase)
            pattern = "(DATE|(YY)?YYMMDD|YEAR|YYYY)"
            m = Regex.Match(cleaned, pattern)
            If m.Success = True Then
                Dim index As Integer = m.Index

                ' 行と列を計算するメソッドを呼び出す
                Dim row As Integer = 0
                Dim col As Integer = 0
                GetRowAndColumn(cleaned, index, row, col)

                'SetDateBaseDetection(m.groups(0).value, m.Index, m.Length, row, col)
                SetBaseDetection(DateBaseTitle, m.Groups(0).Value, m.Index, m.Length, row, col)
            End If

            ' 1. 月名を含む形式を探す
            Dim dateFromMonth = ExtractDateWithMonthName(cleaned)
            If dateFromMonth <> "" Then Return dateFromMonth

            '2. 誤読した英月名で探す
            dateFromMonth = ExtractMisReadDate(cleaned)
            If dateFromMonth <> "" Then Return dateFromMonth

            ' 3. 数字だけの形式を探す
            Dim dateFromNumbers = ExtractDateNumeric(cleaned)
            If dateFromNumbers <> "" Then Return dateFromNumbers

            '  4. 日本語の年、月、日を誤読したとして
            dateFromMonth = ExtractNenTsukiHiDate(cleaned)
            If dateFromMonth <> "" Then Return dateFromMonth

            dateFromMonth = ExtractDateSpace(cleaned)
            If dateFromMonth <> "" Then Return dateFromMonth

            dateFromMonth = ExtractDateWithMonthName2Line(cleaned)
            If dateFromMonth <> "" Then Return dateFromMonth

            Return ""
        End Function

        ' 月名を含む日付
        Private Shared Function ExtractDateWithMonthName(text As String) As String
            text = CleanTextMisreadingDate(text)        ' 月名をミスリードしたものを補正する
            Dim pattern As String
            Dim m As Match
            Dim month As String
            Dim day As String
            Dim year As String
            Dim sm As String

            Dim DateFormat = JugementDateFormatFromEntity(qsoCallsign)      ' Enthityから日付のファーマっとを設

            For Each key In MonthNames.Keys
                'pattern = "\b(\')?(\d{1,4})[\s/-]{0,2}" & key & "[\s/-]{0,2}(\')?(\d{1,4})\b"       '年、月、日または日、月、年の並び順
                '         pattern = "\b(\')?(\d{1,4})[ /-]{0,2}" & key & "[ /-]{0,2}(\')?(\d{1,4})\b"       '年、月、日または日、月、年の並び順
                pattern = "\b(\')?(\d{1,4})[ /-]{0,2}" & key & "[ /-]{0,2}(\')?(\d{1,4})[\b\s\D]"       '年、月、日または日、月、年の並び順
                m = Regex.Match(text, pattern)
                If m.Success Then
                    If (m.Groups(1).Value = "'") OrElse (m.Groups(2).Value.Length = 4) Then         ' (yy)yy-mm-dd  ISO Format の場合
                        month = MonthNames(key)
                        day = Integer.Parse(m.Groups(4).Value)
                        Dim y = m.Groups(2).Value
                        If (y.Length = 3) AndAlso (DateFormat = DateFormats.IsoFormat) Then     ' 3桁の年は誤読している。下2桁を年とする (例: '123 → 2023)
                            year = y.Substring(1, 2)
                        Else
                            year = m.Groups(2).Value
                        End If
                        year = FixYear(m.Groups(2).Value)           ' 年が2桁なら2000年代とみなす
                        If isValidDate(year, month, day) Then
                            sm = FormatDate(year, month, day)
                            UsedRanges.Add((m.Index, m.Length))
                            SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, 0, 0)
                            Return sm
                        End If
                    ElseIf (m.Groups(3).Value = "'") OrElse (m.Groups(4).Value.Length = 4) Then     ' dd-mm-(yy)yy British Format の場合
                        month = MonthNames(key)
                        day = Integer.Parse(m.Groups(2).Value)
                        year = FixYear(m.Groups(4).Value)
                        If isValidDate(year, month, day) Then
                            sm = FormatDate(year, month, day)
                            UsedRanges.Add((m.Index, m.Length))
                            SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, 0, 0)
                            Return sm
                        End If
                    Else
                        month = MonthNames(key)     '　コロンがない場合 年，月、日の順とみなす  
                        Dim day1 = Integer.Parse(m.Groups(4).Value)
                        Dim year1 = FixYear(m.Groups(2).Value).ToString
                        If year1 = 3 AndAlso DateFormat = DateFormats.IsoFormat Then
                            year1 = year1.Substring(1, 2)
                        End If
                        Dim ymd1 = DateSerial(CInt(year1), CInt(month), CInt(day1))

                        Dim day2 = Integer.Parse(m.Groups(2).Value)
                        Dim year2 = FixYear(m.Groups(4).Value).ToString
                        If year2 = 3 AndAlso DateFormat = DateFormats.IsoFormat Then
                            year2 = year2.Substring(1, 2)
                        End If
                        Dim ymd2 = DateSerial(CInt(year2), CInt(month), CInt(day2))

                        If Now - ymd1 < Now - ymd2 Then ' 年月日を入れ替えてみて、現年月日に近いほうを採用する
                            year = year1
                            day = day1
                        Else
                            year = year2
                            day = day2
                        End If

                        year = FixYear(year)           ' 年が2桁なら2000年代とみなす

                        If isValidDate(year, month, day) Then
                            Dim s As String = FormatDate(year, month, day)
                            UsedRanges.Add((m.Index, m.Length))
                            SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, 0, 0)
                            Return s
                        Else
                            day = Integer.Parse(m.Groups(2).Value)      ' 年月日にならない場合、年と日を入れ替える”
                            year = FixYear(m.Groups(4).Value)
                            If isValidDate(year, month, day) Then
                                Dim s As String = FormatDate(year, month, day)
                                UsedRanges.Add((m.Index, m.Length))
                                SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, 0, 0)
                                Return s
                            End If
                        End If
                    End If
                End If

                pattern = "\b" & key & "[.,\s/-]+(\d{1,2})[.,\s/-]+(\d{2,4})\b"              '月、日、年の並び順     American Format の場合
                m = Regex.Match(text, pattern)
                If m.Success Then
                    month = MonthNames(key)
                    day = Integer.Parse(m.Groups(1).Value)
                    year = FixYear(m.Groups(2).Value)
                    If isValidDate(year, month, day) Then
                        Dim s As String = FormatDate(year, month, day)
                        UsedRanges.Add((m.Index, m.Length))
                        SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, 0, 0)
                        Return s
                    End If
                End If

                pattern = "\b(\d{1,2})[.,\s/-]+" & key & "[.,\s/-]+(\d{2,4})\b"              '日、月、年の並び順  不要か？
                m = Regex.Match(text, pattern)
                If m.Success Then
                    month = MonthNames(key)
                    day = Integer.Parse(m.Groups(1).Value)
                    year = FixYear(m.Groups(2).Value)
                    If isValidDate(year, month, day) Then
                        Dim s As String = FormatDate(year, month, day)
                        UsedRanges.Add((m.Index, m.Length))
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
                                UsedRanges.Add((m.Index, m.Length))
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

            Dim a, b, c As Integer

            Dim DateFormat = JugementDateFormat(text)               ' yyyy/mm/dd、dd/mm/yy等の順から日付のフォーマットを設定
            If DateFormat = DateFormats.InvalidFormat Then
                DateFormat = JugementDateFormatFromEntity(qsoCallsign)      '' Enthityから日付のファーマっとを設定
            End If

            ' 年が真ん中に来るはずない
            'Dim pattern = "(^|\s|\D|:|;)(')?(\d{2}|\d{4}|\d)[/\-\. ]{1,2}(\d{1,2})[/\-\. ]{1,2}(')?(\d{2}|\d{4}|\d)(\s|\D|$)"
            'Dim pattern = "(^|\s|\D|:|;)(')?(\d{2}|\d{4}|\d)([/\-\.\/ ])(\d{1,2})\4(')?(\d{2}|\d{4}|\d)(\s|\D|$)"
            Dim pattern = "(^|\s|\D|:|;)(')?(\d{1,4})([/\-\. ])(\d{1,2})\4(')?(\d{1,4})(\s|\D|$)"

            Dim ms = Regex.Matches(text, pattern)
            For Each m As Match In ms
                If m.Index < DateBase.Index Then Continue For

                Dim part1 = m.Groups(3).Value
                Dim part2 = m.Groups(5).Value
                Dim part3 = m.Groups(7).Value
                Dim year As String = ""
                Dim month As String = ""
                Dim day As String = ""

                If (m.Groups(2).Value = "'") OrElse (part1.Length = 4) Then         ' (yy)yy-xx-xx  の場合
                    year = part1
                    month = part2
                    day = part3
                ElseIf (m.Groups(5).Value = "'") OrElse (part3.Length = 4) Then     ' xx-xx-(yy)yy  の場合
                    If (DateFormat = DateFormats.AmericanFormat) Then
                        year = part3
                        month = part1
                        day = part2
                    Else
                        year = part3
                        month = part2
                        day = part1
                    End If
                ElseIf (DateFormat = DateFormats.AmericanFormat) Then     '  American Format の場合
                    year = part3
                    month = part1
                    day = part2
                ElseIf (DateFormat = DateFormats.BritishFormat) Then     '  British Format の場合
                    year = part3
                    month = part2
                    day = part1
                Else
                    ' コロンがない場合 年，月、日の順とみなす  
                    year = part1
                    month = part2
                    day = part3
                End If
                If year.Length = 3 Then    ' 3桁の年は誤読している。下2桁を年とする (例: '123 → 2023)
                    year = year.Substring(1, 2)
                End If
                year = FixYear(year)           ' 年が2桁なら2000年代とみなす

                If Not isValidDate(year, month, day) Then
                    ' 年，日、月の順とみなす  
                    year = part1
                    day = part2
                    month = part3
                End If

                If Not isValidDate(year, month, day) Then
                    Dim temp As String = month
                    month = day
                    day = temp
                    If Not isValidDate(year, month, day) Then
                        Continue For
                    End If
                End If

                a = Integer.Parse(year)
                b = Integer.Parse(month)
                c = Integer.Parse(day)
                a = ConverTo4degitsYear(a)

                ' 日付のTitleがない場合、Titleより後で100文字以内の位置にある日付を採用する
                If (DateBaseTitle.Index = 0) OrElse ((m.Index > DateBaseTitle.Index) AndAlso (m.Index < DateBaseTitle.Index + 100)) Then
                    UsedRanges.Add((m.Index, m.Length))     ' yyyy/mm/dd
                    SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, 0, 0)
                    Dim d = FormatDate(a, b, c)
                    Return d            ' Return $"{a:0000}/{b:00}/{c:00}"
                End If
            Next

            Return ""
        End Function

        '  4. 日本語の年、月、日を誤読したとして
        Private Shared Function ExtractNenTsukiHiDate(Text As String) As String

            'Dim pattern = "(19|20)\d{2}.{1,3}([01]\d).{1,3}([0-3]?\d)"     ' Copilot提案

            'Dim pattern = "((19|20)\d{2}).{1,2}([01][0123]).{1,2}([0123]\d)"
            Dim pattern = "((19|20)\d{2}).{1,2}([01]\d).{1,2}([0-3]\d)"

            Dim ms = Regex.Matches(Text, pattern)

            For Each m As Match In ms
                Dim year = m.Groups(1).Value
                Dim month = m.Groups(3).Value
                Dim day = m.Groups(4).Value

                If Not isValidDate(year, month, day) Then Continue For
                If Not IsUsed(m.Index, m.Length) Then
                    UsedRanges.Add((m.Index, m.Length))
                    SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, 0, 0)
                    Return FormatDate(year, month, day)
                End If
            Next

            Return ""
        End Function


        '  4. 日本語の年、月、日を誤読したとして
        Private Shared Function ExtractDateSpace(text As String) As String

            Dim pattern = "((19|20)?\d{2})[.\s]?([01]\d)[.\s]?([012]\d|3[0-1]\d)"

            Dim ms = Regex.Matches(text, pattern)
            For Each m As Match In ms
                Dim year = m.Groups(1).Value
                Dim month = m.Groups(3).Value
                Dim day = m.Groups(4).Value

                If Not isValidDate(year, month, day) Then Continue For
                If Not IsUsed(m.Index, m.Length) Then
                    UsedRanges.Add((m.Index, m.Length))
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
            Dim year As String
            Dim sm As String
            Dim yearIndex As Integer
            Dim yearLength As Integer

            Dim DateFormat = JugementDateFormatFromEntity(qsoCallsign)      ' Enthityから日付のファーマっとを設

            pattern = "\b?(19|20)(\d{2})\b"              '19xx,20xxのみ
            m = Regex.Match(text, pattern)
            If m.Success Then
                year = m.Groups(1).Value & m.Groups(2).Value
                yearIndex = m.Index
                yearLength = m.Length
                year = FixYear(year)           ' 年が2桁なら2000年代とみなす
            Else
                Return ""
            End If

            For Each key In MonthNames.Keys
                '         pattern = "\b?(\d{1,2})?[\s/-]*" & key & "[\.\s/-']*(\d{1,2})?\b"              '月、日または日、月、年の並び順
                pattern = "\b?([0-2]\d|[3][0-1])?[\s/\-]*" & key & "\b"
                m = Regex.Match(text, pattern)
                If m.Success Then
                    If m.Groups(1).Value <> "" Then         ' (yy)yy-mm-dd  Japanese Format の場合
                        month = MonthNames(key)
                        day = Integer.Parse(m.Groups(1).Value)
                        If isValidDate(year, month, day) Then
                            sm = FormatDate(year, month, day)
                            UsedRanges.Add((m.Index, m.Length))
                            UsedRanges.Add((yearIndex, yearLength))
                            SetBaseDetection(DateBase, m.Groups(0).Value, m.Index, m.Length, yearIndex, yearLength)
                            Return sm
                        End If
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

            s = s.Replace("|", " ")
            's = s.Replace("O", "0")
            s = s.Replace("I", "1")
            s = s.Replace(",", "/")     '年月日なので、スラッシュに変換
            s = s.Replace(".", "/")
            s = s.Replace("[", " ")
            s = s.Replace("]", " ")

            s = s.Replace(";", "/")
            s = s.Replace("(", " ")

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

        '**************************************************************************************************************

        ' 時刻抽出のメイン関数
        Public Shared Function ExtractTime(text As String) As String


            Dim pattern As String
            Dim m As Match
            Dim mc As MatchCollection

            ClearBaseDetection(BandBaseTitle)
            pattern = "(TIME|HHMM|JST|UTC)"
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

            ' 時刻パターン（例：09:00JST 9:00など）
            pattern = "\b([01]\d|2[0-3]|[1-9])[:\./\-]+([0-5]\d)([ UJZ]*)"      ' 時と分の間は0個か1個 UTC,JSZ,Zが続く
            mc = Regex.Matches(text, pattern)
            For Each m In mc
                'm = Regex.Match(cleaned, pattern, RegexOptions.IgnoreCase)
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

            ' 時刻パターン（例：0900 など）
            pattern = "\b([01]\d|2[0-3])([0-5]\d)\b"     ' 数字4桁
            mc = Regex.Matches(text, pattern)
            For Each m In mc
                ' m = Regex.Match(cleaned, pattern, RegexOptions.IgnoreCase)
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

            If DetectionTimes.Count = 0 Then
                ' 時刻パターン（例：0900 など）
                pattern = "([01]\d|2[0-3])[:\./\-]?([0-5]\d)"     ' 数字4桁
                mc = Regex.Matches(text, pattern)
                For Each m In mc
                    ' m = Regex.Match(cleaned, pattern, RegexOptions.IgnoreCase)
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
            End If

            Dim BestValue() = ChooseBestDetection(DetectionTimes, DateBase, False)
            If BestValue(0) <> "" Then
                UsedRanges.Add((BestValue(1), BestValue(2)))
                SetBaseDetection(TimeBase, BestValue(0), BestValue(1), BestValue(2), 0, 0)
                Return FormatTime(BestValue(0))
            End If
            BestValue = ChooseBestDetection(DetectionTimes, TimeBaseTitle, False)
            If BestValue(0) <> "" Then
                UsedRanges.Add((BestValue(1), BestValue(2)))
                SetBaseDetection(TimeBase, BestValue(0), BestValue(1), BestValue(2), 0, 0)
                Return FormatTime(BestValue(0))
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
            s = s.Replace("O", "0")
            s = s.Replace("I", "1")
            s = s.Replace("L", "1")
            s = s.Replace(";", ":")
            s = s.Replace(".", ":")
            s = s.Replace(",", ":")

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
            Dim hour = Integer.Parse(HH)
            Dim minute = Integer.Parse(DD)

            ' 時刻の妥当性チェック
            If hour < 0 Or hour > 23 Then Return False
            If minute < 0 Or minute > 59 Then Return False
            Return True

        End Function

        '**************************************************************************************************************

        Public Shared Function ExtractMode(text As String) As String
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
                pattern = "[:;YE\s](" & key & ")(?=\s|\b|$)"                    ' Yは2WayのY、EはModeのEと誤認識する可能性があるため、前に:;YE\sを追加
                Dim mdx = Regex.Matches(cleaned, pattern)
                For Each m In mdx
                    If m.Success = True Then
                        If IsUsed(m.Index, m.Length) Then Continue For
                        DetectionModes.Add(New Detection With {.Value = m.Groups(0).Value, .Index = m.Index, .Length = m.Length, .Row = 0, .Column = 0})
                    End If
                Next
            Next

            ' 2, エイリアスモードが含まれているかチェック
            For Each keys In AliasModes
                pattern = "[:;\s](" & keys.Key & ")(?=\s|\b|$)"
                Dim mdx = Regex.Matches(cleaned, pattern)
                For Each m In mdx
                    If m.Success = True Then
                        If IsUsed(m.Index, m.Length) Then Continue For
                        Dim Index As Integer = m.Groups(1).Index            ' 前後の空白分を除外して登録
                        Dim Length As Integer = m.Groups(1).Length
                        DetectionModes.Add(New Detection With {.Value = keys.Value, .Index = m.Index, .Length = m.Length, .Row = 0, .Column = 0})
                    End If
                Next
            Next

            ' 3, 誤読辞書に含まれているかチェック
            For Each keys In MisReadingModes
                pattern = "[:;\s](" & keys.Key & ")(?=\s|\b|$)"
                m = Regex.Match(cleaned, pattern)
                If m.Success = True Then
                    If IsUsed(m.Index, m.Length) Then Continue For
                    Dim index = m.Groups(1).Index
                    Dim length = m.Groups(1).Length
                    'UsedRanges.Add((index, length))
                    Dim row = text.LastIndexOf(ControlChars.Lf, index)
                    Dim column = index - text.LastIndexOf(ControlChars.Lf, index)

                    DetectionModes.Add(New Detection With {.Value = keys.Value, .Index = m.Index, .Length = m.Length, .Row = 0, .Column = 0})
                    'Return keys.Value
                End If
            Next

            ' 4. 誤読したモードで探す(Levenshtein distanceで判断)
            pattern = "[:;\s]([A-Z0-9]{4,8})(?=\s|\b|$)"
            Dim md = Regex.Matches(cleaned, pattern)
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
                            DetectionModes.Add(New Detection With {.Value = key, .Index = m.Index, .Length = m.Length, .Row = 0, .Column = 0})


                            'AddModeBase(key, Index, length, row, column)
                            'SetBaseDetection(ModeBase, m.Groups(1).Value, m.Groups(1).Index, m.Groups(1).Length, 0, 0)      ' Band選択のため

                            'Return key
                        End If
                    End If
                Next
            Next

            ' 6. Modeの前後いずれかが空白を救う
            For Each key In Modes           ' Modesの中で、誤認識されやすいものを抽出する
                pattern = "(^|\s)(" & key & "|" & key & ") (/=\s|\d|$)"
                m = Regex.Match(cleaned, pattern)
                If m.Success = True Then
                    If IsUsed(m.Index, m.Length) Then Continue For
                    If ModeBase.Index <> 0 AndAlso ((m.Index < ModeBase.Index) OrElse (m.Index > ModeBase.Index + 40)) Then Continue For
                    'UsedRanges.Add((m.Index, m.Length))
                    Dim Index = m.Groups(2).Index
                    Dim Length = m.Groups(2).Length
                    Dim row = text.LastIndexOf(ControlChars.Lf, Index)
                    Dim column = Index - text.LastIndexOf(ControlChars.Lf, Index)
                    DetectionModes.Add(New Detection With {.Value = m.Groups(2).Value, .Index = Index, .Length = Length, .Row = row, .Column = column})



                    'AddModeBase(key, Index, Length, row, column)
                    'SetBaseDetection(ModeBase, m.Groups(1).Value, m.Groups(1).Index, m.Groups(1).Length, 0, 0)
                    'Return key
                End If
            Next

            If DetectionModes.Count = 0 Then
                For Each key In Modes
                    pattern = key                     ' Modeが検知されないとき、Keyの前後を無視してkeyで検索
                    Dim mdx = Regex.Matches(cleaned, pattern)
                    For Each m In mdx
                        If m.Success = True Then
                            If IsUsed(m.Index, m.Length) Then Continue For
                            DetectionModes.Add(New Detection With {.Value = m.Groups(0).Value, .Index = m.Index, .Length = m.Length, .Row = 0, .Column = 0})
                        End If
                    Next
                Next
            End If

            ' 7, 誤読辞書に含まれているかチェック　前後の文字を無視
            If DetectionModes.Count = 0 Then
                For Each keys In MisReadingModes
                    pattern = "(" & keys.Key & ")"
                    m = Regex.Match(cleaned, pattern)
                    If m.Success = True Then
                        If IsUsed(m.Index, m.Length) Then Continue For
                        If ModeBase.Index <> 0 AndAlso ((m.Index < ModeBase.Index) OrElse (m.Index > ModeBase.Index + 40)) Then Continue For

                        Dim Index = m.Groups(1).Index
                        Dim Length = m.Groups(1).Length

                        'UsedRanges.Add((m.Index, m.Length))
                        Dim row = text.LastIndexOf(ControlChars.Lf, m.Index)
                        Dim column = m.Index - text.LastIndexOf(ControlChars.Lf, m.Index)
                        DetectionModes.Add(New Detection With {.Value = keys.Value, .Index = Index, .Length = Length, .Row = row, .Column = column})


                        'AddBandBase(keys.Value, m.Index, m.Length, row, column)
                        'SetBaseDetection(ModeBase, m.Groups(1).Value, m.Groups(1).Index, m.Groups(1).Length, row, column)
                    End If
                Next
            End If

            For Each f In DetectionModes        ' Modes,AliasModesの検出結果をもとに、最も近いModeBaseを探す
                With ModeBaseTitle
                    Dim s As Integer = Math.Abs(.Index - f.Index)

                    Debug.Print("ModeBase: " & .Value & " at (" & .Index & "," & .Length & ") - Candidate: " & f.Value & " at (" & f.Index & "," & f.Length & ")")
                    If s < shortestDistance Then
                        bestMode = f.Value
                        IndexOfBestMode = f.Index
                        lengthOfBestMode = f.Length
                        shortestDistance = s
                        '                      SetBandBaseByMode(f.Value, f.Index, f.Length, f.Row, f.Column)        ' Band選択のため
                    End If
                End With
            Next
            If bestMode <> "" Then
                UsedRanges.Add((IndexOfBestMode, lengthOfBestMode))
                SetBaseDetection(ModeBase, bestMode, IndexOfBestMode, lengthOfBestMode, 0, 0)
                Return bestMode
            End If


            ' 5. signalレポートからMode:を推測する
            '            pattern = "SIGS?[ :;]*([+\-\d]{1,3})\b"  '[+\-\d]
            pattern = "[\s\b](SIGS?|DB)*[ :;]*([\+\-][0-5]\d|[3-5][5-9]|[3-5][5-9]{2})(DB)*[\s\b]"  '±50dBならデジタル、35から59ならフォン、355から599ならCWと推測する
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
                ElseIf IsNumeric(sr) Then
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

        '**************************************************************************************************************


        ' 周波数抽出（例：7.074）
        ' 周波数 → バンド変換のメイン関数
        Public Shared Function ExtractBand(text As String) As String
            Dim pattern As String
            Dim band As String
            Dim Bandformat As BandFormats

            text = CleanTextBandCommon(text)

            Bandformat = BandFormats.Unknown

            SetBandBase("", 0, 0, 0, 0)      ' BandBaseを初期化する)
            DetectionBands.Clear()
            pattern = "(BAND|FREQ|MHZ|FREQUENCY)"
            Dim m = Regex.Match(text, pattern, RegexOptions.IgnoreCase)
            If m.Success = True Then
                Dim b = m.Groups(1).Value
                Dim index As Integer = m.Index

                If (b = "FREQ") OrElse (b = "MHZ") OrElse (b = "FREQUENCY") Then        ' 面倒なのでMHzに統一
                    Bandformat = BandFormats.MHz
                End If

                ' 行と列を計算するメソッドを呼び出す
                Dim row As Integer = 0
                Dim col As Integer = 0
                GetRowAndColumn(text, index, row, col)
                'AddBandBase(b, index, m.Length, row, col)
                SetBaseDetection(BandBaseTitle, m.Groups(1).Value, m.Groups(1).Index, m.Groups(1).Length, row, col)
            End If

            ' 1. まず Band 表記があるか探す           ' 40M, 2M 等の波長表示
            If Bandformat <> BandFormats.MHz Then       ' MHz表記の時は波長表記の処理を飛ばす
                band = ExtractBandDirect(text)
                If band <> "" Then Return band
            End If

            ' 2. 周波数を求め、Bandにする        ' 周波数抽出（例：7.074）
            band = ExtractFrequency(text)
            If band <> "" Then Return band

            ' 3. 周波数の誤読補正をし、周波数を求め,Bandにする        ' 周波数抽出（例：7.074）
            band = ExtractMisreadFrequency(text)
            If band <> "" Then Return band

            band = ExtractMisreadBand(text)         ' 3.5を35と誤認識することがあるので、周波数抽出の前に、誤認識のBand表記を先に抽出する
            If band <> "" Then Return band

            'band = ExtractBandByBand(text)
            'If band <> "" Then Return band

            'If freq > 0 Then
            '    Return ConvertFreqToBand(freq)
            'End If

            Return ""
        End Function

        ' Band 表記を直接抽出
        Private Shared Function ExtractBandDirect(text As String) As String
            Dim upper = CleanTextBand(text)

            For Each b In BandList.Bands        ' 40M, 2M 等の波長表示
                If upper.Contains(b) Then
                    Dim pattern = "[\(\s^:;](" & b & ")[\)\s\b$]"
                    Dim Regex As New Regex(pattern)
                    For Each m As Match In Regex.Matches(upper)
                        If IsUsed(m.Index, m.Length) Then
                            Continue For
                        End If

                        DetectionBands.Add(New Detection With {.Value = b, .Index = m.Index, .Length = m.Length, .Row = 0, .Column = 0})

                        'UsedRanges.Add((m.Index, m.Length))
                        'Return m.Groups(1).Value
                    Next
                End If
            Next

            If ModeBase.Value <> "" Then
                Dim bestAwnser As String() = ChooseBestDetection(DetectionBands, ModeBase, True)
                If bestAwnser(0) <> "" Then
                    UsedRanges.Add((bestAwnser(1), bestAwnser(2)))
                    Return bestAwnser(0)
                End If
            End If

            If TimeBase.Value <> "" Then
                Dim bestAwnser As String() = ChooseBestDetection(DetectionBands, TimeBase, False)
                If bestAwnser(0) <> "" Then
                    UsedRanges.Add((bestAwnser(1), bestAwnser(2)))
                    Return bestAwnser(0)
                End If
            End If

            If DateBase.Value <> "" Then
                Dim bestAwnser As String() = ChooseBestDetection(DetectionBands, DateBase, False)
                If bestAwnser(0) <> "" Then
                    UsedRanges.Add((bestAwnser(1), bestAwnser(2)))
                    Return bestAwnser(0)
                End If
            End If

            If BandBase.Value <> "" Then
                Dim bestAwnser As String() = ChooseBestDetection(DetectionBands, BandBase, False)
                If bestAwnser(0) <> "" Then
                    UsedRanges.Add((bestAwnser(1), bestAwnser(2)))
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
                UsedRanges.Add((IndexOfBestFreq, lengthOfBestFreq))
                Return bestBand
            End If

            Return ""
        End Function

        Private Shared Function ExtractFrequency(text As String) As String

            text = CleanTextFreq(text)

            DetectionBands.Clear()
            Dim bestBand As String = ""
            Dim IndexOfBestFreq As Integer = 0
            Dim lengthOfBestFreq As Integer = 0
            Dim shortestDistance As Integer = Integer.MaxValue
            Dim pattern As String
            Dim m As Match
            Dim bn As String
            Dim ff As Double

            text = text.Replace("FREQ", "    ")
            text = text.Replace("BAND", "    ")

            Dim unit As String = ""
            pattern = "[\s|\d](MHZ|KHZ|GHZ)[:;\s]"
            m = Regex.Match(text, pattern, RegexOptions.IgnoreCase)
            If m.Success = True Then
                unit = m.Groups(1).Value
            End If

            '            pattern = "(?<=\s|:|;|\b)([1-9]\d{0,6})([.,\.]{0,2})(\d{0,6})(?=[\s\]MKG]|$)"    ' 直前が空白であり、直後が数字以外であること 整数部7桁、小数部6桁
            pattern = "\b([1-9]\d{0,6})([.,\.]{0,2})(\d{0,6})(\b|M)"    ' 直前が空白であり、直後が数字以外であること 整数部7桁、小数部6桁

            '     ' まず変換する前に実行する
            For Each m In Regex.Matches(text, pattern)

                Debug.Print(m.Groups(0).Value & " index=" & m.Groups(0).Index & " length=" & m.Groups(0).Length)

                If IsUsed(m.Index, m.Length) Then Continue For
                If m.Groups(4).Value = "W" Then Continue For      ' 50Wの表記などを除く

                Dim integerPart As String = m.Groups(1).Value      ' 整数部
                Dim decimalPart As String = m.Groups(3).Value & "0"     ' 小数部がない場合"0"を補う

                bn = ""
                If unit = "KHZ" Then
                    If Double.TryParse(integerPart, ff) Then                ' KHzで記入されている場合 → MHzに変換する
                        Dim fs As String = (ff / 1000).ToString
                        bn = ConvertFreqToBand(fs)
                    Else
                        Continue For
                    End If
                ElseIf unit = "GHZ" Then                                    ' GHzで記入されている場合 → MHzに変換する
                    If Double.TryParse(integerPart & "." & decimalPart, ff) Then
                        Dim fs As String = (ff * 1000).ToString
                        bn = ConvertFreqToBand(fs)
                    Else
                        Continue For
                    End If
                ElseIf (CInt(decimalPart) = 0) AndAlso (integerPart.Length > 3) Then       ' 7025 14200等　整数部のみ3桁以上
                    For i = 1 To integerPart.Length
                        Dim int As String = integerPart.Substring(0, i)
                        Dim dec As String = integerPart.Substring(i)
                        Dim num = int & "." & dec & "0"
                        bn = ConvertFreqToBand(num)
                        If bn <> "" Then
                            integerPart = int
                            decimalPart = dec
                            Exit For
                        End If
                    Next
                ElseIf (integerPart <> "0") Then         ' 7.025 14.200等 整数部＋小数部
                    If (decimalPart <> "0") Then
                        Dim num = integerPart & "." & decimalPart
                        bn = ConvertFreqToBand(num)
                        If bn <> "" Then
                            integerPart = CInt(integerPart)
                            decimalPart = CInt(decimalPart)
                        Else
                            num = integerPart & "." & decimalPart.Substring(0, 1)                  ' 少数２桁以下が誤読しているとして
                            bn = ConvertFreqToBand(num)
                            If bn <> "" Then
                                integerPart = CInt(integerPart)
                                decimalPart = 0
                            End If
                        End If
                    Else
                        Dim num = integerPart                       ' 整数部のみ
                        bn = ConvertTypicalFreqToBand(num)
                        If bn <> "" Then
                            integerPart = CInt(integerPart)
                            decimalPart = 0
                        End If
                    End If
                Else
                    bn = ConvertFreqToBand(integerPart)
                End If
                If bn = "" Then Continue For

                ' 行とカラムを計算するメソッドを呼び出す
                Dim row As Integer = 0
                Dim col As Integer = 0
                GetRowAndColumn(text, m.Index, row, col)
                DetectionBands.Add(New Detection With {.Value = bn, .Index = m.Index, .Length = m.Length, .Row = row, .Column = col})
            Next

            bestBand = ""
            shortestDistance = Integer.MaxValue

            If ModeBase.Value <> "" Then
                Dim bestAwnser As String() = ChooseBestDetection(DetectionBands, ModeBase, True)
                If bestAwnser(0) <> "" Then
                    UsedRanges.Add((bestAwnser(1), bestAwnser(2)))
                    Return bestAwnser(0)
                End If
            End If

            If TimeBase.Value <> "" Then
                Dim bestAwnser As String() = ChooseBestDetection(DetectionBands, TimeBase, False)
                If bestAwnser(0) <> "" Then
                    UsedRanges.Add((bestAwnser(1), bestAwnser(2)))
                    Return bestAwnser(0)
                End If
            End If

            If DateBase.Value <> "" Then
                Dim bestAwnser As String() = ChooseBestDetection(DetectionBands, DateBase, False)
                If bestAwnser(0) <> "" Then
                    UsedRanges.Add((bestAwnser(1), bestAwnser(2)))
                    Return bestAwnser(0)
                End If
            End If

            If BandBase.Value <> "" Then
                Dim bestAwnser As String() = ChooseBestDetection(DetectionBands, BandBase, False)
                If bestAwnser(0) <> "" Then
                    UsedRanges.Add((bestAwnser(1), bestAwnser(2)))
                    Return bestAwnser(0)
                End If
            End If

            If DetectionBands.Count = 1 Then
                bestBand = DetectionBands(0).Value
                IndexOfBestFreq = DetectionBands(0).Index
                lengthOfBestFreq = DetectionBands(0).Length
            End If

            If bestBand <> "" Then
                UsedRanges.Add((IndexOfBestFreq, lengthOfBestFreq))
                Return bestBand
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
                        UsedRanges.Add((cleaned.IndexOf(d.Key), d.Key.Length))
                        Return d.Value
                    End If
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
            s = s.Replace("I", "1")
            s = s.Replace("L", "1")
            s = s.Replace("T", "7")
            s = s.Replace(",", ".")
            '          s = s.Replace("/", ".")
            s = s.Replace("-", " ")
            s = s.Replace("¥", "7")
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
                    If front Then
                        If (s < shortestDistance) AndAlso (s < 150) AndAlso (f.Index < .Index) Then
                            bestValue = f.Value
                            IndexOfBestValue = f.Index
                            lengthOfBestValue = f.Length
                            shortestDistance = s
                        End If
                    Else
                        If (s < shortestDistance) AndAlso (s < 150) AndAlso (.Index + .Length - 1 < f.Index) Then
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

    Private Shared Function ConvertFreqToBand(freq As String) As String
        ' 周波数(文字列) → バンド変換
        Dim f As Double
        If Double.TryParse(freq, f) Then

        Else
            Return ""
        End If

        Return ConvertFreqToBand(f)
    End Function

    Private Shared Function ConvertFreqToBand(freq As Double) As String
        ' 周波数(数値) → バンド変換
        For Each b In FreqTable
            If freq >= b.Low AndAlso freq <= b.High Then
                Return b.Name
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
