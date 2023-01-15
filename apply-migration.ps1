$name=$args[0]

if ($args.Count -eq 0) {
  $name= Read-Host -Prompt "Enter the name of the Migration in TitleCase"
}

dotnet ef migrations add $name -s ".\src\WorkTimer.Blazor\" -p ".\src\WorkTimer.Persistence\"