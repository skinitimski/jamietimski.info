@echo off


psftp ftp.jamietimski.info -P 2222 -l timski -pw tde8Pa.fu -b hostweb.fat.gallery.clean.psftp -batch

mv -f hostweb.fat.gallery.clean.next.psftp hostweb.fat.gallery.clean.psftp

psftp ftp.jamietimski.info -P 2222 -l timski -pw tde8Pa.fu -b hostweb.fat.gallery.host.psftp -batch


echo.
echo.
echo.
echo Done.

