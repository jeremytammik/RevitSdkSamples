#
# gen.py - parse Revit SDK Samples database and generate input for menu generator external application
#
# Copyright (C) 2007 Jeremy Tammik, Autodesk Inc.
#
# Read a file listing all Revit SDK samples with certain attributes
# and generate an input file for the menu generator which defines a menu
# classifying the samples by programming topic, level of complexity 
# advanced or basic, base platform Architectural, Structural or MEP, 
# history, programming language C# or VB.NET etc.
#
# We do not really need the key, right now, we assume (and assert)
# that all the names (ECName) are unique by themselves.
#
# Menu structure:
#
# RvtSamples
#   By version
#     8.0
#     8.1
#     9.0
#     9.1
#     2008.0
#     2008.2
#   By platform
#     All
#     Architecture
#     Structure
#   By language
#     CS
#     VB
#   By topic
#     Elements
#     Parameters
#     Creation
#     Geometry
#     Structure
#     View
#     Misc
#
# History:
#
# 2007-08-23 initial implementation
# 2007-08-29 use tab delimited text file exported from Revit_SDK_Samples.xlsx as input 
#
import os.path, re

sdk_root = 'C:/a/lib/revit/2008/SDK/Samples/'
input_filename = 'C:/a/j/adn/train/revit/rac_api/Revit_SDK_Samples.txt'
output_filename = sdk_root + 'RvtSamples/RvtSamples.txt'

#
# record structure in Revit_SDK_Samples.txt:
#
# Key ECName ECDescription ECClassName ECAssembly Flavour Version Level Type Lang Topic Files Bytes Notes
#
KEY = 0
NAME = 1
DESC = 2
CLS = 3
ASM = 4
FLAV = 5
VER = 6
LVL = 7
TYP = 8
LANG = 9
TOP = 10
NFIL = 11
NBYT = 12
NOTE = 13

flavours = ( 'all', 'rac', 'rst', 'rme' )
versions = ( '8.0', '8.1', '9.0', '9.1', '2008.0', '2008.2' )
levels = ( 'basic', 'intermediate', 'advanced' )
types = ( 'cmd', 'app', 'exe' )
languages = ( 'cs', 'vb' )
#topics = { 'create', 'element', 'geometry', 'parameter', 'misc' }

default_data = ( '', '', '', '', '', 'all', '8.0', 'basic', 'cmd', 'cs', '', '', '', '' )

nfields = len( default_data )

def void_or_in_list( x, L ):
    return '' == x or x in L

#
# assert_valid_record - database integrity check:
#
def assert_valid_record( a ):
    assert( void_or_in_list( a[LVL], levels ) )
    assert( void_or_in_list( a[TYP], types ) )
    assert( void_or_in_list( a[LANG], languages ) )
    assert( void_or_in_list( a[VER], versions ) )
    assert( void_or_in_list( a[FLAV], flavours ) )

def add_entry( d, key, e ):
    "Add the given entry 'e' to the container d, which maintains a list for each key."
    if not d.has_key( key ): d[key] = [e]
    else: d[key].append( e )

def capitalise( s ):
    "Capitalise first letter of word."
    return s[0].upper() + s[1:]
    
def emit_entries( category, d, by_name ):
    "Emit menu entry definition data for the given category."
    root = '/RvtSamples/By ' + capitalise( category ) + '/'
    keys = d.keys()
    keys.sort()
    for key in keys:
        names = d[key]
        names.sort()
        if isinstance( key, float ): key = '%.1f' % key
        else: key = capitalise( key )
        for name in names:
            print '\n' + root + key + '/' + capitalise( name )
            for line in by_name[name]:
                print line
    
def main():
    f = open( input_filename )
    lines = f.readlines()
    f.close()
    count = lineno = 0
    #by_key = {}
    by_name = {}
    by_flavour = {}
    by_version = {}
    by_level = {}
    by_language = {}
    by_topic = {}
    for line in lines:
        lineno += 1
        a = line.split( '\t' )
        assert( nfields == len( a ) )
        if 1 < lineno:
            assert_valid_record( a )
            key = a[KEY]
            name = a[NAME].strip( '"' )
            desc = a[DESC].strip( '"' )
            className = a[CLS]
            assemblyPath = a[ASM]
            typ = a[TYP]
            if 'cmd' == typ:
                count += 1
                #assert( not by_key.has_key( key ) )
                #by_key[key] = [name, className, assemblyPath]
                assert( not by_name.has_key( name ) )
                by_name[name] = [desc, sdk_root + assemblyPath, className]
                add_entry( by_flavour, a[FLAV], name )
                add_entry( by_version, float( a[VER] ), name )
                level = a[LVL]
                if not level: level = 'basic'
                add_entry( by_level, level, name )
                add_entry( by_language, a[LANG], name )
                add_entry( by_topic, a[TOP], name )

    print '#', lineno, 'lines,', count, 'external commands defined.'
    
    #keys = by_topic.keys()
    #keys.sort()
    #for key in keys: print ' ', key, by_topic[key]

    #
    # generate menu entry definitions for RvtSamples external application:
    #
    #f = open( output_filename, 'w' )
    #f.close()
    
    '''
    root = '/RvtSamples/By Flavour/'
    keys = by_flavour.keys()
    keys.sort()
    for key in keys:
        names = by_version[key]
        names.sort()
        isinstance( key, float ): key = '%.1f' % key
        else: key = key[0].upper() + key[1:]
        for name in names:
            print '\n' + root + key + '/' + name[0].upper() + name[1:]
            for line in by_name[name]:
                print line

    root = '/RvtSamples/By Version/'
    keys = [float( key ) for key in by_version.keys()]
    keys.sort()
    for key in keys:
        names = by_version[key]
        names.sort()
        key = '%.1f' % key
        for name in names:
            print '\n' + root + key + '/' + name[0].upper() + name[1:]
            for line in by_name[name]:
                print line
    '''
    emit_entries( 'flavour', by_flavour, by_name )
    emit_entries( 'level', by_level, by_name )
    #
    # if the revit menu gets too big, an exception 
    # System.InvalidCastException "Specified cast is not valid."
    # is thrown in MenuItem.Append() when adding the menu item
    # /RvtSamples/By Version/9.0/Material Properties VB.
    #
    # we can avoid that by reducing the size, for instance by 
    # commenting out the following line:
    #
    #emit_entries( 'language', by_language, by_name )
    emit_entries( 'topic', by_topic, by_name )
    emit_entries( 'version', by_version, by_name )
   
if __name__ == '__main__':
    main()
