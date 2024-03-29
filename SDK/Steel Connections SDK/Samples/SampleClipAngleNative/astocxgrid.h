#if !defined(AFX_ASTOCXGRID_H__A5C43C32_147D_4787_BA3E_AA692503E10F__INCLUDED_)
#define AFX_ASTOCXGRID_H__A5C43C32_147D_4787_BA3E_AA692503E10F__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// Machine generated IDispatch wrapper class(es) created by Microsoft Visual C++
// NOTE: Do not modify the contents of this file.  If this class is regenerated by
//  Microsoft Visual C++, your modifications will be overwritten.


// Dispatch interfaces referenced by this interface
class CUnitsManager;

/////////////////////////////////////////////////////////////////////////////
// CAstOCXGrid wrapper class

typedef enum ColumnType
{
	kEditBox = 0,
	kListBox = 1,
	kComboBox = 2,
	kCheckBox = 3,
	kProfileEdit = 4,
	kButton = 5,
	kWeldEdit = 6,
	kCrumbBox = 7
} eColumnType;

typedef enum CellDataType
{
	kUnits = 0,
	kDouble = 1,
	kInteger = 2,
	kString = 3
} eColumnDataType;

typedef enum CellUnitType
{
	kUnitDistance = 0,
	kUnitAngle = 1,
	kUnitWeight = 2,
	kUnitDistanceGUI = 3,
	kUnitArea = 4,
	kUnitVolume = 5,
	kUnitForce = 6,
	kUnitMoment = 7
} eColumnUnitType;

class CAstOCXGrid : public CWnd
{
protected:
	DECLARE_DYNCREATE(CAstOCXGrid)
public:
	CLSID const& GetClsid()
	{
		static CLSID const clsid
			= { 0x2633a54, 0x53da, 0x4506, { 0xa8, 0xc5, 0xc5, 0x10, 0xc1, 0x81, 0xf2, 0x3c } };
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
	long GetColumnCount();
	void SetColumnCount(long);
	long GetRowCount();
	void SetRowCount(long);
	long GetFixedRowCount();
	void SetFixedRowCount(long);
	long GetFixedColumnCount();
	void SetFixedColumnCount(long);
	CUnitsManager GetUnits();
	void SetUnits(LPDISPATCH);
	BOOL GetHeaderSort();
	void SetHeaderSort(BOOL);
	BOOL GetListMode();
	void SetListMode(BOOL);

// Operations
public:
	long GetRowHeight(long nRow);
	void SetRowHeight(long nRow, long nNewValue);
	long GetColumnWidth(long nColumn);
	void SetColumnWidth(long nColumn, long nNewValue);
	unsigned long GetCellBkColor(long nRow, long nCol);
	void SetCellBkColor(long nRow, long nCol, unsigned long newValue);
	unsigned long GetCellFgColor(long nRow, long nCol);
	void SetCellFgColor(long nRow, long nCol, unsigned long newValue);
	unsigned long GetGridBkColor();
	void SetGridBkColor(unsigned long newValue);
	long GetColumnType(long nCol);
	void SetColumnType(long nCol, long nNewValue);
	long GetColumnDataType(long nCol);
	void SetColumnDataType(long nCol, long nNewValue);
	long GetColumnUnits(long nCol);
	void SetColumnUnits(long nCol, long nNewValue);
	double GetItemDoubleValue(long nRow, long nCol);
	void SetItemDoubleValue(long nRow, long nCol, double newValue);
	long GetItemIntegerValue(long nRow, long nCol);
	void SetItemIntegerValue(long nRow, long nCol, long nNewValue);
	CString GetItemTextValue(long nRow, long nCol);
	void SetItemTextValue(long nRow, long nCol, LPCTSTR lpszNewValue);
	long GetFocusCellRow();
	long GetFocusCellColumn();
	void EnableCell(long nRow, long nCol, short bEnable);
	long InsertColumn(BSTR* strHeading, long nFormat, long nCol, long nType, long dataType = kUnits, long unitType = kUnitDistance);
	long InsertRow(BSTR* strHeading, long nRow);
	BOOL DeleteColumn(long nCol);
	BOOL DeleteRow(long nRow);
	BOOL DeleteAllItems();
	void AddStringToControl(BSTR* newValue);
	BOOL SortItems(long nCol, short bAscending);
	void SetChecked(long nRow, long nCol, short nChecked);
	void AutoSizeRow(long nRow);
	void AutoSizeColumn(long nCol);
	void AutoSizeRows();
	void AutoSizeColumns();
	void AutoSize();
	void ExpandColumnsToFit();
	void ExpandRowsToFit();
	void ExpandToFit();
	void AboutBox();
	void SetEditable(BOOL bEditable);
	void RedrawCell(long nRow, long nCol);
	void AddTableToComboCol(long nColumn, LPCTSTR tableName);
	BOOL GetSortCombo();
	void SetSortCombo(BOOL newStyle);
	void SetLongKey(long nRow, long nCol, long nNewValue);
	long GetLongKey(long nRow, long nCol);
	void SetStringKey(long nRow, long nCol, LPCTSTR lpszNewValue);
	BSTR GetStringKey(long nRow, long nCol);
	void SetProfileName(long nRow, long nCol, LPCTSTR lpszNewValue);
	CString GetProfileName(long nRow, long nCol);
	void FillWeldCombo(long nColumn);
	void SetCellReadOnly(long nRow, long nCol, short bReadOnly);
	long GetDataType(long nRow, long nCol);
	void SetDataType(long nRow, long nCol, long nNewValue);
	long GetUnits(long nRow, long nCol);
	void SetUnits(long nRow, long nCol, long nNewValue);
	void RedrawAllCells();
	void AppendProfileAcceptedClassGroup(LPCTSTR lpszNewValue);
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_ASTOCXGRID_H__A5C43C32_147D_4787_BA3E_AA692503E10F__INCLUDED_)
