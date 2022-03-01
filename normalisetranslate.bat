eddtest normalisetranslate %cd%\eddiscovery\translations 2 example-ex deutsch-de NS renames.lst
call copyback.bat
eddtest normalisetranslate %cd%\eddiscovery\translations 2 example-ex chinese-zh NS renames.lst
call copyback.bat
eddtest normalisetranslate %cd%\eddiscovery\translations 2 example-ex francais-fr NS renames.lst
call copyback.bat
eddtest normalisetranslate %cd%\eddiscovery\translations 2 example-ex italiano-it NS  renames.lst
call copyback.bat
eddtest normalisetranslate %cd%\eddiscovery\translations 2 example-ex polski-pl NS renames.lst
call copyback.bat
eddtest normalisetranslate %cd%\eddiscovery\translations 2 example-ex portugues-pt-br NS renames.lst
call copyback.bat
eddtest normalisetranslate %cd%\eddiscovery\translations 2 example-ex russian-ru NS renames.lst
call copyback.bat
eddtest normalisetranslate %cd%\eddiscovery\translations 2 example-ex spanish-es NS renames.lst
call copyback.bat
del copyback.bat
del report.txt
