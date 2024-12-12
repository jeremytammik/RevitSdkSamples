#
# chkasm.py - parse and check validity of Revit_SDK_Samples.xlsx EC info
#
# Copyright (C) 2007 Jeremy Tammik, Autodesk Inc.
#
# History:
#
# 2007-08-28 initial implementation
# 2007-08-29 updated for improved versions of Revit_SDK_Samples.xlsx
#
import os

def main():
    f = open( 'C:/a/j/adn/train/revit/rac_api/Revit_SDK_Samples.txt' )
    lines = f.readlines()
    f.close()
    exePath = 'C:/j/bin/CheckAsm.exe'
    sdkRoot = 'C:/a/lib/revit/2008/SDK/Samples/'
    for line in lines[1:]:
        a = line.split( '\t' )
        assert( 14 == len( a ) )
        key = a[0]
        className = a[3]
        assemblyPath = a[4]
        typ = a[8]
        if 'exe' != typ:
            args = [exePath, sdkRoot + assemblyPath, className]
            if 'app' == typ: args.append( '-a' )
            rc = os.spawnv( os.P_WAIT, exePath, args )
            if 0 != rc:
                print key, 'returned error code', rc

if __name__ == '__main__':
    main()
