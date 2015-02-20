@echo off



REM Tar up the web site
rm ../www.tar
tar -cf ../www.tar ../.www


putty login.ccs.neu.edu -l timski -pw tde8Pa.fu -v -m hostweb.ccs.putty.pre


psftp login.ccs.neu.edu -l timski -pw tde8Pa.fu -b hostweb.ccs.psftp -bc -batch


putty login.ccs.neu.edu -l timski -pw tde8Pa.fu -v -m hostweb.ccs.putty.post


echo.
echo.
echo.
echo Done.

