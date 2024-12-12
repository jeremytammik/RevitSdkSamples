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
//----- Section.h : Declaration of the CSection
//-----------------------------------------------------------------------------
#pragma once
#include "resource.h"

#include "Miro.h"
#include "Utils.h"

//----- CSection
class ATL_NO_VTABLE CSection : 
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CSection, &CLSID_Section>,
	public ISupportErrorInfo,
	public IDispatchImpl<ISection, &IID_ISection, &LIBID_AsdkMiroLib, /*wMajor =*/ 2, /*wMinor =*/ 0>,
	public IAcPiCategorizeProperties,
	public IDynamicProperty2
{
public:
	CSection () {
	}

	DECLARE_REGISTRY_RESOURCEID(IDR_SECTION)

	BEGIN_COM_MAP(CSection)
		COM_INTERFACE_ENTRY(ISection)
		COM_INTERFACE_ENTRY(IDispatch)
		COM_INTERFACE_ENTRY(ISupportErrorInfo)
		COM_INTERFACE_ENTRY(IAcPiCategorizeProperties)
		COM_INTERFACE_ENTRY(IDynamicProperty2)
	END_COM_MAP()

	//----- ISupportsErrorInfo
	STDMETHOD(InterfaceSupportsErrorInfo)(REFIID riid);

	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct () {
		return (S_OK) ;
	}
	
	void FinalRelease () {
	}

	IDynamicPropertyNotify2 *m_pNotify ;

public:
	//- IDynamicProperty2
	STDMETHOD(GetGUID)(GUID* propGUID) ;
	STDMETHOD(GetDisplayName)(BSTR* bstrName) ;
	STDMETHOD(IsPropertyEnabled)(IUnknown *pUnk, BOOL* pbEnabled) ;
	STDMETHOD(IsPropertyReadOnly)(BOOL* pbReadonly) ;
	STDMETHOD(GetDescription)(BSTR* bstrName) ;
	STDMETHOD(GetCurrentValueName)(BSTR* pbstrName) ;
	STDMETHOD(GetCurrentValueType)(VARTYPE* pVarType) ;
	STDMETHOD(GetCurrentValueData)(IUnknown *pUnk, VARIANT* pvarData) ;
	STDMETHOD(SetCurrentValueData)(IUnknown *pUnk, const VARIANT varData) ;
	STDMETHOD(Connect)(IDynamicPropertyNotify2* pSink) ;
	STDMETHOD(Disconnect)() ;

	//- IAcPiCategorizeProperties
	virtual HINSTANCE GetResourceInstance () { return (_hdllInstance) ; }
	STDMETHOD(GetUniqueID) (BSTR *pVal) { *pVal =::SysAllocString (L"{01B97D1D-4C82-4157-8F9A-2D43DE8ED67B}") ; return (S_OK) ; }
	STDMETHOD(MapPropertyToCategory) (DISPID dispid, PROPCAT *ppropcat) ;
	STDMETHOD(GetCategoryName) (PROPCAT propcat, LCID lcid, BSTR *pbstrName) ;
	STDMETHOD(GetCategoryDescription) (PROPCAT propcat, LCID lcid, BSTR *pbstrDesc) ;
	STDMETHOD(GetCategoryWeight) (PROPCAT CatID, long *pCategoryWeight) ;
	STDMETHOD(GetParentCategory) (PROPCAT CatID, PROPCAT *pParentCatID) ;
	STDMETHOD(GetCommandButtons) (PROPCAT CatID, VARIANT *pCatCmdBtns) ;

	//- ISection

} ;

OBJECT_ENTRY_AUTO(__uuidof(Section), CSection)
OPM_DYNPROP_OBJECT_ENTRY_AUTO(CSection, AcDbLine)
OPM_DYNPROP_OBJECT_ENTRY_AUTO(CSection, AcDbBlockReference)
