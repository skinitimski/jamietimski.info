@echo off


psftp ftp.jamietimski.info -P 2222 -l timski -pw tde8Pa.fu -b hostweb.fat.calendars.clean.psftp -batch

mv -f hostweb.fat.calendars.clean.next.psftp hostweb.fat.calendars.clean.psftp

psftp ftp.jamietimski.info -P 2222 -l timski -pw tde8Pa.fu -b hostweb.fat.calendars.host.psftp -batch


echo.
echo.
echo.
echo Done.

