param(
    [Parameter(Mandatory=$true)]
    [string]$SourceFolder
)

# Callsign 正規表現
# 1桁の数字(0〜1回) + 英字1〜3 + 数字1〜5 + 英字1 + 英数字0〜8
$CallsignPattern = '^([1-9]?[A-Z]{1,4}[0-9]{1,5}[A-Z][A-Z0-9]{0,8})$'
$SwlPattern = '^([1-9]?[A-Z]{1,3}[0-9]{1,5}-[0-9]{0,8})$'

# --- 設定 ---
$ValidExt = @(".jpg",".jpeg",".png",".bmp",".gif",".tiff")
$Delimiters = '[ _\-\(\)/\.]'
$SwlDelimiters = '[ _\(\)/\.]'

# --- Callsign からフォルダ名を決定する関数 ---
function Get-TargetFolderName($callsign) {

    # 2桁callsignはない
    if ($callsign.length -le 2) {name
    }

    $prefix1 = $callsign.Substring(0,1).ToUpper()
    $prefix2 = $callsign.Substring(0,2).ToUpper()
    # "K1K"のような3桁callsignの対処
    if ($callsign.length -gt 3) {
        $prefix3 = $callsign.Substring(0,3).ToUpper()
    }
    else {
        $prefix3 = $suffix2
    }

    # 国内局判定（JA～JS、7J～7N、8J～8N）
    if ($prefix2 -match '^(JA|JB|JC|JD|JE|JF|JG|JH|JI|JJ|JK|JL|JM|JN|JO|JP|JQ|JR|JS)$') {
        return $prefix3
    }
    elseif ($prefix2 -match '^(7J|7K|7L|7M|7N)$') {
        return $prefix3
    }
    elseif ($prefix2 -match '^(8J|8K|8L|8M|8N)$') {
        return $prefix3
    }
    else {
        return $prefix1   # 海外局は頭1桁
    }
}

# --- Callsign を抽出する関数 ---
function Get-Callsign($filename) {

    # 拡張子チェック
    $ext = [IO.Path]::GetExtension($filename).ToLower()
    if ($ValidExt -notcontains $ext) { return $null }

    # ファイル名 抽出（拡張子除き）
    $name = [IO.Path]::GetFileNameWithoutExtension($filename)

    # Callsign 抽出
    if ($name -match "^(.+?)$Delimiters") {
        $cs = $Matches[1]
    }    
    else {
        $cs = $name
    }    

    if ($cs -match $CallsignPattern) {
        return $cs
    }
    return $null
}

# --- SWL No を抽出する関数 ---
function Get-SwlNo($filename) {

    # 拡張子チェック
    $ext = [IO.Path]::GetExtension($filename).ToLower()
    if ($ValidExt -notcontains $ext) { return $null }

    # ファイル名 抽出（拡張子除き）
    $name = [IO.Path]::GetFileNameWithoutExtension($filename)

      # SWL No 抽出
    if ($name -match "^(.+?)$SwlDelimiters") {
        $cs = $Matches[1]
    }    
    else {
        $cs = $name
    }    

    if ($cs -match $SwlNoPattern) {
        return $cs
    }
    return $null
}



# --- メイン処理 ---

# コンソール出力を UTF-8 に変更
#$OutputEncoding = [Console]::OutputEncoding = [Text.UTF8Encoding]::UTF8
#[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

# Pending フォルダーのパス
$PendinFolderName = "_Pending"
$PendingFolder = Join-Path $SourceFolder $PendinFolderName

# Pending フォルダーが無ければ作成
if (-not (Test-Path $PendingFolder)) {
    New-Item -ItemType Directory -Path $PendingFolder | Out-Null
}

# 対象のフォルダーのリストを作成
$subFolders = Get-ChildItem -Path $SourceFolder -Directory

# フォルダの総数を数える（進捗計算用）
$totalCount = $subFolders.Count
$counter = 0

Write-Host "QslTidyupを処理中..."
Write-Host "────────────────────────────────────────"

foreach ($folder in $subFolders) {

    Start-Sleep -Milliseconds 50 # 動きを見るための仮ウェイト
    # カウンターを進める
    $counter++

    # プログレスバーを表示
    $percent = [int](($counter / $totalCount) * 100)
    write-progress -Activity "データ処理中" -Status "$counter / $totalCount complete..." -PercentComplete $percent
    
    # Pending フォルダーは対象外 
    if ($folder.Name -eq $PendingFolder)  {continue }

    # 対象のファイルのリストを作成
    $files = Get-ChildItem -Path $folder.FullName -File

    foreach ($file in $files) {

        $callsign = Get-Callsign($file.Name)
        if ($null -eq $callsign) {
            $callsign = Get-SwlNo($file.Name)
            if ($null -eq $callsign) {
                continue    # 対象外
            }
        }

        $targetFolderName = Get-TargetFolderName($callsign)
        $targetFolderPath = Join-Path $SourceFolder $targetFolderName

        if (-not (Test-Path $targetFolderPath)) {
            New-Item -ItemType Directory -Path $targetFolderPath | Out-Null
        }

        # フォルダ名が一致しない場合のみ移動
        if ($folder.Name -ne $targetFolderName) {

            $destFile = Join-Path $targetFolderPath $file.Name

            if (Test-Path $destFile) {
                # 重複ファイルは Pending へ退避
                $PendingFile = Join-Path $PendingFolder $file.Name

                # Pending に同名がある場合はさらに番号を付ける
                if (Test-Path $PendingFile) {
                    $base = [IO.Path]::GetFileNameWithoutExtension($file.Name)
                    $ext  = [IO.Path]::GetExtension($file.Name)
                    $i = 1
                    do {
                        $PendingFile = Join-Path $PendingFolder ("${base}_dup${i}${ext}")
                        $i++
                    } while (Test-Path $PendingFile)
                }

                Move-Item -Path $file.FullName -Destination $PendingFile
                Write-Host "重複のため $PendingFileフォルダー に退避: $($destFile.Name)"
            }
            else {
                Move-Item -Path $file.FullName -Destination $targetFolderPath
            }
        }

        # --- フォルダ削除判定 ---
        $remaining = Get-ChildItem -Path $folder.FullName -File
        $validRemaining = $remaining | Where-Object {
            $ValidExt -contains ([IO.Path]::GetExtension($_.Name).ToLower())
        }

        if ($validRemaining.Count -eq 0 -and $remaining.Count -eq 0) {
            Remove-Item -Path $folder.FullName -Force
        }
    }
}

if (Test-Path $PendingFolder) {
    $remaining = Get-ChildItem -Path $PendingFolder -file
    if ($remaining.Count -eq 0) {
        Remove-Item $PendingFolder
    }
}

Write-Host "QslTidyupが終了しました"
