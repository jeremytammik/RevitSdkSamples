#if !defined(AFX_ASTBITMAPCONTROL_H__E09120E3_6AA7_4764_8EC4_A794AAE392AE__INCLUDED_)
#define AFX_ASTBITMAPCONTROL_H__E09120E3_6AA7_4764_8EC4_A794AAE392AE__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// Machine generated IDispatch wrapper class(es) created by Microsoft Visual C++

// NOTE: Do not modify the contents of this file.  If this class is regenerated by
//  Microsoft Visual C++, your modifications will be overwritten.
/////////////////////////////////////////////////////////////////////////////
// CAstBitmapControl wrapper class

class CAstBitmapControl : public CWnd
{
protected:
	DECLARE_DYNCREATE(CAstBitmapControl)
public:
	CLSID const& GetClsid()
	{
		static CLSID const clsid
			= { 0x916da9c4, 0x75e2, 0x46c6, { 0xa4, 0x1f, 0xe1, 0x52, 0x2e, 0xcc, 0xf8, 0x71 } };
		return clsid;
	}
	virtual BOOL Create(LPCTSTR lpszClassName,
		LPCTSTR lpszWindowName, DWORD dwStyle,
		const RECT& rect,
		CWnd* pParentWnd, UINT nID,
		CCreateContext* pContext = NULL)
	{ return CreateControl(GetClsid(), lpszWindowName, dwStyle, rect, pParentWnd, nID); }

    BOOL Create(LPCTSTR lpszWindowName, DWORD dwStyle,
		const RECT& rect, CWnd* pParentWnd, UINT nID,
		CFile* pPersist = NULL, BOOL bStorage = FALSE,
		BSTR bstrLicKey = NULL)
	{ return CreateControl(GetClsid(), lpszWindowName, dwStyle, rect, pParentWnd, nID,
		pPersist, bStorage, bstrLicKey); }

// Attributes
public:
	long GetKey();
	void SetKey(long);
	BOOL GetEnabled();
	void SetEnabled(BOOL);

// Operations
public:
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_ASTBITMAPCONTROL_H__E09120E3_6AA7_4764_8EC4_A794AAE392AE__INCLUDED_)
