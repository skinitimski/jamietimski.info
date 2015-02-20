@echo off


:: Get the date/time
for /F "tokens=1-4 delims=/ " %%i IN ('date /t') DO (
set DT_DAY=%%i
set DT_MM=%%j
set DT_DD=%%k
set DT_CCYY=%%l)

:: Set the CCYYMMDD timestamp
set DT=%DT_CCYY%%DT_MM%%DT_DD%

:: Set the filenames to CCYYMMDD-timski
set TAR_FILENAME=%DT%-timski.tar
set GZIP_FILENAME=%DT%-timski.tar.gz


:: Remove old .www folder
echo Cleaning .www folder...
rd /s /q ..\.www

:: Remove all "archive" files from the data folder
echo Cleaning archives...
rm -fv .\*-timski.tar.gz



:: Create a new snapshot archive
echo Generating archive...

cd ..\..\
tar -cf %TAR_FILENAME% timski
gzip %TAR_FILENAME%
mv %GZIP_FILENAME% timski\data

cd timski\data

:: Create the web folder
mkdir ..\.www




if "%1" == "" goto generate


:gallery

echo Generating gallery...
generategallery "%1"


:generate

echo Transforming pages...
transformpages

echo Generating calendars...
generatecalendars

:: echo Generating homepage...
:: generateindex


echo Copying files...
batchCopy

echo Generating host/cleanup scripts...
generatehostscripts






:done
time /t
echo Done

goto end


:end



