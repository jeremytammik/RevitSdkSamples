// NativeClipAnglePage.cpp : implementation file
//

#include "pch.h"
#include "afxdialogex.h"
#include "NativeClipAnglePage.h"


// NativeClipAnglePage dialog
CWnd* NativeClipAnglePage::m_pWnd = NULL;

IMPLEMENT_DYNAMIC(NativeClipAnglePage, CDialog)

NativeClipAnglePage::NativeClipAnglePage(CWnd* pParent /*=nullptr*/)
	: CDialog(IDD_CLIPANGLEDIALOG, pParent)
{
	m_pCurRule = nullptr;
	m_bIsInitialising = true;
}

NativeClipAnglePage::~NativeClipAnglePage()
{
	if (m_pCurRule)
	{
		m_pCurRule->Release();
		m_pCurRule = NULL;
	}
}

void 
NativeClipAnglePage::setJoint(SampleClipAngle* pVal)
{
	if (m_pCurRule)
	{
		m_pCurRule->Release();
		m_pCurRule = NULL;
	}
	m_pCurRule = pVal;
	m_pCurRule->AddRef();
	//angle
	_bstr_t tmpClass(m_pCurRule->m_sProfType, TRUE); //avoid memory leaks
	_bstr_t tmpName(m_pCurRule->m_sProfSize, TRUE); //avoid memory leaks

	m_Profile.SetCurrentProfileName(CString("AISC 15.0 Angle equal#@§@#L4X4X3/8").AllocSysString());
	m_Profile.SetUseFilterClass(TRUE);
	m_Profile.SetShowHideAllSections(TRUE);
	m_Profile.AppendAcceptedClassGroup(CString("W").AllocSysString());
	m_Profile.SetCurrentClass((LPCTSTR)tmpClass);
	m_Profile.SetCurrentSection((LPCTSTR)tmpName);


	_bstr_t boltsStd(m_pCurRule->m_sBoltsStandard, true);
	_bstr_t boltsMat(m_pCurRule->m_sBoltsMaterial, true);
	_bstr_t boltSet(m_pCurRule->m_sBoltsSet, true);

	m_Bolts.SetBoltStandard((LPCTSTR)boltsStd);
	m_Bolts.SetBoltMaterial((LPCTSTR)boltsMat);
	m_Bolts.SetBoltSet((LPCTSTR)boltSet);
	m_Bolts.SetBoltDiameter(m_pCurRule->m_dBoltsDiameter);

	UpdateData(FALSE);

	m_bIsInitialising = false;
}


void NativeClipAnglePage::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_ASTPROFILECONTROL1, m_Profile);
	DDX_Control(pDX, IDC_BOLTSTANDARDSCTRL1, m_Bolts);
}

CWnd*
NativeClipAnglePage::GetParentWindow()
{
	if (m_pWnd == nullptr)
	{
		// Restore the module state from the main executable, in order to access the main hwnd
		AFX_MODULE_STATE* appModuleState = AfxGetAppModuleState();

		// Get the current module state
		AFX_MODULE_STATE* currModuleState = AfxGetModuleState();

		// Switch the module state
		AfxSetModuleState(appModuleState);

		m_pWnd = AfxGetMainWnd();
		if (m_pWnd == nullptr)
		{
			try
			{
				CWinApp* pApp = AfxGetApp();
				if (pApp)
				{
					m_pWnd = pApp->GetMainWnd();
					if (m_pWnd == nullptr)
					{
						m_pWnd = (CFrameWnd*)pApp->m_pMainWnd;
					}
				}
			}
			catch (...)
			{

			}
			
		}

		// Switch back the module state
		AfxSetModuleState(currModuleState);
	}
	return m_pWnd;
}


void
NativeClipAnglePage::OnProfileChanged()
{
	// TODO: Add your control notification handler code here
	if (!m_bIsInitialising && m_pCurRule)
	{
		UpdateData(TRUE);
		IJointPtr curJoint;
		m_pCurRule->get_Joint(&curJoint);

		if (m_Profile.GetEnabled() == FALSE)
		{
			return;
		}

		_bstr_t tempClassJoint(m_pCurRule->m_sProfType, TRUE);
		_bstr_t tempNameJoint(m_pCurRule->m_sProfSize, TRUE);

		_bstr_t tempClassDialog(m_Profile.GetCurrentClass().AllocSysString(), TRUE);
		_bstr_t tempNameDialog(m_Profile.GetCurrentSection().AllocSysString(), TRUE);

		if (tempClassJoint != tempClassDialog ||
			tempNameJoint != tempNameDialog)
		{
			CString currentClass = m_Profile.GetCurrentClass();
			CString currentSection = m_Profile.GetCurrentSection();

			m_pCurRule->m_sProfType = currentClass.AllocSysString();
			m_pCurRule->m_sProfSize = currentSection.AllocSysString();

			//setGrayedVals();

			curJoint->SaveData(m_pCurRule);
			curJoint->UpdateDrivenConstruction();
		}
	}
}


void 
NativeClipAnglePage::OnBoltsChanged()
{
	if (!m_bIsInitialising && m_pCurRule)
	{
		UpdateData(TRUE);
		IJointPtr curJoint;
		m_pCurRule->get_Joint(&curJoint);

		bool bRet = false;

		double tempValueDiameter = m_Bolts.GetBoltDiameter();
		_bstr_t tempValueGrade((m_Bolts.GetBoltMaterial()).AllocSysString(), TRUE);
		_bstr_t tempValueAssembly((m_Bolts.GetBoltSet()).AllocSysString(), TRUE);
		_bstr_t tempValueType((m_Bolts.GetBoltStandard()).AllocSysString(), TRUE);

		_bstr_t jointGrade(m_pCurRule->m_sBoltsMaterial, TRUE);
		_bstr_t jointAssembly(m_pCurRule->m_sBoltsSet, TRUE);
		_bstr_t jointType(m_pCurRule->m_sBoltsStandard, TRUE);

		if ((tempValueDiameter != m_pCurRule->m_dBoltsDiameter) ||
			(tempValueGrade != jointGrade) ||
			(tempValueAssembly != jointAssembly) ||
			(tempValueType != jointType))
      {
         m_pCurRule->m_dBoltsDiameter = tempValueDiameter;
         m_pCurRule->m_sBoltsMaterial = CString(tempValueGrade.GetBSTR()).AllocSysString();
         m_pCurRule->m_sBoltsSet = CString(tempValueAssembly.GetBSTR()).AllocSysString();
         m_pCurRule->m_sBoltsStandard = CString(tempValueType.GetBSTR()).AllocSysString();
         curJoint->SaveData(m_pCurRule);
         curJoint->UpdateDrivenConstruction();
      }
	}
}

BEGIN_MESSAGE_MAP(NativeClipAnglePage, CDialog)
END_MESSAGE_MAP()


// NativeClipAnglePage message handlers
BEGIN_EVENTSINK_MAP(NativeClipAnglePage, CDialog)
	//{{AFX_EVENTSINK_MAP(CUSClipAngle_Page1)
	ON_EVENT(NativeClipAnglePage, IDC_ASTPROFILECONTROL1, 20 /* ProfileChanged */, OnProfileChanged, VTS_NONE)
	ON_EVENT(NativeClipAnglePage, IDC_BOLTSTANDARDSCTRL1, 5 /* ProfileChanged */, OnBoltsChanged, VTS_NONE)
END_EVENTSINK_MAP()
