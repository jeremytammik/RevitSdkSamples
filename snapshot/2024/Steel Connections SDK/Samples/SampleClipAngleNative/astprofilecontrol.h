#if !defined(AFX_ASTPROFILECONTROL_H__AB9EFEEC_B473_4CD6_BED1_C5923EA79B15__INCLUDED_)
#define AFX_ASTPROFILECONTROL_H__AB9EFEEC_B473_4CD6_BED1_C5923EA79B15__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// Machine generated IDispatch wrapper class(es) created by Microsoft Visual C++

// NOTE: Do not modify the contents of this file.  If this class is regenerated by
//  Microsoft Visual C++, your modifications will be overwritten.
/////////////////////////////////////////////////////////////////////////////
// CAstProfileControl wrapper class

class CAstProfileControl : public CWnd
{
protected:
	DECLARE_DYNCREATE(CAstProfileControl)
public:
	CLSID const& GetClsid()
	{
		static CLSID const clsid
			= { 0x574d51ef, 0x85f9, 0x415c, { 0x81, 0xce, 0x5a, 0xf, 0xc4, 0x5b, 0x1f, 0x79 } };
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
	BOOL GetShowHideAllSections();
	void SetShowHideAllSections(BOOL);
	CString GetCurrentProfileName();
	void SetCurrentProfileName(LPCTSTR);
	CString GetCurrentClass();
	void SetCurrentClass(LPCTSTR);
	CString GetCurrentSection();
	void SetCurrentSection(LPCTSTR);
	long GetCaptionClass();
	void SetCaptionClass(long);
	long GetCaptionTyp();
	void SetCaptionTyp(long);
	long GetCaptionShowHideAllSections();
	void SetCaptionShowHideAllSections(long);
	BOOL GetUseFilterClass();
	void SetUseFilterClass(BOOL);
	BOOL GetCheckBoxOnLeft();
	void SetCheckBoxOnLeft(BOOL);
	BOOL GetShowPromptOnTop();
	void SetShowPromptOnTop(BOOL);
	long GetLabelLength();
	void SetLabelLength(long);
	CString GetCurrentCrossSection();
	void SetCurrentCrossSection(LPCTSTR);
	BOOL GetSummaryRepresentation();
	void SetSummaryRepresentation(BOOL);
	BOOL GetAppearance();
	void SetAppearance(BOOL);
	long GetDropHeight();
	void SetDropHeight(long);
	long GetDropWidth();
	void SetDropWidth(long);
	BOOL GetSummaryDroppedDown();
	void SetSummaryDroppedDown(BOOL);
	BOOL GetEnabled();
	void SetEnabled(BOOL);

// Operations
public:
	void AppendAcceptedClass(LPCTSTR newAcceptedClass);
	void RemoveAcceptedClass(LPCTSTR AcceptedClass);
	void AppendAcceptedClassGroup(LPCTSTR SubtypeName);
  void RemoveAcceptedClassGroup(LPCTSTR SubtypeName);
  void Reset();
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_ASTPROFILECONTROL_H__AB9EFEEC_B473_4CD6_BED1_C5923EA79B15__INCLUDED_)
