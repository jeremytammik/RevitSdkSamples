#
# walk.py - determine source file number and size of Revit SDK samples
#
# Copyright (C) 2007 Jeremy Tammik, Autodesk Inc.
#
# History:
#
# 2007-08-29 initial implementation
#
import os
from os.path import join, getsize

def ext( s ):
    n = len( s )
    return s[-3:]

sdkroot = 'C:/a/lib/revit/2008/SDK/Samples'
n = len( sdkroot ) + 1

print '%6s %7s  %s' % ('files', 'bytes', 'project')
print '%6s %7s  %s' % ('-----', '-----', '-------')

for root, dirs, files in os.walk( sdkroot ):
    #print files
    srcfiles = [name for name in files if ext( name) in ['.cs', '.vb' ]]
    #print srcfiles
    nbytes = sum( getsize( join( root, name ) ) for name in srcfiles )
    if 0 < nbytes:
        #print root[n:], 'consumes', nbytes,
        #print 'bytes in', len( srcfiles ), 'cs or vb source files'
        print '%6s %7s  %s' % (len( srcfiles ), nbytes, root[n:])
    # don't visit certain subdirectories
    for a in ['bin', 'obj', 'Properties', 'My Project']:
        if a in dirs:
            dirs.remove( a )
