#pragma once
#include "afxdialogex.h"
#include "astprofilecontrol.h"
#include "boltstandards.h"

// NativeClipAnglePage dialog

class NativeClipAnglePage : public CDialog
{
	DECLARE_DYNAMIC(NativeClipAnglePage)
private:
	SampleClipAngle* m_pCurRule;
	static CWnd* m_pWnd;
	CAstProfileControl	m_Profile;
	CBoltStandards			m_Bolts;
	bool                m_bIsInitialising;

public:
	NativeClipAnglePage(CWnd* pParent = nullptr);   // standard constructor
	virtual ~NativeClipAnglePage();

	void setJoint(SampleClipAngle* pVal);

	static CWnd* GetParentWindow();

// Dialog Data
//#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_CLIPANGLEDIALOG};
//#endif
	

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	afx_msg void OnProfileChanged();
	afx_msg void OnBoltsChanged();
	DECLARE_EVENTSINK_MAP()
	DECLARE_MESSAGE_MAP()
};
