param(
		[Parameter(Position=0, Mandatory=$true)]
		[ValidateNotNullOrEmpty()]
		[System.String]
		$appName,

		[Parameter(Position=1, Mandatory=$true)]
		[ValidateNotNullOrEmpty()]
		[System.String]
		$projectDir
	)

Set-Location $projectDir.Substring(0,$projectDir.LastIndexOf("\"))
docker build -t $appName -f "$($projectDir)\dockerfile".
docker tag "$($appName):latest" vsimone67/"$($appName):latest"
docker push vsimone67/"$($appName):latest"