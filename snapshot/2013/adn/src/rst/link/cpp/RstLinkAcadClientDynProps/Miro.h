

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 7.00.0555 */
/* at Fri Feb 15 18:32:19 2013
 */
/* Compiler settings for Miro.idl:
    Oicf, W1, Zp8, env=Win32 (32b run), target_arch=X86 7.00.0555 
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
/* @@MIDL_FILE_HEADING(  ) */

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 475
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __Miro_h__
#define __Miro_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __IRevitID_FWD_DEFINED__
#define __IRevitID_FWD_DEFINED__
typedef interface IRevitID IRevitID;
#endif 	/* __IRevitID_FWD_DEFINED__ */


#ifndef __IForce_FWD_DEFINED__
#define __IForce_FWD_DEFINED__
typedef interface IForce IForce;
#endif 	/* __IForce_FWD_DEFINED__ */


#ifndef __ISection_FWD_DEFINED__
#define __ISection_FWD_DEFINED__
typedef interface ISection ISection;
#endif 	/* __ISection_FWD_DEFINED__ */


#ifndef __IUsage_FWD_DEFINED__
#define __IUsage_FWD_DEFINED__
typedef interface IUsage IUsage;
#endif 	/* __IUsage_FWD_DEFINED__ */


#ifndef __RevitID_FWD_DEFINED__
#define __RevitID_FWD_DEFINED__

#ifdef __cplusplus
typedef class RevitID RevitID;
#else
typedef struct RevitID RevitID;
#endif /* __cplusplus */

#endif 	/* __RevitID_FWD_DEFINED__ */


#ifndef __Force_FWD_DEFINED__
#define __Force_FWD_DEFINED__

#ifdef __cplusplus
typedef class Force Force;
#else
typedef struct Force Force;
#endif /* __cplusplus */

#endif 	/* __Force_FWD_DEFINED__ */


#ifndef __Section_FWD_DEFINED__
#define __Section_FWD_DEFINED__

#ifdef __cplusplus
typedef class Section Section;
#else
typedef struct Section Section;
#endif /* __cplusplus */

#endif 	/* __Section_FWD_DEFINED__ */


#ifndef __Usage_FWD_DEFINED__
#define __Usage_FWD_DEFINED__

#ifdef __cplusplus
typedef class Usage Usage;
#else
typedef struct Usage Usage;
#endif /* __cplusplus */

#endif 	/* __Usage_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"

#ifdef __cplusplus
extern "C"{
#endif 


#ifndef __IRevitID_INTERFACE_DEFINED__
#define __IRevitID_INTERFACE_DEFINED__

/* interface IRevitID */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IRevitID;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("A534D69F-802A-40cb-B62E-5DAE8C70FF76")
    IRevitID : public IDispatch
    {
    public:
    };
    
#else 	/* C style interface */

    typedef struct IRevitIDVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IRevitID * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IRevitID * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IRevitID * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IRevitID * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IRevitID * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IRevitID * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IRevitID * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        END_INTERFACE
    } IRevitIDVtbl;

    interface IRevitID
    {
        CONST_VTBL struct IRevitIDVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IRevitID_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IRevitID_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IRevitID_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IRevitID_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IRevitID_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IRevitID_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IRevitID_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IRevitID_INTERFACE_DEFINED__ */


#ifndef __IForce_INTERFACE_DEFINED__
#define __IForce_INTERFACE_DEFINED__

/* interface IForce */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IForce;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("F648290E-EDB2-49aa-A437-69B1280D9A96")
    IForce : public IDispatch
    {
    public:
    };
    
#else 	/* C style interface */

    typedef struct IForceVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IForce * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IForce * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IForce * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IForce * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IForce * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IForce * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IForce * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        END_INTERFACE
    } IForceVtbl;

    interface IForce
    {
        CONST_VTBL struct IForceVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IForce_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IForce_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IForce_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IForce_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IForce_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IForce_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IForce_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IForce_INTERFACE_DEFINED__ */


#ifndef __ISection_INTERFACE_DEFINED__
#define __ISection_INTERFACE_DEFINED__

/* interface ISection */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_ISection;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("185408D6-1454-422f-B65F-585EBE11D4C7")
    ISection : public IDispatch
    {
    public:
    };
    
#else 	/* C style interface */

    typedef struct ISectionVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ISection * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ISection * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ISection * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            ISection * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            ISection * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            ISection * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            ISection * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        END_INTERFACE
    } ISectionVtbl;

    interface ISection
    {
        CONST_VTBL struct ISectionVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ISection_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ISection_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ISection_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define ISection_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define ISection_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define ISection_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define ISection_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ISection_INTERFACE_DEFINED__ */


#ifndef __IUsage_INTERFACE_DEFINED__
#define __IUsage_INTERFACE_DEFINED__

/* interface IUsage */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IUsage;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("184F914F-6836-4b23-8584-C8EF5797448D")
    IUsage : public IDispatch
    {
    public:
    };
    
#else 	/* C style interface */

    typedef struct IUsageVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IUsage * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IUsage * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IUsage * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IUsage * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IUsage * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IUsage * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IUsage * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        END_INTERFACE
    } IUsageVtbl;

    interface IUsage
    {
        CONST_VTBL struct IUsageVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IUsage_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IUsage_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IUsage_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IUsage_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IUsage_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IUsage_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IUsage_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IUsage_INTERFACE_DEFINED__ */



#ifndef __AsdkMiroLib_LIBRARY_DEFINED__
#define __AsdkMiroLib_LIBRARY_DEFINED__

/* library AsdkMiroLib */
/* [helpstring][version][uuid] */ 


EXTERN_C const IID LIBID_AsdkMiroLib;

EXTERN_C const CLSID CLSID_RevitID;

#ifdef __cplusplus

class DECLSPEC_UUID("B44BF477-78EF-4455-AB16-69FDD5668484")
RevitID;
#endif

EXTERN_C const CLSID CLSID_Force;

#ifdef __cplusplus

class DECLSPEC_UUID("3E21B286-B799-4bce-B115-EB06287D1454")
Force;
#endif

EXTERN_C const CLSID CLSID_Section;

#ifdef __cplusplus

class DECLSPEC_UUID("0F88B0AA-BAE6-47f2-B8D3-FB8B98A15747")
Section;
#endif

EXTERN_C const CLSID CLSID_Usage;

#ifdef __cplusplus

class DECLSPEC_UUID("61DF3C01-4BEB-48fe-A7DE-CC2F549202FE")
Usage;
#endif
#endif /* __AsdkMiroLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


