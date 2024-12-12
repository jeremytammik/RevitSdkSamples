// pch.h: This is a precompiled header file.
// Files listed below are compiled only once, improving build performance for future builds.
// This also affects IntelliSense performance, including code completion and many code browsing features.
// However, files listed here are ALL re-compiled if any one of them is updated between builds.
// Do not add files here that you will be updating frequently as this negates the performance advantage.

#ifndef PCH_H
#define PCH_H

// add headers that you want to pre-compile here

#include <afxwin.h>
#include <afxdisp.h>
#include <afxdlgs.h>
#include <afxcmn.h>
#include <atlbase.h>
//You may derive a class from CComModule and use it if you want to override
//something, but do not change the name of _Module
extern CComModule _Module;
#include <atlcom.h>
#include <comdef.h>
#include <vector>
#include <math.h>
#include <string>

#import "AstorMain5.tlb" raw_interfaces_only, raw_native_types, no_namespace, named_guids 
#import "DscOdbcCom.tlb" raw_interfaces_only, raw_native_types, no_namespace, named_guids


#include "resource.h"
#include "SampleClipAngleNative_h.h"

#include "ClipAngle.h"

#include "framework.h"

#endif //PCH_H

