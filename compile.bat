call "C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\vcvarsall.bat" x86_amd64

del *.exe
csc^
 /r:System.Data.SQLite.dll^
 /t:winexe^
 /out:MPR.exe^
  SetSettings\*cs Main\*cs GVars\*cs Auth\*cs Settings\*cs Locale\*cs Logger\*cs MainWindow\*cs Overwiew\*cs ResultList\*cs UserGuide\*cs Sqldb\*cs Auth\*cs ResultDataGrid\*cs DataBaseSettings\*cs

cl^
 /EHsc  /Fo.\.obj\ mprc\src\*.c /link /out:mprc.exe >nul

csc^
 /t:winexe^
 /out:mpr_tst.cs.exe^
 mpr_tst\src\*cs mpr_tst\*cs

