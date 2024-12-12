// (C) Copyright 2002-2008 by Autodesk, Inc. 
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted, 
// provided that the above copyright notice appears in all copies and 
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting 
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS. 
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC. 
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to 
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//

//-----------------------------------------------------------------------------
#include "StdAfx.h"
#include "Utils.h"

//-----------------------------------------------------------------------------
MyXData::MyXData () {
	mRevitID =0 ;
	mForce =0.0 ;
	mSection.Empty () ;
	mUsage =-1 ;
}

//-----------------------------------------------------------------------------
/*static*/ bool MyXData::InitializeFromXData (MyXData &xData, CComVariant &xType, CComVariant&xValues) {
	if (   V_VT(&xType) == VT_EMPTY || V_VT(&xValues) == VT_EMPTY
		|| xType.parray->rgsabound->cElements <= 0
		|| xValues.parray->rgsabound->cElements <= 0
	)
		return (false) ;

	for ( long k =0 ; k < xValues.parray->rgsabound->cElements ; k++ ) {
		unsigned short typ ;
		CComVariant val ;
		SafeArrayGetElement (xType.parray, &k, (void *)&typ) ;
		SafeArrayGetElement (xValues.parray, &k, (void *)&val) ;
		switch ( typ ) {
			case 1070: xData.mUsage =val.iVal ; break ;
			//MS
			//case 1040: xData.mForce =val.dblVal ; break ;
			case 1000: xData.mSection =(TCHAR *)_bstr_t (val.bstrVal, true) ; break ;
			case 1071: xData.mRevitID =val.lVal ; break ;
		}
	}
	return (true) ;
}

//-----------------------------------------------------------------------------
/*static*/ bool MyXData::PrepareForSaveXData (MyXData &xData, CComVariant &xType, CComVariant &xValues) {
	VariantClear (&xType) ;
	V_VT(&xType) =VT_I2 | VT_ARRAY ;
	V_ARRAY(&xType) =xData.asSafeArrayPtr (false) ;

	VariantClear (&xValues) ;
	V_VT(&xValues) =VT_VARIANT | VT_ARRAY ;
	V_ARRAY(&xValues) =xData.asSafeArrayPtr (true) ;

	return (true) ;
}

//-----------------------------------------------------------------------------
SAFEARRAY *MyXData::asSafeArrayPtr (bool bValues) {
	SAFEARRAYBOUND rgsaBound ;
	rgsaBound.lLbound =0L ;
//MS
	//rgsaBound.cElements =4L + (mUsage != -1 ? 1L : 0L) ;
	rgsaBound.cElements =3L + (mUsage != -1 ? 1L : 0L) ;
	SAFEARRAY *pArray =SafeArrayCreate (bValues ? VT_VARIANT : VT_I2, 1, &rgsaBound) ;
	long i =0 ;
	if ( bValues ) {
		SafeArrayPutElement (pArray, &i, (LPVOID)&(CComVariant (_T("RSMember")))) ;
		i++ ;
		SafeArrayPutElement (pArray, &i, (LPVOID)&(CComVariant (mRevitID))) ;
		i++ ;
		//SafeArrayPutElement (pArray, &i, (LPVOID)&(CComVariant (mForce))) ;
		//i++ ;
		SafeArrayPutElement (pArray, &i, (LPVOID)&(CComVariant (mSection))) ;
		i++ ;
		if ( mUsage != -1 )
			SafeArrayPutElement (pArray, &i, (LPVOID)&(CComVariant (mUsage))) ;
	} else {
		unsigned short val =1001 ;
		SafeArrayPutElement (pArray, &i, (LPVOID)&(val)) ;
		i++ ;
		val =1071 ;
		SafeArrayPutElement (pArray, &i, (LPVOID)&(val)) ;
		//MS
		/*i++ ;
		val =1040 ;
		SafeArrayPutElement (pArray, &i, (LPVOID)&(val)) ;*/
		i++ ;
		val =1000 ;
		SafeArrayPutElement (pArray, &i, (LPVOID)&(val)) ;
		i++ ;
		val =1070 ;
		if ( mUsage != -1 )
			SafeArrayPutElement (pArray, &i, (LPVOID)&(val)) ;
	}
	return (pArray) ;
}