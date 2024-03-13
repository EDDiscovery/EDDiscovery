grep -r "<HintPath>..\\..\\packages" -rep "<HintPath>$(SolutionDir)\\packages" .csproj
grep -r "<HintPath>..\\packages" -rep "<HintPath>$(SolutionDir)\\packages" .csproj

