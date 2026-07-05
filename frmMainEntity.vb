Imports System.Diagnostics.Metrics
Imports System.IO
Imports System.Text.Json.Nodes
Imports System.Text.RegularExpressions
Imports System.Xml
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Partial Class frmMain

    Private Const CtyFilePath As String = "cty.csv"
    Private Const CtyCachePath As String = "cty.json"
    Private Shared CtyDatabase As List(Of PrefixInfo)

    Private Class PrefixInfo
        Public Property Prefix As String
        Public Property Entity As String
        Public Property Continent As String
    End Class

    Public Sub LoadCtyDatabase()

        Dim currentStamp = File.GetLastWriteTime(CtyFilePath)       ' CtyDatファイルのタイムスタンプを取得
        '    Dim savedStamp As DateTime = DateTime.MinValue

        '        Dim form As New frmSetting

        ' 設定を読み込み
        AppSettings.LoadJson(AppSettings.SettingsFile)
        Dim JSONsavedStamp = AppSettings.GetJson("CtyDat", "TimeStamp", "")

        Dim savedStamp As DateTime
        If Not DateTime.TryParse(JSONsavedStamp, savedStamp) OrElse currentStamp <> savedStamp Then
            ' 新しい cty.dat を読み込む
            CtyDatabase = ParseCtyDat(CtyFilePath)

            ' JSON に保存
            Dim json = JsonConvert.SerializeObject(CtyDatabase, Newtonsoft.Json.Formatting.Indented)
            File.WriteAllText(CtyCachePath, json)

            ' タイムスタンプ保存（ISO 形式で保存）
            AppSettings.SetJson("CtyDat", "TimeStamp", currentStamp)
            AppSettings.SaveJson(AppSettings.SettingsFile)
        Else
            ' JSON から読み込む
            Dim json = File.ReadAllText(CtyCachePath)
            CtyDatabase = JsonConvert.DeserializeObject(Of List(Of PrefixInfo))(json)
        End If

    End Sub

    Private Function ParseCtyDat(path As String) As List(Of PrefixInfo)
        Dim list As New List(Of PrefixInfo)

        For Each line In File.ReadLines(path)
            If line.StartsWith("#") OrElse String.IsNullOrWhiteSpace(line) Then Continue For

            ' cty.dat の形式に合わせてパース（簡略版）
            Dim parts = line.Split(","c)
            Dim prefixs = parts(9).Trim()
            Dim Entity = parts(0).Trim().ToUpper()
            Dim continent = parts(3).Trim()

            prefixs = prefixs.Replace(" ", ";")
            Dim p = prefixs.Split(";"c)

            For Each prefix In p
                If prefix = "" Then Continue For
                If prefix.Substring(0, 1) = "=" Then Exit For

                Dim pattern = "\([A-Z0-9]+\)|\[[A-Z0-9]+\]"
                prefix = Regex.Replace(prefix, pattern, "")

                list.Add(New PrefixInfo With {
                .Prefix = prefix,
                .Entity = Entity,
                .Continent = continent
                })
            Next
        Next

        Return list
    End Function

    '   使い方
    '    Dim (country, continent) = LookupCountryAndContinent(callsign)
    '    Dim format = GetDateFormat(country, continent)


    Public Shared Function LookupEntityyAndContinent(callsign As String) As (Entity As String, Continent As String)

        Dim cs = callsign.ToUpper()

        ' Prefix + suffix の最初の1文字
        Dim len As Int32 = cs.Length
        If len > 4 Then cs = cs.Substring(0, 4)
        For length = cs.Length To 1 Step -1
            Dim prefix = cs.Substring(0, length)

            Dim match = CtyDatabase.FirstOrDefault(Function(x) prefix.StartsWith(x.Prefix))
            'Dim match = CtyDatabase.FirstOrDefault(Function(f) f.Prefix = prefix)


            'Dim match As JToken = CtyDatabase.FirstOrDefault(Function(x) x("id").Value(Of Integer)() = 2)

            'Dim match As JsonNode = CtyDatabase.FirstOrDefault(Function(x) x("Prefix").GetValue(Of String)() = prefix)


            If match IsNot Nothing Then
                Return (match.Entity, match.Continent)
            End If
        Next

        Return ("", "")
    End Function


    Public Shared Function GetDateFormat(Entity As String, continent As String) As DateFormats

        Entity = Entity.ToUpper()

        ' ISO式
        Dim IsoCountries = {
            "JA", "HL", "BY", "BV,""A5", "HA", "JT", "LY", "JA/O", "JA/M", "P5"
        }

        If IsoCountries.Contains(Entity) Then
            Return DateFormats.IsoFormat
        End If

        ' アメリカ式
        Dim americanCountries = {
            "K", "KL7", "KH6", "V6", "V7", "T8", "DU"
        }

        If americanCountries.Contains(Entity) Then
            Return DateFormats.AmericanFormat
        End If

        ' ブリチッシュ式
        Dim BritishCountries = {
            "UA9"
        }

        If BritishCountries.Contains(Entity) Then
            Return DateFormats.BritishFormat
        End If

        If continent = "NA" OrElse continent = "SA" Then
            Return DateFormats.AmericanFormat
        End If

        ' その他はイギリス式
        If Entity <> "" Then
            Return DateFormats.BritishFormat
        End If

        Return DateFormats.InvalidFormat
    End Function

End Class