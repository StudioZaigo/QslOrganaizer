param(
    [Parameter(Mandatory=$true)]
    [string]$SourceFolder,

    [Parameter(Mandatory=$true)]
    [string]$DestinationFolder,

    [Parameter(Mandatory=$true)]
    [ValidateSet("Q","H")]
    [string]$FolderMode,   # Q = QslOrganizer, H = HAMLOG

    [switch]$RenameOutput  # -RenameOutput を付けたらリネーム動作
)

# Callsign 正規表現
# 1桁の数字(0〜1回) + 英字1〜3 + 数字1〜5 + 英字1 + 英数字0〜8
$CallsignPattern = '^(?:[1-9]?)([A-Z]{1,3})([0-9]{1,5})([A-Z][A-Z0-9]{0,8})$'

# 許可する拡張子
$ValidExtensions = @(".jpg",".jpeg",".png",".bmp",".gif",".tiff")

# 区切り文字
$Delimiters = '[ _\-\(\)/\.]'

function Get-CallsignFromFilename($filename) {
    $name = [System.IO.Path]::GetFileNameWithoutExtension($filename)

    # Callsign の後に区切り文字がある場合
    if ($name -match "^(.+?)$Delimiters") {
        $cs = $Matches[1]
    }
    else {
        # Callsign のみ
        $cs = $name
    }

    if ($cs -match $CallsignPattern) {
        return $cs
    }
    return $null
}

function Get-DestinationSubfolder($callsign, $mode) {

    $prefix2 = $callsign.Substring(0,2)
    $prefix3 = $callsign.Substring(0,3)
    $prefix1 = $callsign.Substring(0,1)

    $isDomestic =
        ($prefix2 -ge "JA" -and $prefix2 -le "JS") -or
        ($prefix2 -ge "7J" -and $prefix2 -le "7N") -or
        ($prefix2 -ge "8J" -and $prefix2 -le "8N")

    if ($mode -eq "Q") {
        if ($isDomestic) {
            return $prefix3
        } else {
            return $prefix1
        }
    }
    elseif ($mode -eq "H") {
        if ($isDomestic) {
            return $prefix2
        } else {
            return ""   # 海外局はフォルダを作らない
        }
    }
}

function Get-NextSequenceNumber($folder, $callsign) {
    $files = Get-ChildItem -Path $folder -File -ErrorAction SilentlyContinue |
             Where-Object { $_.Name -match "^$callsign`_([0-9]{3})" }

    if ($files.Count -eq 0) { return "001" }

    $max = ($files | ForEach-Object {
        if ($_.Name -match "^$callsign`_([0-9]{3})") {
            [int]$Matches[1]
        }
    } | Measure-Object -Maximum).Maximum

    return "{0:000}" -f ($max + 1)
}

# --- メイン処理 ---

$allFiles = Get-ChildItem -Path $SourceFolder -Recurse -File

foreach ($file in $allFiles) {

    # 拡張子チェック
    if ($ValidExtensions -notcontains $file.Extension.ToLower()) {
        continue
    }

    # Callsign 抽出
    $callsign = Get-CallsignFromFilename $file.Name
    if (-not $callsign) {
        continue
    }

    # 行先フォルダ決定
    $sub = Get-DestinationSubfolder $callsign $FolderMode
    $dest = if ($sub -eq "") { $DestinationFolder } else { Join-Path $DestinationFolder $sub }

    if (-not (Test-Path $dest)) {
        New-Item -ItemType Directory -Path $dest | Out-Null
    }

    # 出力ファイル名
    if ($RenameOutput) {
        if ($file.BaseName -match "^$callsign`_[0-9]{8}_[0-9]{4}") {
            # 既に YYYYMMDD_HHNN 形式 → そのまま
            $newName = $file.Name
        }
        else {
            # Callsign_nnn 形式に変換
            $seq = Get-NextSequenceNumber $dest $callsign
            $newName = "$callsign" + "_" + "$seq" + $file.Extension
        }
    }
    else {
            # Callsign_nnn 形式に変換
            $seq = Get-NextSequenceNumber $dest $callsign
            $newName = "$callsign" + "_" + "$seq" + $file.Extension
        # リネームなし → 同名があればスキップ
#        if (Test-Path (Join-Path $dest $file.Name)) {
#            continue
#        }
#        $newName = $file.Name
    }

    # Move
    Move-Item -Path $file.FullName -Destination (Join-Path $dest $newName)
}

# --- 空フォルダ削除 ---
$folders = Get-ChildItem -Path $SourceFolder -Recurse -Directory |
           Sort-Object FullName -Descending

foreach ($folder in $folders) {
    $remaining = Get-ChildItem -Path $folder.FullName
    if ($remaining.Count -eq 0) {
        Remove-Item $folder.FullName
    }
}