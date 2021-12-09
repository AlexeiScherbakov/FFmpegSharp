Add-Type -assembly "system.io.compression.filesystem"

# NuGet http://server.name/api/v3/index.json
# BaGet http://server.name/v3/index.json
$nuget_server = $Env:PUSH_NUGET_SERVER
$nuget_server_api_key = $Env:PUSH_NUGET_SERVER_APIKEY

if ($null -eq $nuget_server)
{
    Write-Output "PUSH_NUGET_SERVER env var is empty"
    return;
}

if ($null -eq $nuget_server_api_key)
{
    Write-Output "PUSH_NUGET_SERVER_APIKEY env var is empty"
    return;
}


$files = Get-ChildItem ".\artifacts"  -Filter *.nupkg

foreach ($f in $files)
{
    Write-Output $f.FullName
    $zip = [io.compression.zipfile]::OpenRead($f.FullName)
    $files = $zip.Entries | where-object { $_.Name.EndsWith(".nuspec")}
    if ($files.Count -eq 0)
    {
        Write-Output "Invalid nupkg"
        continue
    }
    $file=$files[0]
    $stream = $file.Open()
    $streamReader = New-Object -TypeName System.IO.StreamReader -ArgumentList $stream
    $xml = $streamReader.ReadToEnd();
    $streamReader.Dispose()
    $stream.Dispose()
    $zip.Dispose()
    [System.Xml.XmlDocument]$xmlDoc = new-object System.Xml.XmlDocument
    $xmlDoc.LoadXml($xml);
    $id= $xmlDoc.SelectSingleNode("/*[local-name() = 'package']/*[local-name() = 'metadata']/*[local-name() = 'id']").InnerText
    $version= $xmlDoc.SelectSingleNode("/*[local-name() = 'package']/*[local-name() = 'metadata']/*[local-name() = 'version']").InnerText

    $output = nuget search $id -Source "$nuget_server" -Prerelease -ForceEnglishOutput
    $searchString="> $id | $version"
    $isExists = $false
    foreach($line in $output)
    {
        $position=$line.IndexOf($searchString)
        if ($position -ge 0)
        {
            $isExists = $true
            break;
        }
    }
    

    if ($isExists -eq $true)
    {
        Write-Output "Package $id | $version is already exists"
        continue
    }

    $output = nuget push $f.FullName -Source $nuget_server -ForceEnglishOutput -ApiKey $nuget_server_api_key
}