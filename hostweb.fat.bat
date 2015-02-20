@echo off


psftp ftp.jamietimski.info -P 2222 -l timski -pw tde8Pa.fu -b hostweb.fat.clean.psftp -batch

mv -f hostweb.fat.clean.next.psftp hostweb.fat.clean.psftp

psftp ftp.jamietimski.info -P 2222 -l timski -pw tde8Pa.fu -b hostweb.fat.host.psftp -batch
psftp ftp.jamietimski.info -P 2222 -l timski -pw tde8Pa.fu -b hostweb.fat.archive.psftp -batch


echo.
echo.
echo.
echo Done.

