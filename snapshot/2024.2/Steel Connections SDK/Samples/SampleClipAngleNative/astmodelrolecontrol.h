#pragma once

// Machine generated IDispatch wrapper class(es) created by Microsoft Visual C++

// NOTE: Do not modify the contents of this file.  If this class is regenerated by
//  Microsoft Visual C++, your modifications will be overwritten.
/////////////////////////////////////////////////////////////////////////////
// CAstmodelrolecontrol wrapper class

class CAstModelRoleControl : public CWnd
{
protected:
	DECLARE_DYNCREATE(CAstModelRoleControl)
public:
	CLSID const& GetClsid()
	{
		static CLSID const clsid
			= { 0xEECAC4EE, 0xAA4C, 0x471C, { 0xAD, 0xA, 0xC1, 0x55, 0x7E, 0xB8, 0x28, 0xDC } };
		return clsid;
	}
	virtual BOOL Create(LPCTSTR lpszClassName, LPCTSTR lpszWindowName, DWORD dwStyle,
						const RECT& rect, CWnd* pParentWnd, UINT nID, 
						CCreateContext* pContext = NULL)
	{ 
		return CreateControl(GetClsid(), lpszWindowName, dwStyle, rect, pParentWnd, nID); 
	}

    BOOL Create(LPCTSTR lpszWindowName, DWORD dwStyle, const RECT& rect, CWnd* pParentWnd, 
				UINT nID, CFile* pPersist = NULL, BOOL bStorage = FALSE,
				BSTR bstrLicKey = NULL)
	{ 
		return CreateControl(GetClsid(), lpszWindowName, dwStyle, rect, pParentWnd, nID,
		pPersist, bStorage, bstrLicKey); 
	}

// Attributes
public:
enum
{
    kEditBox = 0,
    kListBox = 1,
    kComboBox = 2,
    kCheckBox = 3,
    kProfileEdit = 4,
    kButton = 5
}eColumnType;
enum
{
    kUnits = 0,
    kDouble = 1,
    kInteger = 2,
    kString = 3
}eColumnDataType;
enum
{
    kUnitDistance = 0,
    kUnitAngle = 1,
    kUnitWeight = 2,
    kUnitDistanceGUI = 3,
    kUnitArea = 4,
    kUnitVolume = 5,
    kUnitForce = 6,
    kUnitMoment = 7
}eColumnUnitType;
enum
{
    kBeam = 1,
    kPlate = 2
}eGroupingRole;
enum
{
    kGratingStandard = 0,
    kGratingVariable = 1
}eGratingType;
enum
{
    kType = 0,
    kClass = 1,
    kSize = 2
}eGratingControls;
enum
{
    kNull = 0,
    kAnglePage = 1,
    kAreaPage = 2,
    kWeightPage = 4,
    kLengthPage = 8,
    kWeightPerDistancePage = 16,
    kAllPages = 255
}eFirstPage;


// Operations
public:

// _DAstModelRoleControl

// Functions
//

	void SelectItemByKey(LPCTSTR Key)
	{
		static BYTE parms[] = VTS_BSTR ;
		InvokeHelper(0x4, DISPATCH_METHOD, VT_EMPTY, NULL, parms, Key);
	}
	CString getSelectedItemKey()
	{
		CString result;
		InvokeHelper(0x5, DISPATCH_METHOD, VT_BSTR, (void*)&result, NULL);
		return result;
	}
	void SetObjectType(long objectType)
	{
		static BYTE parms[] = VTS_I4 ;
		InvokeHelper(0x6, DISPATCH_METHOD, VT_EMPTY, NULL, parms, objectType);
	}

// Properties
//

long GetLabelDbKey()
{
	long result;
	GetProperty(0x2, VT_I4, (void*)&result);
	return result;
}
void SetLabelDbKey(long propVal)
{
	SetProperty(0x2, VT_I4, propVal);
}
long GetLabelLength()
{
	long result;
	GetProperty(0x3, VT_I4, (void*)&result);
	return result;
}
void SetLabelLength(long propVal)
{
	SetProperty(0x3, VT_I4, propVal);
}
BOOL GetEnabled()
{
	BOOL result;
	GetProperty(DISPID_ENABLED, VT_BOOL, (void*)&result);
	return result;
}
void SetEnabled(BOOL propVal)
{
	SetProperty(DISPID_ENABLED, VT_BOOL, propVal);
}


};