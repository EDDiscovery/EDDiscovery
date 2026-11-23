rgrep "..\..\packages" /r .csproj -rep "$(SolutionDir)\packages"
rgrep "..\packages" /r .csproj -rep "$(SolutionDir)\packages"

