// (C) Copyright 2002-2010 by Autodesk, Inc. 
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
//----- Section.cpp : Implementation of CSection
//-----------------------------------------------------------------------------
#include "StdAfx.h"
#include "Section.h"

//----- CSection
STDMETHODIMP CSection::InterfaceSupportsErrorInfo(REFIID riid) {
	static const IID* arr [] ={
		&IID_ISection
	} ;

	for ( int i =0 ; i < sizeof (arr) / sizeof (arr [0]) ; i++ ) {
		if ( InlineIsEqualGUID (*arr [i], riid) )
			return (S_OK) ;
	}
	return (S_FALSE) ;
}

//----- IDynamicProperty
STDMETHODIMP CSection::GetGUID (GUID *pPropGUID) {
	if ( pPropGUID == NULL )
		return (E_POINTER) ;
	memcpy (pPropGUID, &CLSID_Section, sizeof(GUID)) ;
	return (S_OK) ;
}

STDMETHODIMP CSection::GetDisplayName (BSTR *pBstrName) {
	if ( pBstrName == NULL )
		return (E_POINTER) ;
	*pBstrName =::SysAllocString (L"Section") ;
	return (S_OK) ;
}

STDMETHODIMP CSection::IsPropertyEnabled (IUnknown *pUnk, BOOL *pbEnabled) {
	if ( pUnk == NULL )
		return (E_INVALIDARG) ;
	if ( pbEnabled == NULL )
		return (E_POINTER) ;
	*pbEnabled =TRUE ;
	return (S_OK) ;
}

STDMETHODIMP CSection::IsPropertyReadOnly (BOOL *pbReadOnly) {
	if ( pbReadOnly == NULL )
		return (E_POINTER) ;
	*pbReadOnly =FALSE ;
	return (S_OK) ;
}

STDMETHODIMP CSection::GetDescription (BSTR *pBstrName) {
	if ( pBstrName == NULL )
		return (E_POINTER) ;
	*pBstrName =::SysAllocString (L"Cross-Section Type Code") ;
	return (S_OK) ;
}

STDMETHODIMP CSection::GetCurrentValueName (BSTR *pBstrName) {
	if ( pBstrName == NULL )
		return (E_POINTER) ;
	return (E_NOTIMPL) ;
}

STDMETHODIMP CSection::GetCurrentValueType (VARTYPE *pVarType) {
	if ( pVarType == NULL )
		return (E_POINTER) ;
	*pVarType =VT_BSTR ;
	return (S_OK) ;
}

STDMETHODIMP CSection::GetCurrentValueData (IUnknown *pUnk, VARIANT *pVarData) {
	if ( pUnk == NULL )
		return (E_INVALIDARG) ;
	if ( pVarData == NULL )
		return (E_POINTER) ;

	CComQIPtr<IAcadObject> pObj (pUnk) ;

	MyXData xData ;
	CComVariant xType, xValues ;
	if (   pObj->GetXData (CComBSTR (_T("RSMember")), &xType, &xValues) != S_OK
		|| MyXData::InitializeFromXData (xData, xType, xValues) == false
	)
		return (E_FAIL) ;

	::VariantInit (pVarData) ;
	V_VT(pVarData) =VT_BSTR ;
	V_BSTR(pVarData) =xData.mSection.AllocSysString () ;
	return (S_OK) ;
}

STDMETHODIMP CSection::SetCurrentValueData (IUnknown *pUnk, const VARIANT varData) {
	if ( pUnk == NULL )
		return (E_INVALIDARG) ;

	CComQIPtr<IAcadObject> pObj (pUnk) ;

	MyXData xData ;
	CComVariant xType, xValues ;
	pObj->GetXData (CComBSTR (_T("RSMember")), &xType, &xValues) ;
	MyXData::InitializeFromXData (xData, xType, xValues) ;

	xData.mSection =(TCHAR *)_bstr_t (V_BSTR(&varData), true) ;
	MyXData::PrepareForSaveXData (xData, xType, xValues) ;
	pObj->SetXData (xType, xValues) ;
	return (S_OK) ;
}

STDMETHODIMP CSection::Connect (IDynamicPropertyNotify2 *pSink) {
	if ( pSink == NULL )
		return (E_POINTER) ;
	m_pNotify =pSink ;
	m_pNotify->AddRef () ;
	return (S_OK) ;
}

STDMETHODIMP CSection::Disconnect () {
	if ( m_pNotify ) {
		m_pNotify->Release () ;
		m_pNotify= NULL ;
	}
	return (S_OK) ;
}

//----- IAcPiCategorizeProperties
STDMETHODIMP CSection::MapPropertyToCategory (DISPID dispid, PROPCAT *ppropcat) {
	if ( ppropcat == NULL )
		return (E_POINTER) ;
	*ppropcat =0 ;
	return (S_OK) ;
}

STDMETHODIMP CSection::GetCategoryName (PROPCAT propcat, LCID lcid, BSTR *pbstrName) {
	if ( pbstrName == NULL )
		return (E_POINTER) ;
	if ( propcat != 0 )
		return (E_INVALIDARG) ;
	*pbstrName =::SysAllocString (L"Revit Structure") ;
	return (S_OK) ;
}

STDMETHODIMP CSection::GetCategoryDescription (PROPCAT propcat, LCID lcid, BSTR *pbstrDesc) {
	if ( pbstrDesc == NULL )
		return (E_POINTER) ;
	if ( propcat != 0 )
		return (E_INVALIDARG) ;
	*pbstrDesc =::SysAllocString (L"Xdata specific for Revit Structure Link") ;
	return (S_OK) ;
}

STDMETHODIMP CSection::GetCategoryWeight (PROPCAT CatID, long *pCategoryWeight) {
	if ( pCategoryWeight == NULL )
		return (E_POINTER) ;
	if ( CatID != 0 )
		return (E_INVALIDARG) ;
	*pCategoryWeight =1 ; //- To be first
	return (S_OK) ;
}

STDMETHODIMP CSection::GetParentCategory (PROPCAT CatID, PROPCAT *pParentCatID) {
	if ( pParentCatID == NULL )
		return (E_POINTER) ;
	if ( CatID != 0 )
		return (E_INVALIDARG) ;
	//- Not a child categorie
	return (E_NOTIMPL) ;
}

STDMETHODIMP CSection::GetCommandButtons (PROPCAT CatID, VARIANT *pCatCmdBtns) {
	if ( pCatCmdBtns == NULL )
		return (E_POINTER) ;
	if ( CatID != 0 )
		return (E_INVALIDARG) ;
	//- No buttons in categorie title bar
	return (E_NOTIMPL) ;
}