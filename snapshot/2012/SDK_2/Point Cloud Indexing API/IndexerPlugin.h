//
//  (C) Copyright 2011 by Autodesk, Inc.
//
//
//  The information contained herein is confidential, proprietary to Autodesk,
//  Inc., and considered a trade secret as defined in section 499C of the
//  penal code of the State of California.  Use of this information by anyone
//  other than authorized employees of Autodesk, Inc. is granted only under a
//  written non-disclosure agreement, expressly prescribing the scope and
//  manner of such use.
//
//
//         IndexerPlugin.h
//
//   Point Cloud Decimation Engine Codec File Indexer Plugin class definition
//

#ifndef __INDEXER_PLUGIN_H__
#define __INDEXER_PLUGIN_H__

#pragma once

#include <CodecFileReader.h>

////////////////////////////////////////////////////////////////////////////////////////////////////
// Class IndexerPlugin

class IIndexerPlugin
{
// construction/destruction
public:
  virtual ~IIndexerPlugin() {}

// Interface
public:
   virtual const wchar_t* getCodecs() = 0 ; // semi-colon delimited list of "codecs" (Extensions) supported by the plugin
                                            // i.e. (.ptg;.ptx)
   virtual CodecFileReader* getFileReader(const wchar_t* codec) = 0;  // return the reader for the specified codec.
   virtual bool canImport(const wchar_t* inputFile) = 0; // true if supports the specified input/codec. 
   virtual void destroy() = 0;  // called when we are done with the plugin and it should destroy itself and all 
                                // resources.
};


// Function implemented by plugin provider to return plugin implementation for the indexer
#if defined(INDEXERPLUGIN_DLL)
# define INDEXERPLUGIN_API __declspec(dllexport)
#else
# define INDEXERPLUGIN_API __declspec(dllimport)
#endif

extern "C" INDEXERPLUGIN_API IIndexerPlugin* getIndexerPlugin();

#endif //  __INDEXING_PLUGIN_H__

