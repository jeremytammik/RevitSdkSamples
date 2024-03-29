#if !defined(AFX_UNITSMANAGER_H__CEBB4162_730D_40CD_8126_AD9245ED4795__INCLUDED_)
#define AFX_UNITSMANAGER_H__CEBB4162_730D_40CD_8126_AD9245ED4795__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// Machine generated IDispatch wrapper class(es) created by Microsoft Visual C++
// NOTE: Do not modify the contents of this file.  If this class is regenerated by
//  Microsoft Visual C++, your modifications will be overwritten.

/////////////////////////////////////////////////////////////////////////////
// CUnitsManager wrapper class

class CUnitsManager : public COleDispatchDriver
{
public:
	CUnitsManager() {}		// Calls COleDispatchDriver default constructor
	CUnitsManager(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CUnitsManager(const CUnitsManager& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	void SetUseRegionalSettings(long nNewValue);
	long GetUseRegionalSettings();
	void getUnitOfDistance(LPDISPATCH retval);
	void setUnitOfDistance(LPDISPATCH retval);
	void getUnitOfWeight(LPDISPATCH retval);
	void setUnitOfWeight(LPDISPATCH retval);
	void getUnitOfAngle(LPDISPATCH retval);
	void setUnitOfAngle(LPDISPATCH retval);
	void doModal();
	void getUnitOfArea(LPDISPATCH retval);
	void setUnitOfArea(LPDISPATCH retval);
	void getUnitOfVolume(LPDISPATCH retval);
	void setUnitOfVolume(LPDISPATCH retval);
	void getUnitOfDistanceGUI(LPDISPATCH retval);
	void setUnitOfDistanceGUI(LPDISPATCH retval);
	void getUnitOfForce(LPDISPATCH retval);
	void setUnitOfForce(LPDISPATCH retval);
	void getUnitOfMoment(LPDISPATCH retval);
	void setUnitOfMoment(LPDISPATCH retval);
	void getUnitOfWeightPerDistance(LPDISPATCH retval);
	void setUnitOfWeightPerDistance(LPDISPATCH retval);
	void getUnitOfAreaPerDistance(LPDISPATCH retval);
	void setUnitOfAreaPerDistance(LPDISPATCH retval);
	void doModalWithCaption(LPCTSTR caption);
	void doModalWithChooseFirstPage(long firstPageNo);
	void doModalUnitPage(long pageNo);
	void doModalUnitPages(long pagesSelection, long page);
	void doModalWithCaptionUnitPages(LPCTSTR caption, long pagesSelection, long page);
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_UNITSMANAGER_H__CEBB4162_730D_40CD_8126_AD9245ED4795__INCLUDED_)
