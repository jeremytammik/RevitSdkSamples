// Machine generated IDispatch wrapper class(es) created by Microsoft Visual C++

// NOTE: Do not modify the contents of this file.  If this class is regenerated by
//  Microsoft Visual C++, your modifications will be overwritten.


#include "pch.h"
#include "aststaticprompt.h"

/////////////////////////////////////////////////////////////////////////////
// CAstStaticPrompt

IMPLEMENT_DYNCREATE(CAstStaticPrompt, CWnd)

/////////////////////////////////////////////////////////////////////////////
// CAstStaticPrompt properties

long CAstStaticPrompt::GetKey()
{
	long result;
	GetProperty(0x1, VT_I4, (void*)&result);
	return result;
}

void CAstStaticPrompt::SetKey(long propVal)
{
	SetProperty(0x1, VT_I4, propVal);
}

BOOL CAstStaticPrompt::GetEnabled()
{
	BOOL result;
	GetProperty(DISPID_ENABLED, VT_BOOL, (void*)&result);
	return result;
}

void CAstStaticPrompt::SetEnabled(BOOL propVal)
{
	SetProperty(DISPID_ENABLED, VT_BOOL, propVal);
}

/////////////////////////////////////////////////////////////////////////////
// CAstStaticPrompt operations